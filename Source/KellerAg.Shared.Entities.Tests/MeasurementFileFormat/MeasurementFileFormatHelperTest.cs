using KellerAg.Shared.Entities.FileFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace KellerAg.Shared.Entities.Tests.MeasurementFileFormat
{
    [TestClass]
    public class MeasurementFileFormatHelperTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void GenerateUniqueSerialNumber_WhenLoggerIdIsGenerated_ThenTheIsCorrect()
        {
            var number = MeasurementFileFormatHelper.GenerateUniqueSerialNumber("5.5", "123");
            number.ShouldBe("REC-5.5-123");
        }

        [TestMethod]
        public void GenerateUniqueSerialNumber_WhenGSMIdIsGenerated_ThenTheIsCorrect()
        {
            var number = MeasurementFileFormatHelper.GenerateUniqueSerialNumber("9.5", "123");
            number.ShouldBe("GSM-123");
        }

        [TestMethod]
        public void GenerateUniqueSerialNumberGsm_WhenGSMIdIsGenerated_ThenTheIsCorrect()
        {
            var number = MeasurementFileFormatHelper.GenerateUniqueSerialNumberGsm("123");
            number.ShouldBe("GSM-123");
        }

        [TestMethod]
        public void GenerateUniqueSerialNumber_WhenAdtIdIsGenerated_ThenTheIsCorrect()
        {
            var number = MeasurementFileFormatHelper.GenerateUniqueSerialNumber("19.0", string.Empty, "555");
            number.ShouldBe("EUI-555");
        }

        [TestMethod]
        public void GenerateUniqueSerialNumberAdt_WhenAdtIdIsGenerated_ThenTheIsCorrect()
        {
            var number = MeasurementFileFormatHelper.GenerateUniqueSerialNumberLoRa("19.0", "555");
            number.ShouldBe("EUI-555");
        }

        [TestMethod]
        public void GenerateUniqueSerialNumber_WhenArcIdIsGenerated_ThenTheIsCorrect()
        {
            var number = MeasurementFileFormatHelper.GenerateUniqueSerialNumber("9.20", "123");
            number.ShouldBe("ARC-9.20-123");
        }
    }
}