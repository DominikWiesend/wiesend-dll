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
using System.Text;
using Wiesend.DataTypes.Formatters.Interfaces;

namespace Wiesend.DataTypes.Formatters
{
    /// <summary>
    /// Generic string formatter
    /// </summary>
    public class GenericStringFormatter : IFormatProvider, ICustomFormatter, IStringFormatter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStringFormatter()
        {
            DigitChar = '#';
            AlphaChar = '@';
            EscapeChar = '\\';
        }

        /// <summary>
        /// Represents alpha characters (defaults to @)
        /// </summary>
        public virtual char AlphaChar { get; protected set; }

        /// <summary>
        /// Represents digits (defaults to #)
        /// </summary>
        public virtual char DigitChar { get; protected set; }

        /// <summary>
        /// Represents the escape character (defaults to \)
        /// </summary>
        public virtual char EscapeChar { get; protected set; }

        /// <summary>
        /// Formats the string
        /// </summary>
        /// <param name="format">Format to use</param>
        /// <param name="arg">Argument object to use</param>
        /// <param name="formatProvider">Format provider to use</param>
        /// <returns>The formatted string</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return Format(arg.ToString(), format);
        }

        /// <summary>
        /// Formats the string based on the pattern
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="FormatPattern">Format pattern</param>
        /// <returns>The formatted string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public virtual string Format(string Input, string FormatPattern)
        {
            if (!IsValid(FormatPattern))
                throw new ArgumentException("FormatPattern is not valid");
            StringBuilder ReturnValue = new();
            for (int x = 0; x < FormatPattern.Length; ++x)
            {
                if (FormatPattern[x] == EscapeChar)
                {
                    ++x;
                    ReturnValue.Append(FormatPattern[x]);
                }
                else
                {
                    char NextValue = char.MinValue;
                    Input = GetMatchingInput(Input, FormatPattern[x], out NextValue);
                    if (NextValue != char.MinValue)
                        ReturnValue.Append(NextValue);
                }
            }
            return ReturnValue.ToString();
        }

        /// <summary>
        /// Gets the format associated with the type
        /// </summary>
        /// <param name="formatType">Format type</param>
        /// <returns>The appropriate formatter based on the type</returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        /// <summary>
        /// Gets matching input
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="FormatChar">Current format character</param>
        /// <param name="MatchChar">The matching character found</param>
        /// <returns>The remainder of the input string left</returns>
        protected virtual string GetMatchingInput(string Input, char FormatChar, out char MatchChar)
        {
            bool Digit = FormatChar == DigitChar;
            bool Alpha = FormatChar == AlphaChar;
            if (!Digit && !Alpha)
            {
                MatchChar = FormatChar;
                return Input;
            }
            int Index = 0;
            MatchChar = char.MinValue;
            for (int x = 0; x < Input.Length; ++x)
            {
                if ((Digit && char.IsDigit(Input[x])) || (Alpha && char.IsLetter(Input[x])))
                {
                    MatchChar = Input[x];
                    Index = x + 1;
                    break;
                }
            }
            return Input.Substring(Index);
        }

        /// <summary>
        /// Checks if the format pattern is valid
        /// </summary>
        /// <param name="FormatPattern">Format pattern</param>
        /// <returns>Returns true if it's valid, otherwise false</returns>
        protected virtual bool IsValid([NotNull] string FormatPattern)
        {
            if (string.IsNullOrEmpty(FormatPattern)) throw new ArgumentNullException(nameof(FormatPattern));
            bool EscapeCharFound = false;
            for (int x = 0; x < FormatPattern.Length; ++x)
            {
                if (EscapeCharFound && FormatPattern[x] != DigitChar
                        && FormatPattern[x] != AlphaChar
                        && FormatPattern[x] != EscapeChar)
                    return false;
                else if (EscapeCharFound)
                    EscapeCharFound = false;
                else EscapeCharFound |= FormatPattern[x] == EscapeChar;
            }
            if (EscapeCharFound)
                return false;
            return true;
        }
    }
}