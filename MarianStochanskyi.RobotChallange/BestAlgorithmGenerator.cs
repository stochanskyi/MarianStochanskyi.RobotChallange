using Robot.Common;
using System;
using System.Collections.Generic;
using MarianStochanskyi.RobotChallenge.CellFinder;
using Machine = Robot.Common.Robot;

namespace MarianStochanskyi.RobotChallenge
{
    public class BestAlgorithmGenerator
    {
        private readonly Machine currentMachine;

        private readonly Utils utils = new Utils();

        public BestAlgorithmGenerator(Machine currentMachine)
        {
            this.currentMachine = currentMachine;
        }


        public RobotCommand GenerateAction(IList<Machine> robots, Map map)
        {

            if (WorseToClone(robots))
            {
                return new CreateNewRobotCommand()
                {
                    NewRobotEnergy =
                    (int)(currentMachine.Energy * Constants.CLONING_ENERGY_DIVIDING_PERCENTAGE)
                };
            }

            int attackProfit = -1;
            Position attackPosition = calculateClosestRobotPositionWithProfit(robots, ref attackProfit);
            int collectProfit = -1;
            Position collectPosition = CalculateClosestStationWithProfit(map, robots, ref collectProfit);

            if (collectProfit <= 0 && attackProfit <= 0)
            {
                return GenerateMovementCommand(currentMachine.Position);
            }

            if (collectProfit > attackProfit)
            {
                if (collectPosition == currentMachine.Position)
                {
                    return GenerateCollectionCommand();
                }
                else 
                {
                    return GenerateMovementCommand(collectPosition);
                }
            } 
            else
            {
                return GenerateMovementCommand(attackPosition);
            }
        }


        private bool WorseToClone(IList<Machine> robots)
        {
            var robotsCount = 0;
            foreach (Machine robot in robots)
            {
                if (IsMyRobot(robot)) robotsCount++;
            }
            return currentMachine.Energy >= Constants.CLONE_ENERGY_RECOMENDED && robotsCount < Constants.MAX_ROBOTS_COUNT;
        }


        private Position calculateClosestRobotPositionWithProfit(IList<Machine> robots, ref int profit)
        {

            Position closestPosition = null;
            var attackProfit = -1;

            foreach (Machine machine in robots)
            {
                if (IsMyRobot(machine)) continue;

                var currentAttackProfit = CalculateAttackProfit(machine);
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

        private Position CalculateClosestStationWithProfit(Map map, IList<Machine> robots, ref int profit)
        {
            EnergyStation bestStation = null;
            int collectProfit = -1;

            foreach (EnergyStation station in map.Stations)
            {
                var stationProfit = CalculateStationProfit(map, station, robots);

                if (bestStation == null)
                {
                    collectProfit = stationProfit;
                    bestStation = station;
                    continue;
                }
                if (collectProfit == stationProfit && !IsEnergyCollectionAvailable(bestStation) && IsEnergyCollectionAvailable(station))
                {
                    collectProfit = stationProfit;
                    bestStation = station;
                }
                if (collectProfit < stationProfit)
                {
                    collectProfit = stationProfit;
                    bestStation = station;
                }
            }

            profit = collectProfit;
            if (IsEnergyCollectionAvailable(bestStation)) return currentMachine.Position;
            return ClosestStationFreePosition(map, bestStation, robots);
        }

        private int CalculateStationProfit(Map map, EnergyStation station, IList<Machine> robots)
        {
            int energyLost = 0;
            if (new UsersCounter(robots, station).Count() >= 2)
            {
                return -1;
            }
            if (IsEnergyCollectionAvailable(station))
            {
                energyLost = 0;
            }
            else
            {
                energyLost = utils.CalculateEnergyToMove(currentMachine.Position, ClosestStationFreePosition(map, station, robots));
            }

            if (currentMachine.Energy < energyLost) return -1;

            var potentiallyReceivedEnergy = Math.Min(station.Energy, 500) / 12;

            return potentiallyReceivedEnergy - energyLost;
        }

        private bool IsEnergyCollectionAvailable(EnergyStation station)
        {
            if (station == null) return false;
            return utils.CalculateDistance(station.Position, currentMachine.Position) <= Constants.ENERGY_COLLECTING_DISTANCE;
        }

        private Position ClosestStationFreePosition(Map map, EnergyStation station, IList<Machine> robots)
        {
            var closestFreePosition = new CellSearcher(station, robots).Calculate(currentMachine.Position);

            return closestFreePosition;
        }

        //If it is no energy to perform the movement returns -1
        //If there is no profit from atack returns 0
        //Else returns energy profit
        private int CalculateAttackProfit(Machine robot)
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