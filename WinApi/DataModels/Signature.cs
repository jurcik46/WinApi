using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog;
using System.Threading.Tasks;
using System.Net;

namespace WinApi
{
    class Signature
    {
        private string SignProgramPath { get; set; }
        private string FilePath { get; set; }
        private string ProcessName { get; set; }
        private string pdfName = "podpis.txt";
       
        public Signature(string signProgramPath, string fileLink, string processName)
        {
            SignProgramPath = signProgramPath;
            FilePath = @fileLink;
           
            ProcessName = processName;
        //    pdfName = FilePath.Substring(FilePath.LastIndexOf("/")+1);
           // Console.WriteLine(pdfName);
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(FilePath, pdfName);
            }
           
            FilePath = pdfName;

        }


        #region Sign

        public bool SignFile()
        {
            
            bool result = false;
         
            var SignTimeout = 100;

            var startInfo = new ProcessStartInfo(this.SignProgramPath, this.FilePath);

            Process.Start(startInfo);
            using (var process = Process.Start(startInfo))
            {
                var fileInfo = new FileInfo(this.FilePath);
                var lastWrite = fileInfo.LastWriteTime;

                var counter = 0;
                var foundWindow = false;
                TimeSpan diff;
                do
                {
                    Thread.Sleep(1000);
                    fileInfo.Refresh();
                    diff = fileInfo.LastWriteTime - lastWrite;

                    counter++;
                    if (diff.TotalSeconds > 1)
                    {
                        result = true;
                        break;
                    }
                    var found = FindWindowAndClose(false);
                    if (!foundWindow && found)
                    {
                        //Logger.Debug(SignServiceEvents.SignFileWindowFound, "Sign application window found for the first time in {Iteration}. iteration.", counter);
                        foundWindow = true;
                    }
                    if (counter <= 4 || !foundWindow || (found))
                    {
                        continue;
                    }
                    //Logger.Debug(SignServiceEvents.SignFileWindowClosed, "Sign application closed before timeout in {Iteration}. iteration.", counter);
                    break;
                } while (counter < SignTimeout);

                var ForceClose = true;

                if (process != null && !process.HasExited && (ForceClose || result))
                {
                    //Messenger.Default.Send(new BusyMessage(this, callerReference, Resources.StarterClosing));
                    process.CloseMainWindow();
                }
                if (ForceClose)
                {
                    //Messenger.Default.Send(new BusyMessage(this, callerReference, Resources.SignerClosing));

                    FindWindowAndClose(true);
                }
                fileInfo.Refresh();
                diff = fileInfo.LastWriteTime - lastWrite;
                //Logger.With("FileInfo", fileInfo).With("TimeDifference", diff.TotalSeconds).Debug(SignServiceEvents.SignFileAfterWait);
                result = diff.TotalSeconds > 1;
            }
            return result;
        }



        private bool FindWindowAndClose(bool close)
        {
            //FilePath = Path.GetFileName(FilePath);
            var caption = string.Format(this.ProcessName, this.FilePath);// + " - Visual Studio Code";//string.Format(SettingsService.SignWindowCaptionFormat, FilePath);
            var windowPtr = FindWindowByCaption(IntPtr.Zero, caption);
            if (windowPtr == IntPtr.Zero)
            {
                //  Logger.Debug(SignServiceEvents.WindowNotFound, "Window not found: {Caption}", caption);

                return false;
            }
            if (!close)
            {
                // Logger.Debug(SignServiceEvents.WindowFound, "Window found: {Caption}", caption);
                return true;
            }
            //Logger.Debug(SignServiceEvents.WindowFoundAndClosing, "Window found and closing: {Caption}", caption);
            SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            return false;
        }

        /// <summary>
        /// Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "1")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        const UInt32 WM_CLOSE = 0x0010;

        #endregion



    }
}
