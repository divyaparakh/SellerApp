using Common;
using DataAccess.DB;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;
using MongoDB.Bson;
using Common.Models;
using BuyerApp.Validation;
using Microsoft.AspNetCore.Authorization;

namespace BuyerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBidController : Controller
    {
        IDBAccess<ProductBid> dbPBAccess;
        IConfiguration configuration;
        public ProductBidController(IDBAccess<ProductBid> access,  IConfiguration configuration)
        {
            dbPBAccess = access;
            this.configuration = configuration;
        }
        
        [Authorize]
        [HttpPost("Add")]
        public JsonResult Add(ProductBidModel productBidModel)
        {
            ProductBid productBid = MapProductBid(productBidModel);

            ValidateProductBid validateProductBid = new ValidateProductBid();
            validateProductBid.ConnectionString = configuration.GetConnectionString("MongoDB");
            List<Error> errors = validateProductBid.AddValidate(productBid);
            if (errors.Count == 0)
            {
                try
                {
                    dbPBAccess.Insert(productBid);
                    return new JsonResult(new Response<string> { Code = HttpStatusCode.OK, Message = "Success" });
                }
                catch (Exception er)
                {
                    return new JsonResult(new Response<string> { Code = HttpStatusCode.InternalServerError, Message = er.Message });
                }
            }
            else
            {
                return new JsonResult(new Response<List<Error>> { Code = HttpStatusCode.InternalServerError, Message = "Failed", Data = errors });
            }
        }

        private static ProductBid MapProductBid(ProductBidModel productBidModel)
        {
            return new ProductBid
            {
                Phone = productBidModel.Phone,
                Pin = productBidModel.Pin,
                ProductId = string.IsNullOrEmpty(productBidModel.ProductId) ? new ObjectId() : new ObjectId(productBidModel.ProductId),
                Address = productBidModel.Address,
                State = productBidModel.State,
                City = productBidModel.City,
                BidAmount = productBidModel.BidAmount,
                Email = productBidModel.Email,
                FirstName = productBidModel.FirstName,
                LastName = productBidModel.LastName
            };
        }

        [Authorize]
        [HttpPut("Update")]
        public JsonResult Update(ProductBidModel productBidModel)
        {
            ProductBid productBid = dbPBAccess.Get(new ObjectId(productBidModel.Id));
            if (productBid == null && productBid.Email.ToLower() != productBidModel.Email.ToLower())
            {
                return new JsonResult(new Response<string> { Code = HttpStatusCode.NotFound, Message = "Product Bid not found" });
            }
            ValidateProductBid validateProductBid = new ValidateProductBid();
            validateProductBid.ConnectionString = configuration.GetConnectionString("MongoDB");
            List<Error> errors = validateProductBid.UpdateCheck(productBid);
            if (errors.Count == 0)
            {
                try
                {
                    productBid.BidAmount = productBidModel.BidAmount;
                    dbPBAccess.Update(productBid);
                    return new JsonResult(new Response<string> { Code = HttpStatusCode.OK, Message = "Success" });
                }
                catch (Exception er)
                {
                    return new JsonResult(new Response<string> { Code = HttpStatusCode.InternalServerError, Message = er.Message });
                }
            }
            else
            {
                return new JsonResult(new Response<List<Error>> { Code = HttpStatusCode.InternalServerError, Message = "Failed", Data = errors });
            }
        }
    }
}
