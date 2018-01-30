using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public async Task SetTemperatureCelciusTest()
        {
            // Get the current settings in the zone so we can set it back again
            var zone = await session.GetSummarizedZoneState(HomeId, ZoneId);

            // Test setting the zone to another temperature
            var response = await session.SetTemperatureCelsius(HomeId, ZoneId, 5.5);
            Assert.IsNotNull(response, "Failed to set the temperature of a zone");

            if (zone.Setting.Temperature.Celsius.HasValue)
            {
                // Set the zone back to its original temperature
                await session.SetTemperatureCelsius(HomeId, ZoneId, zone.Setting.Temperature.Celsius.Value);
            }
        }

        /// <summary>
        /// Test if the temperature can be set in Fahrenheit
        /// </summary>
        [TestMethod]
        public async Task SetTemperatureFahrenheitTest()
        {
            // Get the current settings in the zone so we can set it back again
            var zone = await session.GetSummarizedZoneState(HomeId, ZoneId);

            // Test setting the zone to another temperature
            var response = await session.SetTemperatureFahrenheit(HomeId, ZoneId, 42);
            Assert.IsNotNull(response, "Failed to set the temperature of a zone");

            if (zone.Setting.Temperature.Fahrenheit.HasValue)
            {
                // Set the zone back to its original temperature
                await session.SetTemperatureFahrenheit(HomeId, ZoneId, zone.Setting.Temperature.Fahrenheit.Value);
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
            if(zone.Setting.Power.Equals("ON", System.StringComparison.InvariantCultureIgnoreCase))
            {
                response = await session.SwitchHeatingOff(HomeId, ZoneId);
            }
            else
            {
                response = await session.SetTemperatureCelsius(HomeId, ZoneId, 10);
            }
            Assert.IsNotNull(response, "Failed to switch the heating in a zone");

            // Switch the heating setting back to its initial value
            if (zone.Setting.Power.Equals("ON", System.StringComparison.InvariantCultureIgnoreCase))
            {
                await session.SetTemperatureCelsius(HomeId, ZoneId, zone.Setting.Temperature.Celsius.Value);
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
    }
}