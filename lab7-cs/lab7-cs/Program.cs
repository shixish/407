using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab7_cs{
    class Program{
        static void Main(string[] args){
		    double[] x = new double[]{0.000000000, 0.015707963, 0.031415927, 0.047123890, 0.062831853, 0.078539816, 0.094247780, 0.109955743, 0.125663706, 0.141371669, 0.157079633, 0.172787596, 0.188495559, 0.204203522, 0.219911486, 0.235619449, 0.251327412, 0.267035376, 0.282743339, 0.298451302, 0.314159265, 0.329867229, 0.345575192, 0.361283155, 0.376991118, 0.392699082, 0.408407045, 0.424115008, 0.439822972, 0.455530935, 0.471238898, 0.486946861, 0.502654825, 0.518362788, 0.534070751, 0.549778714, 0.565486678, 0.581194641, 0.596902604, 0.612610567, 0.628318531, 0.644026494, 0.659734457, 0.675442421, 0.691150384, 0.706858347, 0.722566310, 0.738274274, 0.753982237, 0.769690200, 0.785398163, 0.801106127, 0.816814090, 0.832522053, 0.848230016, 0.863937980, 0.879645943, 0.895353906, 0.911061870, 0.926769833, 0.942477796, 0.958185759, 0.973893723, 0.989601686, 1.005309649, 1.021017612, 1.036725576, 1.052433539, 1.068141502, 1.083849465, 1.099557429, 1.115265392, 1.130973355, 1.146681319, 1.162389282, 1.178097245, 1.193805208, 1.209513172, 1.225221135, 1.240929098, 1.256637061, 1.272345025, 1.288052988, 1.303760951, 1.319468915, 1.335176878, 1.350884841, 1.366592804, 1.382300768, 1.398008731, 1.413716694, 1.429424657, 1.445132621, 1.460840584, 1.476548547, 1.492256510, 1.507964474, 1.523672437, 1.539380400, 1.555088364, 1.570796327};
		    double[] y = new double[]{0.000000000, 0.015707317, 0.031410759, 0.047106451, 0.062790520, 0.078459096, 0.094108313, 0.109734311, 0.125333234, 0.140901232, 0.156434465, 0.171929100, 0.187381315, 0.202787295, 0.218143241, 0.233445364, 0.248689887, 0.263873050, 0.278991106, 0.294040325, 0.309016994, 0.323917418, 0.338737920, 0.353474844, 0.368124553, 0.382683432, 0.397147891, 0.411514359, 0.425779292, 0.439939170, 0.453990500, 0.467929814, 0.481753674, 0.495458668, 0.509041416, 0.522498565, 0.535826795, 0.549022818, 0.562083378, 0.575005252, 0.587785252, 0.600420225, 0.612907054, 0.625242656, 0.637423990, 0.649448048, 0.661311865, 0.673012514, 0.684547106, 0.695912797, 0.707106781, 0.718126298, 0.728968627, 0.739631095, 0.750111070, 0.760405966, 0.770513243, 0.780430407, 0.790155012, 0.799684658, 0.809016994, 0.818149717, 0.827080574, 0.835807361, 0.844327926, 0.852640164, 0.860742027, 0.868631514, 0.876306680, 0.883765630, 0.891006524, 0.898027576, 0.904827052, 0.911403277, 0.917754626, 0.923879533, 0.929776486, 0.935444031, 0.940880769, 0.946085359, 0.951056516, 0.955793015, 0.960293686, 0.964557418, 0.968583161, 0.972369920, 0.975916762, 0.979222811, 0.982287251, 0.985109326, 0.987688341, 0.990023658, 0.992114701, 0.993960955, 0.995561965, 0.996917334, 0.998026728, 0.998889875, 0.999506560, 0.999876632, 1.000000000};
		    TridiagMatrix m = new TridiagMatrix(x, y);
            Console.WriteLine("Cubic Spline interpolation by Andrew Wessels 2012");
            Console.WriteLine("Instructions: Input a floating point value for x,");
            Console.WriteLine(" and I will return the sine, cosine, and tangent of that number.\n");
            Console.WriteLine("Note: You may type \"quit\" at any time to stop the program.\n");
            for(;;){
                double e = 0;
                bool ok = false;
                while (!ok) {
                    Console.Write("Please enter an x value: ");
                    String line = Console.ReadLine();
                    ok = Double.TryParse(line, out e);
                    if (line.ToLower() == "quit") return;
                }
		        try {
                    Console.Write(String.Format("sine({0})={1} \nMath.Sin({2})={3} \ndifference={4}\n\n", e, m.sine(e), e, Math.Sin(e), m.sine(e) - Math.Sin(e)));
                    Console.Write(String.Format("cosine({0})={1} \nMath.Cos({2})={3} \ndifference={4}\n\n", e, m.cosine(e), e, Math.Cos(e), m.cosine(e) - Math.Cos(e)));
                    Console.Write(String.Format("tangent({0})={1} \nMath.Tan({2})={3} \ndifference={4}\n\n", e, m.tangent(e), e, Math.Tan(e), m.tangent(e) - Math.Tan(e)));
                    Console.Write("------------------------\n");
		        } catch (Exception e1) {
			        Console.Write(e1);
			        // TODO Auto-generated catch block
			        //e1.printStackTrace();
		        }
            }
            //Console.ReadLine();
        }
    }

    public class TridiagMatrix {
	    static double[] x, y, a, b, c,
	                    o1, o2, o3;
	    static Boolean debug = false;
	    double half_PI = Math.PI/2.0;
	    double double_PI = Math.PI*2;
	
	
	    public TridiagMatrix(double[] _x, double[] _y){
		    //double x[] = {1,2,3,4};
		    //double y[] = {1,4,6,8};
		    x = _x;
		    y = _y;
		    a = new double[x.Length];
		    b = new double[x.Length];
		    c = new double[x.Length];
		    b[0] = 1;
		    a[1] = x[1]-x[0];
		    for (int i = 1, l = x.Length-1; i < l; i++){
			    c[i] = a[i+1] = x[i+1]-x[i];
			    b[i] = 2*(a[i]+c[i]);
		    }
		    solveMatrix();
		    if (debug){
			    Console.Write("a:");
			    foreach (double i in y)
				    Console.Write(String.Format("{0} ", i));
			    Console.Write('\n');
			    Console.Write("b:");
			    foreach (double i in o1)
                    Console.Write(String.Format("{0} ", i));
			    Console.Write('\n');
			    Console.Write("c:");
			    foreach (double i in o2)
                    Console.Write(String.Format("{0} ", i));
			    Console.Write('\n');
			    Console.Write("d:");
			    foreach (double i in o3)
                    Console.Write(String.Format("{0} ", i));
			    Console.Write('\n');
		    }
	    }
	
	    public double sine(double e){
		    double answer;
		    int idx;
		    e %= double_PI;
		    if (e < 0) e+= double_PI;
		    int Q = (int)(e/half_PI); //the quadrant
		    e %= half_PI;
            if (debug) Console.Write(String.Format("Quadrant: {0}, x={1} \n", Q, e));
		    if (Q%2 == 1) e = half_PI - e;
		    for (idx = 0; x[idx] < e && idx < x.Length; idx++); if (idx != 0) idx--; //find the correct spine
		    answer = y[idx] + o1[idx]*(e-x[idx]) + o2[idx]*Math.Pow(e-x[idx],2) + o3[idx]*Math.Pow(e-x[idx],3);
		    if (Q>1) answer *= -1;
		    //Console.Write(String.Format("%.8f%+.8f(x%+.8f)%+.8f(x%+.8f)^2%+.8f(x%+.8f)^3\n", y[idx], o1[idx], x[idx]*-1, o2[idx], x[idx]*-1, o3[idx], x[idx]*-1);
		    return answer;
	    }
	
	    public double cosine(double e){
		    return sine(e+half_PI);
	    }
	
	    public double tangent(double e){
		    double sin = sine(e), cos = cosine(e);
		    if (cos == 0) throw new Exception("Division by zero. Tangent is undefined here. \n");
		    return sin/cos;
	    }
	
	    private void solveMatrix(){
		    /**
		     * a - sub-diagonal (means it is the diagonal below the main diagonal) -- indexed from 1..n-1
		     * b - the main diagonal
		     * c - sup-diagonal (means it is the diagonal above the main diagonal) -- indexed from 0..n-2
		     * v - right part
		     * x - the answer
		     */
		    int n = x.Length, l = n-1;
		    o1 = new double[n];
		    o2 = new double[n];
		    o3 = new double[n];
		    double[] v = new double[n];
		    for (int i = 1; i < l; i++){
			    v[i] = (3/a[i+1])*(y[i+1]-y[i])-(3/a[i])*(y[i]-y[i-1]);
		    }
		
		    //double[] x = new double[n];
		    for (int i = 1; i < n; i++){
			    double m = a[i]/b[i-1];
			    b[i] = b[i] - m*c[i-1];
			    v[i] = v[i] - m*v[i-1];
		    }
		    o2[n-1] = v[n-1]/b[n-1];
		
		    //solves for "c"
		    for (int i = n - 2; i >= 0; i--)
			    o2[i]=(v[i]-c[i]*o2[i+1])/b[i];
		
		    //solves for "b"
		    for (int i = 0; i < l; i++)
			    o1[i] = (1/a[i+1])*(y[i+1]-y[i])-(a[i+1]/3)*(o2[i+1]+2*o2[i]);
		
		    //solves for "d"
		    for (int i = 0; i < l; i++)
			    o3[i] = (1/(3*a[i+1]))*(o2[i+1]-o2[i]);
	    }
	
	
    }

}
