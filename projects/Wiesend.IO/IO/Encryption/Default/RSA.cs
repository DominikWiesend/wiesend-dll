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

using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using Wiesend.DataTypes;
using Wiesend.IO.Encryption.BaseClasses;

namespace Wiesend.IO.Encryption.Default
{
    /// <summary>
    /// RSA Encryptor
    /// </summary>
    public class RSA : AsymmetricBase
    {
        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get { return "RSA"; } }

        /// <summary>
        /// Decrypts a byte array using RSA
        /// </summary>
        /// <param name="Input">
        /// Input byte array (should be small as anything over 128 bytes can not be decrypted)
        /// </param>
        /// <param name="Key">Key to use for decryption</param>
        /// <returns>A decrypted byte array</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public override byte[] Decrypt([NotNull] byte[] Input, [NotNull] string Key)
        {
            if (string.IsNullOrEmpty(Key)) throw new ArgumentNullException(nameof(Key));
            if (Input == null) throw new ArgumentNullException(nameof(Input));
            using RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            var EncryptedBytes = RSA.Decrypt(Input, true);
            RSA.Clear();
            return EncryptedBytes;
        }

        /// <summary>
        /// Encrypts a string using RSA
        /// </summary>
        /// <param name="Input">
        /// Input byte array (should be small as anything over 128 bytes can not be decrypted)
        /// </param>
        /// <param name="Key">Key to use for encryption</param>
        /// <returns>An encrypted byte array (64bit string)</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public override byte[] Encrypt([NotNull] byte[] Input, [NotNull] string Key)
        {
            if (string.IsNullOrEmpty(Key)) throw new ArgumentNullException(nameof(Key));
            if (Input == null) throw new ArgumentNullException(nameof(Input));
            using RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            var EncryptedBytes = RSA.Encrypt(Input, true);
            RSA.Clear();
            return EncryptedBytes;
        }

        /// <summary>
        /// Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt/sign with</param>
        /// <param name="Hash">This will be filled with the unsigned hash</param>
        /// <param name="EncodingUsing">Encoding that the input is using (defaults to UTF8)</param>
        /// <returns>A signed hash of the input (64bit string)</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public override string SignHash([NotNull] string Input, [NotNull] string Key, out string Hash, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Key)) throw new ArgumentNullException(nameof(Key));
            if (string.IsNullOrEmpty(Input)) throw new ArgumentNullException(nameof(Input));
            using RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            var HashBytes = Input.ToByteArray(EncodingUsing).Hash();
            var SignedHash = RSA.SignHash(HashBytes, CryptoConfig.MapNameToOID("SHA1"));
            RSA.Clear();
            Hash = HashBytes.ToString(Base64FormattingOptions.None);
            return SignedHash.ToString(Base64FormattingOptions.None);
        }

        /// <summary>
        /// Verifies a signed hash against the unsigned version
        /// </summary>
        /// <param name="Hash">The unsigned hash (should be 64bit string)</param>
        /// <param name="SignedHash">The signed hash (should be 64bit string)</param>
        /// <param name="Key">The key to use in decryption</param>
        /// <returns>True if it is verified, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public override bool VerifyHash([NotNull] string Hash, [NotNull] string SignedHash, [NotNull] string Key)
        {
            if (string.IsNullOrEmpty(Key)) throw new ArgumentNullException(nameof(Key));
            if (string.IsNullOrEmpty(Hash)) throw new ArgumentNullException(nameof(Hash));
            if (string.IsNullOrEmpty(SignedHash)) throw new ArgumentNullException(nameof(SignedHash));
            using RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            var InputArray = SignedHash.FromBase64();
            var HashArray = Hash.FromBase64();
            var Result = RSA.VerifyHash(HashArray, CryptoConfig.MapNameToOID("SHA1"), InputArray);
            RSA.Clear();
            return Result;
        }

        /// <summary>
        /// Gets the provider used
        /// </summary>
        /// <returns>Asymmetric algorithm</returns>
        protected override System.Security.Cryptography.AsymmetricAlgorithm GetProvider()
        {
            return new RSACryptoServiceProvider();
        }
    }
}