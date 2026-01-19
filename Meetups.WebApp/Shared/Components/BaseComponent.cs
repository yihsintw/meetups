using Meetups.WebApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Meetups.WebApp.Shared.Components
{
    public class BaseComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        [Inject]
        private LayoutService LayoutService { get; set; } = default!;
        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        private AuthenticationState? authenticationState;
        private bool isAuthenticated = false;

        override protected void OnInitialized()
        {
            LayoutService.SetSectionContent(null);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authenticationState != null)
            {
                isAuthenticated = authenticationState.User.Identity != null && authenticationState.User.Identity.IsAuthenticated;
            }

            if (isAuthenticated)
            {
                StateHasChanged();

            }
        }

        protected bool IsAuthenticated
        {
            get
            {
                return isAuthenticated;
            }

        }

        protected string? UserEmail
        {
            get
            {
                if (isAuthenticated && authenticationState != null)
                {
                    return authenticationState.User.Identity?.Name;
                }
                return string.Empty;
            }
        }

        protected int? UserId
        {
            get
            {
                if (isAuthenticated && authenticationState != null)
                {
                    var userIdClaim = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        return userId;
                    }
                }
                return null;
            }
        }

        protected string? UserName
        {
            get
            {
                if (isAuthenticated && authenticationState != null)
                {
                    var userNameClaim = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                    if (userNameClaim != null)
                    {
                        return userNameClaim.Value;
                    }
                }
                return string.Empty;
            }
        }
    }
}
