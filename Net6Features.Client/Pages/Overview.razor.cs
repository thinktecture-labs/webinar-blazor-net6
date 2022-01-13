using Microsoft.AspNetCore.Components;
using Net6Features.Client.Components.Collections;

namespace Net6Features.Client.Pages
{
    public partial class Overview
    {
        [Inject] public NavigationManager? NavigationManager { get; set; }

        [Parameter]
        public string Data { get; set; } = String.Empty;

        [Parameter, SupplyParameterFromQuery(Name = "searchTerm")]
        public string SearchTerm { get; set; } = String.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
