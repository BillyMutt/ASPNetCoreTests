using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCoreDeepDive
{
    [Route("api")]
    public class SimpleController : Controller
    {
        public SimpleController(IHubContext<JohanHub> hubContext)
        {
            Program.Model.MyHubContext = hubContext;
        }

        [HttpGet]
        [Route("test")]
        public string GetTest()
        {
            return "Request handled by controller";
        }
    }
}
