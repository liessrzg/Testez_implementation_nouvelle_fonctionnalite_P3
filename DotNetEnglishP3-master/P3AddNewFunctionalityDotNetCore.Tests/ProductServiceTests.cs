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
                product.Name = "Test Product";
                product.Stock = 1;
                product.Price = null;
                Assert.False(ValidateModel(product));
                Assert.Equal("MissingPrice", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestMissingName()
            {
                product.Name = null;
                product.Price = 10.0;
                product.Stock = 1;
                Assert.False(ValidateModel(product));
                Assert.Equal("MissingName", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotANumber()
            {
                product.Name = "Test Product";
                product.Price = double.NaN;// Not a Number
                product.Stock = 1;
                Assert.False(ValidateModel(product));
                Assert.Equal("PriceNotANumber", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotGreaterThanZero()
            {
                product.Name = "Test Product";
                product.Price = 0.0;
                product.Stock = 1;
                Assert.False(ValidateModel(product));
                Assert.Equal("PriceNotGreaterThanZero", GetFirstErrorMessage(product));
            }



            [Fact]
            public void TestQuantityNotGreaterThanZero()
            {
                product.Name = "Test Product";
                product.Price = 10.0;
                product.Stock = 0;
                Assert.False(ValidateModel(product));
                Assert.Equal("QuantityNotGreaterThanZero", GetFirstErrorMessage(product));
            }



            // Ajoutez ici d'autres méthodes pour tester les autres cas...

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
