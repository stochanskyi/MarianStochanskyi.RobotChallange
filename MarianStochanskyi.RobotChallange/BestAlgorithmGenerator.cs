using MarianStochanskyi.RobotChallange.CellFinder;
using Robot.Common;
using System;
using System.Collections.Generic;
using Machine = Robot.Common.Robot;

namespace MarianStochanskyi.RobotChallange
{
    class BestAlgorithmGenerator
    {
        private readonly Machine currentMachine;

        private Utils utils = new Utils();

        public BestAlgorithmGenerator(Machine currentMachine)
        {
            this.currentMachine = currentMachine;
        }


        public RobotCommand GenerateAction(IList<Machine> robots, Map map)
        {
            int attackProfit = -1;
            Position attackPosition = calculateClosestRobotPositionWithProfit(robots, ref attackProfit);

            if (attackProfit > 0) return GenerateMovementCommand(attackPosition);

            if (WorseToClone())
            {
                return new CreateNewRobotCommand()
                {
                    NewRobotEnergy =
                    (int)(currentMachine.Energy * Constants.CLONING_ENERGY_DIVIDING_PERCENTAGE)
                };
            }

            if (IsEnergyCollectingAvailable(map.Stations)) return GenerateCollectionCommand();

            Position closestStation = GetClosestStationPosition(map, robots);

            if (closestStation != null) return GenerateMovementCommand(closestStation);

            return new MoveCommand() { NewPosition = currentMachine.Position };

        }


        private bool WorseToClone()
        {
            //return currentMachine.Energy >= Constants.CLONE_ENERGY_RECOMENDED;
            return false;
        }


        private Position calculateClosestRobotPositionWithProfit(IList<Machine> robots, ref int profit)
        {

            Position closestPosition = null;
            int attackProfit = -1;

            foreach (Machine machine in robots)
            {
                if (IsMyRobot(machine)) continue;

                int currentAttackProfit = CalculateAtackProfit(machine);
                if (currentAttackProfit <= 0) continue;

                if (attackProfit < currentAttackProfit)
                {
                    closestPosition = machine.Position;
                    attackProfit = currentAttackProfit;
                }
            }

            profit = attackProfit;
            return closestPosition;
        }


        private bool IsEnergyCollectingAvailable(IList<EnergyStation> stations)
        {
            foreach (EnergyStation station in stations)
            {
                if (station.Energy <= 0) continue;

                if (utils.CalculateDistance(station.Position, currentMachine.Position) <= Constants.ENERGY_COLLECTING_DISTANCE)
                {
                    return true;
                }
            }

            return false;
        }

        private Position GetClosestStationPosition(Map map, IList<Machine> robots)
        {
            Position closestPosition = null;
            int closestDistance = -1;
            EnergyStation closestStation = null;

            foreach (EnergyStation station in map.Stations)
            {
                if (closestPosition == null)
                {
                    closestPosition = station.Position;
                    closestDistance = utils.CalculateDistance(currentMachine.Position, station.Position);
                    closestStation = station;
                    continue;
                }

                int distance = utils.CalculateDistance(currentMachine.Position, station.Position);

                if (distance < closestDistance)
                {
                    closestPosition = station.Position;
                    closestDistance = distance;
                    closestStation = station;
                }
            }

            var closestFreePosition = new CellSearcher(closestStation, robots).Calculate(currentMachine.Position);

            if (closestFreePosition != null) return closestFreePosition;
            else return map.FindFreeCell(closestPosition, robots);

        }

        //If it is no energy to perform the movement returns -1
        //If there is no profit from atack returns 0
        //Else returns energy profit
        private int CalculateAtackProfit(Machine robot)
        {
            int lostEnergy = utils.CalculateEnergyToMove(currentMachine.Position, robot.Position) + Constants.FIGHTING_ENERGY_LOOSING;
            if (currentMachine.Energy < lostEnergy) return -1;

            int gotEnergy = (int)(robot.Energy * 0.3);

            if (lostEnergy >= gotEnergy) return 0;
            else return gotEnergy - lostEnergy;
        }

        private RobotCommand GenerateMovementCommand(Position position)
        {
            return new MoveCommand() { NewPosition = position };
        }

        private RobotCommand GenerateCollectionCommand()
        {
            return new CollectEnergyCommand();
        }

        private bool IsMyRobot(Machine robot) => robot.OwnerName == currentMachine.OwnerName;

    }
}