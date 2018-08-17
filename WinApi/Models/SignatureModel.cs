using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Models
{
    class SignatureModel
    {
        public string Link { get; set; }
        public string Hash { get; set; }
        public string File { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return base.ToString() + ": Link - " + Link + ", Hash - " + Hash + ", Satus - " + Status;
        }
    }
}
