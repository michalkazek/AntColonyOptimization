using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyOptimization
{
    class Application
    {              
        public static void Run()
        {
            Random randomGenerator = new Random();
            var firstRun = new Alghoritm(1, 5, 0.001f, 0.000001f, randomGenerator);
            firstRun.Run(30, 100, "berlin52");         

            Console.ReadKey();
        }     
    }
}
