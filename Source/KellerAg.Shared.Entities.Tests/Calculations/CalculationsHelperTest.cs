using KellerAg.Shared.Entities.Calculations;
using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace KellerAg.Shared.Entities.Tests.Calculations
{
    [TestClass]
    public class CalculationsHelperTest
    {
        //private CalculationsHelper _testee;

        [TestInitialize]
        public void Initialize()
        {
            //_testee =  CalculationsHelper;
        }

        [TestMethod]
        public void GenerateCalculation_WhenWaterHeightCalculationIsGenerated_ThenThGenericCalculationHasAllAttributesFilled()
        {
            var deviceCalculation = new MeasurementFileFormatWaterCalculationStoredInDevice
            {
                WaterLevelCalculation = new MeasurementFileFormatWaterLevel
                {
                    WaterLevelType = WaterLevelType.HeightOfWater,
                    HydrostaticPressureChannelId = 1,
                    BarometricPressureChannelId = 2,
                    Gravity = 3,
                    Density = 4,
                    Offset = 5,
                    UseBarometricPressureToCompensate = true
                }
            };

            var genericCalculation = CalculationsHelper.GenerateCalculation(deviceCalculation, ChannelInfo.GetChannels());

            genericCalculation.CalculationTypeId.ShouldBe(1);
            genericCalculation.CalculationParameters.Count.ShouldBe(9);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.HydrostaticPressureMeasurementDefinitionId);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.BarometricPressureMeasurementDefinitionId);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Gravity);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Density);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Offset);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.UseBarometricPressureToCompensate);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.CorrespondingMeasurementDefinitionId);

            genericCalculation.CalculationParameters[CalculationParameter.HydrostaticPressureMeasurementDefinitionId].ShouldBe("1");
            genericCalculation.CalculationParameters[CalculationParameter.BarometricPressureMeasurementDefinitionId].ShouldBe("2");
            genericCalculation.CalculationParameters[CalculationParameter.Gravity].ShouldBe("3");
            genericCalculation.CalculationParameters[CalculationParameter.Density].ShouldBe("4");
            genericCalculation.CalculationParameters[CalculationParameter.Offset].ShouldBe("5");
            genericCalculation.CalculationParameters[CalculationParameter.UseBarometricPressureToCompensate].ShouldBe("True");
            genericCalculation.CalculationParameters[CalculationParameter.CorrespondingMeasurementDefinitionId].ShouldBe("34");
        }

        [TestMethod]
        public void GenerateCalculation_WhenWaterDepthCalculationIsGenerated_ThenThGenericCalculationHasAllAttributesFilled()
        {
            var deviceCalculation = new MeasurementFileFormatWaterCalculationStoredInDevice
            {
                WaterLevelCalculation = new MeasurementFileFormatWaterLevel
                {
                    WaterLevelType = WaterLevelType.DepthToWater,
                    HydrostaticPressureChannelId = 1,
                    BarometricPressureChannelId = null,
                    Gravity = 3,
                    Density = 4,
                    Offset = 5,
                    InstallationLength = 6,
                    UseBarometricPressureToCompensate = false
                }
            };
            var genericCalculation = CalculationsHelper.GenerateCalculation(deviceCalculation, ChannelInfo.GetChannels());

            genericCalculation.CalculationTypeId.ShouldBe(2);

            genericCalculation.CalculationParameters.Count.ShouldBe(10);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.HydrostaticPressureMeasurementDefinitionId);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.BarometricPressureMeasurementDefinitionId);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Gravity);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Density);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Offset);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.InstallationLength);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.UseBarometricPressureToCompensate);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.CorrespondingMeasurementDefinitionId);

            genericCalculation.CalculationParameters[CalculationParameter.HydrostaticPressureMeasurementDefinitionId].ShouldBe("1");
            genericCalculation.CalculationParameters[CalculationParameter.BarometricPressureMeasurementDefinitionId].ShouldBe(null);
            genericCalculation.CalculationParameters[CalculationParameter.Gravity].ShouldBe("3");
            genericCalculation.CalculationParameters[CalculationParameter.Density].ShouldBe("4");
            genericCalculation.CalculationParameters[CalculationParameter.Offset].ShouldBe("5");
            genericCalculation.CalculationParameters[CalculationParameter.InstallationLength].ShouldBe("6");
            genericCalculation.CalculationParameters[CalculationParameter.UseBarometricPressureToCompensate].ShouldBe("False");
            genericCalculation.CalculationParameters[CalculationParameter.CorrespondingMeasurementDefinitionId].ShouldBe("35");
        }

        [TestMethod]
        public void GenerateCalculation_WhenWaterHeightAboveSeaLevelCalculationIsGenerated_ThenThGenericCalculationHasAllAttributesFilled()
        {
            var deviceCalculation = new MeasurementFileFormatWaterCalculationStoredInDevice
            {
                WaterLevelCalculation = new MeasurementFileFormatWaterLevel
                {
                    WaterLevelType = WaterLevelType.HeightOfWaterAboveSeaLevel,
                    HydrostaticPressureChannelId = 1,
                    BarometricPressureChannelId = 2,
                    Gravity = 3,
                    Density = 4,
                    Offset = 5,
                    InstallationLength = 6,
                    HeightOfWellhead = 7,
                    UseBarometricPressureToCompensate = true
                }
            };
            var genericCalculation = CalculationsHelper.GenerateCalculation(deviceCalculation, ChannelInfo.GetChannels());

            genericCalculation.CalculationTypeId.ShouldBe(3);

            genericCalculation.CalculationParameters.Count.ShouldBe(11);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.HydrostaticPressureMeasurementDefinitionId);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.BarometricPressureMeasurementDefinitionId);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Gravity);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Density);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.Offset);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.InstallationLength);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.HeightOfWellheadAboveSea);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.UseBarometricPressureToCompensate);
            genericCalculation.CalculationParameters.ShouldContainKey(CalculationParameter.CorrespondingMeasurementDefinitionId);

            genericCalculation.CalculationParameters[CalculationParameter.HydrostaticPressureMeasurementDefinitionId].ShouldBe("1");
            genericCalculation.CalculationParameters[CalculationParameter.BarometricPressureMeasurementDefinitionId].ShouldBe("2");
            genericCalculation.CalculationParameters[CalculationParameter.Gravity].ShouldBe("3");
            genericCalculation.CalculationParameters[CalculationParameter.Density].ShouldBe("4");
            genericCalculation.CalculationParameters[CalculationParameter.Offset].ShouldBe("5");
            genericCalculation.CalculationParameters[CalculationParameter.InstallationLength].ShouldBe("6");
            genericCalculation.CalculationParameters[CalculationParameter.HeightOfWellheadAboveSea].ShouldBe("7");
            genericCalculation.CalculationParameters[CalculationParameter.UseBarometricPressureToCompensate].ShouldBe("True");
            genericCalculation.CalculationParameters[CalculationParameter.CorrespondingMeasurementDefinitionId].ShouldBe("36");
        }
    }
}