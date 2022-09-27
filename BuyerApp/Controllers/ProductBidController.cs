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

namespace BuyerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBidController : Controller
    {
        IDBAccess<ProductBid> dbPBAccess;
        IDBAccess<Product> dbPAccess;
        IConfiguration configuration;
        public ProductBidController(IDBAccess<ProductBid> access, IDBAccess<Product> dbPAccess, IConfiguration configuration)
        {
            dbPBAccess = access;
            this.dbPAccess = dbPAccess;
            this.configuration = configuration;
        }
        [HttpGet("GetAll")]
        public JsonResult GetAll(ProductBidModel productBidModel)
        {

            var product = (from p in dbPAccess.GetAll()
                           join pb in dbPBAccess.GetAll() on p.Id equals pb.ProductId
                           where !string.IsNullOrEmpty(productBidModel.Id) ? p.Id == new ObjectId(productBidModel.Id) : true
                           select new
                           {
                               p.ShortDescription,
                               p.DetailedDescription,
                               p.Category,
                               p.StartingPrice,
                               p.BidEndDate,
                               Name = pb.FirstName + " " + pb.LastName,
                               Address = pb.Address + " " + pb.City + " " + pb.State + " " + pb.Pin,
                               pb.BidAmount,
                               pb.Email,
                               pb.Phone
                           }).OrderByDescending(x => x.BidAmount).ToList();

            return new JsonResult(new Response<object> { Code = HttpStatusCode.OK, Message = "Success", Data = product });
        }

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
