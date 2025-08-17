using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTIL.Errors
{
    public class ErrorItemDetail
    {

        public string issue { get; set; }

        public string description { get; set; }

        public string location { get; set; }

        public string? field { get; set; }

        public string value { get; set; }


        public ErrorItemDetail()
        {
        }

        public ErrorItemDetail(string _issue, string _description, string _location)
        {
            this.issue = _issue;
            this.description = _description;
            this.location = _location;
        }
    }
}
