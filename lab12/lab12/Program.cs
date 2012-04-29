/*
 * Program written by Andrew Wessels Apr, 2012
 * Numerical Analysis with Professor Dr. David Bishop
 * Purpose: Row Reduce a matrix using the Gauss-Jordan method.
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

namespace lab12{
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
        int pivots = 0;

        public Matrix(List<List<double>> m) {
            this.m = m;
            if (m.Count > 0) {
                pivots = m[0].Count;
                WriteHeading("Initial Matrix:");
                Console.WriteLine(this);
                solve();
                reduce();
            } else
                throw new System.ApplicationException("Invalid Matrix");
        }

        public override string ToString() {
            StringBuilder s = new StringBuilder();
            for (int y = 0; y < m.Count; y++) {
                for (int x = 0; x < m[y].Count; x++) {
                    if (x == m.Count && x == m[y].Count-1)
                        s.Append("|  "+m[y][x]);
                    else
                        s.Append(String.Format("{0:0.####}", m[y][x]) + '\t');
                }
                s.Append('\n');
            }
            return s.ToString();
        }

        public void solve(){
            for (int pivot = 0; pivot < m.Count; pivot++) {
                auto_swap(pivot);
                for (int y = 0; y < m.Count; y++) {
                    //get a zero here...
                    solve(pivot, y);
                }
            }
        }

        private void solve(int pivot, int y){
            if (y != pivot && m[pivot].Count - 1 >= pivot) {
                if (m[pivot][pivot] == 0)
                    throw new System.ApplicationException("No unique solution found...");
                double multiplier = -m[y][pivot] / m[pivot][pivot];
                WriteHeading(String.Format("{0:0.00}", multiplier) + "*[row " + (pivot + 1) + "] + [row " + (y + 1) + "] -> [row " + (y + 1) + "]   (Multiplier: " + multiplier + ")");
                for (int x = 0; x < m[y].Count; x++) {
                    if (x == pivot)
                        m[y][x] = 0;
                    else {
                        m[y][x] = multiplier * m[pivot][x] + m[y][x];
                    }
                }
                Console.WriteLine(this);
            }
        }

        public void reduce() {
            WriteHeading("Row Reduced Echelon Form:");
            for (int y = 0; y < m.Count; y++) {
                for (int x = m.Count; x < m[y].Count; x++)
                    m[y][x] /= m[y][y];
                if (m[y].Count - 1 >= y) //for MxN matrix where M>N
                    m[y][y] = 1;
            }
            Console.WriteLine(this);
        }

        private void auto_swap(int row) {
            if (m[row].Count-1 >= row) { //for MxN matrix where M>N
                double ratio = m[row][row] / m[row].Max();
                int winner = 0;
                for (int y = row + 1; y < m.Count; y++){
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
        }

        private void WriteHeading(String heading) {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(heading);
            Console.ResetColor();
        }
    }
}
