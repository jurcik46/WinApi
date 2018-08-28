using GalaSoft.MvvmLight.Messaging;
using RestSharp;
using Serilog;
using System;
using System.Net;
using WinApi.API.Model;
using WinApi.API.Enums;
using WinApi.Interfaces.Model;
using WinApi.Messages;
using WinApi.Logger;

namespace WinApi.API
{
    class Api
    {
        private Uri _apiLink;
        private string _apiKey;
        private string _userId;
        private string _objectId;

        public ILogger Logger => Log.Logger.ForContext<Api>();

        public Uri ApiLink { get => _apiLink; set => _apiLink = value; }
        public string ObjectID { get => _objectId; set => _objectId = value; }
        public string UserID { get => _userId; set => _userId = value; }
        public string Apikey { get => _apiKey; set => _apiKey = value; }

        public Api(IApiOptionModel apiOption)
        {
            Logger.Debug(ApiEvents.Create, "Creating new instance of API with {ApiLink} and {Apikey}", apiOption.ApiLink, apiOption.Apikey);
            this.ApiLink = new Uri(apiOption.ApiLink, UriKind.Absolute);
            this.Apikey = apiOption.Apikey;
            this.ObjectID = apiOption.ObjectID;
            this.UserID = apiOption.UserID;
        }


        public T Execute<T>(RestRequest request) where T : class, new()
        {
            Logger.Debug(ApiEvents.ExecuteType, "API.Execute<{T}>({@request})", typeof(T).FullName, request);
            var client = new RestClient { BaseUrl = this.ApiLink };
            request.AddParameter("api_key", this.Apikey, ParameterType.QueryString);
            var response = client.Execute<T>(request);
            if (response.ErrorException != null)
            {
                if (response.StatusCode == 0)
                    Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = ViewModels.ViewModelLocator.rm.GetString("connectionTitle"), Msg = ViewModels.ViewModelLocator.rm.GetString("connectionMsg"), IconType = Notifications.Wpf.NotificationType.Error, ExpTime = 10 });

                Logger.With("Request", request)
                    .With("Response", response)
                    .With("Type", typeof(T).FullName)
                    .Error(response.ErrorException, ApiEvents.ExecuteTypeError);
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                Logger.With("Request", request)
                    .With("Response", response)
                    .With("Type", typeof(T).FullName)
                    .Error(ApiEvents.ExecuteTypeError, "Request on {Resource} returned {StatusCode}.\nResponse content: {Content}", request.Resource, response.StatusCode, response.Content);
                return null;

            }
            else
            {
                Logger.With("Request", request)
                    .With("Response", response)
                    .With("Type", typeof(T).FullName)
                    .Debug(ApiEvents.ExecuteTypeSuccess, "Request on {Resource} returned {StatusCode}.\nResponse content: {Content}", request.Resource, response.StatusCode, response.Content);
            }
            return response.Data;
        }


        public SignatureFileModel GetDocument()
        {
            Logger.Debug(ApiEvents.GetDocument, "Object-id: {ObjectID}, User-id: {UserID}", this.ObjectID, this.UserID);

            var request = new RestRequest
            {
                Resource = @"/getinfo.json",
                Method = Method.POST,
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("object_id", this.ObjectID, ParameterType.GetOrPost);
            request.AddParameter("user_id", this.UserID, ParameterType.GetOrPost);

            var result = Execute<SignatureFileModel>(request);
            return result;
        }

        public UploadDocumentModel UploadDocument(string hash, string pdfFilePath, string file)
        {
            Logger.Debug(ApiEvents.UploadDocument, "Object-id: {ObjectID}, User-id: {UserID}, Hash: {hash}, PdfFilePath: {pdfFilePath} ", this.ObjectID, this.UserID, hash, pdfFilePath);


            var request = new RestRequest
            {
                Resource = @"/uploadfile.json",
                Method = Method.POST,
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("object_id", this.ObjectID, ParameterType.GetOrPost);
            request.AddParameter("user_id", this.UserID, ParameterType.GetOrPost);

            request.AddParameter("hash", hash, ParameterType.GetOrPost);
            request.AddParameter("pdf_file_path", "/" + pdfFilePath, ParameterType.GetOrPost);

            request.AddFile("file", file);

            var result = Execute<UploadDocumentModel>(request);
            return result;
        }



    }
}
