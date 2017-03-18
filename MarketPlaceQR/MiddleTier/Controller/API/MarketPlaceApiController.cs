using Microsoft.Practices.Unity;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/marketplace")]

    public class MarketPlaceApiController : ApiController
    {
        [Dependency]
        public IMarketPlaceService _MarketPlaceService { get; set; }

        [Route("search"), HttpGet]
        public HttpResponseMessage QuoteRequestMarketPlaceSearch([FromUri] MarketPlaceRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (model.CategoryIds != null && model.CategoryIds.Count > 0)
            {
                
                DataTable CategoryIds = new DataTable(); //<---
                CategoryIds.Clear(); //<---

                // Create column.
                var column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "CategoryId";
                column.AutoIncrement = false;
                column.Caption = "CategoryId";
                column.ReadOnly = false;
                column.Unique = false;
                // Add the column to the table.
                CategoryIds.Columns.Add(column); //<---

                //CategoryIds.Columns.Add("CategoryId");

                foreach (int CategoryId in model.CategoryIds) {
                    //- Populate the DataTable for the loading the schedules
                    DataRow _category = CategoryIds.NewRow(); //<---
                    _category["CategoryId"] = CategoryId; //<---
                    if (_category["CategoryId"] != null)
                    { }
                    CategoryIds.Rows.Add(_category); //<---
                    if (CategoryIds.Rows[0].ItemArray[0] != null)
                    {
                        var thing = CategoryIds.Rows[0].ItemArray[0];
                    }
                };

                model.CategoryIdList = CategoryIds;
            }

            var marketPlaceQuoteRequest = new List<MarketPlaceDomain>();

            // handling multiple catergories is a little buggy. 
            // Do just one if possible.
                marketPlaceQuoteRequest = _MarketPlaceService.GetByQuoteRequestId(model);

            ItemsResponse<MarketPlaceDomain> response = new ItemsResponse<MarketPlaceDomain>();

            response.Items = marketPlaceQuoteRequest;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}