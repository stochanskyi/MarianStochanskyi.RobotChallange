using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;

namespace MarianStochanskyi.RobotChallenge.Test
{
    using Machine = Robot.Common.Robot;
    
    [TestClass]
    public class TestBestAlgorithmGenerator
    {

        [TestMethod]
        public void TestCloneCommandGeneration()
        {
            var map = GenerateMap();

            var primaryRobot = new Robot.Common.Robot {Position = new Position(33, 22), Energy = 20};
            var robots = GenerateRobots(primaryRobot);

            var generator = new BestAlgorithmGenerator(primaryRobot);

            Assert.IsNotInstanceOfType(generator.GenerateAction(robots, map), typeof(CreateNewRobotCommand));

            primaryRobot.Energy = 3000;

            Assert.IsInstanceOfType(generator.GenerateAction(robots, map), typeof(CreateNewRobotCommand));
        }

        [TestMethod]
        public void TestAttackCommandWithoutStations()
        {
            var primaryRobot = new Machine() {Position = new Position(33, 22), Energy = 1000, OwnerName = "as"};

            var robots = new List<Machine>()
            {
                primaryRobot,
                new Machine {Position = new Position(36, 26), Energy = 1000, OwnerName = "sda"},
                new Machine {Position = new Position(32, 20), Energy = 300, OwnerName = "asd"}
            };

            var map = GenerateMap();

            var expectedResult = new MoveCommand {NewPosition = new Position(36, 26)};
            var actualResult = new BestAlgorithmGenerator(primaryRobot).GenerateAction(robots, map);

            Assert.IsInstanceOfType(actualResult, typeof(MoveCommand));
            Assert.AreEqual(expectedResult.NewPosition, (actualResult as MoveCommand).NewPosition);
        }

        [TestMethod]
        public void TestAttackCommandWithoutEnoughEnergy()
        {
            var primaryRobot = new Machine() { Position = new Position(33, 22), Energy = 10, OwnerName = "as" };

            var robots = new List<Machine>()
            {
                primaryRobot,
                new Machine {Position = new Position(36, 26), Energy = 1000, OwnerName = "sda"},
                new Machine {Position = new Position(32, 20), Energy = 300, OwnerName = "asd"}
            };

            var map = GenerateMap();

            var expectedResult = new MoveCommand { NewPosition = new Position(33, 22) };
            var actualResult = new BestAlgorithmGenerator(primaryRobot).GenerateAction(robots, map);

            Assert.IsInstanceOfType(actualResult, typeof(MoveCommand));
            Assert.AreEqual(expectedResult.NewPosition, (actualResult as MoveCommand).NewPosition);
        }

        [TestMethod]
        public void TestCollectAvailable()
        {
            var primaryRobot = new Machine() { Position = new Position(52, 88), Energy = 1000, OwnerName = "as" };
            var map = GenerateMap();

            var expectedType = typeof(CollectEnergyCommand);
            var actualResult = new BestAlgorithmGenerator(primaryRobot).GenerateAction(new List<Machine>(), map);

            Assert.IsInstanceOfType(actualResult, expectedType);
        }

        [TestMethod]
        public void TestMoveToProfitableStation()
        {
            var primaryRobot = new Machine() { Position = new Position(52, 88), Energy = 1000, OwnerName = "as" };

            var map = GenerateMap();
            map.Stations.Add(new EnergyStation() {Energy = 1500, Position = new Position(59, 88)});

            var expectedType = typeof(MoveCommand);
            var actualResult = new BestAlgorithmGenerator(primaryRobot).GenerateAction(new List<Machine>(), map);
            Assert.IsInstanceOfType(actualResult, expectedType);
        }

        private Map GenerateMap()
        {
            var stations = new List<EnergyStation>()
            {
                new EnergyStation {Energy = 89, Position = new Position(51, 90)},
                new EnergyStation {Energy = 30, Position = new Position(68, 89)},
                new EnergyStation {Energy = 280, Position = new Position(89, 93)}
            };

            return new Map {Stations = stations};
        }

        private IList<Robot.Common.Robot> GenerateRobots(Robot.Common.Robot primaryRobot)
        {
            return new List<Robot.Common.Robot>()
            {
                primaryRobot,
                new Robot.Common.Robot {Position = new Position(0, 0)},
                new Robot.Common.Robot {Position = new Position(0, 1)},
                new Robot.Common.Robot {Position = new Position(0, 2)}
            };
        }
    }
}