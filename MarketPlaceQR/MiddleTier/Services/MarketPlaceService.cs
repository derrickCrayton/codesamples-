using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Enums.QuoteWorkflow;
using Sabio.Web.Models.Requests;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class MarketPlaceService : BaseService, IMarketPlaceService
    {
        public List<MarketPlaceDomain> GetByQuoteRequestId(MarketPlaceRequest model)
        {
            List<MarketPlaceDomain> marketplaceQR = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.QuoteRequests_GetBySearchRadius"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@categoryidList", model.CategoryIdList);
                  paramCollection.AddWithValue("@latpoint", model.LatPoint);
                  paramCollection.AddWithValue("@lngpoint", model.LngPoint);
                  paramCollection.AddWithValue("@radius", model.Radius);
                  
              }, map: delegate (IDataReader reader, short set)
              {
                  MarketPlaceDomain items = new MarketPlaceDomain();
                  int startingIndex = 0; //startingOrdinal

                  items.QuoteRequestId = reader.GetSafeInt32(startingIndex++);
                  items.AddressId = reader.GetSafeInt32(startingIndex++);
                  items.CompanyId = reader.GetSafeInt32(startingIndex++);
                  items.Name = reader.GetSafeString(startingIndex++);
                  items.Estimation = reader.GetSafeString(startingIndex++);
                  items.DueDate = reader.GetSafeDateTime(startingIndex++);
                  items.CategoryId = reader.GetSafeInt32(startingIndex++);
                  items.Details = reader.GetSafeString(startingIndex++);
                  items.Address1 = reader.GetSafeString(startingIndex++);
                  items.City = reader.GetSafeString(startingIndex++);
                  items.State = reader.GetSafeString(startingIndex++);
                  items.ZipCode = reader.GetSafeString(startingIndex++);
                  items.StatusId = (QRState) reader.GetSafeInt32(startingIndex++);
                  items.LatPoint = reader.GetSafeDecimal(startingIndex++);
                  items.LngPoint = reader.GetSafeDecimal(startingIndex++);
                  items.Distance = reader.GetSafeDouble(startingIndex++);
                  items.Status = items.StatusId.ToString();
                  
                  if (marketplaceQR == null)
                  {
                      marketplaceQR = new List<MarketPlaceDomain>();
                  }

                  marketplaceQR.Add(items);

              });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return marketplaceQR;
        }




        public List<MarketPlaceDomain> GetBySingleQuoteRequestId(MarketPlaceRequest model)
        {
            List<MarketPlaceDomain> marketplaceQR = null;
            int category = model.CategoryIds[0]/100;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.QuoteRequests_GetBySingleCategorySearchRadius"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@categoryid", category);
                  paramCollection.AddWithValue("@latpoint", model.LatPoint);
                  paramCollection.AddWithValue("@lngpoint", model.LngPoint);
                  paramCollection.AddWithValue("@radius", model.Radius);

              }, map: delegate (IDataReader reader, short set)
              {
                  MarketPlaceDomain items = new MarketPlaceDomain();
                  int startingIndex = 0; //startingOrdinal

                  items.QuoteRequestId = reader.GetSafeInt32(startingIndex++);
                  items.AddressId = reader.GetSafeInt32(startingIndex++);
                  items.CompanyId = reader.GetSafeInt32(startingIndex++);
                  items.Name = reader.GetSafeString(startingIndex++);
                  items.Estimation = reader.GetSafeString(startingIndex++);
                  items.DueDate = reader.GetSafeDateTime(startingIndex++);
                  items.CategoryId = reader.GetSafeInt32(startingIndex++);
                  items.Details = reader.GetSafeString(startingIndex++);
                  items.Address1 = reader.GetSafeString(startingIndex++);
                  items.City = reader.GetSafeString(startingIndex++);
                  items.State = reader.GetSafeString(startingIndex++);
                  items.ZipCode = reader.GetSafeString(startingIndex++);
                  items.StatusId = (QRState)reader.GetSafeInt32(startingIndex++);
                  items.LatPoint = reader.GetSafeDecimal(startingIndex++);
                  items.LngPoint = reader.GetSafeDecimal(startingIndex++);
                  items.Distance = reader.GetSafeDouble(startingIndex++);
                  items.Status = items.StatusId.ToString();

                  if (marketplaceQR == null)
                  {
                      marketplaceQR = new List<MarketPlaceDomain>();
                  }

                  marketplaceQR.Add(items);

              });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return marketplaceQR;
        }
    }
}