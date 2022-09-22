using Common;
using DataAccess.DB;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Net;


namespace SellerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        [HttpGet("GetAll")]
        public JsonResult GetAll()
        {
            DBAccess dBAccess = new DBAccess("Product");
            var data = dBAccess.GetAll<DataAccess.Models.Product>();
            return new JsonResult(new Response<List<DataAccess.Models.Product>> { Code = HttpStatusCode.OK, Message = "Success", Data = data });
        }
        [HttpPost("Add")]
        public JsonResult Add(Product product)
        {
            List<Error> errors = ValidateProduct.AddValidate(product);
            if (errors.Count == 0)
            {
                DBAccess dBAccess = new DBAccess("Product");
                try
                {
                    dBAccess.Insert(product);
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
