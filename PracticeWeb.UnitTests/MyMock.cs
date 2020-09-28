using Practice.Domain.Abstract;
using Practice.Domain.Concrete;
using Practice.Domain.Entities;
using Microsoft.AspNet.Identity;
using Moq;

namespace PracticeWeb.UnitTests
{
    public class MyMock
    {
        private Mock<IProductRepository> productRepository;
        public Mock<IProductRepository> ProductRepository
        {
            get
            {
                if (productRepository == null)
                    productRepository = GetMockProductRepository();
                return productRepository;
            }
        }

        private Mock<IOrderProcessor> orderProcessor;
        public Mock<IOrderProcessor> OrderProcessor
        {
            get
            {
                if (orderProcessor == null)
                    orderProcessor = GetMockOrderProcessor();
                return orderProcessor;
            }
        }

        private Mock<IOrderLogger> orderLogger;
        public Mock<IOrderLogger> OrderLogger
        {
            get
            {
                if (orderLogger == null)
                    orderLogger = GetMockOrderLogger();
                return orderLogger;
            }
        }

        private Mock<IUserManager> userManager;
        public Mock<IUserManager> UserManager
        {
            get
            {
                if (userManager == null)
                    userManager = GetMockUserManager();
                return userManager;
            }
        }

        private AppUser appUser;
        public AppUser AppUser
        {
            get
            {
                if (appUser == null)
                    appUser = GetUser();
                return appUser;
            }
        }

        public MyMock()
        {}

        private void SetInitialValueOfPR(Mock<IProductRepository> mock)
        {
            mock.Setup(m => m.Cities).Returns(new City[] {
                new City{ID = 1, Name = "City1"},
                new City{ID = 2, Name = "City2"},
                new City{ID = 3, Name = "City3"},
                new City{ID = 4, Name = "City4"}
            });
            mock.Setup(m => m.Countries).Returns(new Country[] {
                new Country{ID = 1, Name = "Country1", CityID = 1},
                new Country{ID = 2, Name = "Country2", CityID = 2},
                new Country{ID = 3, Name = "Country3", CityID = 2},
                new Country{ID = 4, Name = "Country4", CityID = 3}
            });
            mock.Setup(m => m.Brands).Returns(new Brand[] {
                new Brand{ID = 1, Name = "Brand1"},
                new Brand{ID = 2, Name = "Brand2"},
                new Brand{ID = 3, Name = "Brand3"},
                new Brand{ID = 4, Name = "Brand4"}
            });
            mock.Setup(m => m.Categories).Returns(new Category[] {
                new Category{ID = 1, Name = "Category1"},
                new Category{ID = 2, Name = "Category2"},
                new Category{ID = 3, Name = "Category3"},
                new Category{ID = 4, Name = "Category4"}
            });
        }

        private Mock<IProductRepository> GetMockProductRepository()
        {
            var mock = new Mock<IProductRepository>();
            SetInitialValueOfPR(mock);
            return mock;
        }

        private Mock<IOrderProcessor> GetMockOrderProcessor()
        {
            var mock = new Mock<IOrderProcessor>();

            return mock;
        }

        private Mock<IOrderLogger> GetMockOrderLogger()
        {
            var mock = new Mock<IOrderLogger>();

            return mock;
        }

        private Mock<IUserManager> GetMockUserManager()
        {
            var mock = new Mock<IUserManager>();
            mock.Setup(u => u.CurrentUser).Returns(AppUser);
            mock.Setup(u => u.UserManager).Returns(GetUserManager());
            return mock;
        }

        private AppUser GetUser()
        {
            var hpw = new PasswordHasher().HashPassword("testpassword");
            var user = new AppUser()
            {
                Id = "1",
                UserName = "testuser",
                PasswordHash = hpw,
                Email = "test@example.com",
                PhoneNumber = "0987654321",
                CityID = 1,
                CountryID = 1,
                ShippingAddress = "111號"
            };
            return user;
        }

        private AppUserManager GetUserManager()
        {
            var testUser = AppUser;
            var us = new Mock<IUserStore<AppUser>>(MockBehavior.Strict);
            us.Setup(p => p.FindByNameAsync("testuser")).ReturnsAsync(testUser);
            us.As<IUserPasswordStore<AppUser>>()
                .Setup(p => p.GetPasswordHashAsync(It.IsAny<AppUser>())).ReturnsAsync(testUser.PasswordHash);
            return new AppUserManager(us.Object);
        }
    }
}
