using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos.Web.Models;

namespace Cronos.Web.ViewModels
{
    public class ProgressMenuViewModel
    {
        public UserState CurrentState { get; set; }
        public UserState HighestState { get; set; }
    }
}
