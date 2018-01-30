using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace KoenZomers.Tado.Api.UnitTest
{
    /// <summary>
    /// Unit Tests to validate authenticating against the Tado API
    /// </summary>
    [TestClass]
    public class AuthenticationTest : BaseTest
    {
        /// <summary>
        /// Test the scenario where the authentication should succeed
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AuthenticateSuccessTest()
        {
            var session = new Session(Username, Password);

            await session.Authenticate();
            Assert.IsNotNull(session.AuthenticatedSession, "Failed to authenticate");
        }

        /// <summary>
        /// Test the scenario where the authentication would fail
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(Exceptions.SessionAuthenticationFailedException))]
        public async Task AuthenticateFailTest()
        {
            var session = new Session("test@test.com", "someinvalidpassword");
            await session.Authenticate();
        }

        /// <summary>
        /// Test if the an SessionNotAuthenticatedException gets thrown when trying to retrieve data without authenticating first
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exceptions.SessionNotAuthenticatedException))]
        public async Task UnauthenticatedTest()
        {
            var session = new Session(Username, Password);
            await session.GetMe();
        }
    }
}