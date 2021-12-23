using ScenarioAlerter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScenarioAlerter.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void RemovesTimestampFromMessage()
        {
            // arrange
            var logMessage = "[21/12/23][13:32:01] Pop! Scenario: Doomfist Crater";
            var expectedLogMessage = "Pop! Scenario: Doomfist Crater";
            // act
            var cleanLogMessage = Program.RemoveTimestampFromLogMessage(logMessage);
            // assert
            Assert.AreEqual(expectedLogMessage, cleanLogMessage);
        }
    }
}
