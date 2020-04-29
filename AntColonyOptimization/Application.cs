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
        public static float bestDistance;
        public static List<int> bestRoute;

        static public void Run()
        {
            FileReader fileReader = new FileReader();       
            var distanceMatrix = fileReader.CreateDistanceMatrix();
            matrixSize = fileReader.GetMatrixSize();
            PheromoneMatrixManager phm = new PheromoneMatrixManager(matrixSize);
            
            alfa = 4;
            beta = 2;
            bestDistance = 9999999;
            bestRoute = new List<int>();

        
            Start(70, phm, distanceMatrix, 100);
            Console.WriteLine($"Best ant travlled {bestDistance}");

            foreach (var item in bestRoute)
            {
                Console.Write(item + " ");
            }

            Console.ReadKey();
        }
        static public void Start(int numberOfAnts, PheromoneMatrixManager phm, int[,] distanceMatrix, int numberOfIteration)
        {            
            var randomGenerator = new Random();
            List<Ant> ants = CreateAntColony(randomGenerator, numberOfAnts);
            for (int it = 0; it < numberOfIteration; it++)
            {                
                for (int j = 0; j < matrixSize; j++)
                {
                    for (int i = 0; i < numberOfAnts; i++)
                    {
                        var probabilityList = CalculateNextCitiesProbability(ants[i], phm, distanceMatrix);
                        var nextCityForAnt = ChooseNextCityForAnt(ants[i], probabilityList, randomGenerator);
                        MoveAntToNextCity(ants[i], nextCityForAnt, phm, distanceMatrix);
                    }
                }
                PrintAllAntsDistance(ants, it);
                ResetAntMemory(ants, randomGenerator, matrixSize);
            }                                
        }

        private static void ResetAntMemory(List<Ant> ants, Random randomGenerator, int matrixSize)
        {
            foreach (var ant in ants)
            {
                ant.distance = 0.0f;
                ant.visitedCitiesIdList.Clear();
                ant.currentCityID = randomGenerator.Next(matrixSize);
            }                
        }

        private static void PrintAllAntsDistance(List<Ant> ants, int iteration)
        {
            foreach (var ant in ants)
            {
                if(ant.distance < bestDistance)
                {
                    Console.WriteLine($"Ant travelled {ant.distance} in {iteration}");
                    bestDistance = ant.distance;
                    bestRoute = new List<int>(ant.visitedCitiesIdList);
                }
            }
        }

        private static void MoveAntToNextCity(Ant ant, int nextCityForAnt, PheromoneMatrixManager phm, int[,] distanceMatrix)
        {
            var distanceBetweenCities = distanceMatrix[ant.currentCityID, nextCityForAnt];
            ant.distance += distanceBetweenCities;
            if (nextCityForAnt != ant.currentCityID)
            {
                phm.UpdateSelectedPhermoneMatrixCell(ant.currentCityID, nextCityForAnt, 1 / (Math.Pow(distanceBetweenCities, 2)));
            }                        
            ant.currentCityID = nextCityForAnt;
            ant.visitedCitiesIdList.Add(nextCityForAnt);                     
        }

        private static int ChooseNextCityForAnt(Ant ant, List<double> probabilityList, Random randomGenerator)
        {
            var probabilitySum = probabilityList.Sum();
            probabilityList = probabilityList.Select(x => Math.Round(x / probabilitySum, 10)).ToList();
            var drawnNumber = randomGenerator.NextDouble();
            int i = 0;
            var currentProbabilitySum = probabilityList[i];
            
            while(drawnNumber > currentProbabilitySum || ant.visitedCitiesIdList.Contains(i))
            {
                i++;
                currentProbabilitySum += probabilityList[i];               
            }
            return i;
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

        static private List<double> CalculateNextCitiesProbability(Ant ant, PheromoneMatrixManager phm, int[,] distanceMatrix)
        {
            var probabilityList = new List<double>();
            double denominator = 0.0;

            for (int cityID = 0; cityID < matrixSize; cityID++)
            {
                if (ant.visitedCitiesIdList.Contains(cityID))
                {
                    probabilityList.Add(0);
                }
                else
                {
                    var numerator = 0.0;
                    if (cityID != ant.currentCityID)
                    {
                        numerator = Math.Pow(phm.PheromoneMatrix[ant.currentCityID, cityID], alfa) * Math.Pow(1 / Convert.ToDouble(distanceMatrix[ant.currentCityID, cityID]), beta);
                    }
                    else
                    {
                        numerator = 0;
                    }
                    probabilityList.Add(numerator);
                    denominator += Math.Pow(phm.PheromoneMatrix[ant.currentCityID, cityID], alfa) * Math.Pow(phm.PheromoneMatrix[ant.currentCityID, cityID], beta);
                }                
            }
            return probabilityList.Select(x => Math.Round((x / denominator), 4)).ToList();
        }
    }
}
