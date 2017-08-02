using System;
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
        ProductBuilderBuilder _targetBuilder;

        [TestInitialize]
        public void TestInitialize() {
            _targetBuilder = new ProductBuilderBuilder();
        }

        [TestMethod]
        public void Cannot_Build_Product_With_No_Product_Data() {
            // Arrange
            ProductBuilder target = _targetBuilder.Build();

            // Act
            Product result = target.BuildProductData(9).Build();           // Use a product Id that doesn't exist in the mocked repos.

            // Assert
            Assert.IsNull(result);
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

        public ProductBuilderBuilder WithEligibities(int productId) {
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
