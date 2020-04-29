using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class Application
    {
        public static int matrixSize;
        public static float alfa;
        public static float beta;

        static public void Run()
        {
            FileReader fileReader = new FileReader();       
            var distanceMatrix = fileReader.CreateDistanceMatrix();
            matrixSize = fileReader.GetMatrixSize();
            PheromoneMatrixManager phm = new PheromoneMatrixManager(matrixSize);
            
            alfa = 2;
            beta = 5;


            Start(10, phm, distanceMatrix);

            Console.ReadKey();
        }
        static public void Start(int numberOfAnts, PheromoneMatrixManager phm, int[,] distanceMatrix)
        {            
            var randomGenerator = new Random();
            List<Ant> ants = CreateAntColony(randomGenerator, numberOfAnts);

            for (int i = 0; i < numberOfAnts; i++)
            {
                CalculateNextCitiesProbability(ants[i], phm, distanceMatrix);
            }
        }

        static private List<Ant> CreateAntColony(Random randomGenerator, int numberOfAnts)
        {
            List<Ant> ants = new List<Ant>();

            for (int i = 0; i < numberOfAnts; i++)
            {
                var drawnNumber = randomGenerator.Next(matrixSize);
                ants.Add(AntColonyManager.CreateNewAnt(drawnNumber));
            }
            return ants;
        }

        static private void CalculateNextCitiesProbability(Ant ant, PheromoneMatrixManager phm, int[,] distanceMatrix)
        {
            var probabilityList = new List<double>();
            double denominator = 0.0;
            var unvisitedCitiesNumber = matrixSize - ant.visitedCitiesIdList.Count;

            for (int cityID = 0; cityID < unvisitedCitiesNumber; cityID++)
            {
                denominator += Math.Pow(phm.PheromoneMatrix[ant.currentCityID, cityID], alfa) * Math.Pow(phm.PheromoneMatrix[ant.currentCityID, cityID], beta);
            }
            for (int cityID = 0; cityID < unvisitedCitiesNumber; cityID++)
            {
                var numerator = 0.0;
                if(cityID != ant.currentCityID)
                {
                    numerator = Math.Pow(phm.PheromoneMatrix[ant.currentCityID, cityID], alfa) * Math.Pow(1 / Convert.ToDouble(distanceMatrix[ant.currentCityID, cityID]), beta);
                }
                else
                {
                    numerator = 0;
                }

                probabilityList.Add(Math.Round((numerator / denominator), 4));
            }
        }
    }
}
