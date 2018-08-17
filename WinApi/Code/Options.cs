﻿//using Newtonsoft.Json;
//using System;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;
//using Serilog;
//using WinApi.Models;

//namespace WinApi.Models
//{
//    class Options
//    {
//        public OptionsData Data { get; set; }
//        //private const string optionsFile = "options.json";
//        //private const string passwordFile = "bitout.txt";
//        private string passwordHash { get; set; }


//        public Options()
//        {
//            Data = new OptionsData();
//            Data.Succes = false;
//            LoadOption();
//            //  CreatePass("admin");

//            if (Properties.Settings.Default.password == "")
//            {
//                CreatePass("admin");
//            }


//        }

//        /// <summary>
//        /// Metoda na ulozenie Nastaveni do suboru 
//        /// </summary>
//        public void SaveOption()
//        {
//            Log.Information("Ukladanie nastaveni : {0}", Data.ToString());
//            Properties.Settings.Default.ApiLink = this.Data.ApiLink;
//            Properties.Settings.Default.ApiKey = this.Data.Apikey;
//            Properties.Settings.Default.ObjecID = this.Data.ObjecID;
//            Properties.Settings.Default.UserID = this.Data.UserID;
//            Properties.Settings.Default.ProgramPath = this.Data.ProgramPath;
//            Properties.Settings.Default.ProcessName = this.Data.ProcessName;
//            Properties.Settings.Default.PusherKey = this.Data.PusherKey;
//            Properties.Settings.Default.PusherAuthorizer = this.Data.PusherAuthorizer;
//            Properties.Settings.Default.PusherON = this.Data.PusherON;
//            Properties.Settings.Default.Success = this.Data.Succes;
//            Properties.Settings.Default.InProcess = this.Data.InProcess;

//            Properties.Settings.Default.Save();
//            // File.WriteAllText(optionsFile, JsonConvert.SerializeObject(Data));
//        }

//        /// <summary>
//        /// Metoda na nacitanie nastaveni zo suboru do Data
//        /// </summary>

//        public void LoadOption()
//        {

//            try
//            {
//                this.Data.ApiLink = Properties.Settings.Default.ApiLink;
//                this.Data.Apikey = Properties.Settings.Default.ApiKey;
//                this.Data.ObjecID = Properties.Settings.Default.ObjecID;
//                this.Data.UserID = Properties.Settings.Default.UserID;
//                this.Data.ProgramPath = Properties.Settings.Default.ProgramPath;
//                this.Data.ProcessName = Properties.Settings.Default.ProcessName;
//                this.Data.PusherKey = Properties.Settings.Default.PusherKey;
//                this.Data.PusherAuthorizer = Properties.Settings.Default.PusherAuthorizer;
//                this.Data.PusherON = Properties.Settings.Default.PusherON;
//                this.Data.Succes = Properties.Settings.Default.Success;
//                this.Data.InProcess = Properties.Settings.Default.InProcess;
//                this.Data.Succes = true;
//            }
//            catch (Exception)
//            {
//                this.Data.Succes = false;
//            }

//            // this.Data.ModuleID = Properties.Settings.Default.ModuleID;
//            /*
//            if (File.Exists(optionsFile))
//            {

//                this.Data = JsonConvert.DeserializeObject<OptionsData>(File.ReadAllText(optionsFile));
//                this.Data.Succes = true;
//            }

//            */

//        }


//        #region loginToOptions
//        /// <summary>
//        /// Metoda na vygenerovanie hashu pre heslo
//        /// </summary>
//        /// <param name="inputString"></param>
//        /// <returns></returns>
//        public static byte[] GetHash(string inputString)
//        {
//            HashAlgorithm algorithm = SHA256.Create();
//            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inputString"> Heslo </param>
//        /// <returns></returns>

//        public static string GetHashString(string inputString)
//        {
//            StringBuilder sb = new StringBuilder();
//            foreach (byte b in GetHash(inputString))
//                sb.Append(b.ToString("X2")); // format for ToString from byte

//            return sb.ToString();
//        }

//        /// <summary>
//        /// Metoda na vytvorenie suboru option hashom hesla
//        /// </summary>
//        public void CreatePass(string heslo)
//        {
//            //  Properties.Settings.Default.password = GetHashString(heslo);
//            // Properties.Settings.Default.Save();
//            // Create the file.
//            /*
//            using (FileStream fs = File.Create(passwordFile))
//            {
//                Byte[] info = new UTF8Encoding(true).GetBytes(GetHashString(heslo));
//                // Add some information to the file.
//                fs.Write(info, 0, info.Length);
//            }*/

//        }
//        /// <summary>
//        /// Metoda na porovnanie hesiel 
//        /// </summary>
//        /// <param name="enteredPassword"> Heslo ktore sa porovna option ulozenim </param>
//        /// <returns></returns>
//        public bool ComperPassword(string enteredPassword)
//        {

//            string enteredPasswordHash = "";
//            string sourcPassword = Properties.Settings.Default.password;
//            /*using (StreamReader sr = File.OpenText(passwordFile))
//            {
//                sourcPassword = sr.ReadLine();        
//            }
//            */

//            enteredPasswordHash = GetHashString(enteredPassword);
//            return String.Equals(enteredPasswordHash, sourcPassword);
//        }
//    }
//    #endregion
//}
