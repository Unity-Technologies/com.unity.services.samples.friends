using System;
using Unity.Services.Friends.Models;

namespace Unity.Services.Samples.Friends
{
    public interface ILocalPlayerView
    {
        Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

        void Refresh(string name, string activity,
            PresenceAvailabilityOptions presenceAvailabilityOptions);
    }

}
