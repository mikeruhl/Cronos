using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos.Web.Models;

namespace Cronos.Web.ViewModels
{
    public class SelectAlbumsViewModel : FlowBaseViewModel
    {
        public IEnumerable<Album> AlbumResults { get; set; }
    }
}
