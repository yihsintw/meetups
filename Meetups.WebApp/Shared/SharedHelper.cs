using Microsoft.AspNetCore.Components;

namespace Meetups.WebApp.Shared
{
    public class SharedHelper(NavigationManager navigationManager)
    {
        public NavigationManager NavigationManager { get; } = navigationManager;
        private const string ATTENDEE_ROLE = "Attendee";
        private const string ORGANIZER_ROLE = "Organizer";
        private const string ADMIN_ROLE = "Admin";
        private const string GOING_STATUS = "Going";
        private const string NOT_GOING_STATUS = "Not Going";
        //payment cancel
        private const string CANCELLED_STATUS = "Cancelled";

        public static string AttendeeRole => ATTENDEE_ROLE;
        public static string OrganizerRole => ORGANIZER_ROLE;
        public static string AdminRole => ADMIN_ROLE;
        public static string GoingStatus => GOING_STATUS;
        public static string NotGoingStatus => NOT_GOING_STATUS;
        public static string CancelledStatus => CANCELLED_STATUS;




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