using System;
using PusherClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Serilog;
using System.IO;
using RestSharp;
using WinApi.Code;
using Newtonsoft.Json;
using WinApi.Models;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp.Deserializers;
using Hardcodet.Wpf.TaskbarNotification;
using System.ComponentModel;

namespace WinApi
{
    class PusherConnect
    {
        
        public  Pusher _pusher = null;
        static Channel _channel = null;
        private Options opt;
        private RestClient client = new RestClient();
        private Uri myUri;
        public Boolean Proces { get => Proces; set { Proces = false; } }
        internal FileData Data { get => data; set => data = value; }
        private FileData data ;

        #region pusher
        public PusherConnect(string app_key, string endPoint, bool encryption, string cluester, Options options )
        {
            // add api uri to RestClient
                    
            opt = options;
            myUri = new Uri(opt.Data.ApiLink);
            client.BaseUrl = myUri.OriginalString;
          /*  _pusher = new Pusher(app_key, new PusherOptions()
            {                
                Authorizer = new HttpAuthorizer(endPoint),
                Encrypted = encryption,
                Cluster = cluester
            });
            _channel = _pusher.Subscribe("private-" + opt.Data.ObjecID);                  
            InitPusher();*/
            
        //    send("event","adsfa", "sdafs");

        }

 

        private void InitPusher()
        {      

            // Inline binding!
            _channel.Bind("event-" + opt.Data.ModuleID, (dynamic data) =>
            {
                // EventSignature((string)data.link,(string)data.hash,(int)data.active);
                if (!opt.Data.InProcess) { 
                GetInfo();
                 }
            });

            _pusher.Connect();
        }   
        /// <summary>
        /// Metoda na poslanie spravy pre pusher 
        /// </summary>
        /// <param name="eventType"> pre aky event </param>
        /// <param name="msgType"> typ spravy</param>
        /// <param name="msg"> samotna sprava</param>
        public void send(string eventType, string msgType, string msg)
        {
          
             _channel.Trigger(eventType, new { message = msg, name = msgType});
        }

        /// <summary>
        /// Metoda na overenie pripojenia 
        /// </summary>
        /// <param name="URL"> URL addresa</param>
        /// <returns></returns>
        public bool CheckConnection(String URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        #endregion

        #region eventFunctions

        /// <summary>
        /// Metoda na zistanie od APIcka ci je dostupny dokument pre podpisanie  
        /// 
        /// </summary>
        public void GetInfo() {                
                    
            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            var request = new RestRequest
            {
                Resource = @"/getinfo.json?api_key=" + opt.Data.Apikey,
                Method = Method.POST,
                RequestFormat = DataFormat.Json,

            };


            request.AddParameter("object_id", opt.Data.UserID);
            request.AddParameter("user_id",opt.Data.ObjecID);
            request.AddParameter("module_id", opt.Data.ModuleID);
                       

            var response = client.Execute(request);

            if(response.StatusCode == 0)                 
                throw new MyException("Zlyhalo pripojenie k internetu");
                
            data = deserial.Deserialize<FileData>(response);
            if(data.Status != "ok") {              
                throw new MyException("Nenašiel sa žiadny subor pre podpisanie");               
            }
            else
            {

                opt.Data.InProcess = true;
                byte[] file = Convert.FromBase64String(data.File);
                string decodedString = Encoding.UTF8.GetString(file);
            //    Console.WriteLine(decodedString);
                EventSignature(data.Link, data.Hash + ".txt", decodedString); /// potreba zmenit .txt na format suboru aky sa bude otvarat 
            
            }
      

        }
        /// <summary>
        /// Metoda na poslanie podpisaneho suboru pre APIcko 
        /// </summary>
        /// <param name="hash">Hash suboru</param>
        /// <param name="paramFileBytes"> Subor v bytoch  </param>
        /// <param name="link"> Odkaz kam sa ma subor ulozit </param>
        public void UploadFile(string hash, byte[] paramFileBytes, string link) {

            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            var request = new RestRequest
            {
                Resource = @"/uploadfile.json?api_key=" + opt.Data.Apikey,
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
            };


            String file = Convert.ToBase64String(paramFileBytes);

            request.AddParameter("user_id", opt.Data.UserID);
            request.AddParameter("object_id", opt.Data.ObjecID);
            request.AddParameter("module_id", opt.Data.ModuleID);
            request.AddParameter("file", file);
            request.AddParameter("hash", hash);
            request.AddParameter("link", link);


            FileData status = new FileData();
            var response = client.Execute(request);
            status = deserial.Deserialize<FileData>(response);
            opt.Data.InProcess = false;
            //Console.WriteLine(status.Status);
            if(status.Status != "ok")
            {

                throw new MyException("Pdf subor sa nepodarilo nahrat");

            }
            else
            {
                throw new MyException("Pdf subor bol uspesne nahrany");

            }     
        }
        /// <summary>
        /// Metoda na spustenie eventu podpisovanie 
        /// </summary>
        /// <param name="link"> Odkaz kam sa ma subor ulozit /bozp/2000/1/1 </param>
        /// <param name="hash"> Hash suboru</param>
        /// <param name="file">Obsah suboru </param>
        private void EventSignature(string link, string hash, string file) {

            string k = data.Link.Replace('/', '\\');
            k = k.Substring(1, k.LastIndexOf("\\"));
            try
            {
                if (!Directory.Exists(k))
                    Directory.CreateDirectory(k);

            }
            catch (Exception ex)
            {
                throw new MyException("Nepodarilo sa vytvorit zlozku pre dokument");
            }
            //  Console.WriteLine(k);
            try
            {

                System.IO.File.WriteAllText(k+ hash, file);
             }
            catch (Exception ex)
            {
                throw new MyException("Nepodarilo sa uložiť dokument");
             }

            opt.Data.ProcessName = String.Format(opt.Data.ProcessName, hash,k);
     


            Signature test = new Signature( hash, k, opt.Data);

            if (test.SignFile())
            {

                          
                Stream pdffile = File.OpenRead(k+hash);
                Byte[] bytes = File.ReadAllBytes(k+hash);
                try
                {
                    UploadFile(hash, bytes, link);
                }
                catch (MyException ex)
                {
                    throw new MyException(ex.Message);
                }
                finally
                {
                    data = null;
                }
                }

           
            //  _channel.Trigger("event-podpis", new { hash = "asdadasd", active = "1" });

        }
        #endregion

      









    }
}
