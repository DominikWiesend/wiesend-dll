#region Project Description [About this]
// =================================================================================
//            The whole Project is Licensed under the MIT License
// =================================================================================
// =================================================================================
//    Wiesend's Dynamic Link Library is a collection of reusable code that 
//    I've written, or found throughout my programming career. 
//
//    I tried my very best to mention all of the original copyright holders. 
//    I hope that all code which I've copied from others is mentioned and all 
//    their copyrights are given. The copied code (or code snippets) could 
//    already be modified by me or others.
// =================================================================================
#endregion of Project Description
#region Original Source Code [Links to all original source code]
// =================================================================================
//          Original Source Code [Links to all original source code]
// =================================================================================
// https://github.com/JaCraig/Craig-s-Utility-Library
// =================================================================================
//    I didn't wrote this source totally on my own, this class could be nearly a 
//    clone of the project of James Craig, I did highly get inspired by him, so 
//    this piece of code isn't totally mine, shame on me, I'm not the best!
// =================================================================================
#endregion of where is the original source code
#region Licenses [MIT Licenses]
#region MIT License [James Craig]
// =================================================================================
//    Copyright(c) 2014 <a href="http://www.gutgames.com">James Craig</a>
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
// =================================================================================
#endregion of MIT License [James Craig] 
#region MIT License [Dominik Wiesend]
// =================================================================================
//    Copyright(c) 2018 Dominik Wiesend. All rights reserved.
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
// =================================================================================
#endregion of MIT License [Dominik Wiesend] 
#endregion of Licenses [MIT Licenses]

using System;
using System.Globalization;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Represents a fraction
    /// </summary>
    public class Fraction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(int Numerator, int Denominator)
        {
            this.Numerator = Numerator;
            this.Denominator = Denominator;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(double Numerator, double Denominator)
        {
            while (Numerator != System.Math.Round(Numerator, MidpointRounding.AwayFromZero)
                || Denominator != System.Math.Round(Denominator, MidpointRounding.AwayFromZero))
            {
                Numerator *= 10;
                Denominator *= 10;
            }
            this.Numerator = (int)Numerator;
            this.Denominator = (int)Denominator;
            if (this.Denominator == int.MinValue)
                return;
            this.Reduce();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(decimal Numerator, decimal Denominator)
        {
            while (Numerator != System.Math.Round(Numerator, MidpointRounding.AwayFromZero)
                || Denominator != System.Math.Round(Denominator, MidpointRounding.AwayFromZero))
            {
                Numerator *= 10;
                Denominator *= 10;
            }
            this.Numerator = (int)Numerator;
            this.Denominator = (int)Denominator;
            this.Reduce();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(float Numerator, float Denominator)
        {
            if (!(Denominator != Int32.MinValue)) throw new ArgumentException($"Condition {nameof(Denominator)} != Int32.MinValue not met.", nameof(Denominator));
            while (Numerator != System.Math.Round(Numerator, MidpointRounding.AwayFromZero)
                || Denominator != System.Math.Round(Denominator, MidpointRounding.AwayFromZero))
            {
                Numerator *= 10;
                Denominator *= 10;
            }
            this.Numerator = (int)Numerator;
            this.Denominator = (int)Denominator;
            if (this.Denominator == int.MinValue)
                return;
            this.Reduce();
        }

        /// <summary>
        /// Denominator of the fraction
        /// </summary>
        public int Denominator { get; set; }

        /// <summary>
        /// Numerator of the faction
        /// </summary>
        public int Numerator { get; set; }

        /// <summary>
        /// Converts the fraction to a decimal
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a decimal</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static implicit operator decimal(Fraction Fraction)
        {
            if (Fraction == null) throw new ArgumentNullException(nameof(Fraction));
            return ((decimal)Fraction.Numerator / (decimal)Fraction.Denominator);
        }

        /// <summary>
        /// Converts the fraction to a double
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a double</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static implicit operator double(Fraction Fraction)
        {
            if (Fraction == null) throw new ArgumentNullException(nameof(Fraction));
            return ((double)Fraction.Numerator / (double)Fraction.Denominator);
        }

        /// <summary>
        /// Converts the fraction to a float
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a float</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static implicit operator float(Fraction Fraction)
        {
            if (Fraction == null) throw new ArgumentNullException(nameof(Fraction));
            return ((float)Fraction.Numerator / (float)Fraction.Denominator);
        }

        /// <summary>
        /// Converts the double to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The double as a fraction</returns>
        public static implicit operator Fraction(double Fraction)
        {
            return new Fraction(Fraction, 1.0);
        }

        /// <summary>
        /// Converts the decimal to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The decimal as a fraction</returns>
        public static implicit operator Fraction(decimal Fraction)
        {
            return new Fraction(Fraction, 1.0m);
        }

        /// <summary>
        /// Converts the float to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The float as a fraction</returns>
        public static implicit operator Fraction(float Fraction)
        {
            return new Fraction(Fraction, 1.0);
        }

        /// <summary>
        /// Converts the int to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The int as a fraction</returns>
        public static implicit operator Fraction(int Fraction)
        {
            return new Fraction(Fraction, 1);
        }

        /// <summary>
        /// Converts the uint to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The uint as a fraction</returns>
        [CLSCompliant(false)]
        public static implicit operator Fraction(uint Fraction)
        {
            return new Fraction((int)Fraction, 1);
        }

        /// <summary>
        /// Converts the fraction to a string
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static implicit operator string(Fraction Fraction)
        {
            if (Fraction == null) throw new ArgumentNullException(nameof(Fraction));
            return Fraction.ToString();
        }

        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="First">First fraction</param>
        /// <param name="Second">Second fraction</param>
        /// <returns>The subtracted fraction</returns>
        public static Fraction operator -(Fraction First, Fraction Second)
        {
            if (First == null) throw new ArgumentNullException(nameof(First));
            if (Second == null) throw new ArgumentNullException(nameof(Second));
            var Value1 = new Fraction(First.Numerator * (int)Second.Denominator, First.Denominator * Second.Denominator);
            var Value2 = new Fraction(Second.Numerator * (int)First.Denominator, Second.Denominator * First.Denominator);
            var Result = new Fraction(Value1.Numerator - Value2.Numerator, Value1.Denominator);
            Result.Reduce();
            return Result;
        }

        /// <summary>
        /// Negation of the fraction
        /// </summary>
        /// <param name="First">Fraction to negate</param>
        /// <returns>The negated fraction</returns>
        public static Fraction operator -(Fraction First)
        {
            if (First == null) throw new ArgumentNullException(nameof(First));
            return new Fraction(-First.Numerator, First.Denominator);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(Fraction First, Fraction Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(Fraction First, double Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(double First, Fraction Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="First">First fraction</param>
        /// <param name="Second">Second fraction</param>
        /// <returns>The resulting fraction</returns>
        public static Fraction operator *(Fraction First, Fraction Second)
        {
            if (First == null) throw new ArgumentNullException(nameof(First));
            if (Second == null) throw new ArgumentNullException(nameof(Second));
            var Result = new Fraction(First.Numerator * Second.Numerator, First.Denominator * Second.Denominator);
            Result.Reduce();
            return Result;
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>The divided fraction</returns>
        public static Fraction operator /(Fraction First, Fraction Second)
        {
            if (First == null) throw new ArgumentNullException(nameof(First));
            if (Second == null) throw new ArgumentNullException(nameof(Second));
            return First * Second.Inverse();
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="First">First fraction</param>
        /// <param name="Second">Second fraction</param>
        /// <returns>The added fraction</returns>
        public static Fraction operator +(Fraction First, Fraction Second)
        {
            if (First == null) throw new ArgumentNullException(nameof(First));
            if (Second == null) throw new ArgumentNullException(nameof(Second));
            var Value1 = new Fraction(First.Numerator * (int)Second.Denominator, First.Denominator * Second.Denominator);
            var Value2 = new Fraction(Second.Numerator * (int)First.Denominator, Second.Denominator * First.Denominator);
            var Result = new Fraction(Value1.Numerator + Value2.Numerator, Value1.Denominator);
            Result.Reduce();
            return Result;
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Fraction First, Fraction Second)
        {
            return First.Equals(Second);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Fraction First, double Second)
        {
            return First.Equals(Second);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(double First, Fraction Second)
        {
            return Second.Equals(First);
        }

        /// <summary>
        /// Determines if the fractions are equal
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "<Pending>")]
        public override bool Equals(object obj)
        {
            Fraction Other = obj as Fraction;
            if (((object)Other) == null)
                return false;
            decimal Value1 = this;
            decimal Value2 = Other;
            return Value1 == Value2;
        }

        /// <summary>
        /// Gets the hash code of the fraction
        /// </summary>
        /// <returns>The hash code of the fraction</returns>
        public override int GetHashCode()
        {
            return Numerator.GetHashCode() % Denominator.GetHashCode();
        }

        /// <summary>
        /// Returns the inverse of the fraction
        /// </summary>
        /// <returns>The inverse</returns>
        public Fraction Inverse()
        {
            return new Fraction((int)Denominator, Numerator);
        }

        /// <summary>
        /// Reduces the fraction (finds the greatest common denominator and divides the
        /// numerator/denominator by it).
        /// </summary>
        public void Reduce()
        {
            if (!(Numerator != Int32.MinValue)) throw new ArgumentOutOfRangeException(nameof(Numerator), "Numerator can't equal Int32.MinValue");
            if (!(Denominator != Int32.MinValue)) throw new ArgumentOutOfRangeException(nameof(Denominator), "Denominator can't equal Int32.MinValue");
            var GCD = Numerator.GreatestCommonDenominator(Denominator);
            if (GCD != 0)
            {
                this.Numerator /= GCD;
                this.Denominator /= GCD;
            }
        }

        /// <summary>
        /// Displays the fraction as a string
        /// </summary>
        /// <returns>The fraction as a string</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Numerator, Denominator);
        }
    }
}