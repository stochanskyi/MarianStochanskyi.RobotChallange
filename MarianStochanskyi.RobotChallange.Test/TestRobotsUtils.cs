using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;

namespace MarianStochanskyi.RobotChallenge.Test
{
    [TestClass]
    public class TestRobotsUtils
    {
        [TestMethod]
        public void TestCalculateEnergyToMove()
        {
            var utils = new Utils();

            var firstPosition = new Position(34, 12);
            var secondPosition = new Position(37, 14);

            var expectedResult = 13;
            var actualResult = utils.CalculateEnergyToMove(firstPosition, secondPosition);

            Assert.AreEqual(expectedResult, actualResult);

            secondPosition.Y = 12;

            expectedResult = 9;
            actualResult = utils.CalculateEnergyToMove(firstPosition, secondPosition);

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}