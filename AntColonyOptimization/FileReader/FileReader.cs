﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class FileReader
    {
        public string FileName { get; set; }
        private int matrixSize;

        public FileReader(string fileName)
        {
            FileName = fileName;
        }       

        public int[,] CreateDistanceMatrix()
        {
            string currentLine;
            int[,] distanceMatrix = null;
            bool isFileCorrect = false;
            bool isFileSymetric = true;

            List<string> splittedLine = new List<string>();
            while (!isFileCorrect)
            {
                string filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + $@"\Data\{FileName}.txt";
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        matrixSize = int.Parse(reader.ReadLine());
                        distanceMatrix = new int[matrixSize, matrixSize];

                        for (int rowId = 0; rowId < matrixSize; rowId++)
                        {
                            currentLine = reader.ReadLine();
                            splittedLine.AddRange(currentLine.Trim().Split(' '));
                            if(rowId == 0 && splittedLine.Count == matrixSize)
                            {
                                isFileSymetric = false;
                            }
                            for (int columnId = 0; columnId < splittedLine.Count; columnId++)
                            {
                                distanceMatrix[rowId, columnId] = int.Parse(splittedLine[columnId]);
                                if (isFileSymetric)
                                {
                                    distanceMatrix[columnId, rowId] = int.Parse(splittedLine[columnId]);
                                }    
                            }
                            splittedLine.Clear();
                        }
                    }
                    isFileCorrect = true;
                } catch (IOException error)
                {
                    Console.WriteLine(error.Message);
                } catch
                {
                    Console.WriteLine("Something is wrong with an input file. Please check if it's correct and try again.");
                }
            }
            Console.Clear();
            return distanceMatrix;
        }

        public int GetMatrixSize()
        {
            return matrixSize;
        }
    }
}
