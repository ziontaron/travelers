using BusinessSpecificLogic.EF;
using ReusableWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Reusable;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace CQA.Controllers
{
    [RoutePrefix("api/CQAHeader")]
    public class CQAHeaderController : BaseController<CQAHeader>
    {
        public CQAHeaderController(ICQAHeaderLogic logic) : base(logic) { }
    }
}