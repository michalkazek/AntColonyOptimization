using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class InputReader
    {
        public static Dictionary<List<int>, string> ReadInputParamteres(string filePath)
        {
            var parameterList = new Dictionary<List<int>, string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                int id = 0;
                string currentLine;
                while((currentLine = reader.ReadLine()) != null)
                {
                    var splittedLine = new List<string>() { {id.ToString()} };
                    splittedLine.AddRange(currentLine.Split(','));
                    var fileName = splittedLine.Last();
                    parameterList.Add(splittedLine.Take(splittedLine.Count - 1).Select(item => Convert.ToInt32(item)).ToList(), fileName);
                    id++;
                }
            }
            return parameterList;
        }
    }
}
