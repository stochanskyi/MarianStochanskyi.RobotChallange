using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarianStochanskyi.RobotChallange
{
    public class MarianStochanskyiAlgorith : IRobotAlgorithm
    {
        public string Author => "Marian Stochanskyi";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            return new BestAlgorithmGenerator(robots[robotToMoveIndex]).GenerateAction(robots, map);
        }
    }

}