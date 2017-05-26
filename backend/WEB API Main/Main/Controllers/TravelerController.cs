using BusinessSpecificLogic.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Reusable;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Traveler")]
    public class TravelerController : BaseController<TravelerHeader>
    {
        public TravelerController(ITravelerHeaderLogic logic) : base(logic)
        {
        }
    }
}
