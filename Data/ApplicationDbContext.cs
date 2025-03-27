using GharchNews.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportGroup> ReportGroups { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Ads> Ads { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}
