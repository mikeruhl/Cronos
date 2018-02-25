using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cronos.Web.Models;
using Cronos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cronos.Web.Components
{
    public class ProgressMenu : ViewComponent
    {
        private IMapper _mapper;

        public ProgressMenu(IMapper mapper)
        {
            _mapper = mapper;
        }
        public IViewComponentResult Invoke(FlowBaseViewModel progress)
        {
            return View(progress);
        }
    }
}
