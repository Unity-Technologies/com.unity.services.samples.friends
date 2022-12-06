using System;
using Unity.Services.Friends.Models;

namespace Unity.Services.Toolkits.Friends
{
    public interface ILocalPlayerView
    {
        Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

        void Refresh(string name, string id, string activity,
            PresenceAvailabilityOptions presenceAvailabilityOptions);
    }

}

