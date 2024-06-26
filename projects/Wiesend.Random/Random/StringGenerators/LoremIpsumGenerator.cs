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
using System.Text;
using Wiesend.DataTypes;
using Wiesend.Random.BaseClasses;
using Wiesend.Random.Interfaces;

namespace Wiesend.Random.StringGenerators
{
    /// <summary>
    /// Lorem Ipsum Generator
    /// </summary>
    public class LoremIpsumGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="NumberOfParagraphs">Number of paragraphs</param>
        /// <param name="MaxSentenceLength">Maximum sentence length</param>
        /// <param name="MinSentenceLength">Minimum sentence length</param>
        /// <param name="NumberOfSentences">Number of sentences per paragraph</param>
        /// <param name="HTMLFormatting">Determines if this should use HTML formatting or not</param>
        public LoremIpsumGenerator(int NumberOfParagraphs = 1, int NumberOfSentences = 1,
            int MinSentenceLength = 4, int MaxSentenceLength = 10, bool HTMLFormatting = false)
            : base("", "")
        {
            this.NumberOfParagraphs = NumberOfParagraphs;
            this.NumberOfSentences = NumberOfSentences;
            this.MinSentenceLength = MinSentenceLength;
            this.MaxSentenceLength = MaxSentenceLength;
            this.HTMLFormatting = HTMLFormatting;
        }

        /// <summary>
        /// Should the sentence use HTML formatting?
        /// </summary>
        public virtual bool HTMLFormatting { get; protected set; }

        /// <summary>
        /// Maximum sentence length
        /// </summary>
        public virtual int MaxSentenceLength { get; protected set; }

        /// <summary>
        /// Minimum sentence length
        /// </summary>
        public virtual int MinSentenceLength { get; protected set; }

        /// <summary>
        /// Number of paragraphs
        /// </summary>
        public virtual int NumberOfParagraphs { get; protected set; }

        /// <summary>
        /// Number of sentences
        /// </summary>
        public virtual int NumberOfSentences { get; protected set; }

        private readonly string[] Words = { "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod",
                                            "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua",
                                            "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita",
                                            "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet",
                                            "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod",
                                            "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua",
                                            "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita",
                                            "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet",
                                            "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod",
                                            "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua",
                                            "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita",
                                            "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "duis",
                                            "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", "velit", "esse", "molestie",
                                            "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", "vero", "eros", "et",
                                            "accumsan", "et", "iusto", "odio", "dignissim", "qui", "blandit", "praesent", "luptatum", "zzril", "delenit",
                                            "augue", "duis", "dolore", "te", "feugait", "nulla", "facilisi", "lorem", "ipsum", "dolor", "sit", "amet",
                                            "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet",
                                            "dolore", "magna", "aliquam", "erat", "volutpat", "ut", "wisi", "enim", "ad", "minim", "veniam", "quis",
                                            "nostrud", "exerci", "tation", "ullamcorper", "suscipit", "lobortis", "nisl", "ut", "aliquip", "ex", "ea",
                                            "commodo", "consequat", "duis", "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate",
                                            "velit", "esse", "molestie", "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at",
                                            "vero", "eros", "et", "accumsan", "et", "iusto", "odio", "dignissim", "qui", "blandit", "praesent", "luptatum",
                                            "zzril", "delenit", "augue", "duis", "dolore", "te", "feugait", "nulla", "facilisi", "nam", "liber", "tempor",
                                            "cum", "soluta", "nobis", "eleifend", "option", "congue", "nihil", "imperdiet", "doming", "id", "quod", "mazim",
                                            "placerat", "facer", "possim", "assum", "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing",
                                            "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam",
                                            "erat", "volutpat", "ut", "wisi", "enim", "ad", "minim", "veniam", "quis", "nostrud", "exerci", "tation",
                                            "ullamcorper", "suscipit", "lobortis", "nisl", "ut", "aliquip", "ex", "ea", "commodo", "consequat", "duis",
                                            "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", "velit", "esse", "molestie",
                                            "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", "vero", "eos", "et", "accusam",
                                            "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea",
                                            "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit",
                                            "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut",
                                            "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua", "at", "vero", "eos", "et",
                                            "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no",
                                            "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit",
                                            "amet", "consetetur", "sadipscing", "elitr", "at", "accusam", "aliquyam", "diam", "diam", "dolore", "dolores",
                                            "duo", "eirmod", "eos", "erat", "et", "nonumy", "sed", "tempor", "et", "et", "invidunt", "justo", "labore",
                                            "stet", "clita", "ea", "et", "gubergren", "kasd", "magna", "no", "rebum", "sanctus", "sea", "sed", "takimata",
                                            "ut", "vero", "voluptua", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit",
                                            "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut",
                                            "labore", "et", "dolore", "magna", "aliquyam", "erat", "consetetur", "sadipscing", "elitr", "sed", "diam",
                                            "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed",
                                            "diam", "voluptua", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea",
                                            "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum" };

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public virtual string Next(System.Random Rand)
        {
            var Builder = new StringBuilder();
            if (HTMLFormatting)
                Builder.Append("<p>");
            Builder.Append("Lorem ipsum dolor sit amet. ");
            for (int y = 1; y < NumberOfSentences; ++y)
                Builder.Append(NextSentence(Rand, Rand.Next(MinSentenceLength, MaxSentenceLength))).Append(' ');
            if (HTMLFormatting)
                Builder.Append("</p>");
            for (int x = 1; x < NumberOfParagraphs; ++x)
            {
                if (HTMLFormatting)
                    Builder.Append("<p>");
                for (int y = 0; y < NumberOfSentences; ++y)
                    Builder.Append(NextSentence(Rand, Rand.Next(MinSentenceLength, MaxSentenceLength))).Append(' ');
                if (HTMLFormatting)
                    Builder.Append("</p>");
                else
                    Builder.AppendLine().AppendLine();
            }
            return Builder.ToString();
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

        /// <summary>
        /// Creates a Lorem Ipsum sentence.
        /// </summary>
        /// <param name="NumberOfWords">Number of words for the sentence</param>
        /// <param name="Rand">Random number generator using</param>
        /// <returns>A string containing Lorem Ipsum text</returns>
        protected virtual string NextSentence([NotNull] System.Random Rand, int NumberOfWords)
        {
            if (Rand == null) throw new ArgumentNullException(nameof(Rand));
            var Builder = new StringBuilder();
            Builder.Append(Words[Rand.Next(Words.Length)].ToString(StringCase.FirstCharacterUpperCase));
            for (int x = 1; x < NumberOfWords; ++x)
                Builder.Append(' ').Append(Words[Rand.Next(Words.Length)]);
            Builder.Append('.');
            return Builder.ToString();
        }
    }
}