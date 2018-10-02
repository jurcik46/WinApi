using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.API.Model
{
    public class UploadDocumentModel
    {
        [DeserializeAs(Name = "status")]
        public int Status { get; set; }
    }
}
