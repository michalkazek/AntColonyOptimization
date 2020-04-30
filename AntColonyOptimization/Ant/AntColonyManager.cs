using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    static class AntColonyManager
    {
        private static Ant CreateNewAnt(int startingCityID)
        {
            return new Ant(startingCityID);
        }

        public static List<Ant> CreateAntColony(int numberOfAnts, int matrixSize, Random randomGenerator)
        {
            List<Ant> antColony = new List<Ant>();

            for (int i = 0; i < numberOfAnts; i++)
            {
                var drawnNumber = randomGenerator.Next(matrixSize);
                antColony.Add(CreateNewAnt(drawnNumber));
            }
            return antColony;
        }

        public static void ResetAntColonyMemory(List<Ant> antColony, int matrixSize, Random randomGenerator)
        {
            antColony.ForEach(ant => {
                ant.distance = 0.0f;
                ant.currentCityID = randomGenerator.Next(matrixSize);
                ant.visitedCitiesIdList.Clear();
                ant.visitedCitiesIdList.Add(ant.currentCityID);
            });
        }
    }
}
