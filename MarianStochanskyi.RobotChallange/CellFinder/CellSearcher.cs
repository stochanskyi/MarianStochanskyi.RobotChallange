using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarianStochanskyi.RobotChallenge.CellFinder
{
    public class CellSearcher
    {
        protected readonly EnergyStation station;
        protected readonly IList<Robot.Common.Robot> robots;

        public CellSearcher(EnergyStation station, IList<Robot.Common.Robot> robots)
        {
            this.station = station;
            this.robots = robots;
        }

        public Position Calculate(Position currentPosition)
        {
            if (currentPosition.X >= station.Position.X && currentPosition.Y <= station.Position.Y)
            {
                return new FirstQuaterCellSearcher(station, robots).Calculate(currentPosition);
            } 
            else if (currentPosition.X <= station.Position.X && currentPosition.Y <= station.Position.Y)
            {
                return new SecondQuaterCellSearcher(station, robots).Calculate(currentPosition);
            }
            else if (currentPosition.X <= station.Position.X && currentPosition.Y >= station.Position.Y)
            {
                return new ThirdQuaterCellSearcher(station, robots).Calculate(currentPosition);
            }
            else if (currentPosition.X >= station.Position.X && currentPosition.Y >= station.Position.Y)
            {
                return new FourthQuaterCellSearcher(station, robots).Calculate(currentPosition);
            }
            return null;
        }
    }
}
