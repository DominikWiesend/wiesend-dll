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
using JetBrains.Annotations;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager.Mapper.Interfaces;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.QueryProvider
{
    /// <summary>
    /// Query provider manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Providers">The providers.</param>
        public Manager([NotNull] IEnumerable<Interfaces.IQueryProvider> Providers)
        {
            if (Providers == null) throw new ArgumentNullException(nameof(Providers));
            this.Providers = Providers.ToDictionary(x => x.ProviderName);
        }

        /// <summary>
        /// Providers
        /// </summary>
        protected IDictionary<string, Interfaces.IQueryProvider> Providers { get; private set; }

        /// <summary>
        /// Creates a batch object
        /// </summary>
        /// <param name="Source">Source to use</param>
        /// <returns>The batch object</returns>
        public IBatch Batch([NotNull] ISourceInfo Source)
        {
            if (Source == null) throw new ArgumentNullException(nameof(Source));
            return Providers.ContainsKey(Source.SourceType) ? Providers[Source.SourceType].Batch(Source) : null;
        }

        /// <summary>
        /// Creates a generator object
        /// </summary>
        /// <typeparam name="T">Class type the generator uses</typeparam>
        /// <param name="Source">Source to use</param>
        /// <param name="Mapping">Mapping info</param>
        /// <param name="Structure">The structure.</param>
        /// <returns>The generator object</returns>
        public IGenerator<T> Generate<T>([NotNull] ISourceInfo Source, IMapping Mapping, Graph<IMapping> Structure)
            where T : class
        {
            if (Source == null) throw new ArgumentNullException(nameof(Source));
            return Providers.ContainsKey(Source.SourceType) ? Providers[Source.SourceType].Generate<T>(Source, Mapping, Structure) : null;
        }

        /// <summary>
        /// Outputs the provider information as a string
        /// </summary>
        /// <returns>The provider information as a string</returns>
        public override string ToString()
        {
            return "Query providers: " + Providers.OrderBy(x => x.Key).ToString(x => x.Key) + "\r\n";
        }
    }
}