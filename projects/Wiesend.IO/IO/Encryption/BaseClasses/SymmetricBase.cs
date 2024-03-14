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
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Wiesend.DataTypes;
using Wiesend.IO.Encryption.Interfaces;

namespace Wiesend.IO.Encryption.BaseClasses
{
    /// <summary>
    /// Symmetric base class
    /// </summary>
    public abstract class SymmetricBase : ISymmetric
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected SymmetricBase()
        {
            ImplementedAlgorithms = new Dictionary<string, Func<SymmetricAlgorithm>>();
        }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Algorithms this implements
        /// </summary>
        protected IDictionary<string, Func<SymmetricAlgorithm>> ImplementedAlgorithms { get; private set; }

        /// <summary>
        /// Can this handle the algorithm specified
        /// </summary>
        /// <param name="Algorithm">The algorithm name</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanHandle(string Algorithm)
        {
            return ImplementedAlgorithms.ContainsKey(Algorithm.ToUpperInvariant());
        }

        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be decrypted</param>
        /// <param name="Key">Password to decrypt with</param>
        /// <param name="Algorithm">Algorithm to use for decryption</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <returns>A decrypted byte array</returns>
        public byte[] Decrypt(byte[] Data, DeriveBytes Key, string Algorithm = "AES", string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (string.IsNullOrEmpty(InitialVector)) throw new ArgumentNullException(nameof(InitialVector));
            if (Data == null)
                return null;
            using SymmetricAlgorithm SymmetricKey = GetProvider(Algorithm);
#if NET45
            byte[] PlainTextBytes = new byte[0];
#else
            byte[] PlainTextBytes = Array.Empty<byte>();
#endif
            if (SymmetricKey != null)
            {
                SymmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(Key.GetBytes(KeySize / 8), InitialVector.ToByteArray()))
                {
                    using MemoryStream MemStream = new(Data);
                    using CryptoStream CryptoStream = new(MemStream, Decryptor, CryptoStreamMode.Read);
                    PlainTextBytes = CryptoStream.ReadAllBinary();
                }
                SymmetricKey.Clear();
            }
            return PlainTextBytes;
        }

        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be decrypted</param>
        /// <param name="Key">Password to decrypt with</param>
        /// <param name="Algorithm">Algorithm to use for decryption</param>
        /// <param name="Salt">Salt to decrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <returns>A decrypted byte array</returns>
        public byte[] Decrypt(byte[] Data, string Key, string Algorithm, string Salt = "Kosher", string HashAlgorithm = "SHA1", int PasswordIterations = 2, string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            using PasswordDeriveBytes TempKey = new(Key, Salt.ToByteArray(), HashAlgorithm, PasswordIterations);
            return Decrypt(Data, TempKey, Algorithm, InitialVector, KeySize);
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be encrypted</param>
        /// <param name="Key">Password to encrypt with</param>
        /// <param name="Salt">Salt to encrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <param name="Algorithm">Algorithm to use</param>
        /// <returns>The encrypted byte array</returns>
        public byte[] Encrypt(byte[] Data, string Key, string Algorithm, string Salt = "Kosher", string HashAlgorithm = "SHA1", int PasswordIterations = 2, string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            using PasswordDeriveBytes TempKey = new(Key, Salt.ToByteArray(), HashAlgorithm, PasswordIterations);
            return Encrypt(Data, TempKey, Algorithm, InitialVector, KeySize);
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be encrypted</param>
        /// <param name="Key">Password to encrypt with</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <param name="Algorithm">Algorithm to use</param>
        /// <returns>The encrypted byte array</returns>
        public byte[] Encrypt(byte[] Data, DeriveBytes Key, string Algorithm = "AES", string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (string.IsNullOrEmpty(InitialVector)) throw new ArgumentNullException(nameof(InitialVector));
            if (Data == null)
                return null;
            using SymmetricAlgorithm SymmetricKey = GetProvider(Algorithm);
#if NET45
            byte[] CipherTextBytes = new byte[0];
#else
            byte[] CipherTextBytes = Array.Empty<byte>();
#endif
            if (SymmetricKey != null)
            {
                SymmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(Key.GetBytes(KeySize / 8), InitialVector.ToByteArray()))
                {
                    using MemoryStream MemStream = new();
                    using CryptoStream CryptoStream = new(MemStream, Encryptor, CryptoStreamMode.Write);
                    CryptoStream.Write(Data, 0, Data.Length);
                    CryptoStream.FlushFinalBlock();
                    CipherTextBytes = MemStream.ToArray();
                }
                SymmetricKey.Clear();
            }
            return CipherTextBytes;
        }

        /// <summary>
        /// Gets the symmetric algorithm
        /// </summary>
        /// <returns>The symmetric algorithm</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        protected SymmetricAlgorithm GetProvider([NotNull] string Algorithm)
        {
            if (string.IsNullOrEmpty(Algorithm)) throw new ArgumentNullException(nameof(Algorithm));
            if (ImplementedAlgorithms == null) throw new NullReferenceException("ImplementedAlgorithms");
            return ImplementedAlgorithms[Algorithm.ToUpperInvariant()]();
        }
    }
}