using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Data.Data
{
    public static class EntitySeeder
    {
        public static async Task Seed(AppDbContext context)
        {
            if (!await context.EventTypes.AnyAsync())
            {
                var userEvents = new[]
            {
                    "User Registered",
                    "User Logged In",
                    "User Logged Out",
                    "User Updated Profile",
                    "User Deleted Account",
                    "User Password Changed",
                    "User Role Updated",
                    "User Email Confirmed",
                    "User Locked",
                    "User Unlocked"
                    };

                var orderEvents = new[]
                {
                    "Order Created",
                    "Order Confirmed",
                    "Order Packed",
                    "Order Shipped",
                    "Order Delivered",
                    "Order Cancelled",
                    "Order Refunded",
                    "Order Returned"
                     };

                var paymentEvents = new[]
                {
                    "Payment Initiated",
                    "Payment Successful",
                    "Payment Failed",
                    "Payment Refunded",
                    "Invoice Generated",
                    "Invoice Sent"
                };

                var inventoryEvents = new[]
                {
                    "Item Added to Inventory",
                    "Item Removed from Inventory",
                    "Inventory Low",
                    "Inventory Restocked",
                    "Inventory Audit Completed"
                };

                var notificationEvents = new[]
                {
                    "Email Sent",
                    "SMS Sent",
                    "Push Notification Sent",
                    "Notification Delivered",
                    "Notification Failed"
                };

                var systemEvents = new[]
                {
                    "System Start",
                    "System Shutdown",
                    "Backup Started",
                    "Backup Completed",
                    "Error Logged",
                    "Warning Logged"
                };

                var baseEvents = userEvents
                    .Concat(orderEvents)
                    .Concat(paymentEvents)
                    .Concat(inventoryEvents)
                    .Concat(notificationEvents)
                    .Concat(systemEvents)
                    .ToList();

                var randomEvent = new Random();
                var eventTypes = new List<EventType>();

                for (int i = 1; i <= 2000; i++)
                {
                    string baseName = baseEvents[randomEvent.Next(baseEvents.Count)];


                    eventTypes.Add(new EventType
                    {
                        Id = i,
                        Name = baseName
                    });
                }

                await context.EventTypes.AddRangeAsync(eventTypes);
                await context.SaveChangesAsync();
            }

            if (!await context.Users.AnyAsync())
            {
                var randomUser = new Random();
                var users = new List<User>();
                for (int i = 0; i < 1000; i++)
                {
                    users.Add(new User
                    {
                        Id = i.ToString(),
                        FullName = "user" + i.ToString()
                    });
                }
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            if (!await context.Histories.AnyAsync())
            {

                var users = await context.Users
                    .OrderBy(u => u.Id)
                    .Select(u => u.Id)
                    .ToListAsync();

                var random = new Random();

                var histories = Enumerable.Range(1, 999).Select(i =>
                {
                    var userId = users[random.Next(users.Count)];

                    return new History
                    {
                        Text = $"History record {i}",
                        DateTime = DateTime.UtcNow.AddMinutes(-random.Next(0, 60000)),
                        UserId = userId,
                        EventTypeId = random.Next(1, 1001)
                    };
                }).ToList();

                await context.Histories.AddRangeAsync(histories);
                await context.SaveChangesAsync();
            }





        }
    }
}
