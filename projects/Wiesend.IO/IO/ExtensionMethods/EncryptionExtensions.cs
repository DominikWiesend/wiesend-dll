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
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using Wiesend.DataTypes;
using Wiesend.IO.Encryption;

namespace Wiesend.IO
{
    /// <summary>
    /// Extension methods dealing with encryption
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EncryptionExtensions
    {
        /// <summary>
        /// Creates a new set of keys
        /// </summary>
        /// <param name="Random">Random object</param>
        /// <param name="PrivatePublic">True if private key should be included, false otherwise</param>
        /// <returns>XML representation of the key information</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static string CreateKey(this System.Random Random, bool PrivatePublic)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return "";
            return TempManager.CreateKey(PrivatePublic);
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="Data">Text to be decrypted (Base 64 string)</param>
        /// <param name="Key">
        /// Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc.
        /// Really anything that implements DeriveBytes)
        /// </param>
        /// <param name="EncodingUsing">
        /// Encoding that the output string should use (defaults to UTF8)
        /// </param>
        /// <param name="AlgorithmUsing">Algorithm to use for decryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(this string Data,
            DeriveBytes Key,
            Encoding EncodingUsing = null,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return Data.FromBase64()
                        .Decrypt(Key, AlgorithmUsing, InitialVector, KeySize)
                        .ToString(EncodingUsing);
        }

        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">
        /// Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc.
        /// Really anything that implements DeriveBytes)
        /// </param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <returns>An encrypted byte array</returns>
        public static byte[] Decrypt(this byte[] Data,
            DeriveBytes Key,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
            {
#if NET45
                return new byte[0];
#else
                return Array.Empty<byte>();
#endif
            }
            return TempManager.Decrypt(Data, Key, AlgorithmUsing, InitialVector, KeySize);
        }

        /// <summary>
        /// Decrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The decrypted data</returns>
        public static byte[] Decrypt(this byte[] Data, [NotNull] byte[] Key)
        {
            if (Key == null) throw new ArgumentNullException(nameof(Key));
            if (Data == null)
                return null;
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
            {
#if NET45
                return new byte[0];
#else
                return Array.Empty<byte>();
#endif
            }
            return TempManager.Decrypt(Data, Key);
        }

        /// <summary>
        /// Decrypts a string using RSA
        /// </summary>
        /// <param name="Input">
        /// Input string (should be small as anything over 128 bytes can not be decrypted)
        /// </param>
        /// <param name="Key">Key to use for decryption</param>
        /// <param name="EncodingUsing">Encoding that the result should use (defaults to UTF8)</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(this byte[] Input, string Key, Encoding EncodingUsing = null)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return "";
            return TempManager.Decrypt(Input, Key).ToString(EncodingUsing);
        }

        /// <summary>
        /// Encrypts a byte array using RSA
        /// </summary>
        /// <param name="Input">Input (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for encryption</param>
        /// <returns>An encrypted string (64bit string)</returns>
        public static string Encrypt(byte[] Input, string Key)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return "";
            return TempManager.Encrypt(Input, Key).ToString(Base64FormattingOptions.None);
        }

        /// <summary>
        /// Encrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The encrypted data</returns>
        public static byte[] Encrypt(this byte[] Data, byte[] Key)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
            {
#if NET45
                return new byte[0];
#else
                return Array.Empty<byte>();
#endif
            }
            return TempManager.Encrypt(Data, Key);
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">
        /// Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc.
        /// Really anything that implements DeriveBytes)
        /// </param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <param name="EncodingUsing">
        /// Encoding that the original string is using (defaults to UTF8)
        /// </param>
        /// <returns>An encrypted byte array</returns>
        public static string Encrypt(this string Data,
            DeriveBytes Key,
            Encoding EncodingUsing = null,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return Data.ToByteArray(EncodingUsing)
                       .Encrypt(Key, AlgorithmUsing, InitialVector, KeySize)
                       .ToString(Base64FormattingOptions.None);
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">
        /// Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc.
        /// Really anything that implements DeriveBytes)
        /// </param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <returns>An encrypted byte array</returns>
        public static byte[] Encrypt(this byte[] Data,
            DeriveBytes Key,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
            {
#if NET45
                return new byte[0];
#else
                return Array.Empty<byte>();
#endif
            }
            return TempManager.Encrypt(Data, Key, AlgorithmUsing, InitialVector, KeySize);
        }

        /// <summary>
        /// Generates salt
        /// </summary>
        /// <param name="Random">Randomization object</param>
        /// <param name="Size">Size of the salt byte array</param>
        /// <returns>A byte array as salt</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static byte[] GenerateSalt(this System.Random Random, int Size)
        {
            if (!(Size > 0)) throw new ArgumentException("Size must be greater than 0", nameof(Size));
            byte[] Salt = new byte[Size];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetNonZeroBytes(Salt);
            return Salt;
        }

        /// <summary>
        /// Computes the hash of a byte array
        /// </summary>
        /// <param name="Data">Byte array to hash</param>
        /// <param name="Algorithm">Hash algorithm to use (defaults to SHA1)</param>
        /// <returns>The hash of the byte array</returns>
        public static byte[] Hash(this byte[] Data, string Algorithm = "SHA1")
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
            {
#if NET45
                return new byte[0];
#else
                return Array.Empty<byte>();
#endif
            }
            return TempManager.Hash(Data, Algorithm);
        }

        /// <summary>
        /// Computes the hash of a string
        /// </summary>
        /// <param name="Data">string to hash</param>
        /// <param name="Algorithm">Algorithm to use (defaults to SHA1)</param>
        /// <param name="EncodingUsing">Encoding used by the string (defaults to UTF8)</param>
        /// <returns>The hash of the string</returns>
        public static string Hash(this string Data, string Algorithm = "SHA1", Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return BitConverter.ToString(Data.ToByteArray(EncodingUsing).Hash(Algorithm)).Replace("-", "").Encode(null, EncodingUsing);
        }

        /// <summary>
        /// Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt/sign with</param>
        /// <param name="Hash">This will be filled with the unsigned hash</param>
        /// <param name="EncodingUsing">Encoding that the input is using (defaults to UTF8)</param>
        /// <returns>A signed hash of the input (64bit string)</returns>
        public static string SignHash(this string Input, string Key, out string Hash, Encoding EncodingUsing = null)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
            {
                Hash = "";
                return "";
            }
            return TempManager.SignHash(Input, Key, out Hash, EncodingUsing);
        }

        /// <summary>
        /// Verifies a signed hash against the unsigned version
        /// </summary>
        /// <param name="Hash">The unsigned hash (should be 64bit string)</param>
        /// <param name="SignedHash">The signed hash (should be 64bit string)</param>
        /// <param name="Key">The key to use in decryption</param>
        /// <returns>True if it is verified, false otherwise</returns>
        public static bool VerifyHash(this string Hash, string SignedHash, string Key)
        {
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return false;
            return TempManager.VerifyHash(Hash, SignedHash, Key);
        }
    }
}