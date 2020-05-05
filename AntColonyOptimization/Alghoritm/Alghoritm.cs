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
        public float FirstFoundDistance { get; set; }
        public int BestFoundDistanceIteration { get; set; }
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
            //PrintSummary(DistanceChecker.CheckIsBestRouteCorrect(MoveAntToNextCity, BestFoundRoute, BestFoundDistance));
            FileWriter.SaveSummaryIntoFile(FirstFoundDistance, BestFoundDistance, BestFoundDistanceIteration, Alfa, Beta, numberOfAnts, numberOfIterations, fileName);          
        }        

        private void GeneratePrerequisite(string fileName)
        {
            var fileReader = new FileReader(fileName);
            DistanceMatrix = fileReader.CreateDistanceMatrix();
            MatrixSize = fileReader.GetMatrixSize();
            PheromoneMatrix = PheromoneMatrixManager.CreatePhermoneMatrix(MatrixSize, StartingPheromoneValue);
            BestFoundRoute = new List<int>();
        }

        private void Start(int numberOfAnts, int numberOfIterations)
        {
            List<Ant> antColony = AntColonyManager.CreateAntColony(numberOfAnts, MatrixSize, RandomGenerator);
            for (int iterationCounter = 0; iterationCounter < numberOfIterations; iterationCounter++)
            {
                for (int visitedCityCounter = 0; visitedCityCounter < MatrixSize; visitedCityCounter++)
                {
                    for (int antCounter = 0; antCounter < numberOfAnts; antCounter++)
                    {
                        int nextCityIDForCurrentAnt;
                        if (visitedCityCounter == (MatrixSize - 1))
                        {
                            nextCityIDForCurrentAnt = antColony[antCounter].visitedCitiesIdList.First();
                        }
                        else
                        {
                            var probabilityList = CalculateNextCitiesProbability(antColony[antCounter]);
                            nextCityIDForCurrentAnt = ChooseNextCityForAnt(antColony[antCounter], probabilityList);
                        }
                        MoveAntToNextCity(antColony[antCounter], nextCityIDForCurrentAnt);
                    }
                    PheromoneMatrixManager.UpdatePhermoneMatrixByEvaporation(PheromoneMatrix, EvaporationValue);
                }
                CheckIfBetterSolutionWasFound(antColony, iterationCounter);
                AntColonyManager.ResetAntColonyMemory(antColony, MatrixSize, RandomGenerator);
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
            int cityID = 0;
            var currentProbabilitySum = probabilityList[cityID];

            while (drawnNumber > currentProbabilitySum || ant.visitedCitiesIdList.Contains(cityID))
            {
                cityID++;
                currentProbabilitySum += probabilityList[cityID];
            }
            return cityID;
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

        private void CheckIfBetterSolutionWasFound(List<Ant> antColony, int iteration)
        {
            if (iteration == 0)
            {
                FirstFoundDistance = antColony[0].distance;
                BestFoundDistanceIteration = 0;
            }      
            antColony.ForEach(ant => {
                if (ant.distance < BestFoundDistance || iteration == 0)
                {                    
                    BestFoundDistance = ant.distance;
                    BestFoundRoute = new List<int>(ant.visitedCitiesIdList);
                    BestFoundDistanceIteration = iteration;
                }
            });
        }

        private void PrintSummary(string isBestRouteCorrect)
        {
            Console.WriteLine($"The shortest route found by an ant is {BestFoundDistance}. [{isBestRouteCorrect}]");
        }
    }
}
