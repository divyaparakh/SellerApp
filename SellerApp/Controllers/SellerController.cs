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
        [HttpGet]
        public JsonResult Get()
        {
            DBAccess dBAccess = new DBAccess("Seller");
            var data = dBAccess.GetAll<Seller>();
            return new JsonResult(new Response<List<Seller>> { Code = HttpStatusCode.OK,Message = "Success",Data = data });
        }
    }
}
