namespace RLP_API.Enums
{
    public enum LaunchStatus
    {
        Launch_Failed = 0,
        Launch_Successful = 1,
    }

    // Checks the user input and returns the status in the form of what to expect in the DB
    public static class LaunchStatusFunctions
    {
        public static string GetLaunchStatus(LaunchStatus status)
        {
            if (status == LaunchStatus.Launch_Failed)
            {
                return "Launch Failure";
            }
            else if (status == LaunchStatus.Launch_Successful)
            {
                return "Launch Successful";
            }
            else
            {
                return "Launch Failure";
            }
        }
    }
}