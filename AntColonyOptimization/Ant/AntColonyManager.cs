using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    static class AntColonyManager
    {
        public static Ant CreateNewAnt(int startingCityID)
        {
            return new Ant(startingCityID);
        }
    }
}
