using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Model;

namespace WinApi.Models
{
    public class ApiOptionModel : IApiOptionModel
    {
        private string _apiLink;
        private string _apiKey;
        private string _userId;
        private string _objectId;

        public string ApiLink { get => _apiLink; set => _apiLink = value; }
        public string Apikey { get => _apiKey; set => _apiKey = value; }
        public string UserID { get => _userId; set => _userId = value; }
        public string ObjectID { get => _objectId; set => _objectId = value; }
    }
}
