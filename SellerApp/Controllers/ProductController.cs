using Common;
using DataAccess.DB;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Common.Models;

namespace SellerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        IDBAccess<Product> dbAccess;
        IConfiguration configuration;
        public ProductController(IDBAccess<Product> access, IConfiguration configuration)
        {
            dbAccess = access;
            this.configuration = configuration;
        }
        [HttpGet("GetAll")]
        public JsonResult GetAll()
        {
            var data = dbAccess.GetAll();
            return new JsonResult(new Response<List<Product>> { Code = HttpStatusCode.OK, Message = "Success", Data = data });
        }
        [HttpPost("Add")]
        public JsonResult Add(ProductModel productModel)
        {
            Product product = MapProduct(productModel);

            ValidateProduct validateProduct = new ValidateProduct();
            validateProduct.ConnectionString = configuration.GetConnectionString("MongoDB");
            List<Error> errors = validateProduct.AddValidate(product);
            if (errors.Count == 0)
            {
                try
                {
                    dbAccess.Insert(product);
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

        private static Product MapProduct(ProductModel productModel)
        {
            return new Product
            {
                ProductName = productModel.ProductName,
                StartingPrice = productModel.StartingPrice,
                BidEndDate = productModel.BidEndDate,
                SellerId = string.IsNullOrEmpty(productModel.SellerId) ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(productModel.SellerId),
                ShortDescription = productModel.ShortDescription,
                DetailedDescription = productModel.DetailedDescription,
                Category = productModel.Category
            };
        }

        [HttpDelete("Delete")]
        public JsonResult Delete(ProductModel productModel)
        {
            Product product = dbAccess.Get(new ObjectId(productModel.Id));
            if (product == null)
            {
                return new JsonResult(new Response<string> { Code = HttpStatusCode.NotFound, Message = "Product not found" });
            }
            ValidateProduct validateProduct = new ValidateProduct();
            validateProduct.ConnectionString = configuration.GetConnectionString("MongoDB");
            List<Error> errors = validateProduct.DeleteValidate(product);
            if (errors.Count == 0)
            {
                try
                {
                    dbAccess.Delete(product.Id);
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
