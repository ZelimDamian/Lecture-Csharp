using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LectureDemo
{
    /// <summary>
    /// Interaction logic for PInvokeDemo.xaml
    /// </summary>
    public partial class PInvokeDemo : Window
    {
        #region Singleton

        public PInvokeDemo()
        {
            InitializeComponent();
        }

        static PInvokeDemo instance;

        public static PInvokeDemo Show()
        {
            var window = instance as Window;
            if (window == null)
                window = instance = new PInvokeDemo();

            window.Show();
            window.Focus();
            return instance;
        }

        #endregion

        const UInt64 ITERATIONS = 1000000000;

        private void piNativeButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            double pi = piNative(ITERATIONS);
            sw.Stop();

            statusLabel.Content = "Result is PI = " + pi;

            double time = (double)sw.ElapsedMilliseconds / 1000;
            timeLabel.Content = "Native finished in " + time + " sec";
        }

        private void piManagedButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            double pi = piManaged(ITERATIONS);

            sw.Stop();
            double time = (double)sw.ElapsedMilliseconds / 1000;
            timeLabel.Content = "Manged finished in " + time + " sec";

            statusLabel.Content = "Result is PI = " + pi;
        }



        #region PI
        [DllImport("PInvoke-Native.dll", EntryPoint = "piNative",
            CallingConvention = CallingConvention.Cdecl)]
        static extern double piNative(UInt64 iterations);

        static double piManaged(UInt64 iters)
        {
            double n = iters, i;         // Number of iterations and control variable
            double pi = 4;

            for (i = 3; i <= (n + 2); i += 2)
                pi = pi * ((i - 1) / i) * ((i + 1) / i);

            return pi;
        }
        #endregion

        #region Matrix mult

        [DllImport("PInvoke-Native.dll", EntryPoint = "matmultNative",
          CallingConvention = CallingConvention.Cdecl)]
        static extern double matmultNative(double[] a, double[] b, int aRows, int aCols, int bCols, double[] result);

        unsafe void MultiplyMatrix(double[] a, double[] b, int aRows, int aCols, int bCols, double[] result)
        {
            // Note: as is conventional in C#/C/C++, a and b are in row-major order.
            // Note: bRows (the number of rows in b) must equal aCols.
            int bRows = aCols;
            Debug.Assert(a.Length == aRows * aCols);
            Debug.Assert(b.Length == bRows * bCols);

            fixed (double* c = new double[bCols * bRows], x = result)
            {
                for (int i = 0; i < bRows; ++i) // transpose (large-matrix optimization)
                    for (int j = 0; j < bCols; ++j)
                        c[j * bRows + i] = b[i * bCols + j];

                for (int i = 0; i < aRows; ++i)
                {
                    fixed (double* a_i = &a[i * aCols])
                        for (int j = 0; j < bCols; ++j)
                        {
                            double* c_j = c + j * bRows;
                            double s = 0.0;
                            for (int k = 0; k < aCols; ++k)
                                s += a_i[k] * c_j[k];
                            x[i * bCols + j] = s;
                        }
                }
            }
        }

        #endregion

        const int SIZE = 1000;

        private void matmultNativeButton_Click(object sender, RoutedEventArgs e)
        {
            double[] result = new double[SIZE * SIZE];

            double[] a = new double[SIZE * SIZE];
            double[] b = new double[SIZE * SIZE];

            Generate(a, SIZE);
            Generate(b, SIZE);

            Stopwatch sw = Stopwatch.StartNew();

            matmultNative(a, b, SIZE, SIZE, SIZE, result);

            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            timeLabel.Content = "Native finished in " + time + " ms";

            double trace = Trace(result, SIZE);

            statusLabel.Content = "Matrix trace = " + trace;
        }

        private void matmultManagedButton_Click(object sender, RoutedEventArgs e)
        {
            double[] result = new double[SIZE * SIZE];

            double[] a = new double[SIZE * SIZE];
            double[] b = new double[SIZE * SIZE];

            Generate(a, SIZE);
            Generate(b, SIZE);

            Stopwatch sw = Stopwatch.StartNew();

            MultiplyMatrix(a, b, SIZE, SIZE, SIZE, result);

            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            timeLabel.Content = "Manged finished in " + time + " ms";

            double trace = Trace(result, SIZE);

            statusLabel.Content = "Matrix trace = " + trace;
        }

        private void Generate(double[] values, int size)
        {
            for (int i = 0; i < size * size; i++)
            {
                values[i] = Math.Sin((double)i);
            }
        }

        private double Trace(double[] values, int size)
        {
            double trace = 0;

            for (int i = 0; i < size; i++)
            {
                trace += values[i * size + i];
            }

            return trace;
        }
    }
}
