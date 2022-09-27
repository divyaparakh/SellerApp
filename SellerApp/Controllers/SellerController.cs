using Common;
using DataAccess.DB;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace SellerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        IDBAccess<Product> dbPAccess;
        IDBAccess<Seller> dbAccess;
        IDBAccess<ProductBid> dbPBAccess;
        public SellerController(IDBAccess<Seller> dbAccess, IDBAccess<Product> dbPAccess, IDBAccess<ProductBid> dbPBAccess)
        {
            this.dbAccess = dbAccess;
            this.dbPAccess = dbPAccess;
            this.dbPBAccess = dbPBAccess;
        }


        [HttpGet]
        public JsonResult Get()
        {
            var data = dbAccess.GetAll();
            return new JsonResult(new Response<List<Seller>> { Code = HttpStatusCode.OK, Message = "Success", Data = data });
        }
        [HttpPost("Add")]
        public JsonResult Add(Seller seller)
        {
            try
            {
                if (dbAccess.Get(seller.Email, "Email") == null)
                {
                    dbAccess.Insert(seller);
                    return new JsonResult(new Response<string> { Code = HttpStatusCode.OK, Message = "Success" });
                }
                else
                {
                    return new JsonResult(new Response<string> { Code = HttpStatusCode.Ambiguous, Message = "Seller aready exist" });
                }
            }
            catch (Exception er)
            {
                return new JsonResult(new Response<string> { Code = HttpStatusCode.InternalServerError, Message = er.Message });
            }
        }
        [HttpGet("GetAll")]
        public JsonResult GetAll()
        {
            var product = (from p in dbPAccess.GetAll()
                           select new
                           {
                               p.ShortDescription,
                               p.DetailedDescription,
                               p.Category,
                               p.StartingPrice,
                               p.BidEndDate,
                               p.ProductName,
                               Bids = dbPBAccess.GetAll(p.Id, "ProductId").OrderByDescending(x => x.BidAmount).ToList()
                           }).ToList();

            return new JsonResult(new Response<object> { Code = HttpStatusCode.OK, Message = "Success", Data = product });
        }

    }
}
