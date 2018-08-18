//using system;
//using pusherclient;
//using system.text;
//using system.net;
//using serilog;
//using system.io;
//using restsharp;
//using winapi.code;
//using winapi.models;
//using winapi.viewmodel;
//using restsharp.deserializers;
//using hardcodet.wpf.taskbarnotification;

//namespace winapi
//{
//    class pusherconnect
//    {

//        public pusher _pusher = null;
//        public channel _channel = null;
//        private taskbaricon _trayicon;
//        private options opt;
//        private string appname;
//        private restclient client = new restclient();
//        private uri myuri;
//        public boolean proces { get => proces; set { proces = false; } }
//        internal filedata data { get => data; set => data = value; }
//        public taskbaricon trayicon { get => _trayicon; set => _trayicon = value; }

//        private filedata data;


//        #region pusher
//        public pusherconnect(bool encryption, string cluester, options options, string appname)
//        {
//            // add api uri to restclient
//            opt = options;
//            this.appname = appname;
//            myuri = new uri(opt.data.apilink);
//            client.baseurl = myuri.originalstring;

//            if (opt.data.pusheron)
//            {
//                _pusher = new pusher(opt.data.pusherkey, new pusheroptions()
//                {
//                    authorizer = new httpauthorizer(opt.data.pusherauthorizer),
//                    encrypted = encryption,
//                    cluster = cluester
//                });
//                initpusher();
//            }

//        }



//        private void initpusher()
//        {
//            _channel = _pusher.subscribe("private-bozp-" + opt.data.objecid);
//            _pusher.connect();
//        }
//        /// <summary>
//        /// metoda na poslanie spravy pre pusher 
//        /// </summary>
//        /// <param name="eventtype"> pre aky event </param>
//        /// <param name="msgtype"> typ spravy</param>
//        /// <param name="msg"> samotna sprava</param>
//        public void send(string msg)
//        {

//            log.information("odoslanie správy pre pusher: event = client-event-{0}, msg = {1}", opt.data.userid, msg);
//            _channel.trigger(string.format("client-event-{0}", opt.data.userid), new { status = msg });
//        }

//        /// <summary>
//        /// metoda na overenie pripojenia 
//        /// </summary>
//        /// <param name="url"> url addresa</param>
//        /// <returns></returns>
//        public bool checkconnection(string url)
//        {

//            try
//            {
//                log.information("check connection na url : {0}", url);
//                httpwebrequest request = (httpwebrequest)webrequest.create(url);
//                request.timeout = 5000;
//                request.credentials = credentialcache.defaultnetworkcredentials;
//                httpwebresponse response = (httpwebresponse)request.getresponse();

//                if (response.statuscode == httpstatuscode.ok)
//                    return true;
//                else
//                    return false;
//            }
//            catch
//            {

//                return false;

//            }
//        }


//        #endregion

//        #region eventfunctions

//        /// <summary>
//        /// metoda na zistanie od apicka ci je dostupny dokument pre podpisanie  
//        /// 
//        /// </summary>
//        public void getinfo()
//        {
//            trayicon.showballoontip(appname, "hľadám nový súbory na podpísanie", balloonicon.info);
//            //trayicon.showcustomballoon(ballon, system.windows.controls.primitives.popupanimation.slide, 3000);
//            restsharp.deserializers.jsondeserializer deserial = new jsondeserializer();
//            var request = new restrequest
//            {
//                resource = @"/getinfo.json?api_key=" + opt.data.apikey,
//                method = method.post,
//                requestformat = dataformat.json,

//            };


//            request.addparameter("object_id", opt.data.objecid);
//            request.addparameter("user_id", opt.data.userid);
//            //request.addparameter("module_id", opt.data.moduleid);


//            var response = client.execute(request);

//            if (response.statuscode == 0)
//            {
//                log.warning("getinfo request zlyhal pre sietove spojenie {0}", opt.data.tostring());
//                throw new myexception("zlyhalo pripojenie k internetu");
//            }
//            data = deserial.deserialize<filedata>(response);

//            if (data.status != "ok")
//            {

//                log.warning("getinfo request žiadny súbor sa na podpísanie nenašiel {0} ", opt.data.tostring());
//                log.warning("respon: {0}", data.tostring());
//                if (opt.data.pusheron)
//                    send("404");
//                throw new myexception("nenašiel sa žiadny súbor na podpisanie");
//            }
//            else
//            {
//                log.information("getinfo request bol úspešný - data: {0}", data.tostring());
//                opt.data.inprocess = true;
//                byte[] file = convert.frombase64string(data.file);
//                //string decodedstring = encoding.utf8.getstring(file);
//                trayicon.showballoontip(this.appname, "súbor na podpísanie bol úspešne stiahnutý. spúšťam aplikáciu na podpísanie", balloonicon.info);
//                eventsignature(data.link, data.hash, file);

//            }


//        }
//        /// <summary>
//        /// metoda na poslanie podpisaneho suboru pre apicko 
//        /// </summary>
//        /// <param name="hash">hash suboru</param>
//        /// <param name="paramfilebytes"> subor v bytoch  </param>
//        /// <param name="link"> odkaz kam sa ma subor ulozit </param>
//        public void uploadfile(string hash, byte[] paramfilebytes, string link, string path)
//        {
//            trayicon.showballoontip(this.appname, "prebieha nahrávanie súbor na server", balloonicon.info);
//            restsharp.deserializers.jsondeserializer deserial = new jsondeserializer();
//            var request = new restrequest
//            {
//                resource = @"/uploadfile.json?api_key=" + opt.data.apikey,
//                method = method.post,
//                requestformat = dataformat.json,
//            };


//            //    string file = convert.tobase64string(paramfilebytes);
//            // file = file.replace('+', '-');
//            // file = file.replace('/', '_');
//            //   console.writeline(file);
//            //  request.addheader("content-type", "multipart/form-data");
//            request.addparameter("user_id", opt.data.userid);
//            request.addparameter("object_id", opt.data.objecid);
//            //   request.addparameter("module_id", opt.data.moduleid);
//            //  request.addparameter("file", "asd");
//            request.addparameter("hash", hash);
//            request.addparameter("link", link);

//            request.addfile("filee", path + hash);
//            filedata status = new filedata();
//            var response = client.execute(request);

//            status = deserial.deserialize<filedata>(response);
//            opt.data.inprocess = false;

//            if (status.status != "ok")
//            {
//                log.warning("súbor sa nepodarilo nahrať request : {0} , hash {1} , link {2}", opt.data.tostring(), hash, link);
//                log.warning("súbor sa nepodarilo nahrať respon: {0} ", status.tostring());
//                if (opt.data.pusheron)
//                    send("500");
//                throw new myexception("súbor sa nepodarilo nahrať");

//            }
//            else
//            {
//                log.information("súbor bol úspešne nahraný  request : {0} , hash {1} , link {2}", opt.data.tostring(), hash, link);
//                log.information("súbor bol úspešne nahraný  respon : {0} ", status.tostring());
//                if (opt.data.pusheron)
//                    send("200");
//                throw new myexception("súbor bol úspešne nahraný");

//            }
//        }
//        /// <summary>
//        /// metoda na spustenie eventu podpisovanie 
//        /// </summary>
//        /// <param name="link"> odkaz kam sa ma subor ulozit /bozp/2000/1/1 </param>
//        /// <param name="hash"> hash suboru</param>
//        /// <param name="file">obsah suboru </param>
//        private void eventsignature(string link, string hash, byte[] file)
//        {

//            hash += data.link.substring(data.link.lastindexof("."));
//            string directhorypath = data.link.replace('/', '\\');
//            directhorypath = directhorypath.substring(1, directhorypath.lastindexof("\\"));
//            try
//            {
//                if (!directory.exists(directhorypath))
//                {
//                    directory.createdirectory(directhorypath);
//                    log.information("vytváram  zložku : {0}", directhorypath);
//                }
//            }
//            catch (exception ex)
//            {
//                log.warning("nepodarilo sa vytvoriť zložku : {0} : exception {1}", directhorypath, ex.message);
//                throw new myexception("nepodarilo sa vytvoriť zložku pre dokument");
//            }

//            try
//            {
//                log.information("vytváram prijatý súbor hash: {0}", hash);
//                system.io.file.writeallbytes(directhorypath + hash, file);
//            }
//            catch (exception ex)
//            {
//                log.warning("nepodarilo sa vytvoriť dokument hash: {0} : exception {1}", hash, ex.message);
//                throw new myexception("nepodarilo sa uložiť dokument");
//            }

//            opt.data.processname = string.format(opt.data.processname, hash, directhorypath);



//            signature test = new signature(hash, directhorypath, opt.data);

//            if (test.signfile())
//            {


//                stream pdffile = file.openread(directhorypath + hash);
//                byte[] bytes = file.readallbytes(directhorypath + hash);
//                try
//                {
//                    uploadfile(hash, bytes, link, directhorypath);
//                }
//                catch (myexception ex)
//                {

//                    throw new myexception(ex.message);
//                }
//                finally
//                {
//                    data = null;
//                }
//            }




//        }
//        #endregion











//    }
//}
