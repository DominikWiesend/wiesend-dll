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
using JetBrains.Annotations;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Represents a date span
    /// </summary>
    public class DateSpan
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">Start of the date span</param>
        /// <param name="end">End of the date span</param>
        public DateSpan(DateTime start, DateTime end)
        {
            if (!(start <= end)) throw new ArgumentException("Start is after End", nameof(start));
            Start = start;
            End = end;
        }

        /// <summary>
        /// Days between the two dates
        /// </summary>
        public virtual int Days { get { return (End - Start).DaysRemainder(); } }

        /// <summary>
        /// End date
        /// </summary>
        public virtual DateTime End { get; protected set; }

        /// <summary>
        /// Hours between the two dates
        /// </summary>
        public virtual int Hours { get { return (End - Start).Hours; } }

        /// <summary>
        /// Milliseconds between the two dates
        /// </summary>
        public virtual int MilliSeconds { get { return (End - Start).Milliseconds; } }

        /// <summary>
        /// Minutes between the two dates
        /// </summary>
        public virtual int Minutes { get { return (End - Start).Minutes; } }

        /// <summary>
        /// Months between the two dates
        /// </summary>
        public virtual int Months { get { return (End - Start).Months(); } }

        /// <summary>
        /// Seconds between the two dates
        /// </summary>
        public virtual int Seconds { get { return (End - Start).Seconds; } }

        /// <summary>
        /// Start date
        /// </summary>
        public virtual DateTime Start { get; protected set; }

        /// <summary>
        /// Years between the two dates
        /// </summary>
        public virtual int Years { get { return (End - Start).Years(); } }

        /// <summary>
        /// Determines if two DateSpans are not equal
        /// </summary>
        /// <param name="Span1">Span 1</param>
        /// <param name="Span2">Span 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(DateSpan Span1, DateSpan Span2)
        {
            return !(Span1 == Span2);
        }

        /// <summary>
        /// Addition operator
        /// </summary>
        /// <param name="Span1">Span 1</param>
        /// <param name="Span2">Span 2</param>
        /// <returns>The combined date span</returns>
        public static DateSpan operator +(DateSpan Span1, DateSpan Span2)
        {
            if (Span1 == null && Span2 == null)
                return null;
            if (Span1 == null)
                return new DateSpan(Span2.Start, Span2.End);
            if (Span2 == null)
                return new DateSpan(Span1.Start, Span1.End);
            DateTime Start = Span1.Start < Span2.Start ? Span1.Start : Span2.Start;
            DateTime End = Span1.End > Span2.End ? Span1.End : Span2.End;
            return new DateSpan(Start, End);
        }

        /// <summary>
        /// Determines if two DateSpans are equal
        /// </summary>
        /// <param name="Span1">Span 1</param>
        /// <param name="Span2">Span 2</param>
        /// <returns>True if they are, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>")]
        public static bool operator ==(DateSpan Span1, DateSpan Span2)
        {
            if ((object)Span1 == null && (object)Span2 == null)
                return true;
            if ((object)Span1 == null || (object)Span2 == null)
                return false;
            return Span1.Start == Span2.Start && Span1.End == Span2.End;
        }

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The value as a string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static implicit operator string (DateSpan Value)
        {
            if (Value == null) throw new ArgumentNullException(nameof(Value));
            return Value.ToString();
        }

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var Tempobj = obj as DateSpan;
            return Tempobj != null && Tempobj == this;
        }

        /// <summary>
        /// Gets the hash code for the date span
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return End.GetHashCode() & Start.GetHashCode();
        }

        /// <summary>
        /// Returns the intersecting time span between the two values
        /// </summary>
        /// <param name="Span">Span to use</param>
        /// <returns>The intersection of the two time spans</returns>
        public DateSpan Intersection(DateSpan Span)
        {
            if (Span == null)
                return null;
            if (!Overlap(Span))
                return null;
            DateTime Start = Span.Start > this.Start ? Span.Start : this.Start;
            DateTime End = Span.End < this.End ? Span.End : this.End;
            return new DateSpan(Start, End);
        }

        /// <summary>
        /// Determines if two DateSpans overlap
        /// </summary>
        /// <param name="Span">The span to compare to</param>
        /// <returns>True if they overlap, false otherwise</returns>
        public bool Overlap([NotNull] DateSpan Span)
        {
            if (Span == null) throw new ArgumentNullException(nameof(Span));
            return ((Start >= Span.Start && Start < Span.End) || (End <= Span.End && End > Span.Start) || (Start <= Span.Start && End >= Span.End));
        }

        /// <summary>
        /// Converts the DateSpan to a string
        /// </summary>
        /// <returns>The DateSpan as a string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public override string ToString()
        {
            return "Start: " + Start.ToString() + " End: " + End.ToString();
        }
    }
}