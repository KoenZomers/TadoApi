using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace KoenZomers.Tado.Api.UnitTest
{
    /// <summary>
    /// Unit Tests for changing settings through the Tado API
    /// </summary>
    [TestClass]
    public class DataSetTest : BaseTest
    {
        /// <summary>
        /// Tado Session to use for all tests
        /// </summary>
        private static Session session;

        /// <summary>
        /// Sets up a session to be used by all test methods in this class
        /// </summary>
        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext) 
        {
            session = new Session(Username, Password);
            await session.Authenticate();
        }

        /// <summary>
        /// Cleans up the session that was used by all test methods in this class
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup() 
        {
            session.Dispose();
        }

        /// <summary>
        /// Test if the temperature can be set in Celsius
        /// </summary>
        [TestMethod]
        public async Task SetHeatingTemperatureCelciusTest()
        {
            // Get the current settings in the zone so we can set it back again
            var zone = await session.GetSummarizedZoneState(HomeId, ZoneId);

            // Test setting the zone heating temperature to another temperature
            var response = await session.SetHeatingTemperatureCelsius(HomeId, ZoneId, 5.5);
            Assert.IsNotNull(response, "Failed to set the temperature of a zone");

            if (zone.Setting.Temperature.Celsius.HasValue)
            {
                // Set the zone heating temperature back to its original temperature
                await session.SetHeatingTemperatureCelsius(HomeId, ZoneId, zone.Setting.Temperature.Celsius.Value);
            }
        }

        /// <summary>
        /// Test if the temperature can be set in Fahrenheit
        /// </summary>
        [TestMethod]
        public async Task SetHeatingTemperatureFahrenheitTest()
        {
            // Get the current settings in the zone so we can set it back again
            var zone = await session.GetSummarizedZoneState(HomeId, ZoneId);

            // Test setting the zone heating temperature to another temperature
            var response = await session.SetHeatingTemperatureFahrenheit(HomeId, ZoneId, 42);
            Assert.IsNotNull(response, "Failed to set the temperature of a zone");

            if (zone.Setting.Temperature.Fahrenheit.HasValue)
            {
                // Set the zone heating temperature back to its original temperature
                await session.SetHeatingTemperatureFahrenheit(HomeId, ZoneId, zone.Setting.Temperature.Fahrenheit.Value);
            }
        }

        /// <summary>
        /// Test if the heating can be switched off
        /// </summary>
        [TestMethod]
        public async Task SwitchHeatingOffTest()
        {
            // Get the current settings in the zone so we can set it back again
            var zone = await session.GetSummarizedZoneState(HomeId, ZoneId);

            Entities.ZoneSummary response;
            if(zone.Setting.Power == Enums.PowerStates.On)
            {
                response = await session.SwitchHeatingOff(HomeId, ZoneId);
            }
            else
            {
                response = await session.SetHeatingTemperatureCelsius(HomeId, ZoneId, 10);
            }
            Assert.IsNotNull(response, "Failed to switch the heating in a zone");

            // Switch the heating setting back to its initial value
            if (zone.Setting.Power == Enums.PowerStates.On)
            {
                await session.SetHeatingTemperatureCelsius(HomeId, ZoneId, zone.Setting.Temperature.Celsius.Value);
            }
            else
            {
                await session.SwitchHeatingOff(HomeId, ZoneId);
            }
        }

        /// <summary>
        /// Test if the heating can be switched off
        /// </summary>
        [TestMethod]
        public async Task SetEarlyStartTest()
        {
            // Get the current settings in the zone so we can set it back again
            var earlyStart = await session.GetEarlyStart(HomeId, ZoneId);

            // Switch the EarlyStart setting
            var response = await session.SetEarlyStart(HomeId, ZoneId, !earlyStart.Enabled);
            Assert.IsNotNull(response, "Failed to switch the EarlyStart setting of a zone");

            // Switch the EarlyStart setting back to its initial value
            await session.SetEarlyStart(HomeId, ZoneId, earlyStart.Enabled);
        }

        /// <summary>
        /// Test if showing Hi works on a device
        /// </summary>
        [TestMethod]
        public async Task SayHiTest()
        {
            var success = await session.SayHi(DeviceId);
            Assert.IsTrue(success, "Failed to display Hi on a Tado device");
        }

        /// <summary>
        /// Test if the temperature of the hot water boiler can be set in Fahrenheit
        /// </summary>
        [TestMethod]
        public async Task SetHotWaterTemperatureFahrenheitTest()
        {
            // Get the current settings in the zone so we can set it back again. Assuming that hot water is always zone 0. Have yet to verify this.
            var zone = await session.GetSummarizedZoneState(HomeId, 0);

            // Test setting the hot water boiler temperature to another temperature
            var response = await session.SetHotWaterTemperatureFahrenheit(HomeId, 115, Enums.DurationModes.UntilNextManualChange);
            Assert.IsNotNull(response, "Failed to set the temperature of the hot water boiler");

            if (zone.Setting.Power == Enums.PowerStates.On)
            {
                // Set the hot water boiler temperature back to its original temperature
                await session.SetHotWaterTemperatureFahrenheit(HomeId, zone.Setting.Temperature.Fahrenheit.Value, zone.Termination.CurrentType.Value);
            }
            else
            {
                // Switch the hot water boiler back off again
                response = await session.SwitchHotWaterOff(HomeId, zone.Termination.CurrentType.Value);
            }
        }

        /// <summary>
        /// Test if the temperature of the hot water boiler can be set in Celcius
        /// </summary>
        [TestMethod]
        public async Task SetHotWaterTemperatureCelciusTest()
        {
            // Get the current settings in the zone so we can set it back again. Assuming that hot water is always zone 0. Have yet to verify this.
            var zone = await session.GetSummarizedZoneState(HomeId, 0);

            // Test setting the hot water boiler temperature to another temperature
            var response = await session.SetHotWaterTemperatureCelcius(HomeId, 45, Enums.DurationModes.UntilNextManualChange);
            Assert.IsNotNull(response, "Failed to set the temperature of the hot water boiler");

            if (zone.Setting.Power == Enums.PowerStates.On)
            {
                // Set the hot water boiler temperature back to its original temperature
                await session.SetHotWaterTemperatureCelcius(HomeId, zone.Setting.Temperature.Celsius.Value, zone.Termination.CurrentType.Value);
            }
            else
            {
                // Switch the hot water boiler back off again
                response = await session.SwitchHotWaterOff(HomeId, zone.Termination.CurrentType.Value);
            }
        }

        /// <summary>
        /// Test if the hot water boiler can be switched off
        /// </summary>
        [TestMethod]
        public async Task SwitchHotWaterOffTest()
        {
            // Get the current settings in the zone so we can set it back again. Assuming that hot water is always zone 0. Have yet to verify this.
            var zone = await session.GetSummarizedZoneState(HomeId, 0);

            Entities.ZoneSummary response;
            if (zone.Setting.Power == Enums.PowerStates.On)
            {
                response = await session.SwitchHotWaterOff(HomeId, Enums.DurationModes.UntilNextManualChange);
            }
            else
            {
                response = await session.SetHotWaterTemperatureCelcius(HomeId, 45, Enums.DurationModes.UntilNextManualChange);
            }
            Assert.IsNotNull(response, "Failed to switch the temperature of the hot water boiler");

            // Switch the heating setting back to its initial value
            if (zone.Setting.Power == Enums.PowerStates.On)
            {
                await session.SetHotWaterTemperatureCelcius(HomeId, zone.Setting.Temperature.Celsius.Value, zone.Termination.CurrentType.Value);
            }
            else
            {
                await session.SwitchHotWaterOff(HomeId, zone.Termination.CurrentType.Value);
            }
        }

        [TestMethod]
        public async Task SwitchHomePresenceAwayThenHomeTest()
        {
            Assert.IsTrue(await session.SetHomePresence(HomeId, Enums.HomePresence.Away));

            Assert.IsTrue(await session.SetHomePresence(HomeId, Enums.HomePresence.Home));
        }

        /// <summary>
        /// Test if a zone temperature offset in Celsius can be set
        /// </summary>
        [TestMethod]
        public async Task SetZoneTemperatureOffsetInCelciusTest()
        {
            // Get the zones
            var zones = await session.GetZones(HomeId);

            if (zones == null || zones.Length == 0 || zones[0].Devices == null | zones[0].Devices.Length == 0)
            {
                Assert.Inconclusive("Test inconclusive as the test data is not valid");
            }

            var zone = zones.FirstOrDefault(z => z.Id == ZoneId);

            if(zone == null)
            {
                Assert.Inconclusive($"Failed to retrive zone with Id {ZoneId}");
            }

            // Get the currently set offset
            var currentOffset = await session.GetZoneTemperatureOffset(zone.Devices[0]);

            // Test setting the offset of the first zone
            var response = await session.SetZoneTemperatureOffsetCelcius(zone.Devices[0], 1);
            Assert.IsNotNull(response, "Failed to set the offset temperature of a zone");

            if (currentOffset != null)
            {
                // Set the zone temperature offset back to its original offset
                await session.SetZoneTemperatureOffsetCelcius(zone.Devices[0], currentOffset.Celsius.Value);
            }
        }

        /// <summary>
        /// Test if a zone temperature offset in Fahrenheit can be set
        /// </summary>
        [TestMethod]
        public async Task SetZoneTemperatureOffsetInFahrenheit()
        {
            // Get the zones
            var zones = await session.GetZones(HomeId);

            if (zones == null || zones.Length == 0 || zones[0].Devices == null | zones[0].Devices.Length == 0)
            {
                Assert.Inconclusive("Test inconclusive as the test data is not valid");
            }

            var zone = zones.FirstOrDefault(z => z.Id == ZoneId);

            if (zone == null)
            {
                Assert.Inconclusive($"Failed to retrive zone with Id {ZoneId}");
            }

            // Get the currently set offset
            var currentOffset = await session.GetZoneTemperatureOffset(zone.Devices[0]);

            // Test setting the offset of the first zone
            var response = await session.SetZoneTemperatureOffsetFahrenheit(zone.Devices[0], 1.8);
            Assert.IsNotNull(response, "Failed to set the offset temperature of a zone");

            if (currentOffset != null)
            {
                // Set the zone temperature offset back to its original offset
                await session.SetZoneTemperatureOffsetFahrenheit(zone.Devices[0], currentOffset.Fahrenheit.Value);
            }
        }
    }
}
