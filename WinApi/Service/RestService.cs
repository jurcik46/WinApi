using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.API;
using WinApi.Models;
using WinApi.Interfaces.Model;
using WinApi.Interfaces.Service;

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
                    ISignatureFileModel fileModel = new SignatureFileModel(document);
                    return fileModel;
                }
                // Logger.Warning(RestServiceEvents.LastChangeNull);
                return null;
            }
            catch (Exception ex)
            {
                // Logger.Error(ex, RestServiceEvents.EmployeesLastChangeError);
                //  ex.ShowError(this, DialogService, SettingsService.ShowSyncErrors);
                _api = null;
                return null;
            }
            //  }

        }



    }
}
