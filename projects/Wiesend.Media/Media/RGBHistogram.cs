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
using JetBrains.Annotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Wiesend.DataTypes;

namespace Wiesend.Media
{
    /// <summary>
    /// Class used to create an RGB Histogram
    /// </summary>
    public class RGBHistogram
    {
        private int Height;

        private int Width;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Image">Image to load</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2214:Do not call overridable methods in constructors", Justification = "<Pending>")]
        public RGBHistogram(SwiftBitmap Image = null)
        {
            R = new float[256];
            G = new float[256];
            B = new float[256];
            if (Image != null)
                LoadImage(Image);
        }

        /// <summary>
        /// Blue values
        /// </summary>
        public virtual float[] B { get; set; }

        /// <summary>
        /// Green values
        /// </summary>
        public virtual float[] G { get; set; }

        /// <summary>
        /// Red values
        /// </summary>
        public virtual float[] R { get; set; }

        /// <summary>
        /// Equalizes the histogram
        /// </summary>
        public virtual void Equalize()
        {
            float TotalPixels = Width * Height;
            int RMax = int.MinValue;
            int RMin = int.MaxValue;
            int GMax = int.MinValue;
            int GMin = int.MaxValue;
            int BMax = int.MinValue;
            int BMin = int.MaxValue;
            for (int x = 0; x < 256; ++x)
            {
                if (R[x] > 0f)
                {
                    if (RMax < x)
                        RMax = x;
                    if (RMin > x)
                        RMin = x;
                }
                if (G[x] > 0f)
                {
                    if (GMax < x)
                        GMax = x;
                    if (GMin > x)
                        GMin = x;
                }
                if (B[x] > 0f)
                {
                    if (BMax < x)
                        BMax = x;
                    if (BMin > x)
                        BMin = x;
                }
            }

            float PreviousR = R[0];
            R[0] = R[0] * 256 / TotalPixels;
            float PreviousG = G[0];
            G[0] = G[0] * 256 / TotalPixels;
            float PreviousB = B[0];
            B[0] = B[0] * 256 / TotalPixels;
            for (int x = 1; x < 256; ++x)
            {
                PreviousR += R[x];
                PreviousG += G[x];
                PreviousB += B[x];
                R[x] = ((PreviousR - R[RMin]) / (TotalPixels - R[RMin])) * 255;
                G[x] = ((PreviousG - G[GMin]) / (TotalPixels - G[GMin])) * 255;
                B[x] = ((PreviousB - B[BMin]) / (TotalPixels - B[BMin])) * 255;
            }
            Width = 256;
            Height = 1;
        }

        /// <summary>
        /// Loads an image
        /// </summary>
        /// <param name="ImageUsing">Image to load</param>
        public virtual void LoadImage([NotNull] SwiftBitmap ImageUsing)
        {
            if (ImageUsing == null) throw new ArgumentNullException(nameof(ImageUsing));
            Width = ImageUsing.Width;
            Height = ImageUsing.Height;
            ImageUsing.Lock();
            R.Clear();
            G.Clear();
            B.Clear();
            for (int x = 0; x < ImageUsing.Width; ++x)
            {
                for (int y = 0; y < ImageUsing.Height; ++y)
                {
                    var TempColor = ImageUsing.GetPixel(x, y);
                    ++R[(int)TempColor.R];
                    ++G[(int)TempColor.G];
                    ++B[(int)TempColor.B];
                }
            }
            ImageUsing.Unlock();
        }

        /// <summary>
        /// Normalizes the histogram
        /// </summary>
        public virtual void Normalize()
        {
            float TotalPixels = Width * Height;
            if (TotalPixels <= 0)
                return;
            for (int x = 0; x < 256; ++x)
            {
                R[x] /= TotalPixels;
                G[x] /= TotalPixels;
                B[x] /= TotalPixels;
            }
        }
    }
}