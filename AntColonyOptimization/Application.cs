using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class Application
    {              
        private static int[,] distanceMatrix;
        private static int matrixSize;
        private static double[,] pheromoneMatrix;
        private static float alfa = 2;
        private static float beta = 5;
        private static float evaporationValue = 0.00000001f;
        private static float bestDistanceFound;
        private static List<int> bestRouteFound;
        private static Random randomGenerator;

        public static void Run()
        {            
            GeneratePrerequisite();
            Start(50, 100);
            Console.WriteLine($"Best ant travelled {bestDistanceFound}");
            DistanceChecker.CheckIsBestRouteCorrect(bestRouteFound, bestDistanceFound);

            Console.ReadKey();
        }

        private static void GeneratePrerequisite()
        {
            FileReader fileReader = new FileReader();
            distanceMatrix = fileReader.CreateDistanceMatrix();
            matrixSize = fileReader.GetMatrixSize();
            pheromoneMatrix = PheromoneMatrixManager.CreatePhermoneMatrix(matrixSize, 0.001f);
            bestDistanceFound = 9999999;
            bestRouteFound = new List<int>();
        }

        public static void Start(int numberOfAnts, int numberOfIteration)
        {            
            randomGenerator = new Random();
            List<Ant> ants = AntColonyManager.CreateAntColony(numberOfAnts, matrixSize, randomGenerator);
            for (int it = 0; it < numberOfIteration; it++)
            {                
                for (int j = 0; j < matrixSize; j++)
                {
                    for (int i = 0; i < numberOfAnts; i++)
                    {
                        int nextCityForAnt;
                        if (j == matrixSize-1)
                        {
                            nextCityForAnt = ants[i].visitedCitiesIdList.First();
                        }
                        else
                        {
                            var probabilityList = CalculateNextCitiesProbability(ants[i]);
                            nextCityForAnt = ChooseNextCityForAnt(ants[i], probabilityList);
                        }                        
                        MoveAntToNextCity(ants[i], nextCityForAnt);
                    }
                    PheromoneMatrixManager.UpdatePhermoneMatrixByEvaporation(pheromoneMatrix, evaporationValue);
                }
                CheckIfBetterSolutionWasFound(ants, it);
                AntColonyManager.ResetAntColonyMemory(ants, matrixSize, randomGenerator);
            }                                
        }

        private static void CheckIfBetterSolutionWasFound(List<Ant> antColony, int iteration)
        {
            foreach (var ant in antColony)
            {
                if (ant.distance < bestDistanceFound)
                {                   
                    bestDistanceFound = ant.distance;
                    bestRouteFound = new List<int>(ant.visitedCitiesIdList);
                    Console.WriteLine($"Ant travelled {bestDistanceFound} in {iteration}");
                }
            }
        }

        private static List<double> CalculateNextCitiesProbability(Ant ant)
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
                        numerator = Math.Pow(pheromoneMatrix[ant.currentCityID, cityID], alfa) * Math.Pow(1 / Convert.ToDouble(distanceMatrix[ant.currentCityID, cityID]), beta);
                    }
                    else
                    {
                        numerator = 0;
                    }
                    probabilityList.Add(numerator);
                    denominator += Math.Pow(pheromoneMatrix[ant.currentCityID, cityID], alfa) * Math.Pow(pheromoneMatrix[ant.currentCityID, cityID], beta);
                }
            }
            return probabilityList.Select(x => Math.Round((x / denominator), 4)).ToList();
        }

        private static int ChooseNextCityForAnt(Ant ant, List<double> probabilityList)
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

        public static void MoveAntToNextCity(Ant ant, int nextCityForAnt)
        {
            var distanceBetweenCities = distanceMatrix[ant.currentCityID, nextCityForAnt];
            ant.distance += distanceBetweenCities;

            if (nextCityForAnt != ant.currentCityID)
            {
                pheromoneMatrix[ant.currentCityID, nextCityForAnt] = 1 / (Math.Pow(distanceBetweenCities, 2));
            }
            ant.currentCityID = nextCityForAnt;
            ant.visitedCitiesIdList.Add(nextCityForAnt);
        }      
    }
}
