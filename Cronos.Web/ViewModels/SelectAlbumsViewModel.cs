using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos.Web.Extensions;
using Cronos.Web.Models;

namespace Cronos.Web.ViewModels
{
    public class SelectAlbumsViewModel : FlowBaseViewModel, IValidatableObject
    {
        public int Id { get; set; }

        [DisplayName("Album Results")]
        public IEnumerable<Album> AlbumResults { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!AlbumResults?.Any(a=>a.Checked) ?? false)
                yield return new ValidationResult("You must select at least one album to add to a playlist", new[] {nameof(AlbumResults)});
        }
    }
}
