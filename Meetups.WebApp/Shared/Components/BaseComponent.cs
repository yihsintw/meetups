using Meetups.WebApp.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Meetups.WebApp.Shared.Components
{
    public class BaseComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        [Inject]
        private LayoutService LayoutService { get; set; } = default!;

        override protected void OnInitialized()
        {
            LayoutService.SetSectionContent(null);
        }
    }
}
