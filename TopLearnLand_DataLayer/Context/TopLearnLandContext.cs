using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopLearnLand_DataLayer.Entities.Course;
using TopLearnLand_DataLayer.Entities.Order;
using TopLearnLand_DataLayer.Entities.Permissions;
using TopLearnLand_DataLayer.Entities.User;
using TopLearnLand_DataLayer.Entities.Wallet;

namespace TopLearnLand_DataLayer.Context
{
    public class TopLearnLandContext : DbContext
    {
        public TopLearnLandContext(DbContextOptions<TopLearnLandContext> options)
            : base(options)
        {

        }

        #region User

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }
            
        #endregion

        #region Wallet

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletType> WalletTypes { get; set; }

        #endregion

        #region Permission

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region Course

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<CourseComments> CourseComments { get; set; }

        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<DisCount> DisCounts { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>()
                .HasQueryFilter(user => !user.IsDeleted);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(role => !role.IsDeleted);

            modelBuilder.Entity<CourseGroup>()
                .HasQueryFilter(courseGroup => !courseGroup.IsDelete);

            modelBuilder.Entity<CourseEpisode>()
                .HasQueryFilter(episode => !episode.IsDelete);

            modelBuilder.Entity<CourseComments>()
                .HasQueryFilter(comment => !comment.IsDelete);

            base.OnModelCreating(modelBuilder);
        }
    }
}
