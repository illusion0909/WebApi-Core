using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebApiCoreFrontEnd.Models.ViewModels
{
    public class TrailVM
    {
        public Trail Trail { get; set; }
        public IEnumerable<SelectListItem> nationalParkList { get; set; }
    }
}
