using System;
using System.Linq;
using System.Threading.Tasks;
using Faker;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIO.Models;
using Task = System.Threading.Tasks.Task;

namespace ProjectIO.Data
{
    public static class ApplicationDbContextSeedData
    {
        public static void Run(this ApplicationDbContext context, IServiceProvider serviceProvider, string testUserPw)
        {
            GenerateUsersAsync(context, serviceProvider, testUserPw).Wait();
            GenerateCustomersAsync(context).Wait();
            GenerateProjectsAsync(context).Wait();
        }
        public static async Task GenerateUsersAsync(this ApplicationDbContext context, IServiceProvider serviceProvider,
            string testUserPw)
        {
            if (context.Users.Any())
            {
                return;
            }

            var i = 1;
            var userId = await EnsureUser(serviceProvider, testUserPw, $"user{i}@example.com");
            await EnsureRole(serviceProvider, userId, "User");

            i += 1;
            userId = await EnsureUser(serviceProvider, testUserPw, $"user{i}@example.com");
            await EnsureRole(serviceProvider, userId, "User");

            i += 1;
            userId = await EnsureUser(serviceProvider, testUserPw, $"user{i}@example.com");
            await EnsureRole(serviceProvider, userId, "User");

            i += 1;
            userId = await EnsureUser(serviceProvider, testUserPw, $"user{i}@example.com");
            await EnsureRole(serviceProvider, userId, "User");

            i += 1;
            userId = await EnsureUser(serviceProvider, testUserPw, $"user{i}@example.com");
            await EnsureRole(serviceProvider, userId, "User");
        }

        public static async Task GenerateCustomersAsync(this ApplicationDbContext context)
        {
            if (context.Customers.Any())
            {
                return;
            }

            var customers = new Customer[5];

            foreach (var customer in customers)
            {
                customer.Name = string.Join(" ", Lorem.Words(2));
            }

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        public static async Task GenerateProjectsAsync(this ApplicationDbContext context)
        {
            if (context.Projects.Any())
            {
                return;
            }

            var customers = await context.Customers.ToListAsync();
            var projects = new Project[customers.Count];

            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                var project = new Project
                {
                    Name = string.Join(" ", Lorem.Words(2)),
                    Description = string.Join(" ", Lorem.Words(4)),
                    Customer = customer,
                };
                projects[i] = project;
            }

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPw,
            string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser {UserName = userName};
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("Failed to get User. Maybe the password was not strong enough?");
            }

            return await userManager.AddToRoleAsync(user, role);
        }
    }
}