﻿// <copyright file="Hypergeometric.cs" company="Math.NET">
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

using MathNet.Numerics.Properties;
using System.Collections.Generic;

namespace MathNet.Numerics.Distributions
{
    using System;

    /// <summary>
    /// This class implements functionality for the Hypergeometric distribution. This distribution is
    /// a discrete probability distribution that describes the number of successes in a sequence 
    /// of n draws from a finite population without replacement, just as the binomial distribution 
    /// describes the number of successes for draws with replacement
    /// <a href="http://en.wikipedia.org/wiki/Hypergeometric_distribution">Wikipedia - Hypergeometric distribution</a>.
    /// </summary>
    /// <remarks><para>The distribution will use the <see cref="System.Random"/> by default. 
    /// Users can set the random number generator by using the <see cref="RandomSource"/> property</para>.
    /// <para>
    /// The statistics classes will check all the incoming parameters whether they are in the allowed
    /// range. This might involve heavy computation. Optionally, by setting Control.CheckDistributionParameters
    /// to <c>false</c>, all parameter checks can be turned off.</para></remarks>
    public class Hypergeometric : IDiscreteDistribution
    {
        /// <summary>
        /// The size of the population (N).
        /// </summary>
        int _population;

        /// <summary>
        /// The number successes within the population (K, M).
        /// </summary>
        int _success;

        /// <summary>
        /// The number of draws without replacement (n).
        /// </summary>
        int _draws;

        /// <summary>
        /// The distribution's random number generator.
        /// </summary>
        Random _random;

        /// <summary>
        /// Initializes a new instance of the Hypergeometric class.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public Hypergeometric(int population, int success, int draws)
        {
            _random = new Random();
            SetParameters(population, success, draws);
        }

        /// <summary>
        /// Initializes a new instance of the Hypergeometric class.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        /// <param name="randomSource">The random number generator which is used to draw random samples.</param>
        public Hypergeometric(int population, int success, int draws, Random randomSource)
        {
            _random = randomSource ?? new Random();
            SetParameters(population, success, draws);
        }

        /// <summary>
        /// Sets the parameters of the distribution after checking their validity.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        void SetParameters(int population, int success, int draws)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(population, success, draws))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            _population = population;
            _success = success;
            _draws = draws;
        }

        /// <summary>
        /// Checks whether the parameters of the distribution are valid.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        /// <returns><c>true</c> when the parameters are valid, <c>false</c> otherwise.</returns>
        static bool IsValidParameterSet(int population, int success, int draws)
        {
            if (population < 0 || success < 0 || draws < 0)
            {
                return false;
            }

            if (success > population || draws > population)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the size of the population (N).
        /// </summary>
        public int Population
        {
            get { return _population; }
            set { SetParameters(value, _success, _draws); }
        }

        /// <summary>
        /// Gets or sets the number of draws without replacement (n).
        /// </summary>
        public int Draws
        {
            get { return _draws; }
            set { SetParameters(_population, value, _draws); }
        }

        /// <summary>
        /// Gets or sets the number successes within the population (K, M).
        /// </summary>
        public int Success
        {
            get { return _success; }
            set { SetParameters(_population, _success, value); }
        }

        /// <summary>
        /// Gets or sets the size of the population (N).
        /// </summary>
        [Obsolete("Use Population instead. Scheduled for removal in v3.0.")]
        public int PopulationSize
        {
            get { return _population; }
            set { SetParameters(value, _success, _draws); }
        }

        /// <summary>
        /// Gets or sets the number of draws without replacement (n).
        /// </summary>
        [Obsolete("Use Draws instead. Scheduled for removal in v3.0.")]
        public int N
        {
            get { return _draws; }
            set { SetParameters(_population, value, _draws); }
        }

        /// <summary>
        /// Gets or sets the number successes within the population (K, M).
        /// </summary>
        [Obsolete("Use Success instead. Scheduled for removal in v3.0.")]
        public int M
        {
            get { return _success; }
            set { SetParameters(_population, _success, value); }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Hypergeometric(N = " + _population + ", M = " + _success + ", n = " + _draws + ")";
        }

        /// <summary>
        /// Gets or sets the random number generator which is used to draw random samples.
        /// </summary>
        public Random RandomSource
        {
            get { return _random; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _random = value;
            }
        }

        /// <summary>
        /// Gets the mean of the distribution.
        /// </summary>
        public double Mean
        {
            get { return (double) _success*_draws/_population; }
        }

        /// <summary>
        /// Gets the variance of the distribution.
        /// </summary>
        public double Variance
        {
            get { return _draws*_success*(_population - _draws)*(_population - _success)/(_population*_population*(_population - 1.0)); }
        }

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        public double StdDev
        {
            get { return Math.Sqrt(Variance); }
        }

        /// <summary>
        /// Gets the entropy of the distribution.
        /// </summary>
        public double Entropy
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the skewness of the distribution.
        /// </summary>
        public double Skewness
        {
            get { return (Math.Sqrt(_population - 1.0)*(_population - (2*_draws))*(_population - (2*_success)))/(Math.Sqrt(_draws*_success*(_population - _success)*(_population - _draws))*(_population - 2.0)); }
        }

        /// <summary>
        /// Gets the mode of the distribution.
        /// </summary>
        public int Mode
        {
            get { return (_draws + 1)*(_success + 1)/(_population + 2); }
        }

        /// <summary>
        /// Gets the median of the distribution.
        /// </summary>
        public int Median
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the minimum of the distribution.
        /// </summary>
        public int Minimum
        {
            get { return Math.Max(0, _draws + _success - _population); }
        }

        /// <summary>
        /// Gets the maximum of the distribution.
        /// </summary>
        public int Maximum
        {
            get { return Math.Min(_success, _draws); }
        }

        /// <summary>
        /// Computes values of the probability mass function (PMF), i.e. P(X = x).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the probability mass function.</param>
        /// <returns>
        /// the probability mass at location <paramref name="k"/>.
        /// </returns>
        public double Probability(int k)
        {
            return SpecialFunctions.Binomial(_success, k)*SpecialFunctions.Binomial(_population - _success, _draws - k)/SpecialFunctions.Binomial(_population, _draws);
        }

        /// <summary>
        /// Computes values of the log probability mass function (lnPMF), i.e. ln(P(X = x)).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the log probability mass function.</param>
        /// <returns>
        /// the log probability mass at location <paramref name="k"/>.
        /// </returns>
        public double ProbabilityLn(int k)
        {
            return Math.Log(Probability(k));
        }

        /// <summary>
        /// Computes the cumulative distribution function (CDF) of the distribution, i.e. P(X &lt;= x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative density.</param>
        /// <returns>the cumulative density at <paramref name="x"/>.</returns>
        public double CumulativeDistribution(double x)
        {
            if (x < Minimum)
            {
                return 0.0;
            }
            if (x >= Maximum)
            {
                return 1.0;
            }

            var k = (int) Math.Floor(x);
            var denominatorLn = SpecialFunctions.BinomialLn(_population, _draws);
            var sum = 0.0;
            for (var i = 0; i <= k; i++)
            {
                sum += Math.Exp(SpecialFunctions.BinomialLn(_success, i) + SpecialFunctions.BinomialLn(_population - _success, _draws - i) - denominatorLn);
            }
            return sum;
        }

        /// <summary>
        /// Generates a sample from the Hypergeometric distribution without doing parameter checking.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The n parameter of the distribution.</param>
        /// <returns>a random number from the Hypergeometric distribution.</returns>
        internal static int SampleUnchecked(Random rnd, int population, int success, int draws)
        {
            var x = 0;

            do
            {
                var p = (double) success/population;
                var r = rnd.NextDouble();
                if (r < p)
                {
                    x++;
                    success--;
                }

                population--;
                draws--;
            } while (0 < draws);

            return x;
        }

        /// <summary>
        /// Samples a Hypergeometric distributed random variable.
        /// </summary>
        /// <returns>The number of successes in n trials.</returns>
        public int Sample()
        {
            return SampleUnchecked(RandomSource, _population, _success, _draws);
        }

        /// <summary>
        /// Samples an array of Hypergeometric distributed random variables.
        /// </summary>
        /// <returns>a sequence of successes in n trials.</returns>
        public IEnumerable<int> Samples()
        {
            while (true)
            {
                yield return SampleUnchecked(RandomSource, _population, _success, _draws);
            }
        }

        /// <summary>
        /// Samples a random variable.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public static int Sample(Random rnd, int population, int success, int draws)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(population, success, draws))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            return SampleUnchecked(rnd, population, success, draws);
        }

        /// <summary>
        /// Samples a sequence of this random variable.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public static IEnumerable<int> Samples(Random rnd, int population, int success, int draws)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(population, success, draws))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            while (true)
            {
                yield return SampleUnchecked(rnd, population, success, draws);
            }
        }
    }
}
