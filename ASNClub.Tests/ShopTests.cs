using ASNClub.Data;
using ASNClub.Services.AddressServices;
using ASNClub.Services.CategoryServices;
using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.Services.ColorServices;
using ASNClub.Services.ColorServices.Contracts;
using ASNClub.Services.CountyServices;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.Services.ProductServices;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.TypeServices;
using ASNClub.Services.TypeServices.Contracts;
using ASNClub.ViewModels.Category;
using ASNClub.ViewModels.Color;
using ASNClub.ViewModels.Country;
using ASNClub.ViewModels.Discount;
using ASNClub.ViewModels.Product;
using ASNClub.ViewModels.Product.Enums;
using ASNClub.ViewModels.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;


namespace ASNClub.Tests
{
    [TestFixture]
    public class ShopTests
    {
        private DbContextOptions<ASNClubDbContext> dbOptions;
        private ASNClubDbContext dbContext;

        private IProductService productService;
        private IColorService colorService;
        private ICountryService countryService;
        private IMaterialService materialService;
        private ITypeService typeService;
        private IServiceProvider serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ASNClubDbContext>()
                .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ASNClubDbContext(this.dbOptions); // CS1061 might be related to this line

            this.dbContext.Database.EnsureCreated();
            DatabaseSeeder.SeedDatabaseAsync(this.dbContext, serviceProvider);

            this.productService = new ProductService(this.dbContext);
            this.colorService = new ColorService(this.dbContext);
            this.countryService = new CountryService(this.dbContext);
            this.materialService = new MaterialService(this.dbContext);
            this.typeService = new TypeService(this.dbContext);
        }
        [Test]
        public async Task AddProductAsync_Should_Add_Product_And_Related_Entities()
        {
            //Arrange
            var formModel = new ProductFormModel
            {
                // Initialize your formModel properties here
                Make = "Browning",
                Model = "Mk3",
                Price = 320,
                Description = "The best grips on the market",
                TypeId = 1,
                Quantity = 5,
                ColorId = 1,
                MaterialId = 3,
                Discount = new ProductDiscountFormModel()
                {
                    IsDiscount = false,
                },
                ImgUrls = new List<string>()
                {
                    "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL.png"
                }
            };

            // Act
            await productService.AddProductAsync(formModel);

            // Assert
            var savedProduct = await dbContext.Products.FirstOrDefaultAsync();

            Assert.NotNull(savedProduct);
            Assert.NotNull(savedProduct.Discount);
            Assert.NotNull(savedProduct.ImgUrls);
            Assert.That(savedProduct.ImgUrls.Count, Is.EqualTo(formModel.ImgUrls.Count));
            Assert.That("Browning", Is.EqualTo(savedProduct.Make));
            Assert.That("Mk3", Is.EqualTo(savedProduct.Model));
            Assert.That(320, Is.EqualTo(savedProduct.Price));
            Assert.That("The best grips on the market", Is.EqualTo(savedProduct.Description));
            Assert.That(1, Is.EqualTo(savedProduct.TypeId));
            // Add more assertions as needed
        }
        [Test]
        public async Task EditProductAsync_Should_Update_Product_With_Given_FormModel()
        {
            // Arrange
            var formModel = new ProductFormModel
            {
                Id = 1,
                Make = "Edited Make",
                Model = "Edited Model",
                Price = 500,
                Description = "Edited Description",
                TypeId = 2,
                Quantity = 10,
                ColorId = 2,
                MaterialId = 2,
                Discount = new ProductDiscountFormModel()
                {
                    IsDiscount = true,
                    DiscountRate = 10,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7)
                }
            };

            // Act
            await productService.EditProductAsync(formModel);

            // Assert
            var editedProduct = await dbContext.Products.Include(p => p.Discount).FirstOrDefaultAsync(p => p.Id == formModel.Id);

            Assert.NotNull(editedProduct);
            Assert.NotNull(editedProduct.Discount);
            Assert.That("Edited Make", Is.EqualTo(editedProduct.Make));
            Assert.That("Edited Model", Is.EqualTo(editedProduct.Model));
            Assert.That(500, Is.EqualTo(editedProduct.Price));
            Assert.That("Edited Description", Is.EqualTo(editedProduct.Description));
            Assert.That(2, Is.EqualTo(editedProduct.TypeId));
            Assert.That(10, Is.EqualTo(editedProduct.Quantity));
            Assert.That(2, Is.EqualTo(editedProduct.ColorId));
            Assert.That(2, Is.EqualTo(editedProduct.MaterialId));
            Assert.That(true, Is.EqualTo(editedProduct.Discount.IsDiscount));
            Assert.That(10, Is.EqualTo(editedProduct.Discount.DiscountRate));
        }
        [Test]
        public async Task DeleteProductByIdAsync_Should_Delete_Product_From_Database()
        {
            // Arrange
            var productIdToDelete = 1;

            // Act
            await productService.DeleteProductByIdAsync(productIdToDelete);

            // Assert
            var deletedProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productIdToDelete);

            Assert.IsNull(deletedProduct);
        }
        [Test]
        public async Task GetAllProductsAsync_Should_Return_Correctly_Sorted_Products()
        {
            // Arrange
            var queryModel = new AllProductQueryModel
            {
                // Set your query parameters here
                ProductSorting = ProductSorting.PriceAscending,
                CurrentPage = 1,
                ProductsPerPage = 10
            };

            // Act
            var result = await productService.GetAllProductsAsync(queryModel);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Products);
            Assert.That(result.Products, Is.Not.Empty);
            Assert.That(result.TotalProducts, Is.EqualTo(1));

            // Additional assertions based on the sorting logic
            decimal previousPrice = result.Products.First().Price;
            foreach (var product in result.Products.Skip(1))
            {
                Assert.That(product.Price, Is.GreaterThanOrEqualTo(previousPrice));
                previousPrice = product.Price;
            }
        }
        [Test]
        public async Task GetProductDetailsByIdAsync_Should_Return_Product_Details_With_Colors()
        {
            // Arrange
            var productId = 1;

            // Act
            var result = await productService.GetProductDetailsByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Browning", result.Make);
            Assert.AreEqual("Mk3", result.Model);
            Assert.That(result.Price, Is.EqualTo(320));

            if (result.Color != null && result.Color != "None")
            {
                Assert.NotNull(result.Colors);

            }
        }

        [Test]
        public async Task GetProductDetailsByIdAsync_Should_Return_Product_Details_Without_Colors()
        {
            // Arrange
            var productId = 1;


            // Act
            var result = await productService.GetProductDetailsByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Browning", result.Make);
            Assert.AreEqual("Mk3", result.Model);
            Assert.That(result.Price, Is.EqualTo(320));

            if (result.Color == null || result.Color == "None")
            {
                Assert.Null(result.Colors);
            }
        }
        [Test]
        public async Task GetProductForEditByIdAsync_Should_Return_ProductFormModel()
        {
            // Arrange
            var productId = 1;

            // Act
            var productFormModel = await productService.GetProductForEditByIdAsync(productId);

            // Assert
            Assert.NotNull(productFormModel);
            Assert.AreEqual(productId, productFormModel.Id);

            Assert.AreEqual("Browning", productFormModel.Make);
            Assert.AreEqual("Mk3", productFormModel.Model);
            Assert.AreEqual(320, productFormModel.Price);
            Assert.AreEqual("The best grips on the market", productFormModel.Description);

            Assert.NotNull(productFormModel.ImgUrls);
            Assert.That(productFormModel.ImgUrls.Count, Is.EqualTo(1));
            Assert.AreEqual("http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL.png", productFormModel.ImgUrls[0]);

            Assert.NotNull(productFormModel.Discount);
            Assert.IsFalse(productFormModel.Discount.IsDiscount);

        }
        [Test]
        public async Task GetProductByIdAsync_Should_Return_ProductAllViewModel()
        {
            // Arrange
            var productId = 1;

            // Act
            var productViewModel = await productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(productViewModel);
            Assert.AreEqual(productId.ToString(), productViewModel.Id);

            const string expectedMake = "Browning";
            const string expectedModel = "Mk3";
            const decimal expectedPrice = 320;

            Assert.AreEqual(expectedMake, productViewModel.Make);
            Assert.AreEqual(expectedModel, productViewModel.Model);
            Assert.AreEqual(expectedPrice, productViewModel.Price);

            Assert.NotNull(productViewModel.ImgUrl);
            const string expectedImgUrl = "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL.png";
            Assert.AreEqual(expectedImgUrl, productViewModel.ImgUrl);

            Assert.NotNull(productViewModel.Type);

            Assert.AreEqual(false, productViewModel.IsDiscount);
            Assert.Null(productViewModel.DiscountRate);

        }

        [Test]
        public async Task GetProductOfTypeProductByIdAsync_Should_Return_Product_With_Discount()
        {
            // Arrange
            var productId = 1;

            // Act
            var product = await productService.GetProductOfTypeProductByIdAsync(productId);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productId, product.Id);

            Assert.NotNull(product.Discount);
            Assert.AreEqual(false, product.Discount.IsDiscount);
        }
        [Test]
        public async Task AddRatingAsync_Should_Add_Rating_To_Product()
        {
            // Arrange
            var productId = 1;
            var userId = "cbfdce81-6fc9-469d-808b-52306f038d9a";
            var ratingValue = 5;

            // Act
            await productService.AddRatingAsync(productId, ratingValue, userId);

            // Assert
            var product = await productService.GetProductOfTypeProductByIdAsync(productId);
            Assert.NotNull(product);

            var rating = product.Ratings.FirstOrDefault(r => r.UserId.ToString() == userId);
            Assert.NotNull(rating);
            Assert.AreEqual(ratingValue, rating.RatingValue);
        }
        [Test]
        public async Task AddCommentAsync_Should_Add_Comment_To_Product()
        {
            // Arrange
            var productId = 1;
            var username = "some-username";
            var ownerId = Guid.NewGuid().ToString();
            var content = "This is a test comment.";

            // Act
            await productService.AddCommentAsync(productId, username, ownerId, content);

            // Assert
            var product = await productService.GetProductOfTypeProductByIdAsync(productId);
            Assert.NotNull(product);

            var comment = product.Comments.FirstOrDefault(c => c.OwnerId.ToString() == ownerId);
            Assert.NotNull(comment);
            Assert.AreEqual(content, comment.Text);
            Assert.AreEqual(username, comment.OwnerName);
        }
        [Test]
        public async Task GetAllColorsForProductAsync_Should_Return_Colors_For_Product()
        {
            // Arrange
            var make = "Browning";
            var model = "Mk3";
            var typeId = 1;

            // Act
            var colors = await productService.GetAllColorsForProductAsync(make, model, typeId);

            // Assert
            Assert.NotNull(colors);
            Assert.IsTrue(colors.Count > 0);

            foreach (var color in colors)
            {
                Assert.NotNull(color.ColorName);
                Assert.AreEqual(typeId, color.ProductId);
            }
        }
        [Test]
        public async Task AllMakeNamesAsync_Should_Return_All_Make_Names()
        {

            // Act
            var makes = await productService.AllMakeNamesAsync();

            // Assert
            Assert.NotNull(makes);
            Assert.IsTrue(makes.Count() > 0);


        }
        [Test]
        public async Task AllModelNamesAsync_Should_Return_All_Model_Names_For_Given_Make()
        {
            // Arrange
            var expectedMake = "Browning";

            // Act
            var models = await productService.AllModelNamesAsync(expectedMake);

            // Assert
            Assert.NotNull(models);
            Assert.IsTrue(models.Count() > 0);
        }
        // -------------- COLOR SERVICE -------------------
        [Test]
        public async Task AllColorsAsync_Should_Return_All_Colors()
        {

            // Act
            var colors = await colorService.AllColorsAsync();

            // Assert
            Assert.NotNull(colors);
            Assert.IsInstanceOf<IEnumerable<ProductColorFormModel>>(colors);


            var colorCountInDatabase = await dbContext.Colors.CountAsync();
            Assert.AreEqual(colorCountInDatabase, colors.Count());

        }
        // -------------- !COLOR SERVICE! -------------------


        //-------------- COUNTRY SERVICE -------------------
        [Test]
        public async Task GetCountryNamesAsync_Should_Return_All_Countries()
        {

            // Act
            var countries = await countryService.GetCountryNamesAsync();

            // Assert
            Assert.NotNull(countries);
            Assert.IsInstanceOf<List<CountryViewModel>>(countries);


            var countryCountInDatabase = await dbContext.Countries.CountAsync();
            Assert.AreEqual(countryCountInDatabase, countries.Count());
        }
        //-------------- !COUNTRY SERVICE! -------------------


        //-------------- MATERIAL SERVICE -------------------
        [Test]
        public async Task AllCategoriesAsync_Should_Return_All_Categories()
        {
            // Act
            var categories = await materialService.AllCategoriesAsync();

            // Assert
            Assert.NotNull(categories);
            Assert.IsInstanceOf<IEnumerable<ProductMaterialFormModel>>(categories);

            var categoryCountInDatabase = await dbContext.Materials.CountAsync();
            Assert.AreEqual(categoryCountInDatabase, categories.Count());
        }

        [Test]
        public async Task AllCategoryNamesAsync_Should_Return_All_Category_Names()
        {
            // Act
            var categoryNames = await materialService.AllCategoryNamesAsync();

            // Assert
            Assert.NotNull(categoryNames);
            Assert.IsInstanceOf<IEnumerable<string>>(categoryNames);

            var categoryCountInDatabase = await dbContext.Materials.CountAsync();
            Assert.AreEqual(categoryCountInDatabase, categoryNames.Count());
        }
        //-------------- !MATERIAL SERVICE! -------------------


        //-------------- TYPE SERVICE -------------------
        [Test]
        public async Task AllTypeNamesAsync_Should_Return_All_Type_Names()
        {
            // Act
            var typeNames = await typeService.AllTypeNamesAsync();

            // Assert
            Assert.NotNull(typeNames);
            Assert.IsInstanceOf<IEnumerable<string>>(typeNames);

            var typeCountInDatabase = await dbContext.Types.CountAsync();
            Assert.AreEqual(typeCountInDatabase, typeNames.Count());
        }

        [Test]
        public async Task AllTypesAsync_Should_Return_All_Types()
        {
            // Act
            var types = await typeService.AllTypesAsync();

            // Assert
            Assert.NotNull(types);
            Assert.IsInstanceOf<IEnumerable<ProductTypeFormModel>>(types);

            var typeCountInDatabase = await dbContext.Types.CountAsync();
            Assert.AreEqual(typeCountInDatabase, types.Count());
        }
        //-------------- !TYPE SERVICE! -------------------
    }
}
