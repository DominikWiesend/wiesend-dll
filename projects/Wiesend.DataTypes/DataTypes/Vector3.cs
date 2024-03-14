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

using JetBrains.Annotations;
using System;
using System.Xml.Serialization;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Vector class (holds three items)
    /// </summary>
    [Serializable]
    public class Vector3
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X">X direction</param>
        /// <param name="Y">Y direction</param>
        /// <param name="Z">Z direction</param>
        public Vector3(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>
        /// Used for converting this to an array and back
        /// </summary>
        public virtual double[] Array
        {
            get { return new double[] { X, Y, Z }; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value.Length == 3)
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                }
            }
        }

        /// <summary>
        /// Returns the magnitude of the vector
        /// </summary>
        public virtual double Magnitude
        {
            get { return ((X * X) + (Y * Y) + (Z * Z)).Sqrt(); }
        }

        /// <summary>
        /// X value
        /// </summary>
        [XmlElement]
        public virtual double X { get; set; }

        /// <summary>
        /// Y Value
        /// </summary>
        [XmlElement]
        public virtual double Y { get; set; }

        /// <summary>
        /// Z value
        /// </summary>
        [XmlElement]
        public virtual double Z { get; set; }

        /// <summary>
        /// Determines the angle between the vectors
        /// </summary>
        /// <param name="V1">Vector 1</param>
        /// <param name="V2">Vector 2</param>
        /// <returns>Angle between the vectors</returns>
        public static double Angle([NotNull] Vector3 V1, [NotNull] Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            V1.Normalize();
            V2.Normalize();
            return System.Math.Acos(Vector3.DotProduct(V1, V2));
        }

        /// <summary>
        /// The distance between two vectors
        /// </summary>
        /// <param name="V1">Vector 1</param>
        /// <param name="V2">Vector 2</param>
        /// <returns>Distance between the vectors</returns>
        public static double Distance([NotNull] Vector3 V1, [NotNull] Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return (((V1.X - V2.X) * (V1.X - V2.X)) + ((V1.Y - V2.Y) * (V1.Y - V2.Y)) + ((V1.Z - V2.Z) * (V1.Z - V2.Z))).Sqrt();
        }

        /// <summary>
        /// Does a dot product
        /// </summary>
        /// <param name="V1">Vector 1</param>
        /// <param name="V2">Vector 2</param>
        /// <returns>a dot product</returns>
        public static double DotProduct([NotNull] Vector3 V1, [NotNull] Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return (V1.X * V2.X) + (V1.Y * V2.Y) + (V1.Z * V2.Z);
        }

        /// <summary>
        /// Interpolates between the vectors
        /// </summary>
        /// <param name="V1">Vector 1</param>
        /// <param name="V2">Vector 2</param>
        /// <param name="Control">Percent to move between 1 and 2</param>
        /// <returns>The interpolated vector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public static Vector3 Interpolate([NotNull] Vector3 V1, [NotNull] Vector3 V2, double Control)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            var TempVector = new Vector3(0.0, 0.0, 0.0);
            TempVector.X = (V1.X * (1 - Control)) + (V2.X * Control);
            TempVector.Y = (V1.Y * (1 - Control)) + (V2.Y * Control);
            TempVector.Z = (V1.Z * (1 - Control)) - (V2.Z * Control);
            return TempVector;
        }

        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator -(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return new Vector3(V1.X - V2.X, V1.Y - V2.Y, V1.Z - V2.Z);
        }

        /// <summary>
        /// Negation
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator -(Vector3 V1)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            return new Vector3(-V1.X, -V1.Y, -V1.Z);
        }

        /// <summary>
        /// Not equals
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static bool operator !=(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return !(V1 == V2);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="D">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator *(Vector3 V1, double D)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            return new Vector3(V1.X * D, V1.Y * D, V1.Z * D);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="D">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator *(double D, Vector3 V1)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            return new Vector3(V1.X * D, V1.Y * D, V1.Z * D);
        }

        /// <summary>
        /// Does a cross product
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator *(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            var TempVector = new Vector3(0.0, 0.0, 0.0)
            {
                X = (V1.Y * V2.Z) - (V1.Z * V2.Y),
                Y = (V1.Z * V2.X) - (V1.X * V2.Z),
                Z = (V1.X * V2.Y) - (V1.Y * V2.X)
            };
            return TempVector;
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="D">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator /(Vector3 V1, double D)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            return new Vector3(V1.X / D, V1.Y / D, V1.Z / D);
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator +(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return new Vector3(V1.X + V2.X, V1.Y + V2.Y, V1.Z + V2.Z);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static bool operator <(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return V1.Magnitude < V2.Magnitude;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static bool operator <=(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return V1.Magnitude <= V2.Magnitude;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static bool operator ==(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return V1.X == V2.X && V1.Y == V2.Y && V1.Z == V2.Z;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static bool operator >(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return V1.Magnitude > V2.Magnitude;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        /// <param name="V1">Item 1</param>
        /// <param name="V2">Item 2</param>
        /// <returns>The resulting vector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static bool operator >=(Vector3 V1, Vector3 V2)
        {
            if (V1 == null) throw new ArgumentNullException(nameof(V1));
            if (V2 == null) throw new ArgumentNullException(nameof(V2));
            return V1.Magnitude >= V2.Magnitude;
        }

        /// <summary>
        /// Determines if the items are equal
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>true if they are, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>")]
        public override bool Equals(object obj)
        {
            Vector3 Tempobj = obj as Vector3;
            return (object)Tempobj != null && this == Tempobj;
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return (int)(X + Y + Z) % Int32.MaxValue;
        }

        /// <summary>
        /// Normalizes the vector
        /// </summary>
        public virtual void Normalize()
        {
            double Normal = Magnitude;
            if (Normal > 0)
            {
                Normal = 1 / Normal;
                X *= Normal;
                Y *= Normal;
                Z *= Normal;
            }
        }

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>String representation of the vector</returns>
        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }
    }
}