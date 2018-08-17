using System;
using System.Security.Cryptography;
using System.Text;
using WinApi.Interfaces.Service;

namespace WinApi.Service
{
    public class PasswordService : IPasswordService
    {
        private string _password;
        private string _enterPassword;
        public string Password { get => _password; set => _password = value; }
        public string EnteredPassword { get => _enterPassword; set => _enterPassword = value; }

        public PasswordService()
        {

        }

        /// <summary>
        /// Metoda na vygenerovanie hashu pre heslo
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"> Heslo </param>
        /// <returns></returns>

        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2")); // format for ToString from byte

            return sb.ToString();
        }

        public void CreatePass(string heslo)
        {
            Properties.Settings.Default.password = GetHashString(heslo);
            Properties.Settings.Default.Save();
            // Create the file.
            /*
            using (FileStream fs = File.Create(passwordFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(GetHashString(heslo));
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }*/

        }

        public bool ComperPassword(string enteredPassword)
        {

            string enteredPasswordHash = "";
            string sourcPassword = Properties.Settings.Default.password;
            /*using (StreamReader sr = File.OpenText(passwordFile))
            {
                sourcPassword = sr.ReadLine();        
            }
            */

            enteredPasswordHash = GetHashString(enteredPassword);
            return String.Equals(enteredPasswordHash, sourcPassword);
        }
    }
}
