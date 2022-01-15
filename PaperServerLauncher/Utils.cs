using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperServerLauncher
{
    static class Utils
    {
        public static class Constants
        {
            public static int UNIT_MODE_MB = 0;
            public static int UNIT_MODE_GB = 1;
        }

        public static class Maths
        {
            public static UInt64 roundToPowerOf2(double number)
            {
                UInt64 power = 1;
                while (power < number)
                    power*= 2;
                return power / 2;
            }
        }
        
    }
}
