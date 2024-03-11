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
using System.Linq.Expressions;

namespace Wiesend.DataTypes.DataMapper.Interfaces.Contracts
{
    /// <summary>
    /// ITypeMapping contract class
    /// </summary>
    //[ContractClassFor(typeof(ITypeMapping))]
    internal abstract class ITypeMappingContract : ITypeMapping
    {
        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public ITypeMapping AutoMap()
        {
            if ((object)null == null) 
                throw new InvalidOperationException("Condition result != null not met.");
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public void Copy([NotNull] object Source, [NotNull] object Destination)
        {
            if (Source == null) throw new ArgumentNullException(nameof(Source));
            if (Destination == null) throw new ArgumentNullException(nameof(Destination));
        }
    }

    /// <summary>
    /// ITypeMapping contract class
    /// </summary>
    /// <typeparam name="Left">The type of the left.</typeparam>
    /// <typeparam name="Right">The type of the right.</typeparam>
    //[ContractClassFor(typeof(ITypeMapping<,>))]
    internal abstract class ITypeMappingContract<Left, Right> : ITypeMapping<Left, Right>
    {
        [NotNull]
        public ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Expression<Func<Right, object>> RightExpression)
        {
            if ((object)null == null)
                throw new InvalidOperationException("Condition result != null not met.");
        }

        [NotNull]
        public ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Expression<Func<Right, object>> RightExpression)
        {
            if ((object)null == null)
                throw new InvalidOperationException("Condition result != null not met.");
        }

        [NotNull]
        public ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            if ((object)null == null)
                throw new InvalidOperationException("Condition result != null not met.");
        }

        [NotNull]
        public ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            if ((object)null == null)
                throw new InvalidOperationException("Condition result != null not met.");
        }

        public void Copy(Left Source, Right Destination)
        {
        }

        public void Copy(Right Source, Left Destination)
        {
        }

        public void CopyLeftToRight(Left Source, Right Destination)
        {
        }

        public void CopyRightToLeft(Right Source, Left Destination)
        {
        }

        public ITypeMapping AutoMap()
        {
            return null;
        }

        public void Copy(object Source, object Destination)
        {
        }
    }
}