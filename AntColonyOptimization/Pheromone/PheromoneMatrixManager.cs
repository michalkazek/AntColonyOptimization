using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    static class PheromoneMatrixManager
    {
        public static double[,] CreatePhermoneMatrix(int matrixSize, float initialValue)
        {
            var pheromoneMatrix = new double[matrixSize, matrixSize];
            for (int x = 0; x < matrixSize; x++)
            {
                for (int y = 0; y < matrixSize; y++)
                {
                    pheromoneMatrix[x, y] = initialValue;
                }
            }
            return pheromoneMatrix;
        }

        public static void UpdatePhermoneMatrixByEvaporation(double[,] pheromoneMatrix, double evaporationValue)
        {
            int matrixSize = Convert.ToInt32(Math.Sqrt(pheromoneMatrix.Length));
            for (int x = 0; x < matrixSize; x++)
            {
                for (int y = 0; y < matrixSize; y++)
                {
                    pheromoneMatrix[x, y] -= evaporationValue;
                }
            }
        }
    }
}
