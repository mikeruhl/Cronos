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
using Microsoft.Extensions.Caching.Memory;

namespace Cronos.Web.Controllers
{
    public class CronosBaseController : Controller
    {
        private IMemoryCache _memoryCache;
       

        public CronosBaseController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        protected UserProgress CronosState { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            

            if (context.HttpContext.Session.TryGetValue(Constants.SessionKeyName, out var sessionId))
            {
                var debugGuid = Encoding.UTF8.GetString(sessionId).Replace("\"","");
                var guidGood = Guid.TryParse(debugGuid, out var cronosGuid);

                if (guidGood)
                {
                    if (_memoryCache.TryGetValue(cronosGuid, out UserProgress cronosState))
                    {
                        CronosState = cronosState;
                    }
                    else
                    {
                        //Alert user that session expired and they'll have to start over
                        TempData[Constants.ArtistErrorMessage] =
                            "Your session has been reset due to inactivity.  Please start again.";
                        CronosState = new UserProgress {Id = cronosGuid};
                        RedirectToAction("SelectArtist", "Home");
                    }
                }
            }
            else
            {
                CronosState = new UserProgress {Id = Guid.NewGuid()};
            }

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _memoryCache.Set(CronosState.Id, CronosState, cacheEntryOptions);

            context.HttpContext.Session.Set(Constants.SessionKeyName, CronosState.Id);
        }
    }
}
