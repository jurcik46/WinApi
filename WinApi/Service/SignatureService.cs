using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Service;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using WinApi.Interfaces.Model;
using GalaSoft.MvvmLight.Messaging;
using WinApi.Messages;
using WinApi.ViewModels;
using System.Resources;
using System.Reflection;

namespace WinApi.Service
{
    public class SignatureService : ISignatureService
    {

        private bool _inProcces = false;
        private IRestService _restService;
        private ISignatureOptionModel _signatureOptionModel;
        private ISignatureFileModel _signatureFileModel;
        public IRestService RestService { get => _restService; set => _restService = value; }
        public ISignatureOptionModel SignatureOptionModel { get => _signatureOptionModel; set => _signatureOptionModel = value; }
        public ISignatureFileModel SignatureFileModel { get => _signatureFileModel; set => _signatureFileModel = value; }
        public bool InProcces { get => _inProcces; set => _inProcces = value; }

        public SignatureService(IRestService restService, IOptionsService optionsService)
        {
            this.RestService = restService;
            Console.WriteLine("star podpis");

            this.SignatureOptionModel = optionsService.SignatureOptions;
            // this.SignatureFileModel = signatureFileModel;
        }

        #region Start signature 
        public void StartSign()
        {
            SignatureFileModel = RestService.GetDocumentToSignature();
            if (SignatureFileModel != null)
            {
                Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Docustaaaart", Msg = ViewModelLocator.rm.GetString("test"), IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 30 });
                Messenger.Default.Send<ChangeIconMessage>(new ChangeIconMessage() { Icon = Enums.TrayIcons.Working });
                InProcces = true;
                string directhoryPath = SignatureFileModel.PdfFilePath.Replace('/', '\\');
                // prida typ suboru na konci hashu
                string fileName = SignatureFileModel.Hash + SignatureFileModel.PdfFilePath.Substring(SignatureFileModel.PdfFilePath.LastIndexOf("."));
                // Vytvori processname z options a vyplni paramatere {0} - filename {1} - directhoryPath
                string processName = string.Format(SignatureOptionModel.ProcessName, fileName, directhoryPath); ;

                /// vytvori nove zlozku 
                if (CreateDirectory(ref directhoryPath))
                {
                    Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "asdasd", Msg = "Apasdasdtu", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 30 });
                    Console.WriteLine(directhoryPath);
                    // ulozi prijaty dokument do vytvorenej zlozky
                    if (SaveFile(directhoryPath, fileName, SignatureFileModel.File))
                    {
                        string filePath = directhoryPath + fileName;
                        // spusti podpisovaci program 
                        if (SignFile(processName, SignatureOptionModel.ProgramPath, filePath))
                        {
                            if (RestService.UploadSignedDocument(SignatureFileModel.Hash, SignatureFileModel.PdfFilePath.Substring(1, SignatureFileModel.PdfFilePath.Length - 1)))
                                Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Document", Msg = "Document bol uspesne podpisany a nahrany ", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 30 });
                            else
                                Console.WriteLine("asdadas");
                        }
                    }
                }
            }
            else
            {
                Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "nenasiel sa nic", Msg = "start", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 30 });
            }

            Messenger.Default.Send<ChangeIconMessage>(new ChangeIconMessage() { Icon = Enums.TrayIcons.Online });
            InProcces = false;
        }
        #endregion


        #region Create Directory
        private bool CreateDirectory(ref string directhoryPath)
        {
            //hash += data.link.substring(data.link.lastindexof("."));
            directhoryPath = directhoryPath.Substring(1, directhoryPath.LastIndexOf("\\"));
            try
            {
                if (!Directory.Exists(directhoryPath))
                {
                    Directory.CreateDirectory(directhoryPath);
                    //  log.information("vytváram  zložku : {0}", directhorypath);
                }
                return true;
            }
            catch (Exception ex)
            {
                //log.warning("nepodarilo sa vytvoriť zložku : {0} : exception {1}", directhorypath, ex.message);
                return false;
                //throw new myexception("nepodarilo sa vytvoriť zložku pre dokument");
            }
        }
        #endregion

        #region Read and Write File
        private bool SaveFile(string directhoryToSave, string fileName, byte[] file)
        {
            try
            {
                // Console.WriteLine(directhoryToSave + fileName);
                //log.information("vytváram prijatý súbor hash: {0}", hash);
                System.IO.File.WriteAllBytes(directhoryToSave + fileName, file);
                return true;
            }
            catch (Exception ex)
            {
                // log.warning("nepodarilo sa vytvoriť dokument hash: {0} : exception {1}", hash, ex.message);
                //throw new myexception("nepodarilo sa uložiť dokument");
                return false;
            }
        }

        private byte[] ReadFile(string directhorypath, string hash)
        {
            try
            {
                Stream pdffile = File.OpenRead(directhorypath + hash);
                byte[] bytes = File.ReadAllBytes(directhorypath + hash);
                //uploadfile(hash, bytes, link, directhorypath);
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
                //throw new myexception(ex.message);
            }
        }
        #endregion


        #region Sign
        /// <summary>
        /// Metoda na otvorenie suboru v procese a po ulozenej zmene jeho zatvorenie
        /// </summary>
        /// <returns></returns>
        public bool SignFile(string processName, string programPath, string pdfFilePath)
        {

            bool result = false;
            //Dlzka trvania podpisu
            var SignTimeout = Properties.Settings.Default.singTimeOut; ;

            var startInfo = new ProcessStartInfo(programPath, pdfFilePath);

            // Process.Start(startInfo);
            using (var process = Process.Start(startInfo))
            {
                var fileInfo = new FileInfo(pdfFilePath);
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
                    var found = FindWindowAndClose(false, processName);
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

                    FindWindowAndClose(true, processName);
                }
                fileInfo.Refresh();
                diff = fileInfo.LastWriteTime - lastWrite;
                //Log.Information("FileInfo {0} TimeDifference {1} ", fileInfo, diff.TotalSeconds);
                //Logger.With("FileInfo", fileInfo).With("TimeDifference", diff.TotalSeconds).Debug(SignServiceEvents.SignFileAfterWait);
                result = diff.TotalSeconds > 1;
            }
            return result;
        }


        /// <summary>
        /// Metoda na na najdenie okna a zatvorenie 
        /// </summary>
        /// <param name="close"></param>
        /// <returns></returns>
        private bool FindWindowAndClose(bool close, string processName)
        {
            //FilePath = Path.GetFileName(FilePath);
            var caption = string.Format(processName, this.SignatureFileModel.PdfFilePath);// + " - Visual Studio Code";//string.Format(SettingsService.SignWindowCaptionFormat, FilePath);
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
