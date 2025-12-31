using Microsoft.AspNetCore.Components;

namespace Meetups.WebApp.Shared.Services
{
    public class LayoutService
    {
        public RenderFragment? SectionContent { get; private set;}

        public Action? OnSectionContentChanged; 

        public void SetSectionContent(RenderFragment? content)
        {
            SectionContent = content;
            OnSectionContentChanged?.Invoke();
        }
    }
}
