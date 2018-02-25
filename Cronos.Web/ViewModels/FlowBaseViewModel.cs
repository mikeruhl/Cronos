using Cronos.Web.Models;

namespace Cronos.Web.ViewModels
{
    public class FlowBaseViewModel
    {
        public UserState CurrentState { get; set; }
        public UserState HighestState { get; set; }

    }
}
