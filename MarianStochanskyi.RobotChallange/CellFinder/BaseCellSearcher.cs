using Robot.Common;
using System;
using System.Collections.Generic;

namespace MarianStochanskyi.RobotChallenge.CellFinder
{
    abstract class BaseCellSearcher
    {
        protected readonly EnergyStation station;
        protected readonly IList<Robot.Common.Robot> robots;

        protected Utils utils = new Utils();

        protected BaseCellSearcher(EnergyStation station, IList<Robot.Common.Robot> robots)
        {
            this.station = station;
            this.robots = robots;
        }
        public Position Calculate(Position position)
        {
            var startPosition = GetStartPosition(); 

            Position closestPosition = null;
            int closesDistance = 0;

            while (startPosition.X != station.Position.X)
            {
                var checkingPosition = startPosition.Copy();

                while (checkingPosition.X != station.Position.X)
                {
                    if (isPositionTaken(checkingPosition))
                    {
                        MoteToNextPosition(checkingPosition);
                        continue;
                    }

                    var checkingDistance = utils.CalculateEnergyToMove(position, checkingPosition);

                    if (closestPosition == null || closesDistance > checkingDistance)
                    {
                        closestPosition = checkingPosition.Copy();
                        closesDistance = checkingDistance;
                    }

                    MoteToNextPosition(checkingPosition);

                }

                MoveToNextStartPosition(startPosition);
            }

            return closestPosition;
        }

        protected abstract void MoveToNextStartPosition(Position position);

        protected abstract void MoteToNextPosition(Position position);

        protected abstract Position GetStartPosition();

        protected Boolean isPositionTaken(Position position)
        {
            foreach (Robot.Common.Robot robot in robots)
            {
                if (robot.Position == position) return true;
            }
            return false;
        }
    }
}
