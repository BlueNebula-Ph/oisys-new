using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using OisysNew.Models;

namespace OisysNew
{
    public static class DbInitializer
    {
        public static void Seed(OisysDbContext context, IHostingEnvironment env)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if(env.IsDevelopment())
            {
                SeedCategories(context);
                SeedProvincesAndCities(context);
                SeedCustomer(context);
                SeedItems(context);
            }

            SeedUsers(context);

            context.SaveChanges();
        }

        
        private static void SeedCustomer(OisysDbContext context)
        {
            if (!context.Customers.Any())
            {
                context.Customers.Add(new Customer
                {
                    Name = "Mickey Mouse",
                    Email = "mickey@disney.com",
                    ContactNumber = "399-39-39",
                    ContactPerson = "Mr. Mickey",
                    Address = "Disneyland Tokyo",
                    CityId = 4,
                    ProvinceId = 1,
                    Terms = "term1",
                    Discount = 10m,
                    PriceList = PriceList.WalkInPrice,
                    Keywords = "Mickey Mouse"
                });

                context.Customers.Add(new Customer
                {
                    Name = "Mario Cart",
                    Email = "mario@nintendo.com",
                    ContactNumber = "383-33-00",
                    ContactPerson = "Mr. Luigi",
                    Address = "Japan Nintendo Park",
                    CityId = 5,
                    ProvinceId = 2,
                    Terms = "term2",
                    Discount = 5m,
                    PriceList = PriceList.MainPrice,
                    Keywords = "Mario Cart"
                });
            }
        }

        private static void SeedProvincesAndCities(OisysDbContext context)
        {
            if (!context.Provinces.Any())
            {
                var provinces = new List<Province>
                {
                    new Province
                    {
                        Name = "NCR",
                        Cities = new List<City>
                        {
                            new City { Name = "Manila" },
                            new City { Name = "Makati" },
                            new City { Name = "Las Pinas" },
                            new City { Name = "Paranaque" },
                        },
                    },
                    new Province
                    {
                        Name = "Cavite",
                        Cities = new List<City>
                        {
                            new City { Name = "Tanza" },
                            new City { Name = "Indang" },
                            new City { Name = "Gen Trias" },
                        },
                    },
                    new Province
                    {
                        Name = "Bulacan",
                        Cities = new List<City>
                        {
                            new City { Name = "Malolos" },
                            new City { Name = "Bocaue" },
                            new City { Name = "Obando" },
                        },
                    },
                };
                context.Provinces.AddRange(provinces);
            }
        }

        private static void SeedCategories(OisysDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Material123" },
                    new Category { Name = "Lead234" },
                    new Category { Name = "Paint345" },
                    new Category { Name = "Tubes456" },
                    new Category { Name = "Material567" },
                    new Category { Name = "Lead678" },
                    new Category { Name = "Paint456" },
                    new Category { Name = "Tubes234" },
                    new Category { Name = "Material1123" },
                    new Category { Name = "Lead11234" },
                    new Category { Name = "Paint112344" },
                    new Category { Name = "Tubes4466899" },
                    new Category { Name = "Material7897" },
                    new Category { Name = "Lead5679" },
                    new Category { Name = "Paint5679" },
                    new Category { Name = "Tubes5678" },
                    new Category { Name = "Material45689" },
                    new Category { Name = "Lead4568" },
                    new Category { Name = "Paint456890" },
                    new Category { Name = "Tubes56756" },
                    new Category { Name = "Material34579" },
                    new Category { Name = "Lead345733" },
                    new Category { Name = "Paint22336678" },
                    new Category { Name = "Tubes11122666" },
                    new Category { Name = "Material11557" },
                    new Category { Name = "Lead12456" },
                    new Category { Name = "Paint3678" },
                    new Category { Name = "Tubes4455" },
                    new Category { Name = "Material2233" },
                    new Category { Name = "Lead1111222" },
                    new Category { Name = "Paint23244" },
                    new Category { Name = "Tubes1111" },
                    new Category { Name = "Material23344" },
                    new Category { Name = "Lead45646" },
                    new Category { Name = "Paint8888" },
                    new Category { Name = "Tubes7777" },
                    new Category { Name = "Material6666" },
                    new Category { Name = "Lead555" },
                    new Category { Name = "Paint444" },
                    new Category { Name = "Tubes333" },
                    new Category { Name = "Material222" },
                    new Category { Name = "Lead123" },
                    new Category { Name = "Paint234" },
                    new Category { Name = "Tubes567" }
                };
                context.Categories.AddRange(categories);
            }
        }

        private static void SeedItems(OisysDbContext context)
        {
            if (!context.Items.Any())
            {
                var items = new List<Item>
                {
                    new Item { Code = "0001", Name = "Item Number 1", CategoryId = 1, Description = "Item 1. This is item 1", CurrentQuantity = 100, ActualQuantity = 100, Unit = "pcs.", MainPrice = 1919.99m, NEPrice = 2929.99m, WalkInPrice = 3939.39m },
                    new Item { Code = "0002", Name = "Item Number 2", CategoryId = 2, Description = "Item 2. This is item 2", CurrentQuantity = 200, ActualQuantity = 200, Unit = "stacks", MainPrice = 919.99m, NEPrice = 929.99m, WalkInPrice = 939.39m },
                    new Item { Code = "0003", Name = "Item Number 3", CategoryId = 1, Description = "Item 3. This is item 3", CurrentQuantity = 300, ActualQuantity = 300, Unit = "makes", MainPrice = 111m, NEPrice = 222m, WalkInPrice = 333m },
                    new Item { Code = "0004", Name = "Item Number 4", CategoryId = 3, Description = "Item 4. This is item 4", CurrentQuantity = 400, ActualQuantity = 400, Unit = "pc", MainPrice = 12.50m, NEPrice = 29.50m, WalkInPrice = 39.50m },
                    new Item { Code = "0005", Name = "Item Number 5", CategoryId = 4, Description = "Item 5. This is item 5", CurrentQuantity = 500, ActualQuantity = 500, Unit = "shards", MainPrice = 400m, NEPrice = 500m, WalkInPrice = 600m },
                    new Item { Code = "0006", Name = "Item Number 6", CategoryId = 4, Description = "Item 6. This is item 6", CurrentQuantity = 600, ActualQuantity = 600, Unit = "rolls", MainPrice = 1211m, NEPrice = 1222m, WalkInPrice = 1233m },
                };
                context.Items.AddRange(items);
            }
        }

        private static void SeedUsers(OisysDbContext context)
        {
            if (!context.Users.Any())
            {
                var newUser = new ApplicationUser { Username = "Admin", Firstname = "Admin", Lastname = "User", AccessRights = "admin,canView,canWrite,canDelete" };
                var password = new PasswordHasher<ApplicationUser>().HashPassword(newUser, "Admin");
                newUser.PasswordHash = password;

                context.Users.Add(newUser);
            }
        }
    }
}