namespace Unity.Services.Samples
{
    /// <summary>
    /// Extend this interface to create your own player profile service.
    /// Like adding support for user icons,and other information. 
    /// </summary>
    public interface IPlayerProfileService
    {
        public string GetName(string playerId);
    }
}
