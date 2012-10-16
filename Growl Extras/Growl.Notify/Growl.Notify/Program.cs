using Growl.Connector;
using Growl.CoreLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Growl.Notify.Properties;

namespace Growl.Notify
{
    public class Program
    {
        private static bool silent;
        private static int r = -1;
        private static GrowlConnector growl;
        private static EventWaitHandle ewh;

        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = typeof(Program).Namespace + "." + 
                                      new AssemblyName(args.Name).Name + ".dll";
                using (var stream = Assembly.GetExecutingAssembly()
                                            .GetManifestResourceStream(resourceName))
                {
                    var assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }

        public static int Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Invalid arguments. See /? for usage.");
                return -1;
            }
            else if (args[0] == "/?")
            {
                Console.WriteLine();
                Console.WriteLine(Resources.usage);
                return 0;
            }
            else
            {
                Dictionary<string, Program.Parameter> dictionary = new Dictionary<string, Program.Parameter>();
                try
                {
                    foreach (string str in args)
                    {
                        Program.Parameter parameterValue = Program.GetParameterValue(str);
                        dictionary.Add(parameterValue.Argument, parameterValue);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bad arguments : " + ex.Message);
                    return -1;
                }
                string title = "growlnotify";
                string id = "";
                string coalescingID = (string)null;
                bool sticky = false;
                int num = 0;
                string str1 = (string)null;
                string str2 = "growlnotify";
                string uriString = (string)null;
                string[] strArray1 = (string[])null;
                string notificationName = "General Notification";
                string url = (string)null;
                string str3 = "GNTP";
                string hostname = "localhost";
                string password = (string)null;
                Cryptography.SymmetricAlgorithmType symmetricAlgorithmType = Cryptography.SymmetricAlgorithmType.PlainText;
                Cryptography.HashAlgorithmType hashAlgorithmType = Cryptography.HashAlgorithmType.MD5;
                int port = 23053;
                if (!dictionary.ContainsKey("messagetext"))
                {
                    Console.WriteLine("Missing 'messagetext' argument. See /? for usage");
                    return -1;
                }
                else
                {
                    string text = dictionary["messagetext"].Value;
                    if (dictionary.ContainsKey("/t"))
                        title = dictionary["/t"].Value;
                    if (dictionary.ContainsKey("/id"))
                        id = dictionary["/id"].Value;
                    if (dictionary.ContainsKey("/s") && dictionary["/s"].Value.ToLower() == "true")
                        sticky = true;
                    if (dictionary.ContainsKey("/p"))
                        num = Convert.ToInt32(dictionary["/p"].Value);
                    if (dictionary.ContainsKey("/i"))
                    {
                        str1 = dictionary["/i"].Value;
                        if (str1.StartsWith("."))
                            str1 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), str1);
                    }
                    if (dictionary.ContainsKey("/c"))
                        coalescingID = dictionary["/c"].Value;
                    if (dictionary.ContainsKey("/a"))
                        str2 = dictionary["/a"].Value;
                    if (dictionary.ContainsKey("/ai"))
                    {
                        uriString = dictionary["/ai"].Value;
                        if (uriString.StartsWith("."))
                            uriString = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), str1);
                    }
                    if (dictionary.ContainsKey("/r"))
                    {
                        string[] strArray2 = dictionary["/r"].Value.Split(new char[1]
            {
              ','
            });
                        if (strArray2 != null && strArray2.Length > 0)
                        {
                            strArray1 = new string[strArray2.Length];
                            for (int index = 0; index < strArray2.Length; ++index)
                            {
                                string str4 = strArray2[index];
                                if (str4.StartsWith("\""))
                                    str4 = str4.Substring(1, str4.Length - 1);
                                if (str4.EndsWith("\""))
                                    str4 = str4.Substring(0, str4.Length - 1);
                                strArray1[index] = str4;
                            }
                        }
                    }
                    if (dictionary.ContainsKey("/n"))
                        notificationName = dictionary["/n"].Value;
                    if (dictionary.ContainsKey("/cu"))
                        url = dictionary["/cu"].Value;
                    if (dictionary.ContainsKey("/protocol"))
                        str3 = dictionary["/protocol"].Value;
                    if (dictionary.ContainsKey("/host"))
                        hostname = dictionary["/host"].Value;
                    if (dictionary.ContainsKey("/port"))
                        port = Convert.ToInt32(dictionary["/port"].Value);
                    else if (str3 == "UDP")
                        port = 9887;
                    if (dictionary.ContainsKey("/pass"))
                        password = dictionary["/pass"].Value;
                    if (dictionary.ContainsKey("/enc"))
                    {
                        switch (dictionary["/enc"].Value.ToUpper())
                        {
                            case "DES":
                                symmetricAlgorithmType = Cryptography.SymmetricAlgorithmType.DES;
                                break;
                            case "3DES":
                                symmetricAlgorithmType = Cryptography.SymmetricAlgorithmType.TripleDES;
                                break;
                            case "AES":
                                symmetricAlgorithmType = Cryptography.SymmetricAlgorithmType.AES;
                                break;
                            default:
                                symmetricAlgorithmType = Cryptography.SymmetricAlgorithmType.PlainText;
                                break;
                        }
                    }
                    if (dictionary.ContainsKey("/hash"))
                    {
                        switch (dictionary["/hash"].Value.ToUpper())
                        {
                            case "SHA1":
                                hashAlgorithmType = Cryptography.HashAlgorithmType.SHA1;
                                break;
                            case "SHA256":
                                hashAlgorithmType = Cryptography.HashAlgorithmType.SHA256;
                                break;
                            case "SHA512":
                                hashAlgorithmType = Cryptography.HashAlgorithmType.SHA512;
                                break;
                            default:
                                hashAlgorithmType = Cryptography.HashAlgorithmType.MD5;
                                break;
                        }
                    }
                    if (dictionary.ContainsKey("/silent") && dictionary["/silent"].Value.ToLower() == "true")
                        Program.silent = true;
                    Program.ewh = new EventWaitHandle(false, EventResetMode.ManualReset);
                    Program.growl = new GrowlConnector(password, hostname, port);
                    Program.growl.EncryptionAlgorithm = symmetricAlgorithmType;
                    Program.growl.KeyHashAlgorithm = hashAlgorithmType;
                    Program.growl.OKResponse += new GrowlConnector.ResponseEventHandler(Program.growl_Response);
                    Program.growl.ErrorResponse += new GrowlConnector.ResponseEventHandler(Program.growl_Response);
                    if (strArray1 != null || str2 == "growlnotify")
                    {
                        Resource resource = (Resource)null;
                        if (!string.IsNullOrEmpty(uriString))
                        {
                            Uri uri = new Uri(uriString);
                            resource = !uri.IsFile || !File.Exists(uri.LocalPath) ? (Resource)uriString : (Resource) ImageConverter.ImageFromUrl(uri.LocalPath);
                        }
                        if (strArray1 == null)
                            strArray1 = new string[1]
              {
                "General Notification"
              };
                        NotificationType[] notificationTypes = new NotificationType[strArray1.Length];
                        for (int index = 0; index < notificationTypes.Length; ++index)
                        {
                            NotificationType notificationType = new NotificationType(strArray1[index]);
                            notificationTypes[index] = notificationType;
                        }
                        Program.growl.Register(new Application(str2)
                        {
                            Icon = resource
                        }, notificationTypes);
                        Program.ewh.WaitOne();
                    }
                    CallbackContext callbackContext = (CallbackContext)null;
                    if (!string.IsNullOrEmpty(url))
                        callbackContext = new CallbackContext(url);
                    Program.ewh.Reset();
                    Resource icon = (Resource)null;
                    if (!string.IsNullOrEmpty(str1))
                    {
                        Uri uri = new Uri(str1);
                        icon = !uri.IsFile || !File.Exists(uri.LocalPath) ? (Resource)str1 : (Resource)ImageConverter.ImageFromUrl(uri.LocalPath);
                    }
                    Priority priority = Enum.IsDefined(typeof(Priority), (object)num) ? (Priority)num : Priority.Normal;
                    Notification notification = new Notification(str2, notificationName, id, title, text, icon, sticky, priority, coalescingID);
                    Program.growl.Notify(notification, callbackContext);
                    Program.ewh.WaitOne();
                    Console.WriteLine();
                    return Program.r;
                }
            }
        }

        private static void growl_Response(Response response, object state)
        {
            if (!Program.silent)
            {
                if (response.IsOK)
                {
                    Program.r = 0;
                    Console.WriteLine("Notification sent successfully");
                }
                else
                {
                    Program.r = response.ErrorCode;
                    Console.WriteLine(string.Format("Notification failed: {0} - {1}", (object)response.ErrorCode, (object)response.ErrorDescription));
                }
            }
            Program.ewh.Set();
        }

        private static Program.Parameter GetParameterValue(string argument)
        {
            if (!argument.StartsWith("/"))
                return new Program.Parameter("messagetext", argument);
            string[] strArray = argument.Split(new char[1]
      {
        ':'
      }, 2);
            string val = strArray[1];
            if (val.StartsWith("\"") && val.EndsWith("\""))
                val = val.Substring(1, val.Length - 2);
            return new Program.Parameter(strArray[0], val);
        }

        private struct Parameter
        {
            public string Argument;
            public string Value;

            public Parameter(string arg, string val)
            {
                this.Argument = arg;
                if (val == null)
                    val = string.Empty;
                val = val.Trim();
                val = val.Replace("\\n", "\n");
                val = val.Replace("\\\n", "\\n");
                this.Value = val;
            }
        }
    }
}
