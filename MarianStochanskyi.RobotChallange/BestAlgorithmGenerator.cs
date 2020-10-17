using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization.Configuration;
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

            if (IsEnergyCollectingAvailable(map.Stations)) return GenerateCollectionCommand();

            Position closestStation = GetClosestStationPosition(map.Stations);

            if (closestStation != null) return GenerateMovementCommand(closestStation);

            return new MoveCommand() { NewPosition = currentMachine.Position };

        }


        private Position calculateClosestRobotPositionWithProfit(IList<Machine> robots, ref int profit)
        {

            Position closestPosition = null;
            int attackProfit = -1;

            foreach (Machine machine in robots)
            {
                if (IsMyRobot(machine)) continue;

                int currentAttackProfit = CalculateAtackProfit(machine);
                if (attackProfit <= 0) continue;

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

        private Position GetClosestStationPosition(IList<EnergyStation> stations)
        {
            Position closestPosition = null;
            int closestDistance = -1;

            foreach (EnergyStation station in stations)
            {
                if (closestPosition == null)
                {
                    closestPosition = station.Position;
                    closestDistance = utils.CalculateDistance(currentMachine.Position, station.Position);
                    continue;
                }

                int distance = utils.CalculateDistance(currentMachine.Position, station.Position);

                if (distance < closestDistance)
                {
                    closestPosition = station.Position;
                    closestDistance = distance;
                }
            }

            return closestPosition;

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