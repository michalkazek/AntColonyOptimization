using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    static class DistanceChecker
    {
        public static string CheckIsBestRouteCorrect(Action<Ant, int> MoveAntToNextCity, List<int> bestRouteFound, float bestDistanceFound)
        {
            var checkingList = bestRouteFound;
            checkingList.Add(checkingList.First());
            checkingList.RemoveAt(0);
            var ant = new Ant(checkingList.Last());

            for (int i = 0; i < checkingList.Count; i++)
            {
                MoveAntToNextCity(ant, checkingList[i]);
            }

            if(bestDistanceFound - ant.distance == 0)
            {
                return "CORRECT";
            }
            else
            {
                throw new Exception("INCORRECT");
            }            
        }
    }
}
