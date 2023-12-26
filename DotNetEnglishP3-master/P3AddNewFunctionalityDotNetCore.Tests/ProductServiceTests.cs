using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        public class ProductViewModelValidationTest
        {
            private ProductViewModel product;

            public ProductViewModelValidationTest()
            {
                product = new ProductViewModel();
            }

            [Fact]
            public void TestMissingPrice()
            {
                // Arrange
                product.Name = "Test Product";
                product.Stock = "1";
                product.Price = null;

                // Act
                bool validationResult = ValidateModel(product);

                // Assert
                Assert.False(validationResult);
                Assert.Equal("MissingPrice", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestMissingName()
            {
                // Arrange
                product.Name = null;
                product.Price = "10.0";
                product.Stock = "1";

                // Act
                bool validationResult = ValidateModel(product);

                // Assert
                Assert.False(validationResult);
                Assert.Equal("MissingName", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestMissingStock()
            {
                // Arrange
                product.Name = "chaise";
                product.Stock = null;
                product.Price = "3";

                // Act
                bool validationResult = ValidateModel(product);

                // Assert
                Assert.False(validationResult);
                Assert.Equal("MissingStock", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotANumber()
            {
                // Arrange
                product.Name = "Test Product";
                product.Price = "j"; // Not a Number
                product.Stock = "3";

                // Act
                bool validationResult = ValidateModel(product);

                // Assert
                Assert.False(validationResult);
                Assert.Equal("PriceNotANumber", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotGreaterThanZero()
            {
                // Arrange
                product.Name = "Test Product";
                product.Price = "0.0";
                product.Stock = "1";

                // Act
                bool validationResult = ValidateModel(product);

                // Assert
                Assert.False(validationResult);
                Assert.Equal("PriceNotGreaterThanZero", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestQuantityNotGreaterThanZero()
            {
                // Arrange
                product.Name = "Test Product";
                product.Price = "10.0";
                product.Stock = "0";

                // Act
                bool validationResult = ValidateModel(product);

                // Assert
                Assert.False(validationResult);
                Assert.Equal("QuantityNotGreaterThanZero", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestQuantityNotAnInteger()
            {
                // Arrange
                product.Name = "Test Quantity";
                product.Price = "10";
                product.Stock = "3.3";

                // Act
                bool validationResult = ValidateModel(product);

                // Assert
                Assert.False(validationResult);
                Assert.Equal("StockNotAnInteger", GetFirstErrorMessage(product));
            }

            private bool ValidateModel(object model)
            {
                var validationResults = new List<ValidationResult>();
                var ctx = new ValidationContext(model, null, null);
                return Validator.TryValidateObject(model, ctx, validationResults, true);
            }

            private string GetFirstErrorMessage(object model)
            {
                var validationResults = new List<ValidationResult>();
                var ctx = new ValidationContext(model, null, null);
                Validator.TryValidateObject(model, ctx, validationResults, true);
                return validationResults[0].ErrorMessage;
            }
        }

        // TODO écrire des méthodes de test pour assurer une couverture correcte de toutes les possibilités
    }
}
