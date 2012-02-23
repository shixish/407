
public class TridiagMatrix {
	static double x[], y[], a[], b[], c[];
	static double o1[], o2[], o3[];
	static boolean debug = false;
	double half_PI = Math.PI/2.0;
	double double_PI = Math.PI*2;
	
	
	TridiagMatrix(double _x[], double _y[]){
		//double x[] = {1,2,3,4};
		//double y[] = {1,4,6,8};
		x = _x;
		y = _y;
		a = new double[x.length];
		b = new double[x.length];
		c = new double[x.length];
		b[0] = 1;
		a[1] = x[1]-x[0];
		for (int i = 1, l = x.length-1; i < l; i++){
			c[i] = a[i+1] = x[i+1]-x[i];
			b[i] = 2*(a[i]+c[i]);
		}
		solveMatrix();
		if (debug){
			System.out.print("a:");
			for (double i : y)
				System.out.format("%.8f ", i);
			System.out.print('\n');
			System.out.print("b:");
			for (double i : o1)
				System.out.format("%.8f ", i);
			System.out.print('\n');
			System.out.print("c:");
			for (double i : o2)
				System.out.format("%.8f ", i);
			System.out.print('\n');
			System.out.print("d:");
			for (double i : o3)
				System.out.format("%.8f ", i);
			System.out.print('\n');
		}
	}
	
	public double sine(double e){
		double answer;
		int idx;
		e %= double_PI;
		if (e < 0) e+= double_PI;
		int Q = (int)(e/half_PI); //the quadrant
		e %= half_PI;
		if (debug) System.out.format("Quadrant: %d, x=%f \n", Q, e);
		if (Q%2 == 1) e = half_PI - e;
		for (idx = 0; x[idx] < e && idx < x.length; idx++); if (idx != 0) idx--; //find the correct spine
		answer = y[idx] + o1[idx]*(e-x[idx]) + o2[idx]*Math.pow(e-x[idx],2) + o3[idx]*Math.pow(e-x[idx],3);
		if (Q>1) answer *= -1;
		//System.out.format("%.8f%+.8f(x%+.8f)%+.8f(x%+.8f)^2%+.8f(x%+.8f)^3\n", y[idx], o1[idx], x[idx]*-1, o2[idx], x[idx]*-1, o3[idx], x[idx]*-1);
		return answer;
	}
	
	public double cosine(double e){
		return sine(e+half_PI);
	}
	
	public double tangent(double e) throws Exception{
		double sin = sine(e), cos = cosine(e);
		if (cos == 0) throw new Exception("Division by zero. Tangent is undefined here. \n");
		return sine(e)/cosine(e);
	}
	
	private void solveMatrix(){
		/**
		 * a - sub-diagonal (means it is the diagonal below the main diagonal) -- indexed from 1..n-1
		 * b - the main diagonal
		 * c - sup-diagonal (means it is the diagonal above the main diagonal) -- indexed from 0..n-2
		 * v - right part
		 * x - the answer
		 */
		int n = x.length, l = n-1;
		o1 = new double[n];
		o2 = new double[n];
		o3 = new double[n];
		double v[] = new double[n];
		for (int i = 1; i < l; i++){
			v[i] = (3/a[i+1])*(y[i+1]-y[i])-(3/a[i])*(y[i]-y[i-1]);
		}
		
		//double x[] = new double[n];
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
