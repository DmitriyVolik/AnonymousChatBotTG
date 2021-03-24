using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TGbot.Models;

namespace TGbot
{
    public class DbBotContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        // public DbSet<Product> Products { get; set; }

        public DbBotContext()
        {
            //Доводит базу до последней миграции - создает если ее нет
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tgbot;Username=postgres;Password=database");
        }

    }
}
