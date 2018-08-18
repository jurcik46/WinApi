using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinApi.API.Model;
using WinApi.Interfaces.Model;

namespace WinApi.API
{
    class Api
    {
        private Uri _apiLink;
        private string _apiKey;
        private string _userId;
        private string _objectId;

        public Uri ApiLink { get => _apiLink; set => _apiLink = value; }
        public string ObjectID { get => _objectId; set => _objectId = value; }
        public string UserID { get => _userId; set => _userId = value; }
        public string Apikey { get => _apiKey; set => _apiKey = value; }

        public Api(IApiOptionModel apiOption)
        {
            this.ApiLink = new Uri(apiOption.ApiLink, UriKind.Absolute);
            this.Apikey = apiOption.Apikey;
            this.ObjectID = apiOption.ObjectID;
            this.UserID = apiOption.UserID;
        }


        public T Execute<T>(RestRequest request) where T : class, new()
        {
            // Logger.Debug(EntranceAPIEvents.ExecuteType, "EntranceAPI.Execute<{T}>({@request})", typeof(T).FullName, request);
            var client = new RestClient { BaseUrl = this.ApiLink };
            request.AddParameter("api_key", this.Apikey, ParameterType.QueryString);
            var response = client.Execute<T>(request);
            if (response.ErrorException != null)
            {
                //Logger.With("Request", request).With("Response", response).With("Type", typeof(T).FullName).Error(response.ErrorException, EntranceAPIEvents.ExecuteTypeError);
                return null;
            }
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                //    Logger.With("Request", request)
                //        .With("Response", response)
                //        .With("Type", typeof(T).FullName)
                //        .Error(EntranceAPIEvents.ExecuteTypeError, "Request on {Resource} returned {StatusCode}.\nResponse content: {Content}", request.Resource, response.StatusCode, response.Content);
            }
            else
            {
                //Logger.With("Request", request)
                //    .With("Response", response)
                //    .With("Type", typeof(T).FullName)
                //    .Debug(EntranceAPIEvents.ExecuteTypeSuccess, "Request on {Resource} returned {StatusCode}.\nResponse content: {Content}", request.Resource, response.StatusCode, response.Content);
            }
            return response.Data;
        }


        public SignatureFileModel GetDocument()
        {
            //Logger.Debug(EntranceAPIEvents.GetLastChange, "{resource}, {parameter}, {value}", resource, parameter, value);

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



    }
}
