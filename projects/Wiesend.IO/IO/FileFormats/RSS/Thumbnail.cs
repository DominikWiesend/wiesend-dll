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
using System.Globalization;
using System.Xml.XPath;

namespace Wiesend.IO.FileFormats.RSS
{
    /// <summary>
    /// Thumbnail info holder
    /// </summary>
    public class Thumbnail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Thumbnail()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Doc">XML element holding info for the enclosure</param>
        public Thumbnail([NotNull] IXPathNavigable Doc)
        {
            if (Doc == null) throw new ArgumentNullException(nameof(Doc));
            var Element = Doc.CreateNavigator();
            if (Element.GetAttribute("url", "") != null)
            {
                Url = Element.GetAttribute("url", "");
            }
            if (Element.GetAttribute("width", "") != null)
            {
                Width = int.Parse(Element.GetAttribute("width", ""), CultureInfo.InvariantCulture);
            }
            if (Element.GetAttribute("height", "") != null)
            {
                Height = int.Parse(Element.GetAttribute("height", ""), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Image height
        /// </summary>
        public virtual int Height { get; set; }

        /// <summary>
        /// Location of the item
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Image width
        /// </summary>
        public virtual int Width { get; set; }

        /// <summary>
        /// to string item. Used for outputting the item to RSS.
        /// </summary>
        /// <returns>A string formatted for RSS output</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                return "<media:thumbnail url=\"" + Url + "\" width=\"" + Width.ToString(CultureInfo.InvariantCulture) + "\" height=\"" + Height + "\" />\r\n";
            }
            return string.Empty;
        }
    }
}