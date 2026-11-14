using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using DishDrop.Data;
using DishDrop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DishDrop.IntegrationTests
{
    [Trait("Category", "NoDatabase")]
    public class NavigationMappingTests
    {
        private static ApplicationDbContext CreateContext()
        {
            // Metadata only: no database connection is opened by these tests.
            return new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Server=unused;Database=NavigationMetadata;Integrated Security=true")
                    .Options);
        }

        [Fact]
        public void DomainRelationships_HaveInversesForTheSameExplicitForeignKeys()
        {
            using var context = CreateContext();
            var relationships = context.Model.GetEntityTypes()
                .Where(e => e.ClrType.Namespace == typeof(Restaurant).Namespace)
                .SelectMany(e => e.GetForeignKeys())
                .ToList();

            Assert.NotEmpty(relationships);
            foreach (var relationship in relationships)
            {
                var dependentNavigation = relationship.DependentToPrincipal;
                var principalNavigation = relationship.PrincipalToDependent;
                Assert.NotNull(dependentNavigation);
                Assert.NotNull(principalNavigation);
                Assert.Same(relationship, principalNavigation.ForeignKey);
                Assert.Equal(relationship.DeclaringEntityType, principalNavigation.TargetEntityType);
                Assert.Equal(!relationship.IsUnique, principalNavigation.IsCollection);
                Assert.DoesNotContain(relationship.Properties, p => p.IsShadowProperty());

                var attribute = dependentNavigation.PropertyInfo!.GetCustomAttribute<ForeignKeyAttribute>();
                Assert.NotNull(attribute);
                Assert.Equal(Assert.Single(relationship.Properties).Name, attribute.Name);
            }
        }

        [Fact]
        public void Collections_AreInitializedAndUserRelationshipsRemainDistinct()
        {
            using var context = CreateContext();
            var entities = context.Model.GetEntityTypes()
                .Where(e => e.ClrType.Namespace == typeof(Restaurant).Namespace);

            foreach (var entity in entities)
            {
                var instance = Activator.CreateInstance(entity.ClrType)!;
                foreach (var navigation in entity.GetNavigations().Where(n => n.IsCollection))
                {
                    var property = navigation.PropertyInfo!;
                    Assert.Equal(typeof(ICollection<>), property.PropertyType.GetGenericTypeDefinition());
                    var collection = property.GetValue(instance);
                    Assert.NotNull(collection);
                    Assert.Equal(typeof(List<>), collection.GetType().GetGenericTypeDefinition());
                    Assert.Empty(((IEnumerable)collection).Cast<object>());
                    Assert.True(property.CanWrite);
                }
            }

            var user = context.Model.FindEntityType(typeof(ApplicationUser))!;
            var ticket = context.Model.FindEntityType(typeof(SupportTicket))!;
            var created = user.FindNavigation(nameof(ApplicationUser.CreatedSupportTickets))!.ForeignKey;
            var assigned = user.FindNavigation(nameof(ApplicationUser.AssignedSupportTickets))!.ForeignKey;
            Assert.Same(ticket.FindNavigation(nameof(SupportTicket.CreatedByUser))!.ForeignKey, created);
            Assert.Same(ticket.FindNavigation(nameof(SupportTicket.AssignedAdmin))!.ForeignKey, assigned);
            Assert.NotSame(created, assigned);

            foreach (var name in new[]
            {
                nameof(ApplicationUser.UserClaims), nameof(ApplicationUser.UserLogins),
                nameof(ApplicationUser.UserTokens), nameof(ApplicationUser.UserRoles)
            })
            {
                Assert.Equal(DeleteBehavior.Cascade, user.FindNavigation(name)!.ForeignKey.DeleteBehavior);
            }

            // These existing one-to-one links permit a principal without its dependent.
            Assert.False(user.FindNavigation(nameof(ApplicationUser.CourierProfile))!.ForeignKey.IsRequiredDependent);
            Assert.False(context.Model.FindEntityType(typeof(Order))!
                .FindNavigation(nameof(Order.DeliveryAddress))!.ForeignKey.IsRequiredDependent);
            Assert.False(context.Model.FindEntityType(typeof(Review))!
                .FindNavigation(nameof(Review.Reply))!.ForeignKey.IsRequiredDependent);
        }
    }
}
