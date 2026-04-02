using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }
        public DbSet<Chatroom> Chatrooms {get; set;}
        public DbSet<RoomMember> RoomMembers {get; set;}
        public DbSet<Massages> Massages {get; set;} 
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RoomMember>(x => x.HasKey(p => new {p.AppUserId, p.RoomId}));
            builder.Entity<RoomMember>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.RoomMembers)
                .HasForeignKey(p => p.AppUserId);
            builder.Entity<RoomMember>()
                .HasOne(u => u.Chatroom)
                .WithMany(u => u.RoomMembers)
                .HasForeignKey(p => p.RoomId);
            builder.Entity<Massages>()
                .HasOne(m => m.AppUser)
                .WithMany(u => u.Massages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Massages>()
                .HasOne(m => m.Chatroom)
                .WithMany(c => c.Massages) 
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "Admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "1"
                },
                new IdentityRole
                {
                    Id = "User",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "1"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}