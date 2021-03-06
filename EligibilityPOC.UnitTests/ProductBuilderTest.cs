﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EligibilityPOC.Domain.Concrete;
using Moq;
using EligibilityPOC.Domain.Abstract;
using EligibilityPOC.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using EligibilityPOC.Domain.Entities.EligibilityEntities;

namespace EligibilityPOC.UnitTests {
    [TestClass]
    public class ProductBuilderTest {
        [TestMethod]
        public void Cannot_Build_Product_With_No_Product_Data() {
            // Arrange
            ProductBuilder target = new ProductBuilderBuilder().Build();

            // Act
            Product result = target.Build();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throw_When_Product_Id_Changed_Before_Product_Built() {
            // Arrange
            ProductBuilder target = new ProductBuilderBuilder().Build();
            target.BuildProductData(1);
            target.BuildProductData(19);
        }

        [TestMethod]
        public void Can_Reuse_Builder_After_Product_Built() {
            // Arrange
            int productId = 1;
            int productIdNext = 2;
            ProductBuilder target = new ProductBuilderBuilder().WithProductData().WithNoEligibity(productId).WithNoEligibity(productIdNext).Build();     // No eligibility to make the test simpler.
            target.BuildProductData(productId).Build();

            // Act
            Product result = target.BuildProductData(productIdNext).Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ProductData.Id == 2);
        }

        [TestMethod]
        public void Can_Build_Product_With_No_Eligibility() {
            // Arrange
            int productId = 1;
            ProductBuilder target = new ProductBuilderBuilder().WithProductData().WithNoEligibity(productId).Build();

            // Act
            Product result = target.BuildProductData(productId).BuildEligibility().Build();

            // Assert - result in general
            Assert.IsNotNull(result);
            // Assert - product data exists and contains correct data.
            Assert.IsNotNull(result.ProductData);
            Assert.IsTrue(result.ProductData.Id == 1);
            // Assert - lack of eligibility is signalled through Null Eligibility object.
            Assert.IsInstanceOfType(result.Eligibility, typeof(NullEligibility));
        }
        
        [TestMethod]
        public void Can_Build_Product_With_Eligibility() {
            // Arrange
            int productId = 1;
            ProductBuilder target = new ProductBuilderBuilder().WithProductData().WithRuleSetEligibities(productId).Build();

            // Act
            Product result = target.BuildProductData(productId).BuildEligibility().Build();

            // Assert - result in general
            Assert.IsNotNull(result);
            // Assert - product data exists and contains correct data.
            Assert.IsNotNull(result.ProductData);
            Assert.IsTrue(result.ProductData.Id == 1);
            // Assert - eligibility
            Assert.IsNotNull(result.Eligibility);
            Assert.IsTrue(result.Eligibility.RuleSet == 1);
        }
    }

    public class ProductBuilderBuilder {
        Mock<IEligibilityFactory> _mockEligFactory;
        Mock<IProductDataRepository> _mockProductDataRepo;
        ProductBuilder _productBuilder;
        
        public ProductBuilderBuilder() {
            _mockEligFactory = new Mock<IEligibilityFactory>();
            _mockProductDataRepo = new Mock<IProductDataRepository>();
            _productBuilder = new ProductBuilder(_mockEligFactory.Object, _mockProductDataRepo.Object);
        }

        public ProductBuilderBuilder WithProductData() {
            _mockProductDataRepo.Setup(m => m.ProductDatas).Returns(
                new List<ProductData> {
                    new ProductData {
                        Id = 1,
                        Name = "Product 1",
                        Type = 1
                    },
                    new ProductData {
                        Id = 2,
                        Name = "Product 2",
                        Type = 1
                    },
                    new ProductData {
                        Id = 3,
                        Name = "Product 3",
                        Type = 2
                    }
                }.AsQueryable<ProductData>());
            return this;
        }

        public ProductBuilderBuilder WithNoEligibity(int productId) {
            _mockEligFactory.Setup(m => m.Create(productId)).Returns(
                    new NullEligibility()
                );
            return this;
        }

        public ProductBuilderBuilder WithRuleSetEligibities(int productId) {
            _mockEligFactory.Setup(m => m.Create(productId)).Returns(
                new RuleSet1Eligibility {
                    RuleSet = 1,
                    Components = new List<IEligibility> {
                        new FormSubtypeEligibility {
                            RuleSet = 1,
                            ValidSubtypes = "1,5,7"
                        },
                        new SubscriberTypeEligibility {
                            RuleSet = 1,
                            SubscriberType = "1,2,3"
                        },
                        new TariffsEligibility {
                            RuleSet = 1,
                            AllowedTariffs = "14,27,61"
                        },
                        new RuleSetOtherEligibility {
                            RuleSet = 2,
                            Components = new List<IEligibility> {
                                new FormSubtypeEligibility {
                                    RuleSet = 2,
                                    ValidSubtypes = "1"
                                },
                                new SubscriberTypeEligibility {
                                    RuleSet = 2,
                                    SubscriberType = "2"
                                },
                                new RuleSetOtherEligibility {
                                    RuleSet = 3,
                                    Components = new List<IEligibility> {
                                        new SubscriberTypeEligibility {
                                            RuleSet = 3,
                                            SubscriberType = "3"
                                        },
                                        new TariffsEligibility {
                                            RuleSet = 3,
                                            AllowedTariffs = "61"
                                        },
                                        new RuleSetOtherEligibility {
                                            RuleSet = 4,
                                            Components = new List<IEligibility> {
                                                new SubscriberTypeEligibility {
                                                    RuleSet = 4,
                                                    SubscriberType = "1"
                                                },
                                                new TariffsEligibility {
                                                    RuleSet = 4,
                                                    AllowedTariffs = "14"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            return this;
        }

        public ProductBuilder Build() {
            return _productBuilder;
        }
    }
}
