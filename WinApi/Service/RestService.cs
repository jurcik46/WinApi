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

        #region Get document to sign
        public ISignatureFileModel GetDocumentToSignature()
        {
            //Logger.Debug(RestServiceEvents.EmployeesLastChange);
            //using (Logger.BeginTimedOperation(RestServiceEvents.EmployeesLastChange))
            //{
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
                    else
                    {
                        return null;
                    }
                }
                // Logger.Warning(RestServiceEvents.LastChangeNull);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // Logger.Error(ex, RestServiceEvents.EmployeesLastChangeError);
                //  ex.ShowError(this, DialogService, SettingsService.ShowSyncErrors);
                _api = null;
                return null;
            }
            //  }

        }
        #endregion

        #region Upload signed document
        public bool UploadSignedDocument(string hash, string pdfFilePath)
        {
            //Logger.Debug(RestServiceEvents.EmployeesLastChange);
            //using (Logger.BeginTimedOperation(RestServiceEvents.EmployeesLastChange))
            //{
            try
            {
                var status = Api.UploadDocument(hash, pdfFilePath);
                if (status != null)
                {
                    if (status.Status == 200)

                        return true;
                    else
                        return false;
                }
                // Logger.Warning(RestServiceEvents.LastChangeNull);
                return false;
            }
            catch (Exception ex)
            {
                // Logger.Error(ex, RestServiceEvents.EmployeesLastChangeError);
                //  ex.ShowError(this, DialogService, SettingsService.ShowSyncErrors);
                _api = null;
                return false;
            }
            //  }
        }
        #endregion



    }
}
