using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarianStochanskyi.RobotChallange
{
    class Utils
    {

        public int CalculateEnergyToMove(Position currentPosition, Position aimPosition)
        {
            int XDelta = aimPosition.X - currentPosition.X;
            int YDelta = aimPosition.Y - currentPosition.Y;

            return (int)(Math.Pow(XDelta, 2) + Math.Pow(YDelta, 2));
        }

        public int CalculateDistance(Position Position1, Position Position2)
        {
            return Math.Abs(Position1.X - Position2.X) + Math.Abs(Position1.Y - Position2.Y);
        }

        private bool HasEnaughEnergy(EnergyStation Station) 
        {
            return Station.Energy >= 50;
        }


    }
}
