using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Options;

namespace UnityGamingServicesUsesCases
{
    namespace Relationships
    {
        public class UnityFriendsApiWrapper
        {
            public async void AddAsync(string playerID, Action<string> callback, string eventSource = null)
            {
                try
                {
                    await Friends.Instance.AddFriendAsync(playerID, eventSource);
                    callback?.Invoke(playerID);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to add {playerID}.");
                    Debug.LogError(e);
                }
            }

            public async void RemoveAsync(string playerID, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.RemoveFriendAsync(playerID);
                    callback?.Invoke(playerID);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to remove {playerID}.");
                    Debug.LogError(e);
                }
            }

            public async void BlockAsync(string playerID, Action<string> callback, string eventSource = null)
            {
                try
                {
                    await Friends.Instance.BlockAsync(playerID, eventSource);
                    callback?.Invoke(playerID);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to block {playerID}.");
                    Debug.LogError(e);
                }
            }

            public async void UnblockAsync(string playerID, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.UnblockAsync(playerID);
                    callback?.Invoke(playerID);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to unblock {playerID}.");
                    Debug.LogError(e);
                }
            }

            public async void AcceptRequestAsync(string playerID, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.ConsentFriendRequestAsync(playerID);
                    callback?.Invoke(playerID);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to accept request from {playerID}.");
                    Debug.LogError(e);
                }
            }

            public async void DeclineRequestAsync(string playerID, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.IgnoreFriendRequestAsync(playerID);
                    callback?.Invoke(playerID);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to decline request from {playerID}.");
                    Debug.LogError(e);
                }
            }

            public async void GetFriendsWithoutPresenceAsync(Action<List<Player>> callback)
            {
                try
                {
                    var friends = await Friends.Instance.GetFriendsAsync(new PaginationOptions());
                    callback?.Invoke(friends);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log("Failed to retrieve the friend list.");
                    Debug.LogError(e);
                }
            }

            public async void GetFriendsWithPresenceAsync(Action<List<PlayerPresence<Activity>>> callback)
            {
                try
                {
                    var friends = await Friends.Instance.GetFriendsAsync<Activity>(new PaginationOptions());
                    callback?.Invoke(friends);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log("Failed to retrieve the friend list.");
                    Debug.LogError(e);
                }
            }

            public async void SetPresenceAsync(Presence<Activity> presence, Action callback)
            {
                try
                {
                    await Friends.Instance.SetPresenceAsync(presence);
                    callback?.Invoke();
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to set the presence to {presence.GetAvailability()}");
                    Debug.LogError(e);
                }
            }
        }
    }
}