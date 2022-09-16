using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases
{
    namespace Relationships
    {
        public class FriendsUiController : MonoBehaviour
        {
            private UnityFriendsApiWrapper _unityFriendsApiWrapper = null;

            public void Init()
            {
                _unityFriendsApiWrapper = new UnityFriendsApiWrapper();
            }

            #region API Calls

            private void AddFriendById(string playerId, string eventSource)
            {
                _unityFriendsApiWrapper.AddAsync(playerId, OnFriendAdded, eventSource);
            }

            private void RemoveFriendById(string playerId)
            {
                _unityFriendsApiWrapper.RemoveAsync(playerId, OnFriendRemoved);
            }

            private void BlockFriendById(string playerId, string eventSource)
            {
                _unityFriendsApiWrapper.BlockAsync(playerId, OnFriendBlocked, eventSource);
            }

            private void UnblockFriendById(string playerId)
            {
                _unityFriendsApiWrapper.UnblockAsync(playerId, OnFriendUnblocked);
            }

            private void AcceptFriendRequest(string playerId)
            {
                _unityFriendsApiWrapper.AcceptRequestAsync(playerId, OnFriendRequestAccepted);
            }

            private void DeclineFriendRequest(string playerId)
            {
                _unityFriendsApiWrapper.DeclineRequestAsync(playerId, OnFriendRequestDeclined);
            }

            private void GetFriendsWithoutPresence()
            {
                _unityFriendsApiWrapper.GetFriendsWithoutPresenceAsync(OnFriendsWithoutPresenceListReceived);
            }

            private void GetFriendsWithPresence()
            {
                _unityFriendsApiWrapper.GetFriendsWithPresenceAsync(OnFriendsWithPresenceListReceived);
            }

            private void SetPresence(PresenceAvailabilityOptions presenceAvailabilityOptions, string activityStatus)
            {
                var activity = new Activity { Status = activityStatus };
                var presence = new Presence<Activity>(presenceAvailabilityOptions, activity);
                _unityFriendsApiWrapper.SetPresenceAsync(presence, OnPresenceSet);
            }

            #endregion

            #region Callbacks

            private void OnFriendAdded(string playerId)
            {
                Debug.Log($"{playerId} was added to the friends list.");
            }

            private void OnFriendRemoved(string playerId)
            {
                Debug.Log($"{playerId} was added to the friends list.");
            }

            private void OnFriendBlocked(string playerId)
            {
                Debug.Log($"{playerId} was blocked.");
            }

            private void OnFriendUnblocked(string playerId)
            {
                Debug.Log($"{playerId} was unblocked.");
            }

            private void OnFriendRequestAccepted(string playerId)
            {
                Debug.Log($"Friend request from {playerId} was accepted.");
            }

            private void OnFriendRequestDeclined(string playerId)
            {
                Debug.Log($"Friend request from {playerId} was declined.");
            }

            private void OnFriendsWithoutPresenceListReceived(List<Player> friendsList)
            {
            }

            private void OnFriendsWithPresenceListReceived(List<PlayerPresence<Activity>> friendsList)
            {
                foreach (var presence in friendsList)
                {
                    var availability = presence.Presence.GetAvailability();
                    var activity = presence.Presence.GetActivity();
                    Debug.LogError($"{presence.Player.Id} availability :  {availability} and activity : {activity}");
                }
            }

            private void OnPresenceSet()
            {
                Debug.Log("Presence set.");
            }

            #endregion
        }
    }
}