using Sabio.Web.Enums.QuoteWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Domain
{
    public class MarketPlaceDomain
    {
        public int AddressId { get; set; }

        public int CompanyId { get; set; }

        public int QuoteRequestId { get; set; }

        public string Name { get; set; }
        
        public string Estimation { get; set; }

        public DateTime DueDate { get; set; }

        public int CategoryId { get; set; }

        public string Details { get; set; }

        public string Address1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public QRState StatusId { get; set; }

        public decimal LatPoint { get; set; }

        public decimal LngPoint { get; set; }

        public double Distance { get; set; }

        public string Status { get; set; }

        

       


    }
}