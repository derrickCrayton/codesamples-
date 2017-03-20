using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services.Interfaces
{
    public interface IMarketPlaceService
    {

        List<MarketPlaceDomain> GetByQuoteRequestId(MarketPlaceRequest model);

        List<MarketPlaceDomain> GetBySingleQuoteRequestId(MarketPlaceRequest model);

    }
}