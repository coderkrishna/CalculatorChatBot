﻿// <copyright file="GeometricOps.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Operations
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This class represents all of the possible operations that this bot could do
    /// that belong to the Geometric category.
    /// </summary>
    public static class GeometricOps
    {
        /// <summary>
        /// Calculates the discriminant given three integers, a value for A, a value for B, and
        /// a value for C.
        /// </summary>
        /// <param name="inputString">The three integers.</param>
        /// <returns>An integer.</returns>
        public static int CalculateDiscriminant(string inputString)
        {
            if (inputString is null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            string[] inputStringArr = inputString.Split(',');
            int[] inputIntsArr = Array.ConvertAll(inputStringArr, int.Parse);

            int a = inputIntsArr[0];
            int b = inputIntsArr[1];
            int c = inputIntsArr[2];

            int discriminant = (int)Math.Pow(b, 2) - (4 * a * c);

            return discriminant;
        }

        /// <summary>
        /// Given the values of A, B, and C this function will then calculate the necessary
        /// roots of for the equation Ax^2+Bx+C = 0.
        /// </summary>
        /// <param name="inputString">The values of a, b, and c.</param>
        /// <returns>A string that contains the roots.</returns>
        public static string CalculateQuadraticRoots(string inputString)
        {
            if (inputString is null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            string[] inputStringArr = inputString.Split(',');
            int[] inputIntsArr = Array.ConvertAll(inputStringArr, int.Parse);

            var resultString = string.Empty;

            if (inputIntsArr.Length != 3)
            {
                resultString = "ERROR";
            }
            else
            {
                int a = inputIntsArr[0];
                int b = inputIntsArr[1];
                int c = inputIntsArr[2];

                double r1, r2, discriminant;
                int m;

                discriminant = Math.Pow(b, 2) - (4 * a * c);

                if (a == 0)
                {
                    m = 1;
                }
                else if (discriminant > 0)
                {
                    m = 2;
                }
                else if (discriminant == 0)
                {
                    m = 3;
                }
                else
                {
                    m = 4;
                }

                switch (m)
                {
                    case 1:
                        resultString = "Not a quadratic equation, linear equation";
                        break;
                    case 2:
                        r1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                        r2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                        resultString = $"{r1}, {r2}";
                        break;
                    case 3:
                        r1 = r2 = (-b) / (2 * a);
                        resultString = r1.ToString(CultureInfo.InvariantCulture);
                        break;
                    case 4:
                        r1 = (-b) / (2 * a);
                        r2 = Math.Sqrt(-discriminant) / (2 * a);
                        resultString = string.Format(CultureInfo.InvariantCulture, "{0:#.##} + i {1:#.##}", r1, r2) + "," + string.Format(CultureInfo.InvariantCulture, "{0:#.##} - i {1:#.##}", r1, r2);
                        break;
                    default:
                        resultString = "ERROR";
                        break;
                }
            }

            return resultString;
        }

        /// <summary>
        /// Given the values of two legs in a right triangle, this function will actually
        /// calculate the value of the hypotenuse.
        /// </summary>
        /// <param name="inputString">The values of the two legs.</param>
        /// <returns>A string that would list out the Pythagorean Triple.</returns>
        public static string CalculatePythagoreanTriple(string inputString)
        {
            if (inputString is null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            string[] inputStringArr = inputString.Split(',');
            int[] inputIntsArr = Array.ConvertAll(inputStringArr, int.Parse);

            var resultString = string.Empty;

            if (inputIntsArr.Length == 2)
            {
                int a = inputIntsArr[0];
                int b = inputIntsArr[1];

                double c = CalculateHypotenuse(a, b);

                resultString = $"{a}, {b}, {decimal.Round(decimal.Parse(c.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture), 2)}";
            }
            else
            {
                resultString = "ERROR";
            }

            return resultString;
        }

        /// <summary>
        /// Method that would calculate the midpoint of a line segment.
        /// </summary>
        /// <param name="inputString">4 integers that would represent the 2 sets of coordinates.</param>
        /// <returns>The midpoint as a string.</returns>
        public static string CalculateMidpoint(string inputString)
        {
            if (inputString is null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            string[] inputStringArr = inputString.Split(',');
            int[] inputInts = Array.ConvertAll(inputStringArr, int.Parse);

            var resultString = string.Empty;
            if (inputInts.Length == 4)
            {
                int x1 = inputInts[0];
                int y1 = inputInts[1];
                int x2 = inputInts[2];
                int y2 = inputInts[3];

                int midX = (x1 + x2) / 2;
                int midY = (y1 + y2) / 2;

                resultString = $"{midX}, {midY}";
            }
            else
            {
                resultString = "ERROR";
            }

            return resultString;
        }

        /// <summary>
        /// Having the method to calculate the distance between 2 points.
        /// </summary>
        /// <param name="inputString">The two points in a geometric space.</param>
        /// <returns>The distance between 2 points.</returns>
        public static double CalculateDistance(string inputString)
        {
            if (inputString is null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            string[] inputStringArr = inputString.Split(',');
            int[] inputInts = Array.ConvertAll(inputStringArr, int.Parse);

            double result = 0;
            if (inputInts.Length == 4)
            {
                int x1 = inputInts[0];
                int y1 = inputInts[1];

                int x2 = inputInts[2];
                int y2 = inputInts[3];

                var deltaX = x2 - x1;
                var deltaY = y2 - y1;

                var distanceResult = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

                result = distanceResult;
            }
            else
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Method to calculate the hypotenuse.
        /// </summary>
        /// <param name="a">First leg of the right triangle.</param>
        /// <param name="b">Second leg of the right triangle.</param>
        /// <returns>A double value that represents the hypotenuse.</returns>
        private static double CalculateHypotenuse(int a, int b)
        {
            var cSquared = Math.Pow(a, 2) + Math.Pow(b, 2);
            double c = Math.Sqrt(cSquared);

            return c;
        }
    }
}