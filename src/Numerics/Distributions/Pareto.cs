﻿// <copyright file="Pareto.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2013 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Collections.Generic;
using MathNet.Numerics.Properties;

namespace MathNet.Numerics.Distributions
{
    /// <summary>
    /// Continuous Univariate Pareto distribution.
    /// The Pareto distribution is a power law probability distribution that coincides with social, 
    /// scientific, geophysical, actuarial, and many other types of observable phenomena.
    /// For details about this distribution, see 
    /// <a href="http://en.wikipedia.org/wiki/Pareto_distribution">Wikipedia - Pareto distribution</a>.
    /// </summary>
    /// <remarks><para>The distribution will use the <see cref="System.Random"/> by default. 
    /// Users can get/set the random number generator by using the <see cref="RandomSource"/> property.</para>
    /// <para>The statistics classes will check all the incoming parameters whether they are in the allowed
    /// range. This might involve heavy computation. Optionally, by setting Control.CheckDistributionParameters
    /// to <c>false</c>, all parameter checks can be turned off.</para></remarks>
    public class Pareto : IContinuousDistribution
    {
        System.Random _random;

        double _scale;
        double _shape;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pareto"/> class. 
        /// </summary>
        /// <param name="scale">The scale (xm) of the distribution.</param>
        /// <param name="shape">The shape (α) of the distribution.</param>
        /// <exception cref="ArgumentException">If <paramref name="scale"/> or <paramref name="shape"/> are negative.</exception>
        public Pareto(double scale, double shape)
        {
            _random = new System.Random();
            SetParameters(scale, shape);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pareto"/> class. 
        /// </summary>
        /// <param name="scale">The scale (xm) of the distribution.</param>
        /// <param name="shape">The shape (α) of the distribution.</param>
        /// <param name="randomSource">The random number generator which is used to draw random samples.</param>
        /// <exception cref="ArgumentException">If <paramref name="scale"/> or <paramref name="shape"/> are negative.</exception>
        public Pareto(double scale, double shape, System.Random randomSource)
        {
            _random = randomSource ?? new System.Random();
            SetParameters(scale, shape);
        }

        /// <summary>
        /// A string representation of the distribution.
        /// </summary>
        /// <returns>a string representation of the distribution.</returns>
        public override string ToString()
        {
            return "Pareto(xm = " + _scale + ", α = " + _shape + ")";
        }

        /// <summary>
        /// Checks whether the parameters of the distribution are valid. 
        /// </summary>
        /// <param name="scale">The scale (xm) of the distribution.</param>
        /// <param name="shape">The shape (α) of the distribution.</param>
        /// <returns><c>true</c> when the parameters are valid, <c>false</c> otherwise.</returns>
        static bool IsValidParameterSet(double scale, double shape)
        {
            return scale > 0.0 && shape > 0.0;
        }

        /// <summary>
        /// Sets the parameters of the distribution after checking their validity.
        /// </summary>
        /// <param name="scale">The scale (xm) of the distribution.</param>
        /// <param name="shape">The shape (α) of the distribution.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the parameters don't pass the <see cref="IsValidParameterSet"/> function.</exception>
        void SetParameters(double scale, double shape)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(scale, shape))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            _scale = scale;
            _shape = shape;
        }

        /// <summary>
        /// Gets or sets the scale (xm) of the distribution.
        /// </summary>
        public double Scale
        {
            get { return _scale; }
            set { SetParameters(value, _shape); }
        }

        /// <summary>
        /// Gets or sets the shape (α) of the distribution.
        /// </summary>
        public double Shape
        {
            get { return _shape; }
            set { SetParameters(_scale, value); }
        }

        /// <summary>
        /// Gets or sets the random number generator which is used to draw random samples.
        /// </summary>
        public System.Random RandomSource
        {
            get { return _random; }
            set { _random = value ?? new System.Random(); }
        }

        /// <summary>
        /// Gets the mean of the distribution.
        /// </summary>
        public double Mean
        {
            get
            {
                if (_shape <= 1)
                {
                    throw new NotSupportedException();
                }

                return _shape*_scale/(_shape - 1.0);
            }
        }

        /// <summary>
        /// Gets the variance of the distribution.
        /// </summary>
        public double Variance
        {
            get
            {
                if (_shape <= 2.0)
                {
                    return double.PositiveInfinity;
                }

                return _scale*_scale*_shape/((_shape - 1.0)*(_shape - 1.0)*(_shape - 2.0));
            }
        }

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        public double StdDev
        {
            get { return (_scale*Math.Sqrt(_shape))/(Math.Abs(_shape - 1.0)*Math.Sqrt(_shape - 2.0)); }
        }

        /// <summary>
        /// Gets the entropy of the distribution.
        /// </summary>
        public double Entropy
        {
            get { return Math.Log(_shape/_scale) - (1.0/_shape) - 1.0; }
        }

        /// <summary>
        /// Gets the skewness of the distribution.
        /// </summary>
        public double Skewness
        {
            get { return (2.0*(_shape + 1.0)/(_shape - 3.0))*Math.Sqrt((_shape - 2.0)/_shape); }
        }

        /// <summary>
        /// Gets the mode of the distribution.
        /// </summary>
        public double Mode
        {
            get { return _scale; }
        }

        /// <summary>
        /// Gets the median of the distribution.
        /// </summary>
        public double Median
        {
            get { return _scale*Math.Pow(2.0, 1.0/_shape); }
        }

        /// <summary>
        /// Gets the minimum of the distribution.
        /// </summary>
        public double Minimum
        {
            get { return _scale; }
        }

        /// <summary>
        /// Gets the maximum of the distribution.
        /// </summary>
        public double Maximum
        {
            get { return Double.PositiveInfinity; }
        }

        /// <summary>
        /// Computes the density of the distribution (PDF), i.e. dP(X &lt;= x)/dx.
        /// </summary>
        /// <param name="x">The location at which to compute the density.</param>
        /// <returns>the density at <paramref name="x"/>.</returns>
        public double Density(double x)
        {
            return _shape*Math.Pow(_scale, _shape)/Math.Pow(x, _shape + 1.0);
        }

        /// <summary>
        /// Computes the log density of the distribution (lnPDF), i.e. ln(dP(X &lt;= x)/dx).
        /// </summary>
        /// <param name="x">The location at which to compute the log density.</param>
        /// <returns>the log density at <paramref name="x"/>.</returns>
        public double DensityLn(double x)
        {
            return Math.Log(Density(x));
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution, i.e. P(X &lt;= x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        public double CumulativeDistribution(double x)
        {
            return 1.0 - Math.Pow(_scale/x, _shape);
        }

        /// <summary>
        /// Generates a sample from the Pareto distribution without doing parameter checking.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="scale">The scale (xm) of the distribution.</param>
        /// <param name="shape">The shape (α) of the distribution.</param>
        /// <returns>a random number from the Pareto distribution.</returns>
        internal static double SampleUnchecked(System.Random rnd, double scale, double shape)
        {
            return scale*Math.Pow(rnd.NextDouble(), -1.0/shape);
        }

        /// <summary>
        /// Draws a random sample from the distribution.
        /// </summary>
        /// <returns>A random number from this distribution.</returns>
        public double Sample()
        {
            return SampleUnchecked(RandomSource, _scale, _shape);
        }

        /// <summary>
        /// Generates a sequence of samples from the Pareto distribution.
        /// </summary>
        /// <returns>a sequence of samples from the distribution.</returns>
        public IEnumerable<double> Samples()
        {
            while (true)
            {
                yield return SampleUnchecked(RandomSource, _scale, _shape);
            }
        }

        /// <summary>
        /// Generates a sample from the distribution.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="scale">The scale (xm) of the distribution.</param>
        /// <param name="shape">The shape (α) of the distribution.</param>
        /// <returns>a sample from the distribution.</returns>
        public static double Sample(System.Random rnd, double scale, double shape)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(scale, shape))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            return SampleUnchecked(rnd, scale, shape);
        }

        /// <summary>
        /// Generates a sequence of samples from the distribution.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="scale">The scale (xm) of the distribution.</param>
        /// <param name="shape">The shape (α) of the distribution.</param>
        /// <returns>a sequence of samples from the distribution.</returns>
        public static IEnumerable<double> Samples(System.Random rnd, double scale, double shape)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(scale, shape))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            while (true)
            {
                yield return SampleUnchecked(rnd, scale, shape);
            }
        }
    }
}