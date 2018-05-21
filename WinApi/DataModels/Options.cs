using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WinApi.DataModels.Data;

namespace WinApi.DataModels
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
            LoadOption();
            CreatePass();
      
        }

        public void SaveOption()
        {
            File.WriteAllText(optionsFile, JsonConvert.SerializeObject(Data));
        }

        public void LoadOption() {
            if (File.Exists(optionsFile))
            {            
                this.Data = JsonConvert.DeserializeObject<OptionsData>(File.ReadAllText(optionsFile));
               // this.Data.ProcessName = this.Data.ProcessName.Replace(@"\\\\", @"\\");
             //   this.Data.ProgramPath = this.Data.ProgramPath.Replace(@"\\\\", @"\\");
              //  Console.WriteLine(this.Data.ProgramPath);
            //    string test = this.Data.ProgramPath.Replace(@"\", @"\"); 
            }
            else
            {
                throw new Exception("Nastavte nastavenia");
            }

           // string json = JsonConvert.SerializeObject(movie1,Formatting.Indented);
           // Console.Write(json);
        }


        #region loginToOptions
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();  
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2")); // format for ToString from byte

            return sb.ToString();
        }

        public void CreatePass() {

            // Create the file.
            using (FileStream fs = File.Create(passwordFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(GetHashString(password));
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

        }

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
