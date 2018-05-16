using System;
using PusherClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public PusherConnect(string app_key, string endPoint, bool encryption, string cluester,string channelName)
        {
            //31d14ddddef4c14b6ab5
            _pusher = new Pusher(app_key, new PusherOptions()
            {
                
                Authorizer = new HttpAuthorizer(endPoint),
                Encrypted = encryption,
                Cluster = cluester
            });

            _chatChannel = _pusher.Subscribe("private-" + channelName);


            InitPusher();

        }

        private void InitPusher()
        {
        
            _pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
            _pusher.Error += _pusher_Error;

            // Setup private channel
            //_chatChannel = _pusher.Subscribe("private-channel");
            _chatChannel.Subscribed += _chatChannel_Subscribed;

            // Inline binding!
            _chatChannel.Bind("my-event", (dynamic data) =>
            {
                _type = data.name;
                _msg = data.message;

                Console.WriteLine("[" + _type + "] " + _msg);
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



    }
}
