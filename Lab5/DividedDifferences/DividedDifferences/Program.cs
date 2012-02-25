/*
 * Written by Andrew Wessels, 2012
 * Numerical Analysis, Dr. David Bishop
 * Lab 5 - Divided Differences
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace _3p3DividedDifferenceRecursiveProject {
    class DividedDifferenceRecursiveClass {
        static List<double> xvals = new List<double>(); 
        // 1, 1.3, 1.6, 1.9, 2.2
        static List<double> yvals = new List<double>(); 
        // 0.7651977, 0.620086, 0.4554022, 0.2818186, 0.1103623
        static double[][] dynamic;
        static int xcount;
        static bool DEBUG = false;

        static void Main(string[] args) {
            readFile();
            DivDifTbl();
            Console.WriteLine("\nP(x) := " + makeEquation() + "\n");

            Console.Write("Enter a domain value: ");
            double x = double.Parse(Console.ReadLine());
            Console.WriteLine(String.Format("P({0:F3}) = {1:F9}", x, calculate(x)));
            //Console.WriteLine("total:" + calculate(1.2));
            Console.ReadLine(); 
        }

        static void readFile() {
            string infile;
            if (DEBUG)
                infile = "input.txt";
            else{
                Console.Write("Filename: ");
                infile = Console.ReadLine();
                Console.Write("Opening " + infile + "\n");
            }
            
            try {
                using (StreamReader sr = new StreamReader(infile)) {
                    string line;
                    //NOTE: THIS EXPECTS SOME NUMBER OF SPACES BETWEEN x AND f(x) VALUES (NOT TABS)
                    while ((line = sr.ReadLine()) != null) {
                        line = line.Trim();
                        double x = double.Parse(line.Substring(0,line.IndexOf(" ")));
                        double y = double.Parse(line.Substring(line.LastIndexOf(" ")));
                        xvals.Add(x);
                        yvals.Add(y);
                    }
                    xcount = xvals.Count;
                    dynamic = new double[xcount][];
                    for (int i = 0; i < xcount; i++)
                        dynamic[i] = new double[xcount];
               }
           }
           catch (Exception e) {
               Console.WriteLine(String.Format("The file could not be read: {0}", e.Message));
           }
        }
		
        static void DivDifTbl() {
            for (int order = 0; order < xcount; order++) {
                int start = 0;
                int end = order;
                while (end < xcount) {
                    if (start == end) 
                        dynamic[start][end] = yvals[start];
                    else 
                        dynamic[start][end] = (dynamic[start + 1][end] - dynamic[start][end - 1]) / (xvals[end] - xvals[start]);
                    start++;
                    end++;
                }
            }
            for (int x = 0; x < xcount; x++){
                Console.Write(String.Format("{0:F1} ", xvals[x]));
                for (int y = x; y >= 0; y--){
                    Console.Write(String.Format("{0:F9} ", dynamic[y][x]));
                }
                Console.Write("\n");
            }
        }

        static string makeEquation(){
            string output = String.Format("{0:F7}", dynamic[0][xcount-1]);
            for (int i = xcount-2; i >= 0; i--)
                output = String.Format("{0:F7}+(x-{1:F1})({2:F2})", dynamic[0][i], xvals[i], output);
            return output;
        }

        static double calculate(double x) {
            double total = dynamic[0][xcount - 1];
            for (int i = xcount-2; i >= 0; i--){
                if (DEBUG) Console.Write(String.Format("{0:F5}+({1:F1}-{2:F1})({3:F5})", dynamic[0][i], x, xvals[i], total));
                total = dynamic[0][i] + (x - xvals[i])*total;
                if (DEBUG) Console.Write(String.Format("={0:F5}\n", total));
            }
            return total;
        }

        /*static void DivDif2(int start, int end) {
            if (start == end) {
                dynamic[start][end] = yvals[start];
                //return yvals[start];
            } else {
                //Console.WriteLine(String.Format("({0}, {1}) - ({2}, {3})", start + 1, end, start, end-1));
                dynamic[start][end] = (dynamic[start + 1][end] - dynamic[start][end - 1]) / (xvals[end] - xvals[start]);
                //return (DivDif2(start + 1, end) - DivDif2(start, end - 1)) / (xvals[end] - xvals[start]);
            }
        }*/
    }
}

/*
 * P(x) := 0.7651977-0.483705667(x-1)-0.108733889(x-1)(x-1.3)+0.065878395(x-1)(x-1.3)(x-1.6)+0.001825103(x-1)(x-1.3)(x-1.6)(x-1.9)
 * or 
 * P(x) := 0.7651977+(x-1)(-0.483705667-(x-1.3)(0.108733889+(x-1.6)(0.065878395+(x-1.9)(0.001825103))))
*/