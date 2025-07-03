using Microsoft.EntityFrameworkCore;
using Focus.Domain.Entities;

namespace Focus.Infra
{
    public class FocusDbContext : DbContext
    {
        public FocusDbContext(DbContextOptions<FocusDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Media> Media { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {  
            #region User
            
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property( u => u.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property( u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<User>().Property( u => u.Email)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<User>().Property( u => u.Password)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>().Property( u => u.Description)
                .HasMaxLength(300)
                .IsRequired();
            modelBuilder.Entity<User>().Property( u => u.Description)
                .HasMaxLength(250);
            modelBuilder.Entity<User>().Property(u => u.ProfilePictureUrl)
                .HasMaxLength(200);
            modelBuilder.Entity<User>().Property(u => u.PasswordResetToken)
                .IsRequired(false);
            modelBuilder.Entity<User>().Property(u => u.PasswordResetTokenExpiresAt)
                .IsRequired(false);
            modelBuilder.Entity<User>().Property(u => u.EmailVerificationToken)
                .IsRequired(false);
            modelBuilder.Entity<User>().Property(u => u.EmailVerificationTokenExpiresAt)
                .IsRequired(false);
            modelBuilder.Entity<User>().Property(u => u.PendingNewEmail)
                .HasMaxLength(256)
                .IsRequired(false);
            #endregion
            
            #region Group
            
            modelBuilder.Entity<Group>().ToTable("Groups");
            modelBuilder.Entity<Group>().HasKey(g => g.Id);
            modelBuilder.Entity<Group>().Property( g => g.Id)
                .ValueGeneratedOnAdd();
            
            
            modelBuilder.Entity<Group>().Property( g => g.Name)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Group>().Property( g => g.Description)
                .HasMaxLength(200);
            #endregion

            #region Activity
            
            modelBuilder.Entity<Activity>().ToTable("Activities");
            modelBuilder.Entity<Activity>().HasKey(a => a.Id);
            modelBuilder.Entity<Activity>().Property( a => a.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Activity>().Property( a => a.Title)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Activity>().Property( a => a.Description)
                .HasMaxLength(200);
            modelBuilder.Entity<Activity>().Property( a => a.CreatedAt)
                .IsRequired();
            modelBuilder.Entity<Activity>().Property( a => a.UpdatedAt)
                .IsRequired();
            modelBuilder.Entity<Activity>().Property( a => a.StartDate)
                .IsRequired();
            modelBuilder.Entity<Activity>().Property( a => a.EndDate)
                .IsRequired();
            // modelBuilder.Entity<Activity>().Property( a => a.Location)
            //     .HasMaxLength(200);
            // modelBuilder.Entity<Activity>().Property( a => a.Type)
            //     .IsRequired()
            //     .HasMaxLength(50);
            modelBuilder.Entity<Activity>().Property(a => a.Status)
                .IsRequired();

            modelBuilder.Entity<Activity>().HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserId);
            
            #endregion
            
            #region Media

            modelBuilder.Entity<Media>().ToTable("Media");
            modelBuilder.Entity<Media>().HasKey(m => m.Id);
            modelBuilder.Entity<Media>().Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Media>().Property(m => m.Url)
                .HasMaxLength(300)
                .IsRequired();
            
            modelBuilder.Entity<Media>().Property(m => m.Caption)
                .HasMaxLength(300)
                .IsRequired(false);

            modelBuilder.Entity<Activity>()
                .HasMany(a => a.Media)
                .WithOne(m => m.Activity)
                .HasForeignKey(m => m.ActivityId)
                .OnDelete(DeleteBehavior.Cascade); 

            #endregion

            #region UserGroup
            
            modelBuilder.Entity<UserGroup>().ToTable("UserGroups");
            
            modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });  
            
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(ug => ug.UserId);
            
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(ug => ug.GroupId);
            #endregion
            
            #region UserToken
            modelBuilder.Entity<UserToken>().ToTable("UserTokens");

            modelBuilder.Entity<UserToken>().HasKey(ut => ut.Id); 

            modelBuilder.Entity<UserToken>().Property(ut => ut.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserToken>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(ut => ut.UserId);
            #endregion
            
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "TestUser",
                    Description = "This is a test user.",
                    Email = "Test@email.com",
                    Password = "Test123" 
                });
        }
    }
}