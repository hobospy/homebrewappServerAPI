using System.Runtime.CompilerServices;

namespace homebrewAppServerAPI.Helpers
{
    public static class Helper
    {
        public static string GetCurrentMethod([CallerMemberName] string method = "")
        {
            return method;
        }
    }
}
