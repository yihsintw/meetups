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

        private AuthenticationState? _authenticationState;

        private bool isAuthenticated = false;

        //set property and see if user is organizer
        public bool IsOrganizer { get => isAuthenticated && _authenticationState != null && _authenticationState.User.IsInRole(SharedHelper.OrganizerRole); }
        

        override protected void OnInitialized()
        {
            LayoutService.SetSectionContent(null);
        }

        protected override async Task OnInitializedAsync()
        {
            //await base.OnInitializedAsync();
            _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (_authenticationState != null)
            {
                isAuthenticated = _authenticationState.User.Identity != null && _authenticationState.User.Identity.IsAuthenticated;
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
                if (isAuthenticated && _authenticationState != null)
                {
                    return _authenticationState.User.Identity?.Name;
                }
                return string.Empty;
            }
        }

        protected int? UserId
        {
            get
            {
                if (isAuthenticated && _authenticationState != null)
                {
                    var userIdClaim = _authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
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
                if (isAuthenticated && _authenticationState != null)
                {
                    var userNameClaim = _authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
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
