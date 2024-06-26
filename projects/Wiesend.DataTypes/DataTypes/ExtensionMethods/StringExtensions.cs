﻿#region Project Description [About this]
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Wiesend.DataTypes.Formatters;
using Wiesend.DataTypes.Formatters.Interfaces;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// What sort of string capitalization should be used?
    /// </summary>
    public enum StringCase
    {
        /// <summary>
        /// Sentence capitalization
        /// </summary>
        SentenceCapitalize,

        /// <summary>
        /// First character upper case
        /// </summary>
        FirstCharacterUpperCase,

        /// <summary>
        /// Title case
        /// </summary>
        TitleCase
    }

    /// <summary>
    /// What type of string comparison are we doing?
    /// </summary>
    public enum StringCompare
    {
        /// <summary>
        /// Is this a credit card number?
        /// </summary>
        CreditCard,

        /// <summary>
        /// Is this an anagram?
        /// </summary>
        Anagram,

        /// <summary>
        /// Is this Unicode
        /// </summary>
        Unicode
    }

    /// <summary>
    /// Predefined filters
    /// </summary>
    [Flags]
    public enum StringFilter
    {
        /// <summary>
        /// Alpha characters
        /// </summary>
        Alpha = 1,

        /// <summary>
        /// Numeric characters
        /// </summary>
        Numeric = 2,

        /// <summary>
        /// Numbers with period, basically allows for decimal point
        /// </summary>
        FloatNumeric = 4,

        /// <summary>
        /// Multiple spaces
        /// </summary>
        ExtraSpaces = 8
    }

    /// <summary>
    /// String and StringBuilder extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StringExtensions
    {
        /// <summary>
        /// Converts String to SecureString
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>String as SecureString</returns>
        public static SecureString ToSecureString(this string str)
        {
            SecureString secureStr = new();
            if (str.Length > 0)
                foreach (var c in str.ToCharArray()) secureStr.AppendChar(c);
            return secureStr;
        }

        /// <summary>
        /// Does an AppendFormat and then an AppendLine on the StringBuilder
        /// </summary>
        /// <param name="Builder">Builder object</param>
        /// <param name="Format">Format string</param>
        /// <param name="Objects">Objects to format</param>
        /// <returns>The StringBuilder passed in</returns>
        public static StringBuilder AppendLineFormat([NotNull] this StringBuilder Builder, [NotNull] string Format, [NotNull] params object[] Objects)
        {
            if (string.IsNullOrEmpty(Format)) throw new ArgumentNullException(nameof(Format));
            if (Builder == null) throw new ArgumentNullException(nameof(Builder));
            if (Objects == null) throw new ArgumentNullException(nameof(Objects));
            return Builder.AppendFormat(CultureInfo.InvariantCulture, Format, Objects).AppendLine();
        }

        /// <summary>
        /// Centers the input string (if it's longer than the length) and pads it using the padding string
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Length"></param>
        /// <param name="Padding"></param>
        /// <returns>The centered string</returns>
        public static string Center(this string Input, int Length, string Padding = " ")
        {
            if (string.IsNullOrEmpty(Input))
                Input = "";
            string Output = "";
            for (int x = 0; x < (Length - Input.Length) / 2; ++x)
            {
                Output += Padding[x % Padding.Length];
            }
            Output += Input;
            for (int x = 0; x < (Length - Input.Length) / 2; ++x)
            {
                Output += Padding[x % Padding.Length];
            }
            return Output;
        }

        /// <summary>
        /// Converts a string to a string of another encoding
        /// </summary>
        /// <param name="Input">input string</param>
        /// <param name="OriginalEncodingUsing">
        /// The type of encoding the string is currently using (defaults to ASCII)
        /// </param>
        /// <param name="EncodingUsing">
        /// The type of encoding the string is converted into (defaults to UTF8)
        /// </param>
        /// <returns>string of the byte array</returns>
        public static string Encode(this string Input, Encoding OriginalEncodingUsing = null, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            OriginalEncodingUsing = OriginalEncodingUsing.Check(new ASCIIEncoding());
            EncodingUsing = EncodingUsing.Check(new UTF8Encoding());
            return Encoding.Convert(OriginalEncodingUsing, EncodingUsing, Input.ToByteArray(OriginalEncodingUsing))
                           .ToString(EncodingUsing);
        }

        /// <summary>
        /// Converts base 64 string based on the encoding passed in
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>string in the encoding format</returns>
        public static string FromBase64(this string Input, Encoding EncodingUsing)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            var TempArray = Convert.FromBase64String(Input);
            return EncodingUsing.Check(() => new UTF8Encoding()).GetString(TempArray);
        }

        /// <summary>
        /// Converts base 64 string to a byte array
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>A byte array equivalent of the base 64 string</returns>
        public static byte[] FromBase64(this string Input)
        {
#if NET45
            return string.IsNullOrEmpty(Input) ? new byte[0] : Convert.FromBase64String(Input);
#else
            return string.IsNullOrEmpty(Input) ? Array.Empty<byte>() : Convert.FromBase64String(Input);
#endif
        }

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="Value">Value to compare</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string Value, StringCompare ComparisonType)
        {
            if (ComparisonType == StringCompare.CreditCard)
            {
                long CheckSum = 0;
                Value = Value.Replace("-", "").Reverse();
                for (int x = 0; x < Value.Length; ++x)
                {
                    if (!Value[x].Is(CharIs.Digit))
                        return false;
                    int TempValue = (Value[x] - '0') * (x % 2 == 1 ? 2 : 1);
                    while (TempValue > 0)
                    {
                        CheckSum += TempValue % 10;
                        TempValue /= 10;
                    }
                }
                return (CheckSum % 10) == 0;
            }
            if (ComparisonType == StringCompare.Unicode)
            {
                return string.IsNullOrEmpty(Value) || Regex.Replace(Value, @"[^\u0000-\u007F]", "") != Value;
            }
            return Value.Is("", StringCompare.Anagram);
        }

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="Value1">Value 1 to compare</param>
        /// <param name="Value2">Value 2 to compare</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string Value1, string Value2, StringCompare ComparisonType)
        {
            if (ComparisonType != StringCompare.Anagram)
                return Value1.Is(ComparisonType);
            return new string(Value1.OrderBy(x => x).ToArray()) == new string(Value2.OrderBy(x => x).ToArray());
        }

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to keep</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Filter))
                return "";
            var TempRegex = new Regex(Filter);
            var Collection = TempRegex.Matches(Input);
            StringBuilder Builder = new();
            foreach (Match Match in Collection)
                Builder.Append(Match.Value);
            return Builder.ToString();
        }

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string Input, StringFilter Filter)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            var Value = BuildFilter(Filter);
            return Input.Keep(Value);
        }

        /// <summary>
        /// Gets the first x number of characters from the left hand side
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Left(this string Input, int Length)
        {
            if (Length <= 0)
                return "";
            return string.IsNullOrEmpty(Input) ? "" : Input.Substring(0, Input.Length > Length ? Length : Input.Length);
        }

        /// <summary>
        /// Calculates the Levenshtein distance
        /// </summary>
        /// <param name="Value1">Value 1</param>
        /// <param name="Value2">Value 2</param>
        /// <returns>The Levenshtein distance</returns>
        public static int LevenshteinDistance([NotNull] this string Value1, [NotNull] string Value2)
        {
            if (Value1 == null) throw new ArgumentNullException(nameof(Value1));
            if (Value2 == null) throw new ArgumentNullException(nameof(Value2));
            int[,] Matrix = new int[Value1.Length + 1, Value2.Length + 1];
            for (int x = 0; x <= Value1.Length; ++x)
                Matrix[x, 0] = x;
            for (int x = 0; x <= Value2.Length; ++x)
                Matrix[0, x] = x;

            for (int x = 1; x <= Value1.Length; ++x)
            {
                for (int y = 1; y <= Value2.Length; ++y)
                {
                    int Cost = Value1[x - 1] == Value2[y - 1] ? 0 : 1;
                    Matrix[x, y] = new int[] { Matrix[x - 1, y] + 1, Matrix[x, y - 1] + 1, Matrix[x - 1, y - 1] + Cost }.Min();
                    if (x > 1 && y > 1 && Value1[x - 1] == Value2[y - 2] && Value1[x - 2] == Value2[y - 1])
                        Matrix[x, y] = new int[] { Matrix[x, y], Matrix[x - 2, y - 2] + Cost }.Min();
                }
            }

            return Matrix[Value1.Length, Value2.Length];
        }

        /// <summary>
        /// Masks characters to the left ending at a specific character
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="EndPosition">End position (counting from the left)</param>
        /// <param name="Mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskLeft(this string Input, int EndPosition = 4, char Mask = '#')
        {
            string Appending = "";
            for (int x = 0; x < EndPosition; ++x)
                Appending += Mask;
            return Appending + Input.Remove(0, EndPosition);
        }

        /// <summary>
        /// Masks characters to the right starting at a specific character
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="StartPosition">Start position (counting from the left)</param>
        /// <param name="Mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskRight([NotNull] this string Input, int StartPosition = 4, char Mask = '#')
        {
            if (string.IsNullOrEmpty(Input)) throw new ArgumentNullException(nameof(Input));
            if (StartPosition > Input.Length)
                return Input;
            string Appending = "";
            for (int x = 0; x < Input.Length - StartPosition; ++x)
                Appending += Mask;
            return Input.Remove(StartPosition) + Appending;
        }

        /// <summary>
        /// returns the number of times a string occurs within the text
        /// </summary>
        /// <param name="Input">input text</param>
        /// <param name="Match">The string to match (can be regex)</param>
        /// <returns>The number of times the string occurs</returns>
        public static int NumberTimesOccurs(this string Input, string Match)
        {
            return string.IsNullOrEmpty(Input) ? 0 : new Regex(Match).Matches(Input).Count;
        }

        /// <summary>
        /// Pluralizes a word
        /// </summary>
        /// <param name="Word">Word to pluralize</param>
        /// <param name="Culture">
        /// Culture info used to pluralize the word (defaults to current culture)
        /// </param>
        /// <returns>The word pluralized</returns>
        public static string Pluralize(this string Word, CultureInfo Culture = null)
        {
            if (string.IsNullOrEmpty(Word))
                return "";
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Pluralize(Word);
        }

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to remove</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string Remove(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Filter))
                return Input;
            return new Regex(Filter).Replace(Input, "");
        }

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string Remove(this string Input, StringFilter Filter)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            var Value = BuildFilter(Filter);
            return Input.Remove(Value);
        }

        /// <summary>
        /// Replaces everything that is in the filter text with the value specified.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Value">Value to fill in</param>
        /// <param name="Filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>The input text with the various items replaced</returns>
        public static string Replace(this string Input, StringFilter Filter, string Value = "")
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            var FilterValue = BuildFilter(Filter);
            return new Regex(FilterValue).Replace(Input, Value);
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>The reverse of the input string</returns>
        public static string Reverse([NotNull] this string Input)
        {
            if (string.IsNullOrEmpty(Input)) throw new ArgumentNullException(nameof(Input));
            return new string(Input.Reverse<char>().ToArray());
        }

        /// <summary>
        /// Gets the last x number of characters from the right hand side
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Right(this string Input, int Length)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (Length <= 0)
                return "";
            Length = Input.Length > Length ? Length : Input.Length;
            return Input.Substring(Input.Length - Length, Length);
        }

        /// <summary>
        /// Singularizes a word
        /// </summary>
        /// <param name="Word">Word to singularize</param>
        /// <param name="Culture">
        /// Culture info used to singularize the word (defaults to current culture)
        /// </param>
        /// <returns>The word singularized</returns>
        public static string Singularize(this string Word, CultureInfo Culture = null)
        {
            if (string.IsNullOrEmpty(Word))
                return "";
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Singularize(Word);
        }

        /// <summary>
        /// Strips illegal characters for XML content
        /// </summary>
        /// <param name="Content">Content</param>
        /// <returns>The stripped string</returns>
        public static string StripIllegalXML(this string Content)
        {
            if (string.IsNullOrEmpty(Content))
                return "";
            var Builder = new StringBuilder();
            foreach (char Char in Content)
            {
                if (Char == 0x9
                    || Char == 0xA
                    || Char == 0xD
                    || (Char >= 0x20 && Char <= 0xD7FF)
                    || (Char >= 0xE000 && Char <= 0xFFFD))
                    Builder.Append(Char);
            }
            return Builder.ToString().Replace('\u2013', '-').Replace('\u2014', '-')
                .Replace('\u2015', '-').Replace('\u2017', '_').Replace('\u2018', '\'')
                .Replace('\u2019', '\'').Replace('\u201a', ',').Replace('\u201b', '\'')
                .Replace('\u201c', '\"').Replace('\u201d', '\"').Replace('\u201e', '\"')
                .Replace("\u2026", "...").Replace('\u2032', '\'').Replace('\u2033', '\"')
                .Replace("`", "\'")
                .Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
                .Replace("\"", "&quot;").Replace("\'", "&apos;");
        }

        /// <summary>
        /// Strips out any of the characters specified starting on the left side of the input string
        /// (stops when a character not in the list is found)
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripLeft(this string Input, string Characters = " ")
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            if (string.IsNullOrEmpty(Characters))
                return Input;
            return Input.SkipWhile(x => Characters.Contains(x)).ToString(x => x.ToString(), "");
        }

        /// <summary>
        /// Strips out any of the characters specified starting on the right side of the input
        /// string (stops when a character not in the list is found)
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripRight(this string Input, string Characters = " ")
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            if (string.IsNullOrEmpty(Characters))
                return Input;
            int Position = Input.Length - 1;
            for (int x = Input.Length - 1; x >= 0; --x)
            {
                if (!Characters.Contains(Input[x]))
                {
                    Position = x + 1;
                    break;
                }
            }
            return Input.Left(Position);
        }

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="Input">input string</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>the byte array representing the string</returns>
        public static byte[] ToByteArray(this string Input, Encoding EncodingUsing = null)
        {
#if NET45
            return string.IsNullOrEmpty(Input) ? new byte[0] : EncodingUsing.Check(new UTF8Encoding()).GetBytes(Input);
#else
            return string.IsNullOrEmpty(Input) ? Array.Empty<byte>() : EncodingUsing.Check(new UTF8Encoding()).GetBytes(Input);
#endif
        }

        /// <summary>
        /// Converts from the specified encoding to a base 64 string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Options">Base 64 formatting options</param>
        /// <param name="OriginalEncodingUsing">
        /// The type of encoding the string is using (defaults to UTF8)
        /// </param>
        /// <returns>Bas64 string</returns>
        public static string ToString(this string Input, Base64FormattingOptions Options, Encoding OriginalEncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            var TempArray = OriginalEncodingUsing.Check(new UTF8Encoding()).GetBytes(Input);
            return Convert.ToBase64String(TempArray, Options);
        }

        /// <summary>
        /// Formats the string based on the capitalization specified
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Case">Capitalization type to use</param>
        /// <returns>Capitalizes the string based on the case specified</returns>
        public static string ToString(this string Input, StringCase Case)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (Case == StringCase.FirstCharacterUpperCase)
            {
                var InputChars = Input.ToCharArray();
                for (int x = 0; x < InputChars.Length; ++x)
                {
                    if (InputChars[x] != ' ' && InputChars[x] != '\t')
                    {
                        InputChars[x] = char.ToUpper(InputChars[x], CultureInfo.InvariantCulture);
                        break;
                    }
                }
                return new string(InputChars);
            }
            else if (Case == StringCase.SentenceCapitalize)
            {
                string[] Seperator = { ".", "?", "!" };
                var InputStrings = Input.Split(Seperator, StringSplitOptions.None);
                for (int x = 0; x < InputStrings.Length; ++x)
                {
                    if (!string.IsNullOrEmpty(InputStrings[x]))
                    {
                        var TempRegex = new Regex(InputStrings[x]);
                        InputStrings[x] = InputStrings[x].ToString(StringCase.FirstCharacterUpperCase);
                        Input = TempRegex.Replace(Input, InputStrings[x]);
                    }
                }
                return Input;
            }
            else if (Case == StringCase.TitleCase)
            {
                string[] Seperator = { " ", ".", "\t", System.Environment.NewLine, "!", "?" };
                var InputStrings = Input.Split(Seperator, StringSplitOptions.None);
                for (int x = 0; x < InputStrings.Length; ++x)
                {
                    if (!string.IsNullOrEmpty(InputStrings[x])
                        && InputStrings[x].Length > 3)
                    {
                        var TempRegex = new Regex(InputStrings[x].Replace(")", @"\)").Replace("(", @"\(").Replace("*", @"\*"));
                        InputStrings[x] = InputStrings[x].ToString(StringCase.FirstCharacterUpperCase);
                        Input = TempRegex.Replace(Input, InputStrings[x]);
                    }
                }
                return Input;
            }
            return Input;
        }

        /// <summary>
        /// Formats a string based on a format string passed in. The default formatter uses the
        /// following format: # = digits @ = alpha characters \ = escape char
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Format">Format of the output string</param>
        /// <param name="Provider">String formatter provider (defaults to GenericStringFormatter)</param>
        /// <returns>The formatted string</returns>
        public static string ToString(this string Input, string Format, IStringFormatter Provider = null)
        {
            return Provider.Check(new GenericStringFormatter()).Format(Input, Format);
        }

        /// <summary>
        /// Formats a string based on the object's properties
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Object">Object to use to format the string</param>
        /// <param name="StartSeperator">
        /// Seperator character/string to use to describe the start of the property name
        /// </param>
        /// <param name="EndSeperator">
        /// Seperator character/string to use to describe the end of the property name
        /// </param>
        /// <returns>The formatted string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static string ToString(this string Input, object Object, string StartSeperator = "{", string EndSeperator = "}")
        {
            if (Object == null)
                return Input;
            Object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead)
                .ForEach(x =>
                {
                    var Value = x.GetValue(Object, null);
                    Input = Input.Replace(StartSeperator + x.Name + EndSeperator, Value == null ? "" : Value.ToString());
                });
            return Input;
        }

        /// <summary>
        /// Formats a string based on the key/value pairs that are sent in
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Pairs">Key/value pairs. Replaces the key with the corresponding value.</param>
        /// <returns>The string after the changes have been made</returns>
        public static string ToString(this string Input, params KeyValuePair<string, string>[] Pairs)
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            foreach (KeyValuePair<string, string> Pair in Pairs)
            {
                Input = Input.Replace(Pair.Key, Pair.Value);
            }
            return Input;
        }

        /// <summary>
        /// Uses a regex to format the input string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Format">Regex string used to</param>
        /// <param name="OutputFormat">Output format</param>
        /// <param name="Options">Regex options</param>
        /// <returns>The input string formatted by using the regex string</returns>
        public static string ToString([NotNull] this string Input, [NotNull] string Format, [NotNull] string OutputFormat, RegexOptions Options = RegexOptions.None)
        {
            if (string.IsNullOrEmpty(Input)) throw new ArgumentNullException(nameof(Input));
            if (string.IsNullOrEmpty(Format)) throw new ArgumentNullException(nameof(Format));
            if (string.IsNullOrEmpty(OutputFormat)) throw new ArgumentNullException(nameof(OutputFormat));
            return Regex.Replace(Input, Format, OutputFormat, Options);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        private static string BuildFilter(StringFilter Filter)
        {
            string FilterValue = "";
            string Separator = "";
            if (Filter.HasFlag(StringFilter.Alpha))
            {
                FilterValue += Separator + "[a-zA-Z]";
                Separator = "|";
            }
            if (Filter.HasFlag(StringFilter.Numeric))
            {
                FilterValue += Separator + "[0-9]";
                Separator = "|";
            }
            if (Filter.HasFlag(StringFilter.FloatNumeric))
            {
                FilterValue += Separator + @"[0-9\.]";
                Separator = "|";
            }
            if (Filter.HasFlag(StringFilter.ExtraSpaces))
            {
                FilterValue += Separator + @"[ ]{2,}";
                Separator = "|";
            }
            return FilterValue;
        }
    }
}