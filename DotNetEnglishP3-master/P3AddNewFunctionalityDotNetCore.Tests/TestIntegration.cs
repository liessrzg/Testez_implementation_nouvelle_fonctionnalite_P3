﻿// ... (importations)

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class IntegrationTests
    {
        // Arrange
        private readonly IConfiguration _configuration; // Assurez-vous de fournir une instance valide lors de l'injection
        private readonly IStringLocalizer<ProductService> _localizer; // Assurez-vous de fournir une instance valide lors de l'injection

        [Fact]
        public async Task SaveNewProduct()
        {
            // Arrange        
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer("Server=localhost;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            using (var ctx = new P3Referential(options, _configuration))
            {
                // Injectez les dépendances nécessaires
                LanguageService languageService = new();
                Cart cart = new();
                ProductRepository productRepository = new(ctx);
                OrderRepository orderRepository = new(ctx);
                ProductService productService = new(cart, productRepository, orderRepository, _localizer);
                ProductController productController = new(productService, languageService);

                ProductViewModel productViewModel = new() { Name = "Product from CREATE integration test", Description = "Description", Details = "Detail", Stock = 1, Price = 150 };

                int count = await ctx.Product.CountAsync();

                // Act
                productController.Create(productViewModel);

                // Assert
                // Le nombre de produits a augmenté de 1
                Assert.Equal(count + 1, ctx.Product.Count());

                // Le produit peut être trouvé
                var product = await ctx.Product.Where(x => x.Name == "Product from CREATE integration test").FirstOrDefaultAsync();
                Assert.NotNull(product);

                // Nettoyer la BASE DE DONNÉES   
                ctx.Product.Remove(product);
                await ctx.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task DeleteProduct()
        {
            // Arrange        
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer("Server=localhost;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            using (var ctx = new P3Referential(options, _configuration))
            {
                // Injectez les dépendances nécessaires
                LanguageService languageService = new();
                Cart cart = new();
                ProductRepository productRepository = new(ctx);
                OrderRepository orderRepository = new(ctx);
                ProductService productService = new(cart, productRepository, orderRepository, _localizer);
                ProductController productController = new(productService, languageService);

                ProductViewModel productViewModel = new() { Name = "Product from DELETE integration test", Description = "Description", Details = "Detail", Stock = 1, Price = 150 };

                int count = await ctx.Product.CountAsync();

                productController.Create(productViewModel);

                var product = await ctx.Product.Where(x => x.Name == "Product from DELETE integration test").FirstOrDefaultAsync();

                // Act
                productController.DeleteProduct(product.Id);

                // Assert
                Assert.Equal(count, ctx.Product.Count());

                var searchProductAgain = await ctx.Product.Where(x => x.Name == "Product from DELETE integration test").FirstOrDefaultAsync();
                Assert.Null(searchProductAgain);
            }
        }
    }
}
