using UnityEngine;

namespace Unity.Services.Toolkits.Friends
{
    /// <summary>
    /// There is no Unity Player Profile service available at the time we created this Toolkit.
    /// So, to easily test the friends API, we made this simplified local social profile "Service".
    /// It logs in as various
    /// </summary>
    public class DebugSocialProfileService : ISocialProfileService
    {
        const string k_PlayerNamePrefix = "Player_";

        public string GetName(string id)
        {
            //This is where we would put in calls to social profile services to match our UAS ID to Profile ID's to get information outside the friends service.
            return $"{k_PlayerNamePrefix}{id}";
        }

    }
}
