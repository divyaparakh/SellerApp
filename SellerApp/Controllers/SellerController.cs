using Common;
using DataAccess.DB;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace SellerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        IDBAccess<Seller> dbAccess;
        public SellerController(IDBAccess<Seller> dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        
        [HttpGet]
        public JsonResult Get()
        {
            var data = dbAccess.GetAll();
            return new JsonResult(new Response<List<Seller>> { Code = HttpStatusCode.OK, Message = "Success", Data = data });
        }
    }
}
