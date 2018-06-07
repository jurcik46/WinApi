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
       // public ILogger Logger => Log.Logger.ForContext<PusherConnect>();

        #region pusher
        public PusherConnect(string app_key, string endPoint, bool encryption, string cluester, Options options)
        {
            // add api uri to RestClient

            opt = options;
            myUri = new Uri(opt.Data.ApiLink);
            client.BaseUrl = myUri.OriginalString;
           
           // Logger.Warning("asdasd");
         //   Logger.With("FileName", myUri).Error(ex, FileServiceEvents.CleanUpDeleteError);
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
                    Log.Information("Bind na event : event-{0}", opt.Data.ModuleID );
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
            Log.Information("Odoslanie spravu pre pusher: Event = {0}, MsgType = {1}, Msg = {2}", eventType, msgType, msg);
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
                Log.Information("Check Connection na URL : {0}",URL);
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
             //   Log.Warning("Check Connection na URL : {0}", URL);
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


            request.AddParameter("object_id", opt.Data.ObjecID);
            request.AddParameter("user_id", opt.Data.UserID);
            request.AddParameter("module_id", opt.Data.ModuleID);

          
            var response = client.Execute(request);

            if (response.StatusCode == 0)
            {
                Log.Warning("GetInfo Request zlyhal pre sietove spojenie", opt.Data.ModuleID);
                throw new MyException("Zlyhalo pripojenie k internetu");
            }
            data = deserial.Deserialize<FileData>(response);
            Log.Warning("Respon Status : {0}  File : {1}  Hash : {2} Link : {3}", data.Status, data.File, data.Hash, data.Link);
            Log.Warning("GetInfo Request ziadny subor sa na podpisanie nenasiel Send Modul id : {0} UserId:  {1} ObjectID : {2}", opt.Data.ModuleID, opt.Data.UserID, opt.Data.ObjecID);
            
            Log.Warning("Respon {0}  {1} {2} {3}", data.Status, data.File, data.Hash, data.Link);
            if (data.Status != "ok") {
              
                Log.Warning("GetInfo Request ziadny subor sa na podpisanie nenasiel {0}  {1} {2}", opt.Data.ModuleID , opt.Data.UserID , opt.Data.ObjecID);
                throw new MyException("Nenašiel sa žiadny subor pre podpisanie");               
            }
            else
            {
                Log.Information("GetInfo Request bol uspesny s datami: {0}", data.ToString());
                opt.Data.InProcess = true;
                byte[] file = Convert.FromBase64String(data.File);
                string decodedString = Encoding.UTF8.GetString(file);
            //    Console.WriteLine(decodedString);
                EventSignature(data.Link, data.Hash + ".pdf", decodedString); /// potreba zmenit .txt na format suboru aky sa bude otvarat 
            
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
                Log.Warning("Subor sa nepodarilo nahrat File hash : {0}  Link: {1}", hash, link);
                throw new MyException("Súbor sa nepodarilo nahrať");

            }
            else
            {
                Log.Information("Subor bol uspesne nahrany File hash : {0}  Link: {1}", hash, link);
                throw new MyException("Súbor bol úspešne nahraný");

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
                if (!Directory.Exists(k)) { 
                    Directory.CreateDirectory(k);
                    Log.Information("Vytvaram  zlozku : {0}", k);
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Nepodarilo sa vytvorit zlozku : {0} : Exception {1}", k, ex.Message);
                throw new MyException("Nepodarilo sa vytvorit zlozku pre dokument");
            }
            //  Console.WriteLine(k);
            try
            {
                Log.Information("Vytvaram prijaty subor Hash: {0}", hash);
                System.IO.File.WriteAllText(k+ hash, file);
             }
            catch (Exception ex)
            {
                Log.Warning("Nepodarilo sa vytvorit dokument Hash: {0} : Exception {1}", hash,ex.Message );
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
