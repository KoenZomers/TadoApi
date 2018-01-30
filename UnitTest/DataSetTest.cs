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
            var response = await session.SetTemperatureCelsius(HomeId, ZoneId, 5.5);
            Assert.IsNotNull(response, "Failed to set the temperature of a zone");
        }

        /// <summary>
        /// Test if the temperature can be set in Fahrenheit
        /// </summary>
        [TestMethod]
        public async Task SetTemperatureFahrenheitTest()
        {
            var response = await session.SetTemperatureFahrenheit(HomeId, ZoneId, 42);
            Assert.IsNotNull(response, "Failed to set the temperature of a zone");
        }

        /// <summary>
        /// Test if the heating can be switched off
        /// </summary>
        [TestMethod]
        public async Task SwitchHeatingOffTest()
        {
            var response = await session.SwitchHeatingOff(HomeId, ZoneId);
            Assert.IsNotNull(response, "Failed to switch off the heating in a zone");
        }
    }
}