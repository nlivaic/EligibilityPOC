using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EligibilityPOC.Domain.Concrete;
using EligibilityPOC.Domain.Entities;
using System.Collections.Generic;
using Moq;
using EligibilityPOC.Domain.Abstract;
using EligibilityPOC.Domain.Entities.EligibilityEntities;

namespace EligibilityPOC.UnitTests {
    [TestClass]
    public class ActivatorProductParamToEligibilityMapperTest {
        private ProductParamEligibilityListBuilder _paramListBuilder;

        [TestInitialize]
        public void TestInitialize() {
            _paramListBuilder = new ProductParamEligibilityListBuilder();
        }
        
        [TestMethod]
        public void Return_Empty_List_If_No_Eligibilities() {
            // Arrange
            _paramListBuilder.WithNoEligibility();
            ActivatorProductParamToEligibilityMapper target = new ActivatorProductParamToEligibilityMapper();

            // Act
            IList<IEligibility> result = target.MapParamsToEligibility(_paramListBuilder.Build());

            // Assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void Can_Create_A_Single_Eligible_Object_With_Single_Property() {
            // Arrange
            int productId = 1;
            _paramListBuilder.WithSingleEligibilityWithSingleProperty(productId);
            ActivatorProductParamToEligibilityMapper target = new ActivatorProductParamToEligibilityMapper();

            // Act
            IList<IEligibility> result = target.MapParamsToEligibility(_paramListBuilder.Build());
            FormSubtypeEligibility formSubtype = result[0] as FormSubtypeEligibility;

            // Assert
            Assert.IsNotNull(formSubtype);
            Assert.IsTrue(formSubtype.RuleSet == 1);
            Assert.IsTrue(formSubtype.ValidSubtypes == "1,5,7");

        }

        [TestMethod]
        public void Can_Create_A_Single_Eligible_Object_With_Multiple_Properties() {
            // Arrange
            int productId = 1;
            _paramListBuilder.WithSingleEligibilityWithMultipleProperties(productId);
            ActivatorProductParamToEligibilityMapper target = new ActivatorProductParamToEligibilityMapper();

            // Act
            MultipleItemsEligibility result = target.MapParamsToEligibility(_paramListBuilder.Build())[0] as MultipleItemsEligibility;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RuleSet == 1);
            Assert.IsTrue(result.MinCount == "1");
            Assert.IsTrue(result.MaxCount == "4");
        }

        [TestMethod]
        public void Can_Create_Multiple_Eligible_Object() {
            // Arrange
            _paramListBuilder.WithMultipleEligibility(1);
            ActivatorProductParamToEligibilityMapper target = new ActivatorProductParamToEligibilityMapper();

            // Act
            IList<IEligibility> result = target.MapParamsToEligibility(_paramListBuilder.Build());
            FormSubtypeEligibility formSubtype1 = result[0] as FormSubtypeEligibility;
            SubscriberTypeEligibility subscriberType1 = result[1] as SubscriberTypeEligibility;
            TariffsEligibility tariffs1 = result[2] as TariffsEligibility;
            FormSubtypeEligibility formSubtype2 = result[3] as FormSubtypeEligibility;
            SubscriberTypeEligibility subscriberType2 = result[4] as SubscriberTypeEligibility;
            MultipleItemsEligibility multipleItems1 = result[5] as MultipleItemsEligibility;
            TariffsEligibility tariffs2 = result[6] as TariffsEligibility;

            // Assert - FormSubtypeEligibility in rule set 1
            Assert.IsNotNull(formSubtype1);
            Assert.IsTrue(formSubtype1.RuleSet == 1);
            Assert.IsTrue(formSubtype1.ValidSubtypes == "1,5,7");
            // Assert - SubscriberTypeEligibility in rule set 1
            Assert.IsNotNull(subscriberType1);
            Assert.IsTrue(subscriberType1.RuleSet == 1);
            Assert.IsTrue(subscriberType1.SubscriberType == "1,2,3");
            // Assert - TariffsEligibility in rule set 1
            Assert.IsNotNull(tariffs1);
            Assert.IsTrue(tariffs1.RuleSet == 1);
            Assert.IsTrue(tariffs1.AllowedTariffs == "14,27,61");
            // Assert - FormSubtypeEligibility in rule set 2
            Assert.IsNotNull(formSubtype2);
            Assert.IsTrue(formSubtype2.RuleSet == 2);
            Assert.IsTrue(formSubtype2.ValidSubtypes == "1");
            // Assert - SubscriberTypeEligibility in rule set 2
            Assert.IsNotNull(subscriberType2);
            Assert.IsTrue(subscriberType2.RuleSet == 2);
            Assert.IsTrue(subscriberType2.SubscriberType == "2");
            // Assert - MultipleItemsEligibility in rule set 3
            Assert.IsNotNull(multipleItems1);
            Assert.IsTrue(multipleItems1.RuleSet == 3);
            Assert.IsTrue(multipleItems1.MinCount == "2");
            Assert.IsTrue(multipleItems1.MaxCount == "5");
            // Assert - TariffsEligibility in rule set 3
            Assert.IsNotNull(tariffs2);
            Assert.IsTrue(tariffs2.RuleSet == 3);
            Assert.IsTrue(tariffs2.AllowedTariffs == "61");
        }
    }

    public class ProductParamEligibilityListBuilder {
        private IList<ProductEligibilityParam> _paramList;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        public ProductParamEligibilityListBuilder() {
            _paramList = new List<ProductEligibilityParam>();
        }

        public ProductParamEligibilityListBuilder WithNoEligibility() {
            return this;
        }

        public ProductParamEligibilityListBuilder WithSingleEligibilityWithSingleProperty(int productId) {
            _paramList.Add(new ProductEligibilityParam {
                Id = 1,
                ProductId = productId,
                EligibilityName = "FormSubtypeEligibility",
                ParamName = "ValidSubtypes",
                ParamValue = "1,5,7",
                RuleSet = 1
            });
            return this;
        }

        public ProductParamEligibilityListBuilder WithSingleEligibilityWithMultipleProperties(int productId) {
            _paramList.Add(new ProductEligibilityParam {
                Id = 1,
                ProductId = productId,
                EligibilityName = "MultipleItemsEligibility",
                ParamName = "MinCount",
                ParamValue = "1",
                RuleSet = 1
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 1,
                ProductId = productId,
                EligibilityName = "MultipleItemsEligibility",
                ParamName = "MaxCount",
                ParamValue = "4",
                RuleSet = 1
            });
            return this;
        }

        public ProductParamEligibilityListBuilder WithMultipleEligibility(int productId) {
            _paramList.Add(new ProductEligibilityParam {
                Id = 1,
                ProductId = productId,
                EligibilityName = "FormSubtypeEligibility",
                ParamName = "ValidSubtypes",
                ParamValue = "1,5,7",
                RuleSet = 1
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 2,
                ProductId = productId,
                EligibilityName = "SubscriberTypeEligibility",
                ParamName = "SubscriberType",
                ParamValue = "1,2,3",
                RuleSet = 1
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 3,
                ProductId = productId,
                EligibilityName = "TariffsEligibility",
                ParamName = "AllowedTariffs",
                ParamValue = "14,27,61",
                RuleSet = 1
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 4,
                ProductId = productId,
                EligibilityName = "FormSubtypeEligibility",
                ParamName = "ValidSubtypes",
                ParamValue = "1",
                RuleSet = 2
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 5,
                ProductId = productId,
                EligibilityName = "SubscriberTypeEligibility",
                ParamName = "SubscriberType",
                ParamValue = "2",
                RuleSet = 2
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 6,
                ProductId = productId,
                EligibilityName = "MultipleItemsEligibility",
                ParamName = "MinCount",
                ParamValue = "2",
                RuleSet = 3
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 7,
                ProductId = productId,
                EligibilityName = "MultipleItemsEligibility",
                ParamName = "MaxCount",
                ParamValue = "5",
                RuleSet = 3
            });
            _paramList.Add(new ProductEligibilityParam {
                Id = 8,
                ProductId = productId,
                EligibilityName = "TariffsEligibility",
                ParamName = "AllowedTariffs",
                ParamValue = "61",
                RuleSet = 3
            });
            return this;
        }

        public IList<ProductEligibilityParam> Build() {
            return _paramList;
        }
    }
}
