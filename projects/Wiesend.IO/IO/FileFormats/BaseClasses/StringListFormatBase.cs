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

using System.Collections.Generic;

namespace Wiesend.IO.FileFormats.BaseClasses
{
    /// <summary>
    /// Format base class for objects that are string based and list of records
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public abstract class StringListFormatBase<FormatType, RecordType> : StringFormatBase<FormatType>, IList<RecordType>
        where FormatType : StringListFormatBase<FormatType, RecordType>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected StringListFormatBase()
        {
            Records = new List<RecordType>();
        }

        /// <summary>
        /// Count of records
        /// </summary>
        public int Count
        {
            get { return Records.Count; }
        }

        /// <summary>
        /// Is read only?
        /// </summary>
        public bool IsReadOnly
        {
            get { return Records.IsReadOnly; }
        }

        /// <summary>
        /// The list of records
        /// </summary>
        protected IList<RecordType> Records { get; private set; }

        /// <summary>
        /// Individual records
        /// </summary>
        /// <param name="Position">The record that you want to get</param>
        /// <returns>The record requested</returns>
        public RecordType this[int Position]
        {
            get { return Records[Position]; }
            set { Records[Position] = value; }
        }

        /// <summary>
        /// Adds a Record to the file
        /// </summary>
        /// <param name="item">Record to add</param>
        public void Add(RecordType item)
        {
            Records.Add(item);
        }

        /// <summary>
        /// Clears the file
        /// </summary>
        public void Clear()
        {
            Records.Clear();
        }

        /// <summary>
        /// Determines if the file contains a Record
        /// </summary>
        /// <param name="item">Record to check for</param>
        /// <returns>True if it does, false otherwise</returns>
        public bool Contains(RecordType item)
        {
            return Records.Contains(item);
        }

        /// <summary>
        /// Copies the delimited file to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public void CopyTo(RecordType[] array, int arrayIndex)
        {
            Records.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator for the delimited file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        public IEnumerator<RecordType> GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        /// <summary>
        /// Index of a specific Record
        /// </summary>
        /// <param name="item">Record to search for</param>
        /// <returns>The index of a specific Record</returns>
        public int IndexOf(RecordType item)
        {
            return Records.IndexOf(item);
        }

        /// <summary>
        /// Inserts a Record at a specific index
        /// </summary>
        /// <param name="index">Index to insert at</param>
        /// <param name="item">Record to insert</param>
        public void Insert(int index, RecordType item)
        {
            Records.Insert(index, item);
        }

        /// <summary>
        /// Removes a Record from the file
        /// </summary>
        /// <param name="item">Record to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(RecordType item)
        {
            return Records.Remove(item);
        }

        /// <summary>
        /// Removes a Record at a specific index
        /// </summary>
        /// <param name="index">Index of the Record to remove</param>
        public void RemoveAt(int index)
        {
            Records.RemoveAt(index);
        }

        /// <summary>
        /// Gets the enumerator for the delimited file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Records.GetEnumerator();
        }
    }
}