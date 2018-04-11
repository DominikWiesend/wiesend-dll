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
using System.Linq;
using System.Security.Cryptography;
using Wiesend.Configuration.Manager.Interfaces;
using Wiesend.DataTypes;
using Wiesend.IO;

namespace Wiesend.Configuration.Manager.BaseClasses
{
    /// <summary>
    /// Default config base class
    /// </summary>
    /// <typeparam name="ConfigClassType">Config class type</typeparam>
    [Serializable]
    public abstract class Config<ConfigClassType> : Dynamo<ConfigClassType>, IConfig
        where ConfigClassType : Config<ConfigClassType>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StringToObject">String to object</param>
        /// <param name="ObjectToString">Object to string</param>
        protected Config(Func<string, ConfigClassType> StringToObject = null, Func<ConfigClassType, string> ObjectToString = null)
        {
            this.ObjectToString = ObjectToString.Check(x => x.Serialize<string, ConfigClassType>(SerializationType.XML));
            this.StringToObject = StringToObject.Check(x => x.Deserialize<ConfigClassType, string>(SerializationType.XML));
        }

        /// <summary>
        /// Name of the Config object
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Location to save/load the config file from. If blank, it does not save/load but uses any
        /// defaults specified.
        /// </summary>
        protected virtual string ConfigFileLocation { get { return ""; } }

        /// <summary>
        /// Encryption password for properties/fields. Used only if set.
        /// </summary>
        protected virtual string EncryptionPassword { get { return ""; } }

        /// <summary>
        /// Gets a string representation of the object
        /// </summary>
        private Func<ConfigClassType, string> ObjectToString { get; set; }

        /// <summary>
        /// Gets the object
        /// </summary>
        private Func<string, ConfigClassType> StringToObject { get; set; }

        /// <summary>
        /// Loads the config
        /// </summary>
        public void Load()
        {
            if (string.IsNullOrEmpty(ConfigFileLocation))
                return;
            var FileContent = new FileInfo(ConfigFileLocation).Read();
            if (string.IsNullOrEmpty(FileContent))
            {
                Save();
                return;
            }
            LoadProperties(StringToObject(FileContent));
            Decrypt();
        }

        /// <summary>
        /// Saves the config
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrEmpty(ConfigFileLocation))
                return;
            Encrypt();
            new FileInfo(ConfigFileLocation).Write(ObjectToString((ConfigClassType)this));
            Decrypt();
        }

        private void Decrypt()
        {
            if (string.IsNullOrEmpty(EncryptionPassword))
                return;
            using (PasswordDeriveBytes Temp = new PasswordDeriveBytes(EncryptionPassword, "Kosher".ToByteArray(), "SHA1", 2))
            {
                foreach (KeyValuePair<string, object> Item in this.Where(x => x.Value.GetType() == typeof(string)))
                {
                    SetValue(Item.Key, ((string)Item.Value).Decrypt(Temp));
                }
            }
        }

        private void Encrypt()
        {
            if (string.IsNullOrEmpty(EncryptionPassword))
                return;
            using (PasswordDeriveBytes Temp = new PasswordDeriveBytes(EncryptionPassword, "Kosher".ToByteArray(), "SHA1", 2))
            {
                foreach (KeyValuePair<string, object> Item in this.Where(x => x.Value.GetType() == typeof(string)))
                {
                    SetValue(Item.Key, ((string)Item.Value).Encrypt(Temp));
                }
            }
        }

        private void LoadProperties(ConfigClassType Temp)
        {
            if (Temp == null)
                return;
            foreach (KeyValuePair<string, object> Item in this)
            {
                SetValue(Item.Key, Item.Value);
            }
        }
    }
}