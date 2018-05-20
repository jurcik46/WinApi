using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.DataModels.Data;
using Newtonsoft;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace WinApi.DataModels
{
    class Options
    {
        public OptionsData Data { get; set; }
        private const string optionsFile = "options.json";
        private const string passwordFile = "bitout.txt";
        private string passwordHash { get; set; }
        public Options()
        {
            Data = new OptionsData();
            LoadOption();
            createPass();
           
        }

        public void SaveOption(OptionsData data)
        {
            File.WriteAllText(optionsFile, JsonConvert.SerializeObject(data));
        }

        public void LoadOption() {
            if (File.Exists(optionsFile))
            {
                this.Data = JsonConvert.DeserializeObject<OptionsData>(File.ReadAllText(optionsFile));
            }
            else
            {
                throw new Exception("Nastavte nastavenia");
            }

           // string json = JsonConvert.SerializeObject(movie1,Formatting.Indented);
           // Console.Write(json);
        }



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

        public void createPass() {

            // Create the file.
            using (FileStream fs = File.Create(passwordFile))
            {

                Byte[] info = new UTF8Encoding(true).GetBytes(GetHashString("admin"));
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

        } 



    }
}
