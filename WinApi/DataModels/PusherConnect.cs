using System;
using PusherClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Serilog;
using System.IO;
using WinApi.DataModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace WinApi
{
    class PusherConnect
    {

       // public event PropertyChangedEventHandler PropertyChanged;
        static Pusher _pusher = null;
        static Channel _chatChannel = null;
        static PresenceChannel _presenceChannel = null;
        public string _type { get; private set; }
        public string _msg { get; private set; }
        public string _aaa { get; set; }
        private Options opt;

        public PusherConnect(string app_key, string endPoint, bool encryption, string cluester,string channelName, Options options )
        {
            opt = options;
            //31d14ddddef4c14b6ab5
            _pusher = new Pusher(app_key, new PusherOptions()
            {
                
                Authorizer = new HttpAuthorizer(endPoint),
                Encrypted = encryption,
                Cluster = cluester
            });

            _chatChannel = _pusher.Subscribe("private-" + channelName);
            /*
                        using (WebClient client = new WebClient())
                        {
                            client.UploadFile(@"http://192.168.33.10/test.pdf", "aa.pdf");
                        }
                        */
            /*     try
                 {
                     //create WebClient object
                     WebClient client = new WebClient();
                     string myFile = @"aa.pdf";
                     client.Credentials = CredentialCache.DefaultCredentials;
                     client.UploadFile(@"http://192.168.33.10/aa.pdf", "POST", myFile);
                     client.Dispose();
                 }
                 catch (Exception err)
                 {
                     Log.Error(err.Message, "Something went wrong");
                     //MessageBox.Show(err.Message);
                 }*/


           // Stream fl = File.OpenRead("aa.pdf");

           // Upload("http://192.168.33.10/","asdasda",fl);
         

            InitPusher();

        }

 

        private void InitPusher()
        {
        
            _pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
            _pusher.Error += _pusher_Error;

            // Setup private channel
            _chatChannel.Subscribed += _chatChannel_Subscribed;

            // Inline binding!
            _chatChannel.Bind("event-podpis", (dynamic data) =>
            {
                _type = data.hash;
                _type = data.link;
               
                _msg = data.active;

                Signature test = new Signature(opt.Data.ProgramPath, _type, opt.Data.ProcessName);
                    
                bool t = test.SignFile();
                //  Console.WriteLine("[" + _type + "] " + _msg);
                //  VyvolejZmenu("_type");
                //VyvolejZmenu("_msg");
            });

            _chatChannel.Bind("my-test", (dynamic data) =>
            {
                _type = data.name;
                _msg = data.message;

                Console.WriteLine("" + _type + " " + _msg);
                //  VyvolejZmenu("_type");
                //VyvolejZmenu("_msg");
            });

            _pusher.Connect();
        }



        static void _pusher_Error(object sender, PusherException error)
        {
            //if (error != null)
          
            // throw new Exception ("Pusher Error: " + error.ToString());
            // throw new Exception("Pusher Error: ");


        }

        static void _pusher_ConnectionStateChanged(object sender, ConnectionState state)
        {
            Console.WriteLine("Connection state: " + state.ToString());
           
        }

       

        public void send(string eventType, string msgType, string msg)
        {
          //  _chatChannel.Trigger("client-my-event", new { message = msgType, name = msg });
             _chatChannel.Trigger(eventType, new { message = msg, name = msgType});
        }

        static void _chatChannel_Subscribed(object sender)
        {
            // Console.WriteLine("Hi " + _name + "! Type 'quit' to exit, otherwise type anything to chat!");
        }

     
        protected void VyvolejZmenu(string vlastnost)
        {

            //if (PropertyChanged != null)
              //  PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }

        private System.IO.Stream Upload(string actionUrl, string paramString, Stream paramFileStream)
        {
         //   HttpContent stringContent = new StringContent(paramString);
            HttpContent fileStreamContent = new StreamContent(paramFileStream);
          //  HttpContent bytesContent = new ByteArrayContent(paramFileBytes);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
          //      formData.Add(stringContent, "param1", "param1");
                formData.Add(fileStreamContent, "aa.pdf");
           //     formData.Add(bytesContent, "file2", "file2");
                var response = client.PostAsync(actionUrl, formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content.ReadAsStreamAsync().Result;
            }
        }





    }
}
