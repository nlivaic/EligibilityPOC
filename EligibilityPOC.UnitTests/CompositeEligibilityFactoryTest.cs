using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EligibilityPOC.Domain.Abstract;
using Moq;
using EligibilityPOC.Domain.Concrete;
using EligibilityPOC.Domain.Entities;
using System.Linq;
using EligibilityPOC.Domain.Entities.EligibilityEntities;

namespace EligibilityPOC.UnitTests {
    [TestClass]
    public class CompositeEligibilityFactoryTest {
        private CompositeEligibilityFactoryBuilder _targetBuilder;

        [TestInitialize]
        public void TestInitialize() {
            _targetBuilder = new CompositeEligibilityFactoryBuilder();
        }

        [TestMethod]
        public void Can_Create_A_Single_Rule_Set_With_Single_Eligible_Object() {
            // Arrange
            int productId = 1;
            _targetBuilder.WithSingleRuleSetWithSingleEligibility(productId);
            CompositeEligibilityFactory target = _targetBuilder.Build();

            // Act
            IEligible result = target.Create(productId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(FormSubtypeEligibility));
        }

        [TestMethod]
        public void Can_Create_A_Single_Rule_Set_With_Single_Composite_Eligible_Object() {
            Assert.Fail();
        }

        [TestMethod]
        public void Can_Create_A_Multi_Rule_Set_Composite_Eligible_Object() {
            Assert.Fail();
        }
    }

    public class CompositeEligibilityFactoryBuilder {
        private Mock<IProductEligibilityParamRepository> _mockRepo;
        private CompositeEligibilityFactory _eligibilityFactory;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        public CompositeEligibilityFactoryBuilder() {
            _mockRepo = new Mock<IProductEligibilityParamRepository>();
            _eligibilityFactory = new CompositeEligibilityFactory(_mockRepo.Object);
        }

        public CompositeEligibilityFactoryBuilder WithSingleRuleSetWithSingleEligibility(int productId) {
            _mockRepo.Setup(m => m.GetProductEligibilityParams(productId)).Returns(
                new ProductEligibilityParam[] {
                    new ProductEligibilityParam {
                        Id = 1,
                        ProductId = productId,
                        EligibilityName = "FormSubtypeEligibility",
                        ParamName = "ValidSubtypes",
                        ParamValue = "1,5,7",
                        RuleSet = 1
                    }
                }.AsQueryable<ProductEligibilityParam>());
            return this;
        }

        public CompositeEligibilityFactoryBuilder WithSingleRuleSetWithMultipleEligibility(int productId) {
            _mockRepo.Setup(m => m.GetProductEligibilityParams(productId)).Returns(
                new ProductEligibilityParam[] {
                    new ProductEligibilityParam {
                        Id = 1,
                        ProductId = productId,
                        EligibilityName = "FormSubtypeEligibility",
                        ParamName = "ValidSubtypes",
                        ParamValue = "1,5,7",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 2,
                        ProductId = productId,
                        EligibilityName = "SubscriberTypeEligibility",
                        ParamName = "SubscriberType",
                        ParamValue = "1,2,3",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 3,
                        ProductId = productId,
                        EligibilityName = "TariffsEligibility",
                        ParamName = "AllowedTariffs",
                        ParamValue = "14,27,61",
                        RuleSet = 1
                    }
                }.AsQueryable<ProductEligibilityParam>());
            return this;
        }

        public CompositeEligibilityFactoryBuilder WithMultipleRuleSetWithMultipleEligibility(int productId) {
            _mockRepo.Setup(m => m.GetProductEligibilityParams(productId)).Returns(
                new ProductEligibilityParam[] {
                    new ProductEligibilityParam {
                        Id = 1,
                        ProductId = productId,
                        EligibilityName = "FormSubtypeEligibility",
                        ParamName = "ValidSubtypes",
                        ParamValue = "1,5,7",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 2,
                        ProductId = productId,
                        EligibilityName = "SubscriberTypeEligibility",
                        ParamName = "SubscriberType",
                        ParamValue = "1,2,3",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 3,
                        ProductId = productId,
                        EligibilityName = "TariffsEligibility",
                        ParamName = "AllowedTariffs",
                        ParamValue = "14,27,61",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 4,
                        ProductId = productId,
                        EligibilityName = "FormSubtypeEligibility",
                        ParamName = "ValidSubtypes",
                        ParamValue = "1",
                        RuleSet = 2
                    },
                    new ProductEligibilityParam {
                        Id = 5,
                        ProductId = productId,
                        EligibilityName = "SubscriberTypeEligibility",
                        ParamName = "SubscriberType",
                        ParamValue = "2",
                        RuleSet = 2
                    },
                    new ProductEligibilityParam {
                        Id = 6,
                        ProductId = productId,
                        EligibilityName = "SubscriberTypeEligibility",
                        ParamName = "SubscriberType",
                        ParamValue = "3",
                        RuleSet = 3
                    },
                    new ProductEligibilityParam {
                        Id = 7,
                        ProductId = productId,
                        EligibilityName = "TariffsEligibility",
                        ParamName = "AllowedTariffs",
                        ParamValue = "61",
                        RuleSet = 3
                    }
                }.AsQueryable<ProductEligibilityParam>());
            return this;
        }

        public CompositeEligibilityFactory Build() {
            return _eligibilityFactory;
        }

    }
}
