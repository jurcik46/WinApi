//using system;
//using system.diagnostics;
//using system.threading;
//using system.io;
//using system.runtime.interopservices;
//using winapi.models;
//using system.collections.generic;
//using system.linq;
//using system.text;
//using serilog;
//using system.threading.tasks;
//using system.net;

//namespace winapi
//{
//    class signature
//    {
//        private string signprogrampath { get; set; }
//        private string filepath { get; set; }
//        private string processname { get; set; }



//        public signature(string filename, string filepath, optionsdata options)
//        {
//            signprogrampath = options.programpath;
//            filepath = filepath + filename;
//            processname = options.processname;
//        }


//        #region sign
//        /// <summary>
//        /// metoda na otvorenie suboru v procese a po ulozenej zmene jeho zatvorenie
//        /// </summary>
//        /// <returns></returns>
//        public bool signfile()
//        {

//            bool result = false;

//            var signtimeout = properties.settings.default.singtimeout; ;

//            var startinfo = new processstartinfo(this.signprogrampath, this.filepath);

//            // process.start(startinfo);
//            using (var process = process.start(startinfo))
//            {
//                var fileinfo = new fileinfo(this.filepath);
//                var lastwrite = fileinfo.lastwritetime;

//                var counter = 0;
//                var foundwindow = false;
//                timespan diff;

//                do
//                {
//                    thread.sleep(1000);
//                    fileinfo.refresh();
//                    diff = fileinfo.lastwritetime - lastwrite;

//                    counter++;
//                    if (diff.totalseconds > 1)
//                    {
//                        result = true;
//                        break;
//                    }
//                    var found = findwindowandclose(false);
//                    if (!foundwindow && found)
//                    {
//                        //logger.debug(signserviceevents.signfilewindowfound, "sign application window found for the first time in {iteration}. iteration.", counter);
//                        foundwindow = true;
//                    }
//                    if (counter <= 4 || !foundwindow || (found))
//                    {
//                        continue;
//                    }
//                    //logger.debug(signserviceevents.signfilewindowclosed, "sign application closed before timeout in {iteration}. iteration.", counter);
//                    break;
//                } while (counter < signtimeout);

//                var forceclose = true;

//                if (process != null && !process.hasexited && (forceclose || result))
//                {
//                    //messenger.default.send(new busymessage(this, callerreference, resources.starterclosing));
//                    process.closemainwindow();
//                }
//                if (forceclose)
//                {
//                    //messenger.default.send(new busymessage(this, callerreference, resources.signerclosing));

//                    findwindowandclose(true);
//                }
//                fileinfo.refresh();
//                diff = fileinfo.lastwritetime - lastwrite;
//                log.information("fileinfo {0} timedifference {1} ", fileinfo, diff.totalseconds);
//                //logger.with("fileinfo", fileinfo).with("timedifference", diff.totalseconds).debug(signserviceevents.signfileafterwait);
//                result = diff.totalseconds > 1;
//            }
//            return result;
//        }


//        /// <summary>
//        /// metoda na na najdenie okna a zatvorenie 
//        /// </summary>
//        /// <param name="close"></param>
//        /// <returns></returns>
//        private bool findwindowandclose(bool close)
//        {
//            //filepath = path.getfilename(filepath);
//            var caption = string.format(this.processname, this.filepath);// + " - visual studio code";//string.format(settingsservice.signwindowcaptionformat, filepath);
//            var windowptr = findwindowbycaption(intptr.zero, caption);
//            if (windowptr == intptr.zero)
//            {
//                //  logger.debug(signserviceevents.windownotfound, "window not found: {caption}", caption);

//                return false;
//            }
//            if (!close)
//            {
//                // logger.debug(signserviceevents.windowfound, "window found: {caption}", caption);
//                return true;
//            }
//            //logger.debug(signserviceevents.windowfoundandclosing, "window found and closing: {caption}", caption);
//            sendmessage(windowptr, wm_close, intptr.zero, intptr.zero);
//            return false;
//        }

//        /// <summary>
//        /// find window by caption only. note you must pass intptr.zero as the first parameter.
//        /// </summary>
//        [system.diagnostics.codeanalysis.suppressmessage("microsoft.globalization", "ca2101:specifymarshalingforpinvokestringarguments", messageid = "1")]
//        [system.diagnostics.codeanalysis.suppressmessage("microsoft.design", "ca1060:movepinvokestonativemethodsclass")]
//        [dllimport("user32.dll", entrypoint = "findwindow", setlasterror = true)]
//        static extern intptr findwindowbycaption(intptr zeroonly, string lpwindowname);

//        [system.diagnostics.codeanalysis.suppressmessage("microsoft.design", "ca1060:movepinvokestonativemethodsclass")]
//        [dllimport("user32.dll", charset = charset.auto)]
//        static extern intptr sendmessage(intptr hwnd, uint32 msg, intptr wparam, intptr lparam);

//        const uint32 wm_close = 0x0010;

//        #endregion



//    }
//}
