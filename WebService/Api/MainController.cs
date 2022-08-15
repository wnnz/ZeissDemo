using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text;
using WebService.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace WebService.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : Controller
    {
        private readonly InMemDb dbcontext;
        public MainController(InMemDb dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResults()
        {
            var arr = await dbcontext.socketResults
                .Select(x => x.Result)
                //.Select(x => new { x.Topic, x.Ref, x.Payload, x.Event })
                .ToArrayAsync();
            return Content("[" + string.Join(",", arr) + "]");
            //return Json(arr);
        }
    }
}
