using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class Application
    {              
        public static void Run(string filePath)
        {
            var inputParameteres = InputReader.ReadInputParamteres(filePath);
            Random randomGenerator = new Random();
            foreach (var item in inputParameteres)
            {
                var currentRun = new Alghoritm(item.Key[1], item.Key[2], 0.005f, 0.0000000010f, randomGenerator);
                for (int i = 0; i < item.Key[5]; i++)
                {
                    currentRun.Run(item.Key[3], item.Key[4], item.Value);
                }
            }            
        }     
    }
}
