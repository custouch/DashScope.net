using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashVector.Models.Requests
{
    public class FetchDocRequest
    {
        public List<string> Ids { get; set; }

        public string Partition { get; set; }
    }
}
