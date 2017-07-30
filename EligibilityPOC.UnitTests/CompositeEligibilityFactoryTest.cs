using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EligibilityPOC.Domain.Abstract;
using Moq;
using EligibilityPOC.Domain.Concrete;
using EligibilityPOC.Domain.Entities;
using System.Linq;
using EligibilityPOC.Domain.Entities.EligibilityEntities;
using System.Collections.Generic;
using System.Collections;

namespace EligibilityPOC.UnitTests {
    [TestClass]
    public class CompositeEligibilityFactoryTest {
        private CompositeEligibilityFactoryBuilder _targetBuilder;

        [TestInitialize]
        public void TestInitialize() {
            _targetBuilder = new CompositeEligibilityFactoryBuilder();
        }

        [TestMethod]
        public void Return_Null_Object_If_No_Eligibilities() {
            // Arrange
            _targetBuilder.WithNoEligibility();
            CompositeEligibilityFactory target = _targetBuilder.Build();

            // Act
            IEligibility result = target.Create(1);
            IEligibility nullResult = result as NullEligibility;

            // Assert
            Assert.IsInstanceOfType(nullResult, typeof(NullEligibility));
            _targetBuilder._mockMapper.Verify(m => m.MapParamsToEligibility(It.IsAny<IList<ProductEligibilityParam>>()), Times.Once);
        }

        [TestMethod]
        public void Can_Create_A_Composite_With_Single_Rule_Set_With_Single_Eligibility() {
            // Arrange
            int productId = 1;
            _targetBuilder.WithSingleRuleSetWithSingleEligibility(productId);
            CompositeEligibilityFactory target = _targetBuilder.Build();

            // Act
            RuleSet1Eligibility ruleSetResult = target.Create(productId) as RuleSet1Eligibility;
            FormSubtypeEligibility result = ruleSetResult.Components[0] as FormSubtypeEligibility;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RuleSet == 1);
            Assert.IsTrue(result.ValidSubtypes == "1,5,7");
            _targetBuilder._mockMapper.Verify(m => m.MapParamsToEligibility(It.IsAny<IList<ProductEligibilityParam>>()), Times.Once);
        }

        [TestMethod]
        public void Can_Create_A_Composite_With_Single_Rule_Set_With_Multiple_Eligibility() {
            // Arrange
            int productId = 1;
            _targetBuilder.WithSingleRuleSetWithMultipleEligibility(productId);
            CompositeEligibilityFactory target = _targetBuilder.Build();

            // Act
            RuleSet1Eligibility ruleSet1result = target.Create(productId) as RuleSet1Eligibility;
            MultipleItemsEligibility multipleResult = ruleSet1result.Components[0] as MultipleItemsEligibility;
            FormSubtypeEligibility formSubtypeResult = ruleSet1result.Components[1] as FormSubtypeEligibility;

            // Assert - MultipleItemsEligibility
            Assert.IsNotNull(multipleResult);
            Assert.IsTrue(multipleResult.RuleSet == 1);
            Assert.IsTrue(multipleResult.MinCount == "1");
            Assert.IsTrue(multipleResult.MaxCount == "4");
            // Assert - FormSubtypeEligibility
            Assert.IsNotNull(formSubtypeResult);
            Assert.IsTrue(formSubtypeResult.RuleSet == 1);
            Assert.IsTrue(formSubtypeResult.ValidSubtypes == "1,5,7");
            // Assert - Verify mocked method was called exactly once.
            _targetBuilder._mockMapper.Verify(m => m.MapParamsToEligibility(It.IsAny<IList<ProductEligibilityParam>>()), Times.Once);
        }

        [TestMethod]
        public void Can_Create_A_Composite_With_Multiple_Rule_Set_With_Multiple_Eligibility() {
            // Arrange
            _targetBuilder.WithMultipleRuleSetWithMultipleEligibility(1);
            CompositeEligibilityFactory target = _targetBuilder.Build();

            // Act
            IEligibility result = target.Create(1);
            // Retrieve first rule set.
            RuleSet1Eligibility ruleSetOne = result as RuleSet1Eligibility;
            FormSubtypeEligibility formSubtype1 = ruleSetOne.Components[0] as FormSubtypeEligibility;
            SubscriberTypeEligibility subscriberType1 = ruleSetOne.Components[1] as SubscriberTypeEligibility;
            TariffsEligibility tariffs1 = ruleSetOne.Components[2] as TariffsEligibility;

            // Retrieve second rule set.
            RuleSetOtherEligibility ruleSetTwo = ruleSetOne.Components[3] as RuleSetOtherEligibility;
            FormSubtypeEligibility formSubtype2 = ruleSetTwo.Components[0] as FormSubtypeEligibility;
            SubscriberTypeEligibility subscriberType2 = ruleSetTwo.Components[1] as SubscriberTypeEligibility;

            // Retrieve third rule set.
            RuleSetOtherEligibility ruleSetThree = ruleSetTwo.Components[2] as RuleSetOtherEligibility;
            SubscriberTypeEligibility subscriberType3 = ruleSetThree.Components[0] as SubscriberTypeEligibility;
            TariffsEligibility tariffs2 = ruleSetThree.Components[1] as TariffsEligibility;

            // Retrieve fourth rule set.
            RuleSetOtherEligibility ruleSetFour = ruleSetThree.Components[2] as RuleSetOtherEligibility;
            SubscriberTypeEligibility subscriberType4 = ruleSetFour.Components[0] as SubscriberTypeEligibility;
            TariffsEligibility tariffs3 = ruleSetFour.Components[1] as TariffsEligibility;

            // Assert - RuleSet1Eligibility representing Rule set 1
            Assert.IsNotNull(ruleSetOne);
            Assert.IsTrue(ruleSetOne.RuleSet == 1);
            Assert.IsTrue(ruleSetOne.Components.Count == 4);
            // Assert - FormSubtypeEligibility in Rule set 1
            Assert.IsNotNull(formSubtype1);
            Assert.IsTrue(formSubtype1.RuleSet == 1);
            Assert.IsTrue(formSubtype1.ValidSubtypes == "1,5,7");
            // Assert - SubscriberTypeEligibility in Rule set 1
            Assert.IsNotNull(subscriberType1);
            Assert.IsTrue(subscriberType1.RuleSet == 1);
            Assert.IsTrue(subscriberType1.SubscriberType == "1,2,3");
            // Assert - TariffsEligibility in Rule set 1
            Assert.IsNotNull(tariffs1);
            Assert.IsTrue(tariffs1.RuleSet == 1);
            Assert.IsTrue(tariffs1.AllowedTariffs == "14,27,61");
            // Assert - RuleSetOtherEligibility representing Rule set 2
            Assert.IsNotNull(ruleSetTwo);
            Assert.IsTrue(ruleSetTwo.RuleSet == 2);
            // Assert - FormSubtypeEligibility in Rule set 2
            Assert.IsNotNull(formSubtype2);
            Assert.IsTrue(formSubtype2.RuleSet == 2);
            Assert.IsTrue(formSubtype2.ValidSubtypes == "1");
            // Assert - SubscriberTypeEligibility in Rule set 2
            Assert.IsNotNull(subscriberType2);
            Assert.IsTrue(subscriberType2.RuleSet == 2);
            Assert.IsTrue(subscriberType2.SubscriberType == "2");
            // Assert - RuleSetOtherEligibility representing Rule set 3
            Assert.IsNotNull(ruleSetThree);
            Assert.IsTrue(ruleSetThree.RuleSet == 3);
            // Assert - SubscriberTypeEligibility in Rule set 3
            Assert.IsNotNull(subscriberType3);
            Assert.IsTrue(subscriberType3.RuleSet == 3);
            Assert.IsTrue(subscriberType3.SubscriberType == "3");
            // Assert - TariffsEligibility in Rule set 3
            Assert.IsNotNull(tariffs2);
            Assert.IsTrue(tariffs2.RuleSet == 3);
            Assert.IsTrue(tariffs2.AllowedTariffs == "61");
            // Assert - RuleSetOtherEligibility representing Rule set 4
            Assert.IsNotNull(ruleSetFour);
            Assert.IsTrue(ruleSetFour.RuleSet == 4);
            // Assert - SubscriberTypeEligibility in Rule set 4
            Assert.IsNotNull(subscriberType4);
            Assert.IsTrue(subscriberType4.RuleSet == 4);
            Assert.IsTrue(subscriberType4.SubscriberType == "1");
            // Assert - TariffsEligibility in Rule set 4
            Assert.IsNotNull(tariffs3);
            Assert.IsTrue(tariffs3.RuleSet == 4);
            Assert.IsTrue(tariffs3.AllowedTariffs == "14");
        }
    }

    public class CompositeEligibilityFactoryBuilder {
        public Mock<IProductEligibilityParamRepository> _mockRepo;
        public Mock<IProductParamToEligibilityMapper> _mockMapper;
        private CompositeEligibilityFactory _eligibilityFactory;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        public CompositeEligibilityFactoryBuilder() {
            _mockRepo = new Mock<IProductEligibilityParamRepository>();
            _mockMapper = new Mock<IProductParamToEligibilityMapper>();
            _eligibilityFactory = new CompositeEligibilityFactory(_mockRepo.Object, _mockMapper.Object);
        }

        public CompositeEligibilityFactoryBuilder WithNoEligibility() {
            _mockRepo.Setup(m => m.GetProductEligibilityParams(It.IsAny<int>())).Returns(
                new ProductEligibilityParam[0].AsQueryable<ProductEligibilityParam>());
            _mockMapper.Setup(m => m.MapParamsToEligibility(It.IsAny<IList<ProductEligibilityParam>>())).Returns(new List<IEligibility>());
            return this;
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
            _mockMapper.Setup(m => m.MapParamsToEligibility(It.IsAny<IList<ProductEligibilityParam>>())).Returns(
                new List<IEligibility> {
                    new FormSubtypeEligibility {
                        RuleSet = 1,
                        ValidSubtypes = "1,5,7"
                    }
                });
            return this;
        }

        public CompositeEligibilityFactoryBuilder WithSingleRuleSetWithMultipleEligibility(int productId) {
            _mockRepo.Setup(m => m.GetProductEligibilityParams(productId)).Returns(
                new ProductEligibilityParam[] {
                    new ProductEligibilityParam {
                        Id = 1,
                        ProductId = productId,
                        EligibilityName = "MultipleItemsEligibility",
                        ParamName = "MinCount",
                        ParamValue = "1",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 2,
                        ProductId = productId,
                        EligibilityName = "MultipleItemsEligibility",
                        ParamName = "MaxCount",
                        ParamValue = "4",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 3,
                        ProductId = productId,
                        EligibilityName = "FormSubtypeEligibility",
                        ParamName = "ValidSubtypes",
                        ParamValue = "1,5,7",
                        RuleSet = 1
                    }
                }.AsQueryable<ProductEligibilityParam>());
            _mockMapper.Setup(m => m.MapParamsToEligibility(It.IsAny<IList<ProductEligibilityParam>>())).Returns(
                new List<IEligibility> {
                    new MultipleItemsEligibility {
                        RuleSet = 1,
                        MinCount = "1",
                        MaxCount = "4"
                    },
                    new FormSubtypeEligibility {
                        RuleSet = 1,
                        ValidSubtypes = "1,5,7"
                    }
                });
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
                    },
                    new ProductEligibilityParam {
                        Id = 8,
                        ProductId = productId,
                        EligibilityName = "SubscriberTypeEligibility",
                        ParamName = "SubscriberType",
                        ParamValue = "1",
                        RuleSet = 4
                    },
                    new ProductEligibilityParam {
                        Id = 9,
                        ProductId = productId,
                        EligibilityName = "TariffsEligibility",
                        ParamName = "AllowedTariffs",
                        ParamValue = "14",
                        RuleSet = 4
                    }
                }.AsQueryable<ProductEligibilityParam>());
            _mockMapper.Setup(m => m.MapParamsToEligibility(It.IsAny<IList<ProductEligibilityParam>>())).Returns(
                new List<IEligibility> {
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
                    new FormSubtypeEligibility {
                        RuleSet = 2,
                        ValidSubtypes = "1"
                    },
                    new SubscriberTypeEligibility {
                        RuleSet = 2,
                        SubscriberType = "2"
                    },
                    new SubscriberTypeEligibility {
                        RuleSet = 3,
                        SubscriberType = "3"
                    },
                    new TariffsEligibility {
                        RuleSet = 3,
                        AllowedTariffs = "61"
                    },
                    new SubscriberTypeEligibility {
                        RuleSet = 4,
                        SubscriberType = "1"
                    },
                    new TariffsEligibility {
                        RuleSet = 4,
                        AllowedTariffs = "14"
                    }
                });
            return this;
        }

        public CompositeEligibilityFactory Build() {
            return _eligibilityFactory;
        }

    }
}
