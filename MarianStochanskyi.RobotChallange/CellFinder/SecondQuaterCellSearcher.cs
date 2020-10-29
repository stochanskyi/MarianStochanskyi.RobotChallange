using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarianStochanskyi.RobotChallenge.CellFinder
{
    class SecondQuaterCellSearcher : BaseCellSearcher
    {
        public SecondQuaterCellSearcher(EnergyStation station, IList<Robot.Common.Robot> robots) : base(station, robots)
        {
        }

        protected override Position GetStartPosition()
        {
            return new Position(station.Position.X - Constants.ENERGY_COLLECTING_DISTANCE, station.Position.Y);
        }

        protected override void MoteToNextPosition(Position position)
        {
            position.Y--;
            position.X++;
        }

        protected override void MoveToNextStartPosition(Position position)
        {
            position.X++;
        }
    }
}
