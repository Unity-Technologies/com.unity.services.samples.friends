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
            public async void AddAsync(string playerId, Action<string> callback, string eventSource = null)
            {
                try
                {
                    await Friends.Instance.AddFriendAsync(playerId, eventSource);
                    callback?.Invoke(playerId);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to add {playerId}.");
                    Debug.LogError(e);
                }
            }

            public async void RemoveAsync(string playerId, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.RemoveFriendAsync(playerId);
                    callback?.Invoke(playerId);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to remove {playerId}.");
                    Debug.LogError(e);
                }
            }

            public async void BlockAsync(string playerId, Action<string> callback, string eventSource = null)
            {
                try
                {
                    await Friends.Instance.BlockAsync(playerId, eventSource);
                    callback?.Invoke(playerId);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to block {playerId}.");
                    Debug.LogError(e);
                }
            }

            public async void UnblockAsync(string playerId, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.UnblockAsync(playerId);
                    callback?.Invoke(playerId);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to unblock {playerId}.");
                    Debug.LogError(e);
                }
            }

            public async void AcceptRequestAsync(string playerId, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.ConsentFriendRequestAsync(playerId);
                    callback?.Invoke(playerId);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to accept request from {playerId}.");
                    Debug.LogError(e);
                }
            }

            public async void DeclineRequestAsync(string playerId, Action<string> callback)
            {
                try
                {
                    await Friends.Instance.IgnoreFriendRequestAsync(playerId);
                    callback?.Invoke(playerId);
                }
                catch (FriendsServiceException e)
                {
                    Debug.Log($"Failed to decline request from {playerId}.");
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