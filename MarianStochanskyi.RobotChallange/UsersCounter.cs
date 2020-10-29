using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarianStochanskyi.RobotChallenge
{
    public class UsersCounter
    {
        private readonly EnergyStation station;
        private readonly IList<Robot.Common.Robot> robots;

        public UsersCounter(IList<Robot.Common.Robot> robots, EnergyStation station)
        {
            this.station = station;
            this.robots = robots;
        }

        public int Count()
        {
            var count = 0;

            var yValue = station.Position.Y + Constants.ENERGY_COLLECTING_DISTANCE;

            while (yValue >= station.Position.Y - Constants.ENERGY_COLLECTING_DISTANCE)
            {
                var yDelta = Math.Abs(yValue - station.Position.Y);
                var xDelta = Constants.ENERGY_COLLECTING_DISTANCE - yDelta;
                var xValue = station.Position.X - xDelta;
                var endXValue = station.Position.X + xDelta;

                while (xValue != endXValue)
                {
                    if (IsPositionTaken(new Position(xValue, yValue)))
                    {
                        count++;
                    }
                    xValue++; 
                }

                yValue--;
            }

            return count;
        }

        private bool IsPositionTaken(Position position)
        {
            foreach (Robot.Common.Robot robot in robots)
            {
                if (position == robot.Position) return true;
            }

            return false;
        }

    }
}
