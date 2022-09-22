using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Options;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class RelationshipsSceneManager : MonoBehaviour
    {
        [SerializeField] private AddFriendView _addFriendView = null;
      

        public void Init()
        {
            _addFriendView.OnAddFriendRequested += AddFriendByIdVoid;
        }

        private async void AddFriendByIdVoid(string id)
        {
            await AddFriendById(id, "button");
        }

        private async Task AddFriendById(string playerId, string eventSource)
        {
            try
            {
                await Friends.Instance.AddFriendAsync(playerId, eventSource);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to add {playerId}.");
                Debug.LogError(e);
            }

            Debug.Log($"{playerId} was added to the friends list.");
        }

        private async Task RemoveFriendById(string playerId)
        {
            try
            {
                await Friends.Instance.RemoveFriendAsync(playerId);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to remove {playerId}.");
                Debug.LogError(e);
            }

            Debug.Log($"{playerId} was added to the friends list.");
        }

        private async Task BlockFriend(string playerId, string eventSource = null)
        {
            try
            {
                await Friends.Instance.BlockAsync(playerId, eventSource);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to block {playerId}.");
                Debug.LogError(e);
            }

            Debug.Log($"{playerId} was blocked.");
        }

        private async Task UnblockFriend(string playerId)
        {
            try
            {
                await Friends.Instance.UnblockAsync(playerId);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to unblock {playerId}.");
                Debug.LogError(e);
            }

            Debug.Log($"{playerId} was unblocked.");
        }

        private async Task AcceptRequest(string playerId)
        {
            try
            {
                await Friends.Instance.ConsentFriendRequestAsync(playerId);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to accept request from {playerId}.");
                Debug.LogError(e);
            }

            Debug.Log($"Friend request from {playerId} was accepted.");
        }

        private async Task DeclineRequest(string playerId)
        {
            try
            {
                await Friends.Instance.IgnoreFriendRequestAsync(playerId);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to decline request from {playerId}.");
                Debug.LogError(e);
            }

            Debug.Log($"Friend request from {playerId} was declined.");
        }

        private async Task<List<Player>> GetFriendsWithoutPresence()
        {
            try
            {
                var friends = await Friends.Instance.GetFriendsAsync(new PaginationOptions());
                return friends;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the friend list.");
                Debug.LogError(e);
            }

            return null;
        }

        private async Task<List<PlayerPresence<Activity>>> GetFriendsWithPresence()
        {
            try
            {
                var friends = await Friends.Instance.GetFriendsAsync<Activity>(new PaginationOptions());
                return friends;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the friend list.");
                Debug.LogError(e);
            }

            return null;
        }

        private async Task SetPresence(PresenceAvailabilityOptions presenceAvailabilityOptions,
            string activityStatus)
        {
            var activity = new Activity { Status = activityStatus };
            var presence = new Presence<Activity>(presenceAvailabilityOptions, activity);

            try
            {
                await Friends.Instance.SetPresenceAsync(presence);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to set the presence to {presence.GetAvailability()}");
                Debug.LogError(e);
            }
        }
    }
}