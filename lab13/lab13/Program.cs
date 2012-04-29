/*
 * Program written by Andrew Wessels Apr, 2012
 * Numerical Analysis with Professor Dr. David Bishop
 * Purpose: The program uses LU decomposition to solve a linear system.
 * Usage: Provide an input file with a matrix defined in the following form:
        x x x x x
        x x x x x
        x x x x x
        x x x x x
 * Note: Any number of spaces or tabs are acceptable delimiters. Accepts +/- decimals.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace lab13{
    class Program{
        static bool DEBUG = false;
        static Matrix matrix;
        static void Main(string[] args) {
            readFile();
            Console.ReadLine();
        }

        static void readFile() {
            string infile;
            List<List<double>> m = new List<List<double>>();
            if (DEBUG)
                infile = "input.txt";
            else {
                Console.Write("Filename: ");
                infile = Console.ReadLine();
                Console.Write("Opening " + infile + "\n");
            }

            try {
                using (StreamReader sr = new StreamReader(infile)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        line = line.Trim();
                        //Console.WriteLine(line.Split(' '));
                        Regex regex = new Regex(@"(-?\d+(\.\d+)?)[\s]*");
                        Match match = regex.Match(line);
                        List<double> temp = new List<double>();
                        while ( match.Success ){
                            //Console.WriteLine ( j++ + " \t" + match ) ;
                            temp.Add(double.Parse(match.ToString()));
                            match = match.NextMatch();
                        }
                        if (temp.Count != 0)
                            m.Add(temp);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(String.Format("The file could not be read: {0}", e.Message));
            }
            try{
                matrix = new Matrix(m);
            } catch (Exception e) {
                Console.WriteLine(String.Format("Could not find solution to matrix. {0}", e.Message));
            }
        }
    }

    class Matrix {
        //double[][] m;
        List<List<double>> m;
        int pivots;

        public Matrix(List<List<double>> m) {
            this.m = m;
            if (m.Count > 0) {
                if (m.Count != m[0].Count - 1)
                    throw new System.ApplicationException("This is not an augmented Matrix! I don't know how to handle this.");
                pivots = m.Count;
                WriteHeading("Initial Matrix:");
                Console.WriteLine(this.ToMatrixString());
                decompose();
                WriteHeading("Decomposed Matrix:");
                Console.WriteLine(this.ToDecomposedString());
                WriteHeading("Solution:");
                backSubstitute(forwardSubstitute());
            } else
                throw new System.ApplicationException("Invalid Matrix");
        }

        public void decompose() {
            for (int pivot = 0; pivot < pivots; pivot++) {
                //auto_swap(pivot); //this actually seems to work, but I would need to make the logic smarter so it's not using the U matrix as we build it up.
                for (int y = pivot+1; y < pivots; y++) {
                    decompose(pivot, y);
                }
            }
        }

        private void decompose(int pivot, int y) {
            if (y != pivot && m[pivot].Count - 1 >= pivot) {
                if (m[pivot][pivot] == 0)
                    throw new System.ApplicationException("No unique solution found...");
                double multiplier = -m[y][pivot] / m[pivot][pivot];
                WriteHeading(String.Format("{0:0.##}", multiplier) + "*[row " + (pivot + 1) + "] + [row " + (y + 1) + "] -> [row " + (y + 1) + "]   (Multiplier: " + multiplier + ")");
                m[y][pivot] = -multiplier;
                for (int x = pivot+1; x < pivots; x++) {
                    m[y][x] = multiplier * m[pivot][x] + m[y][x];
                }
                Console.WriteLine(this);
            }
        }

        private double[] forwardSubstitute() { //all good
            Boolean debug = false;
            double[] Y = new double[pivots];
            Y[pivots - 1] = m[pivots - 1][pivots - 1];
            for (int pivot = 0; pivot < pivots; pivot++) {
                double value = m[pivot][pivots];
                for (int x = 0; x < pivot; x++) {
                    if (debug) Console.WriteLine("value:" + value);
                    value -= Y[x] * m[pivot][x];
                    if (debug) Console.WriteLine("value -= " + Y[x].ToString() + "*" + m[pivot][x].ToString());
                }
                if (debug) Console.WriteLine("value:" + value);
                if (debug) Console.WriteLine("-------");
                Y[pivot] = value;
            }
            Console.Write("y=[\n  " + Y[0].ToString());
            for (int i = 1; i < pivots; i++)
                Console.Write(",\n  " + Y[i].ToString());
            Console.Write("\n]\n\n");
            return Y;
        }

        private double[] backSubstitute(double[] Y) { //all good
            Boolean debug = false;
            double[] X = new double[pivots];
            X[pivots - 1] = m[pivots-1][pivots - 1];
            
            for (int pivot = pivots-1; pivot>= 0; pivot--) {
                double value = Y[pivot];//m[pivot][pivots];
                for (int x = pivots-1; x >= pivot; x--) {
                    if (debug) Console.WriteLine("value:" + value);
                    if (x == pivot) {
                        value /= m[pivot][x];
                        if (debug) Console.WriteLine("value /= " + m[pivot][x].ToString());
                    } else {
                        value -= X[x] * m[pivot][x];
                        if (debug) Console.WriteLine("value -= " + X[x].ToString() + "*" + m[pivot][x].ToString());
                    }
                    
                }
                if (debug) Console.WriteLine("value:" + value);
                if (debug) Console.WriteLine("-------");
                X[pivot] = value;
            }
            Console.Write("x=[\n  " + X[0].ToString());
            for (int i = 1; i < pivots; i++ )
                Console.Write(",\n  " + X[i].ToString());
            Console.Write("\n]\n\n");
            return X;
        }

        /*private void auto_swap(int row) {
            if (m[row].Count-1 >= row) { //for MxN matrix where M>N
                double ratio = m[row][row] / m[row].Max();
                int winner = 0;
                for (int y = row + 1; y < pivots; y++){
                    double tratio = m[y][row] / m[y].Max();
                    if (tratio > ratio) {
                        winner = y;
                        ratio = tratio;
                    }
                }

                if (winner > 0) { //perform the swap
                    WriteHeading("Swap row " + (row + 1) + " with row " + (winner + 1));
                    List<double> temp = m[row];
                    m[row] = m[winner];
                    m[winner] = temp;
                    Console.WriteLine(this);
                }
            }
        }*/

        private void WriteHeading(String heading) {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(heading);
            Console.ResetColor();
        }

        public string ToDecomposedString() {
            StringBuilder L = new StringBuilder();
            StringBuilder U = new StringBuilder();
            StringBuilder b = new StringBuilder();
            L.Append("Lower Matrix:\n");
            U.Append("Upper Matrix:\n");
            b.Append("b=[");
            Boolean first = true;
            for (int y = 0; y < pivots; y++) {
                for (int x = 0; x < m[y].Count; x++) {
                    if (x == pivots && x == m[y].Count - 1) {
                        if (first)
                            first = false;
                        else
                            b.Append(", ");
                        b.Append(m[y][x]);
                    } else {
                        String v = String.Format("{0:0.###}", m[y][x]) + '\t'; //m[y][x].ToString() + '\t';
                        if (x == y) {//pivot
                            L.Append("1\t");
                            U.Append(v);
                        } else if (x > y) {
                            L.Append("0\t");
                            U.Append(v);
                        } else {
                            L.Append(v);
                            U.Append("0\t");
                        }
                    }
                }
                L.Append('\n');
                U.Append('\n');
            }
            b.Append(']');

            return U.ToString() + '\n' + L.ToString() + '\n' + b.ToString() + '\n';
        }

        public string ToMatrixString(){
            StringBuilder s = new StringBuilder();
            for (int y = 0; y < pivots; y++) {
                for (int x = 0; x < m[y].Count; x++) {
                    if (x == pivots && x == m[y].Count - 1)
                        s.Append("|  " + m[y][x]);
                    else
                        s.Append(String.Format("{0:0.###}", m[y][x]) + '\t');
                }
                s.Append('\n');
            }
            return s.ToString();
        }

        public override string ToString() {
            return ToMatrixString();
            //return ToDecomposedString();
        }
    }
}
