using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases
{
    namespace Relationships
    {
        public class RelationshipsSceneManager : MonoBehaviour
        {
            private RelationshipsApiWrapper m_RelationshipsApiWrapper = null;

            async void Start()
            {
                await UnityServices.InitializeAsync();

                AuthenticationService.Instance.ClearSessionToken();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("Authenticated as " + AuthenticationService.Instance.PlayerId);
                Debug.Log("Token " + AuthenticationService.Instance.AccessToken);
                
                m_RelationshipsApiWrapper = new RelationshipsApiWrapper();
                
            }

            #region API Calls

            private void AddFriendById(string playerId, string eventSource)
            {
                m_RelationshipsApiWrapper.AddAsync(playerId, OnFriendAdded, eventSource);
            }

            private void RemoveFriendById(string playerId)
            {
                m_RelationshipsApiWrapper.RemoveAsync(playerId, OnFriendRemoved);
            }

            private void BlockFriendById(string playerId, string eventSource)
            {
                m_RelationshipsApiWrapper.BlockAsync(playerId, OnFriendBlocked, eventSource);
            }

            private void UnblockFriendById(string playerId)
            {
                m_RelationshipsApiWrapper.UnblockAsync(playerId, OnFriendUnblocked);
            }

            private void AcceptFriendRequest(string playerId)
            {
                m_RelationshipsApiWrapper.AcceptRequestAsync(playerId, OnFriendRequestAccepted);
            }

            private void DeclineFriendRequest(string playerId)
            {
                m_RelationshipsApiWrapper.DeclineRequestAsync(playerId, OnFriendRequestDeclined);
            }

            private void GetFriendsWithoutPresence()
            {
                m_RelationshipsApiWrapper.GetFriendsWithoutPresenceAsync(OnFriendsWithoutPresenceListReceived);
            }

            private void GetFriendsWithPresence()
            {
                m_RelationshipsApiWrapper.GetFriendsWithPresenceAsync(OnFriendsWithPresenceListReceived);
            }

            private void SetPresence(PresenceAvailabilityOptions presenceAvailabilityOptions, string activityStatus)
            {
                var activity = new Activity { Status = activityStatus };
                var presence = new Presence<Activity>(presenceAvailabilityOptions, activity);
                m_RelationshipsApiWrapper.SetPresenceAsync(presence, OnPresenceSet);
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
                    Debug.Log($"{presence.Player.Id} availability :  {availability} and activity : {activity}");
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