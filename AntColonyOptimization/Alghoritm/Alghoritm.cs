using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class Alghoritm
    {
        public float Alfa { get; set; }
        public float Beta { get; set; }
        public float StartingPheromoneValue { get; set; }
        public float EvaporationValue { get; set; }
        public Random RandomGenerator { get; set; }        

        public int[,] DistanceMatrix { get; set; }
        public int MatrixSize { get; set; }
        public double[,] PheromoneMatrix { get; set; }
        public float BestFoundDistance { get; set; }
        public List<int> BestFoundRoute { get; set; }

        public Alghoritm(float alfa, float beta, float startingPheromoneValue, float evaporationValue, Random randomGenerator)
        {
            Alfa = alfa;
            Beta = beta;
            EvaporationValue = evaporationValue;
            RandomGenerator = randomGenerator;
            StartingPheromoneValue = startingPheromoneValue;
        }

        public void Run(int numberOfAnts, int numberOfIterations, string fileName)
        {
            GeneratePrerequisite(fileName);
            Start(numberOfAnts, numberOfIterations);
            Console.WriteLine($"Best ant travelled {BestFoundDistance}");
            DistanceChecker.CheckIsBestRouteCorrect(MoveAntToNextCity, BestFoundRoute, BestFoundDistance);
        }

        private void GeneratePrerequisite(string fileName)
        {
            var fileReader = new FileReader(fileName);
            DistanceMatrix = fileReader.CreateDistanceMatrix();
            MatrixSize = fileReader.GetMatrixSize();
            PheromoneMatrix = PheromoneMatrixManager.CreatePhermoneMatrix(MatrixSize, StartingPheromoneValue);
            BestFoundDistance = 10000000;
            BestFoundRoute = new List<int>();
        }

        private void Start(int numberOfAnts, int numberOfIteration)
        {
            List<Ant> ants = AntColonyManager.CreateAntColony(numberOfAnts, MatrixSize, RandomGenerator);
            for (int it = 0; it < numberOfIteration; it++)
            {
                for (int j = 0; j < MatrixSize; j++)
                {
                    for (int i = 0; i < numberOfAnts; i++)
                    {
                        int nextCityForAnt;
                        if (j == MatrixSize - 1)
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
                    PheromoneMatrixManager.UpdatePhermoneMatrixByEvaporation(PheromoneMatrix, EvaporationValue);
                }
                CheckIfBetterSolutionWasFound(ants, it);
                AntColonyManager.ResetAntColonyMemory(ants, MatrixSize, RandomGenerator);
            }
        }

        private void CheckIfBetterSolutionWasFound(List<Ant> antColony, int iteration)
        {
            foreach (var ant in antColony)
            {
                if (ant.distance < BestFoundDistance)
                {
                    BestFoundDistance = ant.distance;
                    BestFoundRoute = new List<int>(ant.visitedCitiesIdList);
                    Console.WriteLine($"Ant travelled {BestFoundDistance} in {iteration}");
                }
            }
        }

        private List<double> CalculateNextCitiesProbability(Ant ant)
        {
            var probabilityList = new List<double>();
            double denominator = 0.0;

            for (int cityID = 0; cityID < MatrixSize; cityID++)
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
                        numerator = Math.Pow(PheromoneMatrix[ant.currentCityID, cityID], Alfa) * Math.Pow(1 / Convert.ToDouble(DistanceMatrix[ant.currentCityID, cityID]), Beta);
                    }
                    else
                    {
                        numerator = 0;
                    }
                    probabilityList.Add(numerator);
                    denominator += Math.Pow(PheromoneMatrix[ant.currentCityID, cityID], Alfa) * Math.Pow(PheromoneMatrix[ant.currentCityID, cityID], Beta);
                }
            }
            return probabilityList.Select(x => Math.Round((x / denominator), 4)).ToList();
        }

        private int ChooseNextCityForAnt(Ant ant, List<double> probabilityList)
        {
            var probabilitySum = probabilityList.Sum();
            probabilityList = probabilityList.Select(x => Math.Round(x / probabilitySum, 10)).ToList();
            var drawnNumber = RandomGenerator.NextDouble();
            int i = 0;
            var currentProbabilitySum = probabilityList[i];

            while (drawnNumber > currentProbabilitySum || ant.visitedCitiesIdList.Contains(i))
            {
                i++;
                currentProbabilitySum += probabilityList[i];
            }
            return i;
        }

        private void MoveAntToNextCity(Ant ant, int nextCityForAnt)
        {
            var distanceBetweenCities = DistanceMatrix[ant.currentCityID, nextCityForAnt];
            ant.distance += distanceBetweenCities;

            if (nextCityForAnt != ant.currentCityID)
            {
                PheromoneMatrix[ant.currentCityID, nextCityForAnt] = 1 / (Math.Pow(distanceBetweenCities, 2));
            }
            ant.currentCityID = nextCityForAnt;
            ant.visitedCitiesIdList.Add(nextCityForAnt);
        }
    }
}
