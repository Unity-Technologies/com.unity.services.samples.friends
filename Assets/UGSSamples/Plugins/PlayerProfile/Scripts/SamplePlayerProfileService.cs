
namespace Unity.Services.Samples
{
    /// <summary>
    /// Replace this with your own player profile service.
    /// </summary>
    public class SamplePlayerProfileService : IPlayerProfileService
    {
        private const string k_NamePrefix = "Player_";

        public string GetName(string playerId)
        {
            //Get a player name with the UAS Id from a profile service.
            return $"{k_NamePrefix}{playerId}";
        }

    }
}
