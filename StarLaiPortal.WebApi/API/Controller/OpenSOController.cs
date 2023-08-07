using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Dapper;
using StarLaiPortal.WebApi.Model;
using DevExpress.ExpressApp.Security;

namespace StarLaiPortal.WebApi.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OpenSOController : ControllerBase
    {
        IObjectSpaceFactory objectSpaceFactory;
        ISecurityProvider securityProvider;
        public OpenSOController(IObjectSpaceFactory objectSpaceFactory, ISecurityProvider securityProvider)
        {
            this.objectSpaceFactory = objectSpaceFactory;
            this.securityProvider = securityProvider;
        }
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        //return Ok(rtn.ToList());
        //        using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("ConnectionString")))
        //        {
        //            var com = conn.Query<Companys>("select * from StarLai_Common..ODBC");
        //            return Ok(JsonConvert.SerializeObject(com, Formatting.Indented));
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem(ex.Message);
        //    }
        //}

    }
}
