using KellerAg.Shared.Entities.Device;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace KellerAg.Shared.Entities.Tests.Device
{
    [TestClass]
    public class DeviceInfoTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }
        [TestMethod]
        public void GetInfo_WhenClassAndGroupHaveAre10And2_ThenLeoRecordIsReturned()
        {
            var info = DeviceInfo.GetInfo("10.2");

            info.Class.ShouldBe(10);
            info.Group.ShouldBe(2);
        }

        [TestMethod]
        public void GetInfo_WhenClassAndGroupIsNull_ThenTheDefaultIsReturned()
        {
            var info = DeviceInfo.GetInfo(null);

            info.Class.ShouldBe(0);
            info.Group.ShouldBe(0);
        }

        [TestMethod]
        public void GetInfo_WhenClassAndGroupIsCorrupted_ThenTheDefaultIsReturned()
        {
            var info = DeviceInfo.GetInfo("10.");

            info.Class.ShouldBe(0);
            info.Group.ShouldBe(0);
        }
    }
}