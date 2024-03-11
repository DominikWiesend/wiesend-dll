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
using System.Text;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Matrix used in linear algebra
    /// </summary>
    [Serializable]
    public class Matrix
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Width">Width of the matrix</param>
        /// <param name="Height">Height of the matrix</param>
        /// <param name="Values">Values to use in the matrix</param>
        public Matrix(int Width, int Height, double[,] Values = null)
        {
            _Width = Width;
            _Height = Height;
            this.Values = Values ?? new double[Width, Height];
        }

        /// <summary>
        /// Height of the matrix
        /// </summary>
        public virtual int Height
        {
            get { return _Height; }
            set { _Height = value; Values = new double[Width, Height]; }
        }

        /// <summary>
        /// Values for the matrix
        /// </summary>
        public virtual double[,] Values { get; set; }

        /// <summary>
        /// Width of the matrix
        /// </summary>
        public virtual int Width
        {
            get { return _Width; }
            set { _Width = value; Values = new double[Width, Height]; }
        }

        /// <summary>
        /// Sets the values of the matrix
        /// </summary>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        /// <returns>the value at a point in the matrix</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public virtual double this[int X, int Y]
        {
            get
            {
                if (!(X >= 0 && X <= Width)) throw new ArgumentOutOfRangeException(nameof(X), $"Condition {nameof(X)} >= 0 && {nameof(X)} <= Width not met.");
                if (!(Y >= 0 && Y <= Height)) throw new ArgumentOutOfRangeException(nameof(Y), $"Condition {nameof(Y)} >= 0 && {nameof(Y)} <= Height not met.");
                if (Values == null) throw new NullReferenceException("Values");
                return Values[X, Y];
            }

            set
            {
                if (!(X >= 0 && X <= Width)) throw new ArgumentOutOfRangeException(nameof(X), $"Condition {nameof(X)} >= 0 && {nameof(X)} <= Width not met.");
                if (!(Y >= 0 && Y <= Height)) throw new ArgumentOutOfRangeException(nameof(Y), $"Condition {nameof(Y)} >= 0 && {nameof(Y)} <= Height not met.");
                if (Values == null) throw new NullReferenceException("Values");
                Values[X, Y] = value;
            }
        }

        private int _Height = 1;

        private int _Width = 1;

        /// <summary>
        /// Subtracts two matrices
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix M1, Matrix M2)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            if (M2 == null) throw new ArgumentNullException(nameof(M2));
            if (!(M1.Width == M2.Width && M1.Height == M2.Height)) throw new ArgumentException("Both matrices must be the same dimensions.");
            var TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] - M2[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Negates a matrix
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix M1)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            var TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = -M1[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Determines if two matrices are unequal
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(Matrix M1, Matrix M2)
        {
            return !(M1 == M2);
        }

        /// <summary>
        /// Multiplies two matrices
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix M1, Matrix M2)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            if (M2 == null) throw new ArgumentNullException(nameof(M2));
            if (!(M1.Width == M2.Width && M1.Height == M2.Height)) throw new ArgumentException("Both matrices must be the same dimensions.");
            var TempMatrix = new Matrix(M2.Width, M1.Height);
            for (int x = 0; x < M2.Width; ++x)
            {
                for (int y = 0; y < M1.Height; ++y)
                {
                    TempMatrix[x, y] = 0.0;
                    for (int i = 0; i < M1.Width; ++i)
                        for (int j = 0; j < M2.Height; ++j)
                            TempMatrix[x, y] += (M1[i, y] * M2[x, j]);
                }
            }
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to multiply by</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix M1, double D)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            var TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] * D;
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to multiply by</param>
        /// <returns>The result</returns>
        public static Matrix operator *(double D, Matrix M1)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            var TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] * D;
            return TempMatrix;
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to divide by</param>
        /// <returns>The result</returns>
        public static Matrix operator /(Matrix M1, double D)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            return M1 * (1 / D);
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to divide by</param>
        /// <returns>The result</returns>
        public static Matrix operator /(double D, Matrix M1)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            return M1 * (1 / D);
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator +(Matrix M1, Matrix M2)
        {
            if (M1 == null) throw new ArgumentNullException(nameof(M1));
            if (M2 == null) throw new ArgumentNullException(nameof(M2));
            if (!(M1.Width == M2.Width && M1.Height == M2.Height)) throw new ArgumentException("Both matrices must be the same dimensions.");
            var TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] + M2[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Determines if two matrices are equal
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>True if they are equal, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>")]
        public static bool operator ==(Matrix M1, Matrix M2)
        {
            if ((object)M1 == null && (object)M2 == null)
                return true;
            if ((object)M1 == null)
                return false;
            if ((object)M2 == null)
                return false;
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                return false;
            for (int x = 0; x <= M1.Width; ++x)
                for (int y = 0; y <= M1.Height; ++y)
                    if (M1[x, y] != M2[x, y])
                        return false;
            return true;
        }

        /// <summary>
        /// Gets the determinant of a square matrix
        /// </summary>
        /// <returns>The determinant of a square matrix</returns>
        public virtual double Determinant()
        {
            if (!(Width == Height)) throw new InvalidOperationException("The determinant can not be calculated for a non square matrix");
            if (Width == 2)
                return (this[0, 0] * this[1, 1]) - (this[0, 1] * this[1, 0]);
            double Answer = 0.0;
            for (int x = 0; x < Width; ++x)
            {
                var TempMatrix = new Matrix(Width - 1, Height - 1);
                int WidthCounter = 0;
                for (int y = 0; y < Width; ++y)
                {
                    if (y != x)
                    {
                        for (int z = 1; z < Height; ++z)
                            TempMatrix[WidthCounter, z - 1] = this[y, z];
                        ++WidthCounter;
                    }
                }
                if (x % 2 == 0)
                {
                    Answer += TempMatrix.Determinant();
                }
                else
                {
                    Answer -= TempMatrix.Determinant();
                }
            }
            return Answer;
        }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var Tempobj = obj as Matrix;
            return Tempobj != null && this == Tempobj;
        }

        /// <summary>
        /// Gets the hash code for the object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            double Hash = 0;
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    Hash += this[x, y];
            return (int)Hash;
        }

        /// <summary>
        /// Gets the string representation of the matrix
        /// </summary>
        /// <returns>The matrix as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            string Seperator = "";
            Builder.Append('{').Append(System.Environment.NewLine);
            for (int x = 0; x < Width; ++x)
            {
                Builder.Append('{');
                for (int y = 0; y < Height; ++y)
                {
                    Builder.Append(Seperator).Append(this[x, y]);
                    Seperator = ",";
                }
                Builder.Append('}').Append(System.Environment.NewLine);
                Seperator = "";
            }
            Builder.Append('}');
            return Builder.ToString();
        }

        /// <summary>
        /// Transposes the matrix
        /// </summary>
        /// <returns>Returns a new transposed matrix</returns>
        public virtual Matrix Transpose()
        {
            var TempValues = new Matrix(Height, Width);
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    TempValues[y, x] = Values[x, y];
            return TempValues;
        }
    }
}