using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Practice.Domain.Entities;
using Microsoft.AspNet.Identity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Practice.Domain.Concrete
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        //"TechStore" .-> Web.config中的設定
        public AppIdentityDbContext() : base("TechStore") { }

        static AppIdentityDbContext()
        {
            //設定當被第一次創建時則用IdentityDbInit初始化資料庫(seed)
            //Entity Framework Code First
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public static AppIdentityDbContext Create()
        {
            //當OWIN需要實體時則用此函式
            return new AppIdentityDbContext();
        }
    }

    public class IdentityDbInit : NullDatabaseInitializer<AppIdentityDbContext>
    { }

    //public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    //{
    //    protected override void Seed(AppIdentityDbContext context)
    //    {
    //        PerformInitialSetup(context);
    //        PerformInitialProductSetup(context);
    //        base.Seed(context);
    //    }

    //    public void PerformInitialSetup(AppIdentityDbContext context)
    //    {
    //        //因函式比OWIN要早執行，所以要各創建個實體來操作
    //        AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
    //        AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

    //        string roleName = "Administrators";
    //        string userName = "admin";
    //        string password = "Zzxc1123";
    //        string email = "admin@example.com";

    //        if (!roleMgr.RoleExists(roleName))
    //        {
    //            roleMgr.Create(new AppRole(roleName));
    //        }

    //        AppUser user = userMgr.FindByName(userName);
    //        if (user == null)
    //        {
    //            userMgr.Create(new AppUser { UserName = userName, Email = email }, password);
    //            user = userMgr.FindByName(userName);
    //        }
    //        if (!userMgr.IsInRole(user.Id, roleName))
    //        {
    //            userMgr.AddToRole(user.Id, roleName);
    //        }
    //    }

    //    public void PerformInitialProductSetup(AppIdentityDbContext context)
    //    {
    //        var brands = new List<Brand>
    //        {
    //            new Brand { Name = "SanDisk"},
    //            new Brand { Name = "Micron"},
    //            new Brand { Name = "Samsung"},
    //            new Brand { Name = "Seagate"},
    //            new Brand { Name = "WD"},
    //            new Brand { Name = "MSI"},
    //            new Brand { Name = "ASUS"},
    //            new Brand { Name = "GIGABYTE"},
    //            new Brand { Name = "EVGA"}
    //        };
    //        brands.ForEach(s => context.Brands.Add(s));
    //        context.SaveChanges();

    //        var categories = new List<Category>
    //        {
    //            new Category { Name = "Storage" },
    //            new Category { Name = "VideoCard" }
    //        };
    //        categories.ForEach(s => context.Categories.Add(s));
    //        context.SaveChanges();

    //        var products = new List<Product>
    //        {
    //            new Product {Name = "SanDisk Ultra 3D 1TB SSD", BrandID = 1, Description = "Desc", CategoryID = 1, Price = 2988},
    //            new Product {Name = "SanDisk Ultra 3D 512GB SSD", BrandID = 1, Description = "Desc", CategoryID = 1, Price = 2099},
    //            new Product {Name = "SanDisk Ultra 3D 2TB SSD", BrandID = 1, Description = "Desc", CategoryID = 1, Price = 6899},
    //            new Product {Name = "Micron MX500 1TB SSD", BrandID = 2, Description = "Desc", CategoryID = 1, Price = 2988},
    //            new Product {Name = "Micron MX550 512GB SSD", BrandID = 2, Description = "Desc", CategoryID = 1, Price = 1988},
    //            new Product {Name = "Micron MX550 2TB SSD", BrandID = 2, Description = "Desc", CategoryID = 1, Price = 6988},
    //            new Product {Name = "Samsung 860 QVO 1TB SSD", BrandID = 3, Description = "Desc", CategoryID = 1, Price = 2999},
    //            new Product {Name = "Samsung 960 OVO 1TB SSD", BrandID = 3, Description = "Desc", CategoryID = 1, Price = 3499},
    //            new Product {Name = "Samsung 660 QVO 512GB SSD", BrandID = 3, Description = "Desc", CategoryID = 1, Price = 1999},
    //            new Product {Name = "Seagate IronWolf 12TB 3.5", BrandID = 4, Description = "Desc", CategoryID = 1, Price = 11450},
    //            new Product {Name = "Seagate IronWolf 4TB 3.5", BrandID = 4, Description = "Desc", CategoryID = 1, Price = 3750},
    //            new Product {Name = "Seagate IronWolf 2TB 3.5", BrandID = 4, Description = "Desc", CategoryID = 1, Price = 2490},
    //            new Product {Name = "Seagate IronWolf 3TB 3.5", BrandID = 4, Description = "Desc", CategoryID = 1, Price = 3190},
    //            new Product {Name = "Seagate IronWolf 4TB 3.5", BrandID = 4, Description = "Desc", CategoryID = 1, Price = 5190},
    //            new Product {Name = "WD 紅標 2TB 3.5 NAS HDD", BrandID = 5, Description = "Desc", CategoryID = 1, Price = 2190},
    //            new Product {Name = "WD 紅標 4TB 3.5 NAS HDD", BrandID = 5, Description = "Desc", CategoryID = 1, Price = 3890},
    //            new Product {Name = "WD 紅標 1TB 3.5 NAS HDD", BrandID = 5, Description = "Desc", CategoryID = 1, Price = 2090},
    //            new Product {Name = "WD 紅標 6TB 3.5 NAS HDD", BrandID = 5, Description = "Desc", CategoryID = 1, Price = 5990},
    //            new Product {Name = "MSI GeForece RTX 2080 SUPER", BrandID = 6, Description = "Desc", CategoryID = 2, Price = 20900},
    //            new Product {Name = "MSI GeForece RTX 2070 SUPER", BrandID = 6, Description = "Desc", CategoryID = 2, Price = 13990},
    //            new Product {Name = "MSI GeForece RTX 2060 SUPER", BrandID = 6, Description = "Desc", CategoryID = 2, Price = 11490},
    //            new Product {Name = "ASUS ROG Strix GeForece RTX 2080 SUPER", BrandID = 7, Description = "Desc", CategoryID = 2, Price = 22990},
    //            new Product {Name = "ASUS ROG Strix GeForece RTX 2070 SUPER", BrandID = 7, Description = "Desc", CategoryID = 2, Price = 16990},
    //            new Product {Name = "ASUS ROG Strix GeForece RTX 2060 SUPER", BrandID = 7, Description = "Desc", CategoryID = 2, Price = 13490},
    //            new Product {Name = "GIGABYTE GeForece RTX 2080 SUPER", BrandID = 8, Description = "Desc", CategoryID = 2, Price = 23090},
    //            new Product {Name = "GIGABYTE GeForece RTX 2070 SUPER", BrandID = 8, Description = "Desc", CategoryID = 2, Price = 15990},
    //            new Product {Name = "GIGABYTE GeForece RTX 2060 SUPER", BrandID = 8, Description = "Desc", CategoryID = 2, Price = 11690},
    //            new Product {Name = "EVGA GeForece RTX 2080 SUPER", BrandID = 9, Description = "Desc", CategoryID = 2, Price = 22490},
    //            new Product {Name = "EVGA GeForece RTX 2070 SUPER", BrandID = 9, Description = "Desc", CategoryID = 2, Price = 15990},
    //            new Product {Name = "EVGA GeForece RTX 2060 SUPER", BrandID = 9, Description = "Desc", CategoryID = 2, Price = 12990}
    //        };
    //        products.ForEach(s => context.Products.Add(s));
    //        context.SaveChanges();
    //    }
    //}
}
