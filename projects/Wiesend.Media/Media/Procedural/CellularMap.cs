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
//    Copyright(c) 2016 Dominik Wiesend. All rights reserved.
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
using System.Collections.Generic;

namespace Wiesend.Media.Procedural
{
    /// <summary>
    /// A cellular map creator
    /// </summary>
    public class CellularMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Seed">Seed for random generation</param>
        /// <param name="Width">Width of the image</param>
        /// <param name="Height">Height of the image</param>
        /// <param name="NumberOfPoints">Number of cells</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public CellularMap(int Seed, int Width, int Height, int NumberOfPoints)
        {
            _Width = Width;
            _Height = Height;
            MinDistance = float.MaxValue;
            MaxDistance = float.MinValue;
            var Rand = new Random.Random(Seed);
            Distances = new float[Width, Height];
            ClosestPoint = new int[Width, Height];
            for (int x = 0; x < NumberOfPoints; ++x)
            {
                var TempPoint = new Point();
                TempPoint.X = Rand.Next(0, Width);
                TempPoint.Y = Rand.Next(0, Height);
                Points.Add(TempPoint);
            }
            CalculateDistances();
        }

        /// <summary>
        /// List of closest cells
        /// </summary>
        public virtual int[,] ClosestPoint { get; set; }

        /// <summary>
        /// Distances to the closest cell
        /// </summary>
        public virtual float[,] Distances { get; set; }

        /// <summary>
        /// Maximum distance to a point
        /// </summary>
        public virtual float MaxDistance { get; set; }

        /// <summary>
        /// Minimum distance to a point
        /// </summary>
        public virtual float MinDistance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        private int _Height = 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        private int _Width = 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        private readonly List<Point> Points = new List<Point>();

        /// <summary>
        /// Calculate the distance between the points
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        private void CalculateDistances()
        {
            if (!(_Width >= 0)) throw new ArgumentOutOfRangeException("_Width", "_Width");
            if (!(_Height >= 0)) throw new ArgumentOutOfRangeException("_Height", "_Height");
            if (Points == null) throw new NullReferenceException("Points");
            for (int x = 0; x < _Width; ++x)
            {
                for (int y = 0; y < _Height; ++y)
                {
                    FindClosestPoint(x, y);
                }
            }
        }

        /// <summary>
        /// Finds the closest cell center
        /// </summary>
        /// <param name="x">x axis</param>
        /// <param name="y">y axis</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        private void FindClosestPoint(int x, int y)
        {
            if (Points == null) throw new NullReferenceException("Points");
            float MaxDistance = float.MaxValue;
            int Index = -1;
            for (int z = 0; z < Points.Count; ++z)
            {
                var Distance = (float)System.Math.Sqrt(((Points[z].X - x) * (Points[z].X - x)) + ((Points[z].Y - y) * (Points[z].Y - y)));
                if (Distance < MaxDistance)
                {
                    MaxDistance = Distance;
                    Index = z;
                }
            }
            ClosestPoint[x, y] = Index;
            Distances[x, y] = MaxDistance;
            if (MaxDistance < this.MinDistance)
                this.MinDistance = MaxDistance;
            if (MaxDistance > this.MaxDistance)
                this.MaxDistance = MaxDistance;
        }
    }

    /// <summary>
    /// Individual point
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "<Pending>")]
    internal class Point
    {
        /// <summary>
        /// X axis
        /// </summary>
        public virtual int X { get; set; }

        /// <summary>
        /// Y axis
        /// </summary>
        public virtual int Y { get; set; }
    }
}