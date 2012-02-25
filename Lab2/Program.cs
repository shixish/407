using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args){
            double tol = .00001, x = .1;
            int i = 1;
            for (; x / (i - 1) - x / i > tol; i++) ;
            Console.WriteLine("n = {0}", i);
            Console.ReadLine();
        }
    }
}
