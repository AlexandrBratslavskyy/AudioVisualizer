using System;
using System.Collections.Generic;

namespace AudioVisualizer
{
    public class A
    {
        public A() { }
        public A(long N) {
            for (long i = 0; i < N; ++i)
            {
                data.Add(new Complex(0, 0));
            }
        }
        public void Add(double real, double imaginary)
        {
            data.Add(new Complex(real, imaginary));
        }
        public void Set(long f, double real, double imaginary)
        {
            data[(int)f] = new Complex(real, imaginary);
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

        //Getter for real portion
        public double getReal()
        {
            return re;
        }

        //Getter for imaginary portion
        public double getImm()
        {
            return im;
        }
        //Return amplitude using pythag sqrt(real*real + im*im)
        public double getAmp()
        {
            return Math.Sqrt(re * re + im * im);
        }
    }
}
