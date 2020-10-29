using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;

namespace MarianStochanskyi.RobotChallenge.Test
{
    [TestClass]
    public class TestUsersCounter
    {
        [TestMethod]
        public void TestCount()
        {
            var primaryStation = new EnergyStation() {Position = new Position(33, 22), Energy = 260};


            var robots = GenerateFourRobotsAroundStation(primaryStation);

            var expectedResult = 4;

            var actualResult = new UsersCounter(robots, primaryStation).Count();

            Assert.AreEqual(expectedResult, actualResult);
        }

        private IList<Robot.Common.Robot> GenerateFourRobotsAroundStation(EnergyStation station)
        {
            return new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() {Position = station.Position.Copy()},
                new Robot.Common.Robot() {Position = new Position(station.Position.X, station.Position.Y + 3)},
                new Robot.Common.Robot() {Position = new Position(station.Position.X - 4, station.Position.Y)},
                new Robot.Common.Robot() {Position = new Position(station.Position.X - 2, station.Position.Y + 2)},
                
                //Robots out of range
                new Robot.Common.Robot() {Position = new Position(station.Position.X - 4, station.Position.Y - 1)},
                new Robot.Common.Robot() {Position = new Position(station.Position.X + 3, station.Position.Y - 2)},
            };
        }
    }
}
