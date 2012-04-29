using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab10 {
    class Program {
        static int N;// = 11;
        static double[][] coef;
        static double tollerance = 0.00000000001;
        
        static void Main(string[] args) {
            input();
            fillMatrix();
            //printMatrix();
            
            output();
            Console.ReadLine();
        }

        static void input() {
            String prompt = "Enter the highest degree of the Legendre polynomials to generate: ";
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out N))
                Console.Write(prompt);
            coef = new double[N+1][];
        }

        static void output() {
            for (int y = 0; y <= N; y++){
                Console.Write(String.Format("P{0}: \n\nCoefficients (largest power of x to smallest):\n", y));
                //double[] t = new double[y+1];
                for (int x = y; x >= 0; x--){
                    Console.Write(String.Format("{0:0.##########}\n", coef[y][x]));
                }
                Console.Write("\nRoots:\n");
                if (y == 0){
                    Console.WriteLine("None");
                }else{
                    double[] roots = getRoots(y);
                    for (int r = 0; r < roots.Length; r++)
                        Console.WriteLine(String.Format("{0:0.##########}", roots[r]));
                }
                if (y != N)
                    Console.Write("\n\n-----\n\n");
            }
        }

        static void printMatrix() { 
            for (int y = 0; y <= N; y++){
                for (int x = 0; x <= y; x++){
                    Console.Write(String.Format("{0:0.0}\t", coef[y][x]));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void fillMatrix() {
            coef[0] = new double[1] {1};
            coef[1] = new double[2] {0, 1};
            for (int y = 2; y <= N; y++){
                coef[y] = new double[y + 1];
                for (int x = 0; x <= y; x++){
                    double v1 = 0, v2 = 0;
                    try{
                        v1 = coef[y - 1][x - 1];
                    }catch{
                        v1 = 0;
                    }
                    try{
                        v2 = coef[y-2][x];
                    }catch{
                        v2 = 0;
                    }
                    coef[y][x] = ((2 * y - 1) * v1 - (y - 1) * v2) / y;
                }
            }
        }

        /*
         * I had help on these three functions. The original functions originate from this link:
         * http://rosettacode.org/wiki/Numerical_integration/Gauss-Legendre_Quadrature
         * I modified the C code there to allow me to find the roots of the resultant polynomials.
         */

        static double evaluate(int n, double x) {
            /*double r = 0;
            for (int i = 0; i <= n; i++)
                r += coef[n][i] * Math.Pow(x, i);
            return r;*/
            double s = coef[n][n];
            for (int i = n; i!=0; i--)
                s = s * x + coef[n][i - 1];
            return s;
        }

        static double diff(int n, double x){
            return n * (x * evaluate(n, x) - evaluate(n - 1, x)) / (x * x - 1);
        }

        static double[] getRoots(int order){
            double x, x1;
            double[] roots = new double[order];//, weight = new double[order];
            for (int i = 1; i <= order; i++){
                x = Math.Cos(Math.PI * (i - .25) / (order + .5));
                do{
                    x1 = x;
                    x -= evaluate(order, x) / diff(order, x);
                } while (x - x1 > tollerance);
                roots[i - 1] = x;
                //x1 = diff(order, x);
                //weight[i - 1] = 2 / ((1 - x * x) * x1 * x1);
            }
            return roots;
        }

    }
}
