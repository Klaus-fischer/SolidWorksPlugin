namespace SIM.SolidWorksPlugin.Tests.Documents
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class PropertyManager_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var pm = new PropertyManager();
            Assert.IsNotNull(pm);
        }

        [TestMethod]
        public void GetSummaryInfo_Test()
        {
            var model = new Mock<IModelDoc2>();
            model.Setup(o => o.get_SummaryInfo((int)swSummInfoField_e.swSumInfoTitle)).Returns("Title");
            model.Setup(o => o.get_SummaryInfo((int)swSummInfoField_e.swSumInfoSubject)).Returns("Subject");
            model.Setup(o => o.get_SummaryInfo((int)swSummInfoField_e.swSumInfoComment)).Returns("Comment");
            model.Setup(o => o.get_SummaryInfo((int)swSummInfoField_e.swSumInfoAuthor)).Returns("Author");
            model.Setup(o => o.get_SummaryInfo((int)swSummInfoField_e.swSumInfoKeywords)).Returns("Keywords");

            var pm = new PropertyManager();
            pm.ActiveModel = model.Object;

            Assert.AreEqual("Title", pm.Title);
            Assert.AreEqual("Subject", pm.Subject);
            Assert.AreEqual("Comment", pm.Comments);
            Assert.AreEqual("Author", pm.Author);
            Assert.AreEqual("Keywords", pm.Keywords);
        }

        [TestMethod]
        public void SetSummaryInfo_Test()
        {
            Dictionary<swSummInfoField_e, string> values = new Dictionary<swSummInfoField_e, string>();

            var model = new Mock<IModelDoc2>();
            model.Setup(o => o.set_SummaryInfo(It.IsAny<int>(), It.IsAny<string>()))
                .Callback<int, string>((i, s) =>
            {
                values[(swSummInfoField_e)i] = s;
            });

            var pm = new PropertyManager();
            pm.ActiveModel = model.Object;

            pm.Title = "Titel";
            pm.Comments = "Kommentare";
            pm.Author = "Autor";
            pm.Subject = "Betreff";
            pm.Keywords = "Schlüsselwörter";

            Assert.AreEqual("Titel", values[swSummInfoField_e.swSumInfoTitle]);
            Assert.AreEqual("Betreff", values[swSummInfoField_e.swSumInfoSubject]);
            Assert.AreEqual("Kommentare", values[swSummInfoField_e.swSumInfoComment]);
            Assert.AreEqual("Autor", values[swSummInfoField_e.swSumInfoAuthor]);
            Assert.AreEqual("Schlüsselwörter", values[swSummInfoField_e.swSumInfoKeywords]);
        }

        [TestMethod]
        public void SetProperty_Test()
        {
            var dictionary = new Dictionary<string, string>();

            var model = CreateMock(dictionary);

            model.Setup(o => o.SetSaveFlag());

            var pm = new PropertyManager();
            pm.ActiveModel = model.Object;

            pm.SetStringProperty("property1", "Value1");
            pm["property2"] = "Value2";

            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual("Value1", dictionary["property1"]);
            Assert.AreEqual("Value2", dictionary["property2"]);

            pm["property2"] = null;

            Assert.AreEqual(1, dictionary.Count);
            Assert.IsFalse(dictionary.ContainsKey("property2"));
            model.Verify(o => o.SetSaveFlag(), Times.Exactly(3));
        }

        [TestMethod]
        public void GetProperty_Test()
        {
            var dictionary = new Dictionary<string, string>{
                { "property1", "value1" },
                { "property2", "value2"},
                { "_property3", "value3" }
            };

            var model = CreateMock(dictionary);

            var pm = new PropertyManager();
            pm.ActiveModel = model.Object;

            Assert.AreEqual("value1", pm.GetStringProperty("property1"));
            Assert.AreEqual("value2", pm["property2"]);
            Assert.AreEqual("value3", pm.GetStringProperty("_property3"));
            Assert.AreEqual(null, pm.GetStringProperty("property4"));
        }

        [TestMethod]
        public void DeleteProperty_Test()
        {
            var dictionary = new Dictionary<string, string>{
                { "property1", "value1" },
                { "property2", "value2"},
                { "_property3", "value3" }
            };

            var model = CreateMock(dictionary);

            var pm = new PropertyManager();
            pm.ActiveModel = model.Object;

            pm.DeleteProperty("property1");

        }

        [TestMethod]
        public void SetDateProperty_Test()
        {
            var dictionary = new Dictionary<string, string>();

            var model = CreateMock(dictionary);

            model.Setup(o => o.SetSaveFlag());

            var pm = new PropertyManager();
            pm.ActiveModel = model.Object;

            pm.SetDateProperty("property1", new DateTime(2021, 11, 19));

            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("19.11.2021", dictionary["property1"]);

            pm.SetDateProperty("property1", null);

            Assert.AreEqual(0, dictionary.Count);

            model.Verify(o => o.SetSaveFlag(), Times.Exactly(2));
        }

        [TestMethod]
        public void GetDateProperty_Test()
        {
            var dictionary = new Dictionary<string, string>{
                { "property1", "2021.11.19" }
            };

            var model = CreateMock(dictionary);

            var pm = new PropertyManager();
            pm.ActiveModel = model.Object;

            Assert.AreEqual(new DateTime(2021, 11, 19), pm.GetDateProperty("property1"));
            Assert.IsNull(pm.GetDateProperty("notDefinedProperty"));
        }

        [TestMethod]
        public void GetWeight_Test()
        {
            var model = new Mock<IModelDoc2>();
            var extensions = new Mock<ModelDocExtension>();
            var massProperty = new Mock<MassProperty>();

            model.SetupGet(o => o.Extension)
                .Returns(extensions.Object);

            extensions.Setup(o => o.CreateMassProperty())
                .Returns(massProperty.Object);

            massProperty.SetupGet(o => o.Mass).Returns(1.2345);

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            Assert.AreEqual(1.2345, pm.GetWeight());
        }

        [TestMethod]
        public void GetWeightDrawing_Test()
        {
            var model = new Mock<SwDrawing_Tests.DrwDoc>();

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            Assert.AreEqual(-1, pm.GetWeight());
        }

        [TestMethod]
        public void SetWeightLess5Percent_Test()
        {
            var model = new Mock<IModelDoc2>();
            var extensions = new Mock<ModelDocExtension>();
            var massProperty = new Mock<MassProperty>();

            model.SetupGet(o => o.Extension)
                .Returns(extensions.Object);
            model.Setup(o => o.SetSaveFlag());

            extensions.Setup(o => o.CreateMassProperty())
                .Returns(massProperty.Object);

            massProperty.SetupGet(o => o.Mass).Returns(1.2345);
            massProperty.Setup(o => o.SetOverrideMassValue(It.IsAny<double>(), It.IsAny<int>(), It.IsAny<object>()));

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            pm.SetWeight(1.2346);
            massProperty.Verify(o => o.SetOverrideMassValue(It.IsAny<double>(), It.IsAny<int>(), It.IsAny<object>()), Times.Never()); ;
            model.Verify(o => o.SetSaveFlag(), Times.Never);
        }

        [TestMethod]
        public void SetWeightOverride_Test()
        {
            var model = new Mock<IModelDoc2>();
            var extensions = new Mock<ModelDocExtension>();
            var massProperty = new Mock<MassProperty>();

            model.SetupGet(o => o.Extension)
                .Returns(extensions.Object);
            model.Setup(o => o.SetSaveFlag());

            extensions.Setup(o => o.CreateMassProperty())
                .Returns(massProperty.Object);

            massProperty.SetupGet(o => o.OverrideMass).Returns(true);
            massProperty.SetupGet(o => o.Mass).Returns(1.2345);
            massProperty.Setup(o => o.SetOverrideMassValue(It.IsAny<double>(), It.IsAny<int>(), It.IsAny<object>()));

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            pm.SetWeight(1.2346);
            massProperty.Verify(o => o.SetOverrideMassValue(It.IsAny<double>(), It.IsAny<int>(), It.IsAny<object>()), Times.AtLeastOnce());
            model.Verify(o => o.SetSaveFlag(), Times.AtLeastOnce);
        }


        [TestMethod]
        public void SetWeight_Test()
        {
            var model = new Mock<IModelDoc2>();
            var extensions = new Mock<ModelDocExtension>();
            var massProperty = new Mock<MassProperty>();

            model.SetupGet(o => o.Extension)
                .Returns(extensions.Object);
            model.Setup(o => o.SetSaveFlag());

            extensions.Setup(o => o.CreateMassProperty())
                .Returns(massProperty.Object);

            massProperty.SetupGet(o => o.Mass).Returns(1.2345);
            massProperty.Setup(o => o.SetOverrideMassValue(It.IsAny<double>(), It.IsAny<int>(), It.IsAny<object>()));

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            pm.SetWeight(2.2345);

            massProperty.Verify(o => o.SetOverrideMassValue(It.IsAny<double>(), It.IsAny<int>(), It.IsAny<object>()), Times.AtLeastOnce()); ;
            model.Verify(o => o.SetSaveFlag(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void SetWeightDrawing_Test()
        {
            var model = new Mock<SwDrawing_Tests.DrwDoc>();
            model.Setup(o => o.SetSaveFlag());

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            pm.SetWeight(2.2345);

            model.Verify(o => o.SetSaveFlag(), Times.Never);
        }

        [TestMethod]
        public void SetConfiguration_Test()
        {
            var model = new Mock<IModelDoc2>();
            model.Setup(o => o.GetConfigurationNames()).Returns(new object[] { "", "ohne" });

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            pm.CustomPropertyConfiguration = "ohne";
            Assert.AreEqual("ohne", pm.CustomPropertyConfiguration);
        }

        [TestMethod]
        public void SetConfiguration_Fail()
        {
            var model = new Mock<IModelDoc2>();
            model.Setup(o => o.GetConfigurationNames()).Returns(new object[] { "", "ohne" });

            var pm = new PropertyManager
            {
                ActiveModel = model.Object
            };

            pm.CustomPropertyConfiguration = "anders";
            Assert.AreEqual("", pm.CustomPropertyConfiguration);
        }

        [TestMethod]
        public void SetConfiguration_Fail2()
        {
            var pm = new PropertyManager();
            pm.CustomPropertyConfiguration = "anders";

            Assert.AreEqual("", pm.CustomPropertyConfiguration);
        }

        public Mock<IModelDoc2> CreateMock(IDictionary<string, string> values)
        {
            var model = new Mock<IModelDoc2>();
            var extensions = new Mock<ModelDocExtension>();
            var propertyManager = new Mock<CustomPropertyManager>();

            propertyManager
                .Setup(o => o.Delete2(It.IsAny<string>()))
                .Callback<string>(propertyName =>
            {
                values.Remove(propertyName);
            });

            propertyManager
               .Setup(o => o.Add3(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
               .Callback<string, int, string, int>((fieldName, fieldType, fieldValue, overwriteExisting) =>
               {
                   values[fieldName] = fieldValue;
               });

            propertyManager
              .Setup(o => o.Get5(
                  It.IsAny<string>(),
                  It.IsAny<bool>(),
                  out It.Ref<string>.IsAny,
                  out It.Ref<string>.IsAny,
                  out It.Ref<bool>.IsAny))
              .Callback(new Get5Callback((
                  string propertyName,
                  bool useCached,
                  out string valueOut,
                  out string resolvedValueOut,
                  out bool wasResolved) =>
              {
                  valueOut = string.Empty;
                  resolvedValueOut = string.Empty;
                  wasResolved = false;
                  if (!values.TryGetValue(propertyName, out string value))
                  {
                      return;
                  }

                  if (propertyName.StartsWith("_"))
                  {
                      resolvedValueOut = value;
                      return;
                  }

                  valueOut = value;
                  return;
              }))
              .Returns<string, bool, string, string, bool>((p, c, v, r, w) =>
              {
                  if (v != string.Empty)
                  {
                      return (int)swCustomInfoGetResult_e.swCustomInfoGetResult_CachedValue;
                  }
                  if (r != string.Empty)
                  {
                      return (int)swCustomInfoGetResult_e.swCustomInfoGetResult_ResolvedValue;
                  }

                  return (int)swCustomInfoGetResult_e.swCustomInfoGetResult_NotPresent;
              });

            extensions.Setup(o => o.get_CustomPropertyManager(It.IsAny<string>()))
                .Returns(propertyManager.Object);

            model.SetupGet(o => o.Extension)
                .Returns(extensions.Object);

            return model;
        }

        private delegate void Get5Callback(string propertyName, bool useCached, out string valueOut, out string resolvedValueOut, out bool wasResolved);
    }
}
