using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PluralsightDemo
{
    public class PluralsightUserDbContext : IdentityDbContext<PluralsightUser>
    {
       // public DbSet<pendingstudent> UserLogt { get; set; }
        public PluralsightUserDbContext(DbContextOptions<PluralsightUserDbContext> options) : base(options)
        {
            
        }
        //public DbSet<pendingobject> PendingTable { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<PluralsightUser>().HasMany(x => x.Notities).WithOne(y => y.User).IsRequired();


            //builder.Entity<Notitie>(notitie =>
            //{
            //    notitie.ToTable("Notities");
            //    notitie.HasKey(x => x.Id);

            //    notitie.HasOne<PluralsightUser>().WithMany().HasPrincipalKey(x => x.NotititieId).IsRequired(false);


            //});




            //builder.Entity<pendingobject>(pending => {

            //    pending.ToTable("Pendingobjects");
            //    pending.HasKey(x=>x.ID);
            //});
            //builder.Entity<pendingobject>(user => user.HasIndex(x => x.anothervairalbe).IsUnique(false));
            //builder.Entity<pendingobject>(user => user.HasIndex(x => x.weerietsanders).IsUnique(false));
            //builder.Entity<PluralsightUser>(user => user.HasIndex(x => x.Locale).IsUnique(false));

            //builder.Entity<Organization>(org =>
            //{
            //    org.ToTable("Organizations");
            //    org.HasKey(x => x.Id);

            //    org.HasMany<PluralsightUser>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
            //});
        }
    }
}