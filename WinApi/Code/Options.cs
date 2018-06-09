using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using WinApi.Models;

namespace WinApi.Models
{
    class Options
    {
        public OptionsData Data { get; set; }
        private const string optionsFile = "options.json";
        private const string password = "admin";
        private const string passwordFile = "bitout.txt";
        private string passwordHash { get; set; }


        public Options()
        {
            Data = new OptionsData();
            Data.Succes = false;
            LoadOption();
            CreatePass();
      
        }

        /// <summary>
        /// Metoda na ulozenie Nastaveni do suboru 
        /// </summary>
        public void SaveOption()
        {
            Log.Information("Ukladanie nastaveni : {0}", Data.ToString());
            File.WriteAllText(optionsFile, JsonConvert.SerializeObject(Data));
        }

        /// <summary>
        /// Metoda na nacitanie nastaveni zo suboru do Data
        /// </summary>

        public void LoadOption() {
            if (File.Exists(optionsFile))
            {

                this.Data = JsonConvert.DeserializeObject<OptionsData>(File.ReadAllText(optionsFile));
                this.Data.Succes = true;
            }
            

           // string json = JsonConvert.SerializeObject(movie1,Formatting.Indented);
           // Console.Write(json);
        }


        #region loginToOptions
        /// <summary>
        /// Metoda na vygenerovanie hashu pre heslo
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();  
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"> Heslo </param>
        /// <returns></returns>

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2")); // format for ToString from byte

            return sb.ToString();
        }

        /// <summary>
        /// Metoda na vytvorenie suboru option hashom hesla
        /// </summary>
        public void CreatePass() {

            // Create the file.
            using (FileStream fs = File.Create(passwordFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(GetHashString(password));
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

        }
        /// <summary>
        /// Metoda na porovnanie hesiel 
        /// </summary>
        /// <param name="enteredPassword"> Heslo ktore sa porovna option ulozenim </param>
        /// <returns></returns>
        public bool ComperPassword(string enteredPassword) {

            string enteredPasswordHash = "";
            string sourcPassword = "";
            using (StreamReader sr = File.OpenText(passwordFile))
            {
                sourcPassword = sr.ReadLine();        
            }
            enteredPasswordHash = GetHashString(enteredPassword);
            return String.Equals(enteredPasswordHash, sourcPassword);
           }        
    }
            #endregion
}
