using System;
using PusherClient;
using System.Text;
using System.Net;
using Serilog;
using System.IO;
using RestSharp;
using WinApi.Code;
using WinApi.Models;
using RestSharp.Deserializers;


namespace WinApi
{
    class PusherConnect
    {
        
        public  Pusher _pusher = null;
        public Channel _channel = null;
        private Options opt;
        private RestClient client = new RestClient();
        private Uri myUri;
        public Boolean Proces { get => Proces; set { Proces = false; } }
        internal FileData Data { get => data; set => data = value; }
        private FileData data ;
        

        #region pusher
        public PusherConnect( bool encryption, string cluester, Options options)
        {
            // add api uri to RestClient

            opt = options;
            myUri = new Uri(opt.Data.ApiLink);
            client.BaseUrl = myUri.OriginalString;

            if (opt.Data.PusherON)
            {
                _pusher = new Pusher(opt.Data.PusherKey, new PusherOptions()
                {
                    Authorizer = new HttpAuthorizer(opt.Data.PusherAuthorizer),
                    Encrypted = encryption,
                    Cluster = cluester
                });
                InitPusher();
            }

        }

 

        private void InitPusher()
        {
            _channel = _pusher.Subscribe("private-bozp-" + opt.Data.ObjecID);
                     _pusher.Connect();
        }   
        /// <summary>
        /// Metoda na poslanie spravy pre pusher 
        /// </summary>
        /// <param name="eventType"> pre aky event </param>
        /// <param name="msgType"> typ spravy</param>
        /// <param name="msg"> samotna sprava</param>
        public void send(string msg)
        {
            
            Log.Information("Odoslanie správy pre pusher: Event = client-event-{0}-{1}, Msg = {2}", opt.Data.UserID, opt.Data.ModuleID, msg);
            _channel.Trigger(String.Format("client-event-{0}-{1}", opt.Data.UserID, opt.Data.ModuleID), new { status = msg });
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
                Log.Warning("GetInfo Request zlyhal pre sietove spojenie {0}", opt.Data.ToString());
                throw new MyException("Zlyhalo pripojenie k internetu");
            }
            data = deserial.Deserialize<FileData>(response);
       
            if (data.Status != "ok") {
              
                Log.Warning("GetInfo Request žiadny súbor sa na podpísanie nenašiel {0} ", opt.Data.ToString());
                Log.Warning("Respon: {0}", data.ToString());
                if(opt.Data.PusherON)
                send("404");
                throw new MyException("Nenašiel sa žiadny súbor na podpisanie");               
            }
            else
            {
                Log.Information("GetInfo Request bol úspešný - Data: {0}", data.ToString());
                opt.Data.InProcess = true;
                byte[] file = Convert.FromBase64String(data.File);
                //string decodedString = Encoding.UTF8.GetString(file);
            
                EventSignature(data.Link, data.Hash, file); 
            
            }
      

        }
        /// <summary>
        /// Metoda na poslanie podpisaneho suboru pre APIcko 
        /// </summary>
        /// <param name="hash">Hash suboru</param>
        /// <param name="paramFileBytes"> Subor v bytoch  </param>
        /// <param name="link"> Odkaz kam sa ma subor ulozit </param>
        public void UploadFile(string hash, byte[] paramFileBytes, string link, string path) {

            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            var request = new RestRequest
            {
                Resource = @"/uploadfile.json?api_key=" + opt.Data.Apikey,
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
            };

            
        //    String file = Convert.ToBase64String(paramFileBytes);
         // file = file.Replace('+', '-');
         // file = file.Replace('/', '_');
         //   Console.WriteLine(file);
          //  request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("user_id", opt.Data.UserID);
            request.AddParameter("object_id", opt.Data.ObjecID);
            request.AddParameter("module_id", opt.Data.ModuleID);
         //  request.AddParameter("file", "asd");
            request.AddParameter("hash", hash);
            request.AddParameter("link", link);

            request.AddFile("filee", path+hash);
            FileData status = new FileData();
            var response = client.Execute(request);
            
            status = deserial.Deserialize<FileData>(response);
            opt.Data.InProcess = false;
         
            if(status.Status != "ok")
            {
                Log.Warning("Súbor sa nepodarilo nahrať REQUEST : {0} , Hash {1} , Link {2}", opt.Data.ToString() , hash , link);
                Log.Warning("Súbor sa nepodarilo nahrať RESPON: {0} ", status.ToString());
                if (opt.Data.PusherON)
                    send("500");
                throw new MyException("Súbor sa nepodarilo nahrať");

            }
            else
            {
                Log.Information("Súbor bol úspešne nahraný  REQUEST : {0} , Hash {1} , Link {2}", opt.Data.ToString(), hash, link);
                Log.Information("Súbor bol úspešne nahraný  RESPON : {0} ", status.ToString());
                if (opt.Data.PusherON)
                    send("200");
                throw new MyException("Súbor bol úspešne nahraný");

            }     
        }
        /// <summary>
        /// Metoda na spustenie eventu podpisovanie 
        /// </summary>
        /// <param name="link"> Odkaz kam sa ma subor ulozit /bozp/2000/1/1 </param>
        /// <param name="hash"> Hash suboru</param>
        /// <param name="file">Obsah suboru </param>
        private void EventSignature(string link, string hash, byte[] file) {

            hash += data.Link.Substring(data.Link.LastIndexOf("."));        
            string directhoryPath = data.Link.Replace('/', '\\');
            directhoryPath = directhoryPath.Substring(1, directhoryPath.LastIndexOf("\\"));
            try
            {
                if (!Directory.Exists(directhoryPath)) { 
                    Directory.CreateDirectory(directhoryPath);
                    Log.Information("Vytváram  zložku : {0}", directhoryPath);
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Nepodarilo sa vytvoriť zložku : {0} : Exception {1}", directhoryPath, ex.Message);
                throw new MyException("Nepodarilo sa vytvoriť zložku pre dokument");
            }
            
            try
            {
                Log.Information("Vytváram prijatý súbor Hash: {0}", hash);
                System.IO.File.WriteAllBytes(directhoryPath+ hash, file);
             }
            catch (Exception ex)
            {
                Log.Warning("Nepodarilo sa vytvoriť dokument Hash: {0} : Exception {1}", hash,ex.Message );
                throw new MyException("Nepodarilo sa uložiť dokument");
             }

            opt.Data.ProcessName = String.Format(opt.Data.ProcessName, hash,directhoryPath);
     


            Signature test = new Signature( hash, directhoryPath, opt.Data);

            if (test.SignFile())
            {

                          
                Stream pdffile = File.OpenRead(directhoryPath+hash);
                Byte[] bytes = File.ReadAllBytes(directhoryPath+hash);
                try
                {
                    UploadFile(hash, bytes, link, directhoryPath);
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

           
            

        }
        #endregion

      









    }
}
