using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL.Data;
using WebApplication1.DAL.Entities;

namespace WebApplication1.Services
{
    public class DbInitializer
    {
        public static async Task Seed(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager,
                                       RoleManager<IdentityRole> roleManager)
        {
            // создать БД, если она еще не создана
            context.Database.EnsureCreated();
            // проверка наличия ролей
            if (!context.Roles.Any())
            {
                var roleAdmin = new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "admin"
                };
                // создать роль admin
                await roleManager.CreateAsync(roleAdmin);
            }
            // проверка наличия пользователей
            if (!context.Users.Any())
            {
                // создать пользователя user@mail. ru
                var user = new ApplicationUser
                {
                    Email = "user@mail.ru",
                    UserName = "user@mail.ru"
                };
                await userManager.CreateAsync(user, "123456");
                // создать пользователя admin@mail. ru
                var admin = new ApplicationUser
                {
                    Email = "admin@mail.ru",
                    UserName = "admin@mail.ru"
                };
                await userManager.CreateAsync(admin, "123456");
                // назначить роль admin
                admin = await userManager.FindByEmailAsync("admin@mail.ru");
                await userManager.AddToRoleAsync(admin, "admin");
            }
            if (!context.DishGroups.Any())
            {
                context.DishGroups.AddRange(
                new List<DishGroup>
                {
                    new DishGroup {GroupName="Стартеры"},
                    new DishGroup {GroupName="Салаты"},
                    new DishGroup {GroupName="Супы"},
                    new DishGroup {GroupName="Основные блюда"},
                    new DishGroup {GroupName="Напитки"},
                    new DishGroup {GroupName="Десерты"}
                });
                await context.SaveChangesAsync();
            }
            // проверка наличия объектов
            if (!context.Dishes.Any())
            {
                context.Dishes.AddRange(
                new List<Dish>
                {
                new Dish {DishName="Картофель отварной", Description="Национальное белорусское блюдо",
                Calories =200, DishGroupId=4, Image="potato.png" },
                new Dish { DishName="Чизкейк",    Description="Нежный и вкусный",
                Calories =330, DishGroupId=6, Image="cheesecake.png" },
                new Dish {DishName="Шоколадный торт",       Description="Просто объедение",
                Calories =635, DishGroupId=6, Image="cake.png" },
                new Dish { DishName="Запеченый лосось",    Description="Изысканное блюдо",
                Calories =524, DishGroupId=4, Image="fish.png" },
                new Dish { DishName="Лазанья",     Description="Для тех, кто скучает по Италии",
                Calories =280, DishGroupId=4, Image="lasagna.png" }
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
