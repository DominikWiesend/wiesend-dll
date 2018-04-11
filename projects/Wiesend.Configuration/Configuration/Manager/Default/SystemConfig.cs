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

using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using Wiesend.Configuration.Manager.Interfaces;

namespace Wiesend.Configuration.Manager.Default
{
    /// <summary>
    /// Connection string class
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// Actual connection string
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Provider name
        /// </summary>
        public string ProviderName { get; set; }
    }

    /// <summary>
    /// Default config class for web.config and app.config
    /// </summary>
    public class SystemConfig : IConfig
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SystemConfig()
        {
            AppSettings = new Dictionary<string, string>();
            ConnectionStrings = new Dictionary<string, ConnectionString>();
            if (HttpContext.Current == null)
            {
                foreach (ConnectionStringSettings Connection in System.Configuration.ConfigurationManager.ConnectionStrings)
                {
                    ConnectionStrings.Add(Connection.Name, new ConnectionString { Connection = Connection.ConnectionString, ProviderName = Connection.ProviderName });
                }
                foreach (string Key in System.Configuration.ConfigurationManager.AppSettings.Keys)
                {
                    AppSettings.Add(Key, System.Configuration.ConfigurationManager.AppSettings[Key]);
                }
            }
            else
            {
                foreach (ConnectionStringSettings Connection in WebConfigurationManager.ConnectionStrings)
                {
                    ConnectionStrings.Add(Connection.Name, new ConnectionString { Connection = Connection.ConnectionString, ProviderName = Connection.ProviderName });
                }
                foreach (string Key in WebConfigurationManager.AppSettings.Keys)
                {
                    AppSettings.Add(Key, WebConfigurationManager.AppSettings[Key]);
                }
            }
        }

        /// <summary>
        /// Application settings
        /// </summary>
        public IDictionary<string, string> AppSettings { get; private set; }

        /// <summary>
        /// Connection strings
        /// </summary>
        public IDictionary<string, ConnectionString> ConnectionStrings { get; private set; }

        /// <summary>
        /// Name of the Config object
        /// </summary>
        public string Name { get { return "Default"; } }

        /// <summary>
        /// Gets the configuration section based on the name specified
        /// </summary>
        /// <param name="SectionName">Section name</param>
        /// <returns>The configuration section specified</returns>
        public object this[string SectionName]
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return System.Configuration.ConfigurationManager.GetSection(SectionName);
                }
                else
                {
                    return WebConfigurationManager.GetSection(SectionName);
                }
            }
        }

        /// <summary>
        /// Loads the config
        /// </summary>
        public void Load()
        {
        }

        /// <summary>
        /// Saves the config
        /// </summary>
        public void Save()
        {
        }
    }
}