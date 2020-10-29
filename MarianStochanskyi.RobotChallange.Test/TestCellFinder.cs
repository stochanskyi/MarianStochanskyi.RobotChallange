using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarianStochanskyi.RobotChallenge.CellFinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;

namespace MarianStochanskyi.RobotChallenge.Test
{
    [TestClass]
    public class TestCellFinder
    {

        [TestMethod]
        public void TestFindWithoutRobotsFirstQuarter()
        {
            var robot = new Robot.Common.Robot {Position = new Position(36, 17)};

            var station = new EnergyStation {Position = new Position(33, 22)};

            var expectedResult = new Position(34, 19);
            var actualResult = new CellSearcher(station, new List<Robot.Common.Robot>()).Calculate(robot.Position);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestFindWithoutRobotsSecondQuarter()
        {
            var robot = new Robot.Common.Robot {Position = new Position(30, 17)};

            var station = new EnergyStation {Position = new Position(33, 22)};

            var expectedResult = new Position(32, 19);
            var actualResult = new CellSearcher(station, new List<Robot.Common.Robot>()).Calculate(robot.Position);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestFindWithoutRobotsThirdQuarter()
        {
            var robot = new Robot.Common.Robot { Position = new Position(39, 24) };

            var station = new EnergyStation { Position = new Position(33, 22) };

            var expectedResult = new Position(37, 22);
            var actualResult = new CellSearcher(station, new List<Robot.Common.Robot>()).Calculate(robot.Position);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestFindWithoutRobotsFourthQuarter()
        {
            var robot = new Robot.Common.Robot { Position = new Position(28, 26) };

            var station = new EnergyStation { Position = new Position(33, 22) };

            var expectedResult = new Position(30, 23);
            var actualResult = new CellSearcher(station, new List<Robot.Common.Robot>()).Calculate(robot.Position);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestWithRobots()
        {

            var robot = new Robot.Common.Robot { Position = new Position(36, 17) };
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() {Position = new Position(34, 19)}
            };

            var station = new EnergyStation { Position = new Position(33, 22) };

            var expectedResult = new Position(35, 20);
            var actualResult = new CellSearcher(station, robots).Calculate(robot.Position);

            Assert.AreEqual(expectedResult, actualResult);
        }


        //private IList<Robot.Common.Robot> GenerateRobots(Robot.Common.Robot primaryRobot)
        //{
        //}
    }
}
