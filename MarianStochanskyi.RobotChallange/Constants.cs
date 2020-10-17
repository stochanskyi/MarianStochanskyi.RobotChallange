using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarianStochanskyi.RobotChallange
{
    static class Constants
    {

        public static String OWNER = "Marian Stochanskyi";

        public static int FIGHTING_ENERGY_LOOSING = 20;

        public static float FIGHTING_RECEIVING_PERCENTAGE = 0.3f;

        public static int ENERGY_COLLECTING_DISTANCE = 4;

        //Cloning
        public static int CLONING_ENERGY_LOOSING = 1000;

        public static float CLONING_ENERGY_DIVIDING_PERCENTAGE = 0.33f;

        public static int CLONE_ENERGY_RECOMENDED = (int)((600 /  CLONING_ENERGY_DIVIDING_PERCENTAGE) + CLONING_ENERGY_LOOSING);
    }
}
