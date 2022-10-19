using Unity.Services.Friends.Models;

namespace UnityGamingServicesUsesCases.Relationships
{
    [System.Serializable]
    public struct FriendsEntryData
    {
        public string Name;
        public string Id;
        public PresenceAvailabilityOptions Availability;
        public string Activity;
    }
}