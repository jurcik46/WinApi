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
        static Channel _channel = null;
        private Options opt;

        #region pusher
        public PusherConnect(string app_key, string endPoint, bool encryption, string cluester,string channelName, Options options )
        {
            opt = options;      
            _pusher = new Pusher(app_key, new PusherOptions()
            {                
                Authorizer = new HttpAuthorizer(endPoint),
                Encrypted = encryption,
                Cluster = cluester
            });
            _channel = _pusher.Subscribe("private-" + channelName);                  
            InitPusher();

        //    send("event","adsfa", "sdafs");

        }

 

        private void InitPusher()
        {
        
            _pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
            _pusher.Error += _pusher_Error;

            // Setup private channel
            _channel.Subscribed += _chatChannel_Subscribed;

            // Inline binding!
            _channel.Bind("event-podpis", (dynamic data) =>
            {
                EventSignature((string)data.link,(string)data.hash,(int)data.active);
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
             _channel.Trigger(eventType, new { message = msg, name = msgType});
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

        #endregion

        #region eventFunctions
        private System.IO.Stream Upload(string actionUrl, string paramString, Stream paramFileStream, string fileName)
        {
           HttpContent stringContent = new StringContent(paramString);
           HttpContent fileStreamContent = new StreamContent(paramFileStream);
                using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(stringContent, "active");
               formData.Add(fileStreamContent, "file", fileName);
         
                var response = client.PostAsync(actionUrl, formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content.ReadAsStreamAsync().Result;
            }
        }

        private void EventSignature(string link, string hash, int active) {

            string pdfName = "podpis.txt";
            Signature test = new Signature(opt.Data.ProgramPath, link, opt.Data.ProcessName, pdfName);

            if (test.SignFile())
            {

               string pdfNameUpload = link.Substring(link.LastIndexOf("/") + 1);
                Stream pdffile = File.OpenRead(pdfName);

                Upload("http://192.168.33.10/tess.php", "1", pdffile, pdfNameUpload);

            }
            //_channel.Trigger("my-event", new { hash = "asdadasd", active = 1 });

        }
        #endregion









    }
}
