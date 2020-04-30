using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class Ant
    {
        public List<int> visitedCitiesIdList { get; set; }
        public int antID { get; set; }
        public int currentCityID { get; set; }
        public float distance { get; set; }

        public Ant(int startingCityID)
        {
            distance = 0.0f;
            currentCityID = startingCityID;
            visitedCitiesIdList = new List<int>() { startingCityID };
        }
    }
}
