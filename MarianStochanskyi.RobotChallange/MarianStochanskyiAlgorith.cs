using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarianStochanskyi.RobotChallenge
{
    public class MarianStochanskyiAlgorith : IRobotAlgorithm
    {
        public string Author => Constants.OWNER;

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            return new BestAlgorithmGenerator(robots[robotToMoveIndex]).GenerateAction(robots, map);
        }
    }

}