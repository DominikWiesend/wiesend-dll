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

using Wiesend.Random.BaseClasses;
using Wiesend.Random.Interfaces;

namespace Wiesend.Random.NameGenerators
{
    /// <summary>
    /// Company name generator
    /// </summary>
    public class CompanyGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        private readonly string[] CompanyNames = { "Ankh-Sto Associates", "Conglom-O","Cyberdyne Systems Corporation","Globex Corporation","LexCorp",
                                                   "Stark Industries","Sto Plains Holdings","Tri-Optimum Corporation","Umbrella Corporation",
                                                   "Wayne Enterprises","Acme Corp","Weyland-Yutani","ZiffCorp","Grand Trunk Semaphore Company",
                                                   "Monsters, Inc.","SewerCom","Strickland Propane","The Dysk Theatre","The Muppet Theatre",
                                                   "Phillips Broadcasting","Spaceland","Wally World","Ankh Futures","Big Apple Bank",
                                                   "Nakatomi Trading Corporation","Extensive Enterprises","Fronty's Meat Market",
                                                   "PlayTronics","Transworld Consortium","DivaDroid International","Genesis Android Company",
                                                   "Mom's Friendly Robot Company","Tyrell Corporation","Incom Corporation","Kuat Drive Yards",
                                                   "Hudsucker Industries","Videlectrix","Nirvana Corp.","Omni Consumer Products",
                                                   "Spishak","Cogswell Cogs","Duff Brewing Corporation","Paper Street Soap Company",
                                                   "Soylent Corporation","Oscorp Industries","Jupiter Mining Corporation",
                                                   "Le Fin","Moe's","Quark's","Starfishbucks Coffee","S-Mart","Milliways",
                                                   "Sebben & Sebben","Planet Express","Applied Cryogenics","Initech","Rekall, Inc.",
                                                   "Zorg Industries","Blue Sun Corporation","Venture Industries" };

        /// <summary>
        /// Constructor
        /// </summary>
        public CompanyGenerator()
            : base("", "")
        {
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public virtual string Next(System.Random Rand)
        {
            return Rand.Next(CompanyNames);
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public virtual string Next(System.Random Rand, string Min, string Max)
        {
            return Next(Rand);
        }

        /// <summary>
        /// Generates next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return Next(Rand);
        }
    }
}