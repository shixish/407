/*
Written by Andrew Wessels
CS 407, Spring 2012
Instructor: Dr. Bishop
Instructions:
Determine the minimal value of N required for

| ln(1.5)-PN(1.5) | < 10^-5

Where PN is the Nth Taylor polynomial of the natural logarithm function expanded about the point x' = 1.

You must not use the Taylor polynomial remainder term.
---------------------------------------------------------------------------------------------------------------------------------------
Input: There is no input required for this program.
---------------------------------------------------------------------------------------------------------------------------------------
Output: 

YOUR FULL NAME

The smallest value of N as specified above (and a preceding statement describing what the number represents, of course.
----------------------------------------------------------------------------------------------------------------------------------------
Submit the following in a zipped folder:
1.  The source code of this program in a TEXT FILE ONLY in the language specified by the instructor.
2.  The executable file, ready to run in the platform specified by the instructor.
*/

using System;
//using System.Collections.Generic;

namespace cs407_Lab1{
    class Lab1{
        static void Main(String[] args){
            /*
                Discussion:
                f(x): ln(x)
                f'(x): 1/x
                f''(x): -1/x^2
                f'''(x): 2/x^3
                f''''(x): -6/x^4
                ...
                Note: sign flips, power increases by 1
                
                T(x) = SUM{(f^[n](a)(x-a)^n)/n!}
                We are using a = 1 since we said the expansion is about x = 1
                ln x = SUM{(-1)^(i+1)/i*(x-1)^i}
            */
            const double
                x=1.5, //constant given in the problem
                tol=0.00001; //pre-defined tollerence
            int max=100; //the maximum degree of polynomial to try
            
            Console.WriteLine("Program written by Andrew Wessels - Spring 2012");
            Console.WriteLine("CS 407 with Dr. David Bishop");
            int r = R(x, tol, max);
            if (r > 0)
                Console.WriteLine(
                    "A polynomial of degree " + Convert.ToString(r) +
                    " will aproximate ln(1.5) with a tolerence of at most " +
                    Convert.ToString(tol) + "."
                );
            else
                Console.WriteLine("Method failed!");
            Console.ReadLine(); //For teacher's convenience 
        }
        
        /*
        "If f is approximated by the nth Taylor expansion about x = a,
             then the error term Rn is no more than:"
                 [M(x-a)^(n+1)]/[(n+1)!]
         "where M is an overestimation of the value of |f^[n+1](t)| for t between a and x"
         Source: http://www.zweigmedia.com/pdfs/TaylorSeries.pdf (Pg. 7)
         
         We will use this function to test different values of n such that
         the result is less than the desirable tolerence.
         
         We could implement this as a simple function and provide it with
         increasing values of n, but this would duplicate a lot of computation,
         so instead we use a loop to step up values of n,
         which allows us to make use of previous values.
        */
        static int R(double x, double tol, int max){
            int a = 1; //polynomial expansion about a
            double  y=x-a, //constant value used in the numerator
                    power=y, //represents the numerator for the current value of n
                    term=y; //represents the answer of the whole function
            /*
                We want the absolute value of the term to be less than the tolerence
                within 100 iterations otherwise we are not fitting fast enough.
                If we get to the end of the loop we are saying that
                a 100th order polynomial does not give us enough precision.
            */
            for (int n=1; n <= max; n++){
                if (Math.Abs(term) < tol) return n;
                power *= y; //multiply by another (n-a) to increase the order of the polynomial
                term = power / (n+1); //we devide by (n+1) because the denominator is [(n+1)!]
            }
            return -1;
        }
    }
}