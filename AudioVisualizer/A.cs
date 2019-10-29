using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    public class A
    {
        public void Add(double real, double imaginary)
        {
            data.Add(new Complex(real, imaginary));
        }
        public void setReal(int f, double r)
        {
            data[f].setReal(r);
        }
        public void setImm(int f, double i)
        {
            data[f].setImm(i);
        }
        public Complex Get(long f)
        {
            return data[(int)f];
        }
        public int Size()
        {
            return data.Count;
        }
        private List<Complex> data = new List<Complex>();
        
        //statics
        static public A DFT(S s)
        {
            A a = new A();
            long N = s.Size();
            for (long f = 0; f < N; f++)
            {
                double real = 0, imm = 0;
                for (long t = 0; t < N; t++)
                {
                    real += s.Get(t) / N * Math.Cos(2 * Math.PI * t * f / N);
                    imm -= s.Get(t) / N * Math.Sin(2 * Math.PI * t * f / N);
                }
                a.Add(real, imm);
            }
            return a;
        }
        //Return amplitude using pythag sqrt(real*real + im*im)
        public double getAmp(Complex num)
        {
            return Math.Sqrt(Math.Pow(num.getReal(), 2) + Math.Pow(num.getImm(), 2));
        }
    }
    //class for Complex numbers
    //has a real and imaginary portion
    public struct Complex
    {
        //declarations of private members
        private double re;
        private double im;

        //Constructor for Complex class
        public Complex(double r, double i)
        {
            re = r;
            im = i;
        }

        //Setter for real portion
        public void setReal(double r)
        {
            re = r;
        }

        //Getter for real portion
        public double getReal()
        {
            return re;
        }

        //Setter for imaginary portion
        public void setImm(double i)
        {
            im = i;
        }

        //Getter for imaginary portion
        public double getImm()
        {
            return im;
        }

        //statics
        //Return amplitude using pythag sqrt(real*real + im*im)
        static public double getAmp(Complex num)
        {
            return Math.Sqrt(Math.Pow(num.getReal(), 2) + Math.Pow(num.getImm(), 2));
        }
    }
}
