using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class PheromoneMatrixManager
    {
        public float[,] PheromoneMatrix { get; set; }
        public int MatrixSize { get; set; }

        public PheromoneMatrixManager(int matrixSize)
        {
            PheromoneMatrix = new float[matrixSize, matrixSize];
            MatrixSize = matrixSize;
            InitializePhermoneMatrix();
        }

        public void InitializePhermoneMatrix()
        {
            for (int x = 0; x < MatrixSize; x++)
            {
                for (int y = 0; y < MatrixSize; y++)
                {
                    PheromoneMatrix[x, y] = 0.001f;
                }
            }
        }

        public void UpdateSelectedPhermoneMatrixCell(int x, int y, float value)
        {
            PheromoneMatrix[x, y] += value;
        }

        public void UpdatePhermoneMatrixByEvaporation(float evaporationValue)
        {
            for (int x = 0; x < MatrixSize; x++)
            {
                for (int y = 0; y < MatrixSize; y++)
                {
                    PheromoneMatrix[x, y] -= evaporationValue;
                }
            }
        }
    }
}
