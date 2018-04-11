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

using System.Drawing;
using System.Drawing.Imaging;
using Wiesend.DataTypes;

namespace Wiesend.Media.Procedural
{
    /// <summary>
    /// Helper class for doing fault formations
    /// </summary>
    public static class FaultFormation
    {
        /// <summary>
        /// Generates a number of faults, returning an image
        /// </summary>
        /// <param name="Width">Width of the resulting image</param>
        /// <param name="Height">Height of the resulting image</param>
        /// <param name="NumberFaults">Number of faults</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>An image from the resulting faults</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body")]
        public static SwiftBitmap Generate(int Width, int Height, int NumberFaults, int Seed)
        {
            float[,] Heights = new float[Width, Height];
            float IncreaseVal = 0.1f;
            var Generator = new System.Random(Seed);
            for (int x = 0; x < NumberFaults; ++x)
            {
                IncreaseVal = GenerateFault(Width, Height, NumberFaults, Heights, IncreaseVal, Generator);
            }
            var ReturnValue = new SwiftBitmap(Width, Height);
            ReturnValue.Lock();
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    float Value = Heights[x, y];
                    Value = (Value * 0.5f) + 0.5f;
                    Value *= 255;
                    var RGBValue = ((int)Value).Clamp(255, 0);
                    ReturnValue.SetPixel(x, y, Color.FromArgb(RGBValue, RGBValue, RGBValue));
                }
            }
            return ReturnValue.Unlock();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "3#")]
        private static float GenerateFault(int Width, int Height, int NumberFaults, float[,] Heights, float IncreaseVal, System.Random Generator)
        {
            if (Generator == null)
                return 0.0f;
            int Wall = 0;
            int Wall2 = 0;
            while (Wall == Wall2)
            {
                Wall = Generator.Next(4);
                Wall2 = Generator.Next(4);
            }
            int X1 = 0;
            int Y1 = 0;
            int X2 = 0;
            int Y2 = 0;
            while (X1 == X2 || Y1 == Y2)
            {
                switch (Wall)
                {
                    case 0:
                        X1 = Generator.Next(Width);
                        Y1 = 0;
                        break;
                    case 1:
                        Y1 = Generator.Next(Height);
                        X1 = Width;
                        break;
                    case 2:
                        X1 = Generator.Next(Width);
                        Y1 = Height;
                        break;
                    default:
                        X1 = 0;
                        Y1 = Generator.Next(Height);
                        break;
                }

                switch (Wall2)
                {
                    case 0:
                        X2 = Generator.Next(Width);
                        Y2 = 0;
                        break;
                    case 1:
                        Y2 = Generator.Next(Height);
                        X2 = Width;
                        break;
                    case 2:
                        X2 = Generator.Next(Width);
                        Y2 = Height;
                        break;
                    default:
                        X2 = 0;
                        Y2 = Generator.Next(Height);
                        break;
                }
            }
            int M = (Y1 - Y2) / (X1 - X2);
            int B = Y1 - (M * X1);
            var Side = Generator.Next(2);
            int Direction = 0;
            while (Direction == 0)
                Direction = Generator.Next(-1, 2);
            float TempIncreaseVal = (float)Generator.NextDouble() * IncreaseVal * (float)Direction;
            if (Side == 0)
            {
                for (int y = 0; y < Width; ++y)
                {
                    int LastY = (M * y) + B;
                    for (int z = 0; z < LastY; ++z)
                    {
                        if (z < Height)
                        {
                            Heights[y, z] += TempIncreaseVal;
                            if (Heights[y, z] > 1.0f)
                                Heights[y, z] = 1.0f;
                            else if (Heights[y, z] < -1.0f)
                                Heights[y, z] = -1.0f;
                        }
                    }
                }
            }
            else
            {
                for (int y = 0; y < Width; ++y)
                {
                    int LastY = (M * y) + B;
                    if (LastY < 0)
                        LastY = 0;
                    for (int z = LastY; z < Height; ++z)
                    {
                        Heights[y, z] += TempIncreaseVal;
                        if (Heights[y, z] > 1.0f)
                            Heights[y, z] = 1.0f;
                        else if (Heights[y, z] < -1.0f)
                            Heights[y, z] = -1.0f;
                    }
                }
            }
            IncreaseVal -= (0.1f / (float)NumberFaults);
            return IncreaseVal;
        }
    }
}