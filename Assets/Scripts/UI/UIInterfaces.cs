using System;
using Unity.Services.Friends.Models;
using UnityEngine;


public interface IRelationshipsUIController
{
    ILocalPlayerView LocalPlayerView { get; set; }
    void Init();
}
public interface ILocalPlayerView
{
    Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

    void Refresh(string name, string id, string acitvity,
        PresenceAvailabilityOptions presenceAvailabilityOptions);
}

public interface IRequestFriendPopupView
{
    void AddFriendSuccess();
    void AddFriendFailed();
    Action<string> tryAddFriend { get; set; }
    bool IsShowing { get; set; }
}