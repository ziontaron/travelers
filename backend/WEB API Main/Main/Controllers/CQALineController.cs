using BusinessSpecificLogic.EF;
using ReusableWebAPI.Controllers;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace CQA.Controllers
{
    [RoutePrefix("api/CQALine")]
    public class CQALineController : BaseController<CQALine>
    {
        public CQALineController(ICQALineLogic logic) : base(logic) { }
    }
}