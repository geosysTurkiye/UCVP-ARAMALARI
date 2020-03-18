using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchMetadataOverTheCswService.Models
{
    public class CswRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }

        public string PostContent { get; set; }
    }
}
