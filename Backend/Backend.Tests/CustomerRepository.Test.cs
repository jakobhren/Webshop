using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using AdminSystem.Model.Entities;
using AdminSystem.Model.Repositories;
using System;
using System.Collections.Generic;

namespace AdminSystem.Tests.Repositories
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private CustomerRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "ConnectionStrings:DefaultConnection", "Host=localhost;Username=test;Password=test;Database=testdb" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _repository = new CustomerRepository(configuration);
        }

        [TestMethod]
        public void InsertCustomer_ValidCustomer_ReturnsCustomerId()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Password = "securepassword"
            };

            // Act
            var result = _repository.InsertCustomer(customer);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void UpdateCustomer_ValidCustomer_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = 1,
                Name = "Updated Name",
                Email = "updated.email@example.com",
                Password = "updatedpassword"
            };

            // Act
            var result = _repository.UpdateCustomer(customer);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteCustomer_ValidId_ReturnsTrue()
        {
            // Arrange
            int customerId = 1;

            // Act
            var result = _repository.DeleteCustomer(customerId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetCustomerById_ValidId_ReturnsCustomer()
        {
            // Arrange
            int customerId = 1;

            // Act
            var customer = _repository.GetCustomerById(customerId);

            // Assert
            Assert.IsNotNull(customer);
            Assert.AreEqual(customerId, customer.CustomerId);
        }

        [TestMethod]
        public void GetCustomerByEmail_ValidEmail_ReturnsCustomer()
        {
            // Arrange
            string email = "jane.doe@example.com";

            // Act
            var customer = _repository.GetCustomerByEmail(email);

            // Assert
            Assert.IsNotNull(customer);
            Assert.AreEqual(email, customer.Email);
        }
    }
}
