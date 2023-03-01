namespace Unity.Services.Samples.Friends
{
    /// <summary>
    /// Extend this interface and add to it for your own user profile system, like adding support for user Icons,
    /// and other information you want to show in the Relationship UI
    /// </summary>
    public interface IProfileService
    {
        string GetName(string playerID);
    }
}
