using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos.Web.Extensions;
using Cronos.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cronos.Web.Controllers
{
    public class CronosBaseController : Controller
    {
        protected UserProgress CronosState { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.Keys.Contains(Constants.SessionKeyName))
            {

                CronosState = HttpContext.Session.Get<UserProgress>(Constants.SessionKeyName);
            }
            else
            {
                CronosState = new UserProgress();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Session.Set(Constants.SessionKeyName, CronosState);
        }
    }
}
