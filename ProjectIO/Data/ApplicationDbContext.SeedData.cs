﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Faker;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIO.Areas.Identity;
using ProjectIO.Models;
using Task = System.Threading.Tasks.Task;

namespace ProjectIO.Data
{
    public static class ApplicationDbContextSeedData
    {
        public static async Task GenerateUsersAsync(this ApplicationDbContext context, IServiceProvider serviceProvider,
            string testUserPw)
        {
            if (context.Users.Any())
            {
                return;
            }

            var userId = await EnsureUser(serviceProvider, testUserPw, $"admin@example.com");
            await EnsureRole(serviceProvider, userId, "Administrator");
        }

        public static async Task GenerateEmployeesAsync(this ApplicationDbContext context)
        {
            if (context.Employees.Any())
            {
                return;
            }

            var employees = new Employee[]
            {
                new Employee()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "jdoe@example.com",
                    EmployeeId = "D1234",
                    RegistrationCode = "D9876"
                },
                new Employee()
                {
                    FirstName = "Peter",
                    LastName = "Klinger",
                    Email = "pklinger@example.com",
                    EmployeeId = "K1234",
                    RegistrationCode = "K9876"
                },
                new Employee()
                {
                    FirstName = "Jon",
                    LastName = "Smith",
                    Email = "jsmith@example.com",
                    EmployeeId = "S1234",
                    RegistrationCode = "S9876"
                },
            };

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }

        public static void GenerateCustomers(this ApplicationDbContext context, bool force)
        {
            if (context.Customers.Any() && !force)
            {
                return;
            }

            var customers = new Customer[5];

            foreach (var customer in customers)
            {
                customer.Name = string.Join(" ", Lorem.Words(2));
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        public static void GenerateProjects(this ApplicationDbContext context, bool force)
        {
            if (context.Projects.Any() && !force)
            {
                return;
            }

            var customers = context.Customers.ToList();
            var projects = new Project[customers.Count];

            for (var i = 0; i < customers.Count; i++)
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

            context.Projects.AddRange(projects);
            context.SaveChanges();
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPw,
            string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser {UserName = userName};
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

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("Failed to get User. Maybe the password was not strong enough?");
            }

            return await userManager.AddToRoleAsync(user, role);
        }
    }
}