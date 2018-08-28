using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.API;
using WinApi.Models;
using WinApi.Interfaces.Model;
using WinApi.Interfaces.Service;
using GalaSoft.MvvmLight.Messaging;
using WinApi.Messages;
using Serilog;
using WinApi.Enums;
using WinApi.Logger;

namespace WinApi.Service
{
    public class RestService : IRestService
    {
        private Api _api;


        public RestService(IOptionsService optionsService)
        {
            this.OptionsService = optionsService;
        }

        internal IOptionsService OptionsService;
        internal Api Api => _api ?? (_api = new Api(this.OptionsService.ApiOptions));
        public ILogger Logger => Log.Logger.ForContext<RestService>();


        #region Get document to sign
        public ISignatureFileModel GetDocumentToSignature()
        {
            Logger.Debug(RestServiceEvents.GetDocumentToSignature);
            using (Logger.BeginTimedOperation(RestServiceEvents.GetDocumentToSignature))
            {
                try
                {
                    var document = Api.GetDocument();
                    if (document != null)
                    {
                        if (document.Status == 200)
                        {
                            ISignatureFileModel fileModel = new SignatureFileModel(document);
                            return fileModel;
                        }
                    }
                    Logger.Warning(RestServiceEvents.GetDocumentToSignatureNotFound);
                    return null;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, RestServiceEvents.GetDocumentToSignatureError);
                    _api = null;
                    return null;
                }
            }

        }
        #endregion

        #region Upload signed document
        public bool UploadSignedDocument(string hash, string pdfFilePath, string file)
        {
            Logger.Debug(RestServiceEvents.UploadSignedDocument);
            using (Logger.BeginTimedOperation(RestServiceEvents.UploadSignedDocument))
            {
                try
                {
                    var status = Api.UploadDocument(hash, pdfFilePath, file);
                    if (status != null)
                    {
                        if (status.Status == 200)
                            return true;
                    }
                    Logger.Warning(RestServiceEvents.UploadSignedDocumentFailed);
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, RestServiceEvents.UploadSignedDocumentError);
                    _api = null;
                    return false;
                }
            }
        }
        #endregion



    }
}
