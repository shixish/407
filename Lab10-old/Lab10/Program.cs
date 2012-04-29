using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab10 {
    using Extreme.Mathematics.Curves;
    class Program {
        static int N;// = 11;
        static double[,] coef;
        static double[] roots;
        static double[] weight;
        static void Main(string[] args) {
            input();
            fillMatrix();
            //printMatrix();
            output();
            Console.ReadLine();
        }

        static void input() {
            Console.Write("Enter the highest degree of the Legendre polynomials to generate: ");
            while (!int.TryParse(Console.ReadLine(), out N))
                Console.Write("Enter the highest degree of the Legendre polynomials to generate: ");
            coef = new double[N+1,N+1];
            roots = new double[N];
            weight = new double[N];
        }

        static void output() {

            for (int y = 0; y <= N; y++){
                Console.Write(String.Format("P{0}: \n\nCoefficients (largest power of x to smallest):\n", y));
                double[] t = new double[y+1];
                for (int x = y; x >= 0; x--){
                    Console.Write(String.Format("{0:0.0}\n", coef[x, y]));
                    t[x] = coef[x, y];
                }
                Console.Write("\nRoots:\n");
                Polynomial polynomial = new Polynomial(t);
                double[] roots = polynomial.FindRoots();
                for (int r = 0; r < roots.Length; r++) {
                    Console.WriteLine(roots[r]);
                }
                if (y != N)
                    Console.Write("\n-----\n");
            }
        }

        static void printMatrix() { 
            for (int y = 0; y <= N; y++){
                for (int x = 0; x <= y; x++){
                    Console.Write(String.Format("{0:0.0}\t",coef[x, y]));
                }
                Console.Write('\n');
            }
            for (int i = 0; i < N; i++ ){
                Console.Write(roots[i] + "\t");
            }
            Console.WriteLine();
            for (int t = 0; t < N; t++)
            {
                Console.Write(weight[t] + "\t");
            }
            Console.WriteLine();
        }

        static void fillMatrix() {
            coef[0, 0] = coef[1, 1] = 1;
            //coef[0, 1] = 0;
            for (int y = 2; y <= N; y++)
            {
                for (int x = 0; x <= y; x++)
                {
                    double v1 = 0, v2 = 0;
                    try
                    {
                        v1 = coef[x - 1, y - 1];
                    }
                    catch
                    {
                        v1 = 0;
                    }
                    try
                    {
                        v2 = coef[x, y - 2];
                    }
                    catch
                    {
                        v2 = 0;
                    }
                    coef[x, y] = ((2 * y - 1) * v1 - (y - 1) * v2) / y;
                }
            }
        }

        static double lege_eval(int n, double x) {
            /*double s = coef[n,n];
            for (int i = n; i != 0; i--)
                s = s * x + coef[n,i - 1];*/
            double r = 0;
            for (int i = 0; i <= n; i++)
                r += coef[i, n] * Math.Pow(x, i);
            return r;
        }

        static double lege_diff(int n, double x){
            return n * (x * lege_eval(n, x) - lege_eval(n - 1, x)) / (x * x - 1);
        }

        static void lege_roots(){
            int i;
            double x, x1;
            for (i = 1; i <= N; i++){
                x = Math.Cos(Math.PI * (i - .25) / (N + .5));
                do{
                    x1 = x;
                    x -= lege_eval(N, x) / lege_diff(N, x);
                } while (x != x1);
                roots[i - 1] = x;

                x1 = lege_diff(N, x);
                weight[i - 1] = 2 / ((1 - x * x) * x1 * x1);
            }
        }

    }
}
