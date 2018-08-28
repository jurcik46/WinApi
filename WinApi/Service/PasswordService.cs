using Serilog;
using System;
using System.Security.Cryptography;
using System.Text;
using WinApi.Enums;
using WinApi.Interfaces.Service;
using WinApi.Logger;

namespace WinApi.Service
{
    public class PasswordService : IPasswordService
    {
        private string _password = Properties.Settings.Default.password;
        public string Password { get => _password; set => _password = value; }

        public ILogger Logger => Log.Logger.ForContext<PasswordService>();


        public PasswordService()
        {
            if (this.Password == "")
            {
                Logger.Information(PasswordServiceEvents.CreateDefaultPass);

                CreatePass("admin");
                this.Password = Properties.Settings.Default.password;
            }
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
        }

        public bool ComperPassword(string enteredPassword)
        {
            Logger.Debug(PasswordServiceEvents.ComperPassword);

            string enteredPasswordHash = "";

            enteredPasswordHash = GetHashString(enteredPassword);
            return String.Equals(enteredPasswordHash, this.Password);
        }
    }
}
