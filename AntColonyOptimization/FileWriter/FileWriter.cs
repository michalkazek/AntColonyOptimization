using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    static class FileWriter
    {
        public static void SaveSummaryIntoFile(float bestFoundDistance, float alfa, float beta, int numberOfAnts, int numberOfIterations)
        {
            var fileName = $"result-{numberOfAnts}_{numberOfIterations}_{alfa}_{beta}";
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + $@"\Results\{fileName}.txt";
            using (var outputFile = new StreamWriter(path, append: true))
            {
                outputFile.WriteLine(bestFoundDistance);
            }
        }
    }
}
