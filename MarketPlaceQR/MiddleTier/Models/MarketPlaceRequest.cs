using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests
{
    public class MarketPlaceRequest
    {
        public string Name { get; set; }

        public string Estimation { get; set; }
        
        public string Status { get; set; }
        
        public string Address { get; set; }

        public float LatPoint { get; set; }

        public float LngPoint { get; set; }

        public float Radius { get; set; }

        public List<int> CategoryIds { get; set; }

        public DataTable CategoryIdList { get; set; }

    }
}