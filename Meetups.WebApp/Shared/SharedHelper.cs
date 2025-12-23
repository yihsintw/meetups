namespace Meetups.WebApp.Shared
{
    public class SharedHelper
    {
        //GetCategory
        public List<string> GetCategories()
        {
            return [.. Enum.GetNames<MeetupCategoriesEnum>()];
        }
    }
}
