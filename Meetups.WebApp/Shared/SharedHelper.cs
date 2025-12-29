using Microsoft.AspNetCore.Components;

namespace Meetups.WebApp.Shared
{
    public class SharedHelper(NavigationManager navigationManager)
    {
        public NavigationManager NavigationManager { get; } = navigationManager;


        //GetCategories
        public List<string> GetCategories()
        {
            return [.. Enum.GetNames<MeetupCategoriesEnum>()];
        }

        public string GetQueryParamValue(string queryParamName)
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var filter = queryParams[queryParamName] ?? "";
            return filter;


        }
    }
}