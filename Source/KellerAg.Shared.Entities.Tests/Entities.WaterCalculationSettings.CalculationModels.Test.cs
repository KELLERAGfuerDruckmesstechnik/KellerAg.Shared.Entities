using KellerAg.Shared.Entities.Calculations;
using KellerAg.Shared.Entities.Calculations.CalculationModels;
using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;


namespace KellerAg.Shared.Entities.Tests
{
    [TestClass]
    public class CalculationModelTest
    {

        /// <summary>
        /// This test also show how to fill the MeasurementFileFormat regarding the calculations
        ///
        /// In this example there are P1, PBaro and Pd stored in the body
        /// The goal is to be able to export Pd, F and Calculation3 which has a E-calculation stored
        /// There are alternative names stored
        /// </summary>
        [TestMethod]
        public void WhenTryingToHaveTwoWaterCalculationStored_ThenIWantTheFileFormatStoredInCorrectManners()
        {
            var fileFormat = new FileFormat.MeasurementFileFormat
            {
                Header = new MeasurementFileFormatHeader
                {
                    MeasurementDefinitionsInBody = new[]
                    {
                        2    /* P1            */,
                        11   /* Pd (P1-PBaro) */,
                        7    /* PBaro         */,
                        //35 /* F             */,
                        //57 /*Calculation3  which is a E calculation*/
                    },
                }
            };

            fileFormat.Header.WaterCalculationStoredInDeviceSettings = null; //doesn't matter. These property is to store settings stored in the device. It is more relevant for PressureSuite Desktop.

            fileFormat.Header.ChannelCalculations = new List<MeasurementFileFormatChannelCalculation>();

            var depthToWaterCalc = new DepthToWaterChannelCalculationModel
            {
                ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance(ChannelType.mH20_F),
                //CalculationParameters = null,                             //Will automatically set by the base methods/constructors
                //CalculationTypeId = 0                                     //Will automatically set by the base methods/constructors
                HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(ChannelType.Pd_P1PBaro), //Pd
                BarometricPressureChannel = null,  //if this is null, UseBarometricPressureToCompensate has to be false of course
                //CorrespondingChannel = ChannelInfo.GetMeasurementDefinitionId(ChannelType.mH20_F), //Will automatically be default set by the constructors. Has to be overwritten when using channels like the MultiSensorChannel
                Offset = 12.3,
                //Density = will be default set in constructor
                Gravity = 9.8011111,  //default value will be overwritten here
                InstallationLength = 0,
                UseBarometricPressureToCompensate = false
            };

            fileFormat.Header.ChannelCalculations.Add(depthToWaterCalc);

            //MeasurementFileFormatChannelCalculation fForJsonSerializer = depthToWaterCalc.GetBase(); // GetBase has to be used as soon as the FileFormat has to be serialized. Effectively, 

            var multiSensorCalc = new HeightOfWaterChannelCalculationModel()
            {
                ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance(ChannelType.MultiSensor3),
                //CalculationParameters = null,   //Will automatically set by the base methods/constructors
                //CalculationTypeId = 0           //Will automatically set by the base methods/constructors
                HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(ChannelType.P1),
                BarometricPressureChannel = ChannelInfo.GetChannelInfo(ChannelType.PBaro),
                CorrespondingChannel = ChannelInfo.GetChannelInfo(ChannelType.MultiSensor3), //It's overwritten here because it is MeasurementDefinition 57. It is not 34.
                                                                                             //The information that 34 is used by 57 is stored in the calculation settings of 57
                Offset = 23.4,
                Density = 998.22222,
                //Gravity = will be set in constructor
                UseBarometricPressureToCompensate = true  //if this is true, BarometricPressureChannel 
            };

            // multiSensorCalc.ChannelInfo.ChannelType = ChannelType.mH20_F;  // NO! Don't do this.  ChannelType and MeasurementDefinitionId MUST be 100
            // multiSensorCalc.ChannelInfo.UnitType    = UnitType.Length;     // Not necessary: It is preset by this "ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance(ChannelType.MultiSensor3)"

            // MeasurementFileFormatChannelCalculation c = calc.GetBase();
            fileFormat.Header.ChannelCalculations.Add(multiSensorCalc);

            /////////////////////////////////////////////////
            //
            // Now, the settings are set. We can now test it.
            //
            //
            string calcWithoutGetBase = JsonConvert.SerializeObject(multiSensorCalc);
            string calcWithGetBase = JsonConvert.SerializeObject(multiSensorCalc.GetBase());

            calcWithGetBase.ShouldNotBe(calcWithoutGetBase);

            //string baseSettings = "CalculationParameters\":{\"HydrostaticPressureMeasurementDefinitionId\":\"2\",\"BarometricPressureMeasurementDefinitionId\":\"7\",\"CorrespondingMeasurementDefinitionId\":\"57\",\"Gravity\":\"9.80665\",\"Offset\":\"23.4\",\"Density\":\"998.22222\",\"UseBarometricPressureToCompensate\":\"True";
            string baseSettings = "{\"CalculationParameters\":{\"HydrostaticPressureMeasurementDefinitionId\":\"2\",\"BarometricPressureMeasurementDefinitionId\":\"7\",\"CorrespondingMeasurementDefinitionId\":\"57\",\"Gravity\":\"9.80665\",\"Offset\":\"23.4\",\"Density\":\"998.22222\",\"UseBarometricPressureToCompensate\":\"True\",\"From\":null,\"To\":null},\"CalculationTypeId\":1,\"ChannelInfo\":{\"ChannelType\":100,\"MeasurementDefinitionId\":100,\"Name\":\"Multi Sensor Channel 3\",\"Description\":\"Multi Sensor Channel 3\",\"ColorCode\":\"#3f51b5\",\"UnitType\":3}}";

            calcWithoutGetBase.ShouldNotContain(baseSettings);
            calcWithGetBase.ShouldContain(baseSettings);

            calcWithoutGetBase.Length.ShouldBeGreaterThan(900);
            calcWithGetBase.Length.ShouldBeLessThan(500);

            fileFormat.Header.ChannelCalculations[0].CalculationTypeId.ShouldBe((int)CalculationType.DepthToWater);
            fileFormat.Header.ChannelCalculations[0].CalculationParameters[CalculationParameter.Density].ShouldBe("998.2");
            fileFormat.Header.ChannelCalculations[0].CalculationParameters[CalculationParameter.Gravity].ShouldBe("9.8011111");
            fileFormat.Header.ChannelCalculations[0].CalculationParameters[CalculationParameter.BarometricPressureMeasurementDefinitionId].ShouldBeNull();
            fileFormat.Header.ChannelCalculations[0].ChannelInfo.ChannelType.ShouldBe(ChannelType.Calculation);
            fileFormat.Header.ChannelCalculations[0].ChannelInfo.Name.ShouldBe("mH20 (F)");

            fileFormat.Header.ChannelCalculations[1].CalculationTypeId.ShouldBe((int)CalculationType.HeightOfWater);
            fileFormat.Header.ChannelCalculations[1].CalculationParameters[CalculationParameter.Density].ShouldBe("998.22222");
            fileFormat.Header.ChannelCalculations[1].CalculationParameters[CalculationParameter.Gravity].ShouldBe("9.80665");
            fileFormat.Header.ChannelCalculations[1].CalculationParameters[CalculationParameter.BarometricPressureMeasurementDefinitionId].ShouldBe("7");
            fileFormat.Header.ChannelCalculations[1].CalculationParameters[CalculationParameter.CorrespondingMeasurementDefinitionId].ShouldBe("57");

            fileFormat.Header.ChannelCalculations[1].ChannelInfo.ChannelType.ShouldBe(ChannelType.Calculation);
            fileFormat.Header.ChannelCalculations[1].ChannelInfo.Name.ShouldBe("Multi Sensor Channel 3");


            MeasurementFileFormatChannelCalculation x = JsonConvert.DeserializeObject<MeasurementFileFormatChannelCalculation>(calcWithGetBase);

            MeasurementFileFormatChannelCalculation m = new HeightOfWaterChannelCalculationModel(x);

            switch ((CalculationType)x.CalculationTypeId)
            {
                case CalculationType.Unknown:
                    break;
                case CalculationType.HeightOfWater:
                    m = new HeightOfWaterChannelCalculationModel(x);
                    break;
                case CalculationType.DepthToWater:
                    m = new DepthToWaterChannelCalculationModel(x);
                    break;
                case CalculationType.HeightOfWaterAboveSea:
                    break;
                case CalculationType.Offset:
                    break;
                case CalculationType.OverflowPoleni:
                    break;
                case CalculationType.OverflowThomson:
                    break;
                case CalculationType.OverflowVenturi:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            m.CalculationTypeId.ShouldBe((int)CalculationType.HeightOfWater);
            m.CalculationParameters[CalculationParameter.Density].ShouldBe("998.22222");
            m.CalculationParameters[CalculationParameter.Gravity].ShouldBe("9.80665");
            m.CalculationParameters[CalculationParameter.BarometricPressureMeasurementDefinitionId].ShouldBe("7");
            m.CalculationParameters[CalculationParameter.CorrespondingMeasurementDefinitionId].ShouldBe("57");

            if (m is HeightOfWaterChannelCalculationModel model)
            {
                model.Density.ShouldBe(998.22222);
                model.Gravity.ShouldBe(9.80665);
                model.BarometricPressureChannel.MeasurementDefinitionId.ShouldBe(7);
                model.CorrespondingChannel.MeasurementDefinitionId.ShouldBe(57);
            }



        }
    }
}