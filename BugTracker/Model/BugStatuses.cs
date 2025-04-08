namespace BugTracker.Model;

public static class BugStatuses
{
    public const string New = "new";
    public const string Fixed = "fixed";
    public const string Duplicate = "duplicate";
    public const string Closed = "closed";

    public static string[] GetAllAvailableStatuses() => [New, Duplicate, Closed, Fixed];

}