using Unity.Services.Friends.Models;

namespace Unity.Services.Toolkits.Friends
{
    public static class FriendsUtils
    {
        public static int RemapEnumIndex(PresenceAvailabilityOptions presenceAvailabilityOptions)
        {
            int remapped = presenceAvailabilityOptions switch
            {
                PresenceAvailabilityOptions.ONLINE => 0,
                PresenceAvailabilityOptions.BUSY => 1,
                PresenceAvailabilityOptions.AWAY => 2,
                PresenceAvailabilityOptions.INVISIBLE => 3,
                PresenceAvailabilityOptions.OFFLINE => 4,
                PresenceAvailabilityOptions.UNKNOWN => 5,
                _ => 5
            };
            return remapped;
        }
    }
}