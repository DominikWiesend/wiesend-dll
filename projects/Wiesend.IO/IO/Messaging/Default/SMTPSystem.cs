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
using System.Net.Mail;
using System.Net.Mime;
using Wiesend.DataTypes;
using Wiesend.IO.Messaging.BaseClasses;

namespace Wiesend.IO.Messaging.Default
{
    /// <summary>
    /// SMTP emailer
    /// </summary>
    public class SMTPSystem : MessagingSystemBase
    {
        /// <summary>
        /// Message type accepts
        /// </summary>
        public override Type MessageType { get { return typeof(EmailMessage); } }

        /// <summary>
        /// Name of the system
        /// </summary>
        public override string Name { get { return "SMTP"; } }

        /// <summary>
        /// Internal send message
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void InternalSend(Interfaces.IMessage message)
        {
            if (message is not EmailMessage Message)
                return;
            if (string.IsNullOrEmpty(Message.Body))
                Message.Body = " ";
            using MailMessage TempMailMessage = new();
            char[] Splitter = { ',', ';' };
            var AddressCollection = Message.To.Split(Splitter);
            for (int x = 0; x < AddressCollection.Length; ++x)
                if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                    TempMailMessage.To.Add(AddressCollection[x]);
            if (!string.IsNullOrEmpty(Message.CC))
            {
                AddressCollection = Message.CC.Split(Splitter);
                for (int x = 0; x < AddressCollection.Length; ++x)
                    if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                        TempMailMessage.CC.Add(AddressCollection[x]);
            }
            if (!string.IsNullOrEmpty(Message.Bcc))
            {
                AddressCollection = Message.Bcc.Split(Splitter);
                for (int x = 0; x < AddressCollection.Length; ++x)
                    if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                        TempMailMessage.Bcc.Add(AddressCollection[x]);
            }
            TempMailMessage.Subject = Message.Subject;
            if (!string.IsNullOrEmpty(Message.From))
                TempMailMessage.From = new System.Net.Mail.MailAddress(Message.From);
            using AlternateView BodyView = AlternateView.CreateAlternateViewFromString(Message.Body, null, MediaTypeNames.Text.Html);
            foreach (LinkedResource Resource in Message.EmbeddedResources.Check(new List<LinkedResource>()))
                BodyView.LinkedResources.Add(Resource);
            TempMailMessage.AlternateViews.Add(BodyView);
            TempMailMessage.Priority = Message.Priority;
            TempMailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            TempMailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            TempMailMessage.IsBodyHtml = true;
            foreach (Attachment TempAttachment in Message.Attachments.Check(new List<Attachment>()))
                TempMailMessage.Attachments.Add(TempAttachment);
            if (!string.IsNullOrEmpty(Message.Server))
                SendMessage(new SmtpClient(Message.Server, Message.Port), Message, TempMailMessage);
            else
                SendMessage(new SmtpClient(), Message, TempMailMessage);
        }

        /// <summary>
        /// Sends the message
        /// </summary>
        /// <param name="smtpClient">SMTP client object</param>
        /// <param name="Message">Email message object</param>
        /// <param name="message">Mail message object</param>
        private static void SendMessage([NotNull] SmtpClient smtpClient, [NotNull] EmailMessage Message, [NotNull] MailMessage message)
        {
            if (Message == null) throw new ArgumentNullException(nameof(Message));
            if (smtpClient == null) throw new ArgumentNullException(nameof(smtpClient));
            if (message == null) throw new ArgumentNullException(nameof(message));
            using SmtpClient smtp = smtpClient;
            if (!string.IsNullOrEmpty(Message.UserName) && !string.IsNullOrEmpty(Message.Password))
                smtp.Credentials = new System.Net.NetworkCredential(Message.UserName, Message.Password);
            smtp.EnableSsl = Message.UseSSL;
            smtp.Send(message);
        }
    }
}