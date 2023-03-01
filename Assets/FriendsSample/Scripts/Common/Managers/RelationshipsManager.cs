using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace Unity.Services.Samples.Friends
{
    public class RelationshipsManager : MonoBehaviour
    {
        [Tooltip("Reference a GameObject that has a component extending from IRelationshipsUIController.")]
        [SerializeField]
        GameObject m_RelationshipsViewGameObject; //This gameObject reference is only needed to get the IRelationshipUIController component from it.
        IRelationshipsView m_RelationshipsView;

        List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();
        List<IUGSPlayer> m_RequestsEntryDatas = new List<IUGSPlayer>();
        List<IUGSPlayer> m_BlockEntryDatas = new List<IUGSPlayer>();

        ILocalPlayerView m_LocalPlayerView;
        IAddFriendView m_AddFriendView;
        IFriendsListView m_FriendsListView;
        IRequestListView m_RequestListView;
        IBlockedListView m_BlockListView;
        IManagedRelationshipService m_ManagedRelationshipService;
        IUGSAuthService m_UGSAuth;
        IProfileService m_ProfileService;
        IUGSPlayer m_LocalPlayer;


        async void Start()
        {
            //We use the samples implementation of the UGS auth service to ensure we sign-in once across multiple drag&drop samples.

            m_UGSAuth = new UGSAnonymousProfileService();
            m_ProfileService = new SampleSocialProfileService();

            await m_UGSAuth.TryAuthenticate();
            m_LocalPlayer = m_UGSAuth.LocalPlayer;
            await Init();
        }

        async Task Init()
        {
            UIInit();

            await LogInAsync(m_LocalPlayer.Name);

            m_LocalPlayerView.Refresh(m_LocalPlayer.Name, m_LocalPlayer.Id, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);

            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            SubscribeToFriendsEventCallbacks();

            RefreshAll();
        }

        void UIInit()
        {
            if (m_RelationshipsViewGameObject == null)
            {
                Debug.LogError($"Missing GameObject in {name}",gameObject);
                return;
            }

            m_RelationshipsView = m_RelationshipsViewGameObject.GetComponent<IRelationshipsView>();
            if (m_RelationshipsView == null)
            {
                Debug.LogError($"No Component extending IRelationshipsView {m_RelationshipsViewGameObject.name}",
                    m_RelationshipsViewGameObject );
                return;
            }

            m_RelationshipsView.Init();
            m_LocalPlayerView = m_RelationshipsView.LocalPlayerView;
            m_AddFriendView = m_RelationshipsView.AddFriendView;

            //Bind Lists
            m_FriendsListView = m_RelationshipsView.FriendsListView;
            m_FriendsListView.BindList(m_FriendsEntryDatas);
            m_RequestListView = m_RelationshipsView.RequestListView;
            m_RequestListView.BindList(m_RequestsEntryDatas);
            m_BlockListView = m_RelationshipsView.BlockListView;
            m_BlockListView.BindList(m_BlockEntryDatas);

            //Bind Friends SDK Callbacks
            m_AddFriendView.onFriendRequestSent += AddFriendAsync;
            m_FriendsListView.onRemove += RemoveFriendAsync;
            m_FriendsListView.onBlock += BlockFriendAsync;
            m_RequestListView.onAccept += AcceptRequestAsync;
            m_RequestListView.onDecline += DeclineRequestAsync;
            m_RequestListView.onBlock += BlockFriendAsync;
            m_BlockListView.onUnblock += UnblockFriendAsync;
            m_LocalPlayerView.onPresenceChanged += SetPresenceAsync;
        }

        async Task LogInAsync(string playerName)
        {
            m_LocalPlayer.SetName(playerName);
            if (m_ManagedRelationshipService != null)
            {
                m_ManagedRelationshipService.Dispose();
                // Want to make sure wire has a chance to shutdown (we need a dispose async method!)
                await Task.Delay(500);
            }
            m_ManagedRelationshipService = await ManagedRelationshipService.CreateManagedRelationshipServiceAsync();
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            m_LocalPlayerView.Refresh(m_LocalPlayer.Name, m_LocalPlayer.Id, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            RefreshAll();
            Debug.Log($"Logged in as {m_LocalPlayer.Name} id: {m_LocalPlayer.Id}");
        }

        public void RefreshAll()
        {
            RefreshFriends();
            RefreshRequests();
            RefreshBlocks();
        }

        async void BlockFriendAsync(string id)
        {
            await BlockFriend(id);
            RefreshAll();
        }

        async void UnblockFriendAsync(string id)
        {
            await UnblockFriend(id);
            RefreshBlocks();
            RefreshFriends();
        }

        async void RemoveFriendAsync(string id)
        {
            await RemoveFriend(id);
            RefreshFriends();
        }

        async void AcceptRequestAsync(string id)
        {
            await AcceptRequest(id);
            RefreshRequests();
            RefreshFriends();
        }

        async void DeclineRequestAsync(string id)
        {
            await DeclineRequest(id);
            RefreshRequests();
        }

        async void SetPresenceAsync((PresenceAvailabilityOptions presence, string activity) status)
        {
            await SetPresence(status.presence, status.activity);
            m_LocalPlayerView.Refresh(m_LocalPlayer.Name, m_LocalPlayer.Id, status.activity, status.presence);
        }

        async void AddFriendAsync(string id)
        {
            var success = await SendFriendRequest(id);
            if(success)
                m_AddFriendView.FriendRequestSuccess();
            else
                m_AddFriendView.FriendRequestFailed();
        }

        void RefreshFriends()
        {
            m_FriendsEntryDatas.Clear();

            var friends = GetFriends();

            foreach (var friend in friends)
            {
                string activityText;
                if (friend.Presence.Availability == PresenceAvailabilityOptions.OFFLINE ||
                    friend.Presence.Availability == PresenceAvailabilityOptions.INVISIBLE)
                {
                    activityText = friend.Presence.LastSeen.ToShortDateString() + " " + friend.Presence.LastSeen.ToLongTimeString();
                }
                else
                {
                    activityText = friend.Presence.GetActivity<Activity>() == null ? "" : friend.Presence.GetActivity<Activity>().Status;
                }

                var info = new FriendsEntryData
                {
                    Name = m_ProfileService.GetName(friend.Id),
                    Id = friend.Id,
                    Availability = friend.Presence.Availability,
                    Activity = activityText
                };
                m_FriendsEntryDatas.Add(info);
            }
            m_RelationshipsView.RelationshipBarView.Refresh();
        }

        void RefreshRequests()
        {
            m_RequestsEntryDatas.Clear();
            var requests = GetRequests();

            foreach (var request in requests)
            {
                m_RequestsEntryDatas.Add(new UGSPlayer(m_ProfileService.GetName(request.Id), request.Id));
            }
            m_RelationshipsView.RelationshipBarView.Refresh();
        }

        void RefreshBlocks()
        {
            m_BlockEntryDatas.Clear();

            foreach (var block in m_ManagedRelationshipService.Blocks)
            {
                m_BlockEntryDatas.Add(new UGSPlayer(m_ProfileService.GetName(block.Member.Id), block.Member.Id));
            }
            m_RelationshipsView.RelationshipBarView.Refresh();
        }

        async Task<bool> SendFriendRequest(string playerId)
        {
            try
            {
                await m_ManagedRelationshipService.AddFriendAsync(playerId);
                Debug.Log($"{playerId} friend request sent.");
                return true;
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to add {playerId} - {e}.");
                return false;
            }
        }

        async Task RemoveFriend(string playerId)
        {
            try
            {
                await m_ManagedRelationshipService.DeleteFriendAsync(playerId);
                Debug.Log($"{playerId} was removed from the friends list.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to remove {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task BlockFriend(string playerId)
        {
            try
            {
                await m_ManagedRelationshipService.AddBlockAsync(playerId);
                Debug.Log($"{playerId} was blocked.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to block {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task UnblockFriend(string playerId)
        {
            try
            {
                await m_ManagedRelationshipService.DeleteBlockAsync(playerId);
                Debug.Log($"{playerId} was unblocked.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to unblock {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task AcceptRequest(string playerId)
        {
            try
            {
                await SendFriendRequest(playerId);
                Debug.Log($"Friend request from {playerId} was accepted.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to accept request from {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task DeclineRequest(string playerId)
        {
            try
            {
                await m_ManagedRelationshipService.DeleteIncomingFriendRequestAsync(playerId);
                Debug.Log($"Friend request from {playerId} was declined.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to decline request from {playerId}.");
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Get an amount of friends (including presence data).
        /// </summary>
        /// <returns>List of friends.</returns>
        List<Member> GetFriends()
        {
            return GetNonBlockedMembers(m_ManagedRelationshipService.Friends);
        }

        /// <summary>
        /// Get an amount of Requests. The friends SDK maintains relationships unless explicitly deleted, even those
        /// towards blocked players. We don't want to show blocked players' requests, so we filter them out.
        /// </summary>
        /// <returns>List of players.</returns>
        List<Member> GetRequests()
        {
            return GetNonBlockedMembers(m_ManagedRelationshipService.IncomingFriendRequests);
        }

        async Task SetPresence(PresenceAvailabilityOptions presenceAvailabilityOptions,
            string activityStatus = "")
        {
            var activity = new Activity { Status = activityStatus };

            try
            {
                await m_ManagedRelationshipService.SetPresenceAsync(presenceAvailabilityOptions, activity);
                Debug.Log($"Availability changed to {presenceAvailabilityOptions}.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to set the presence to {presenceAvailabilityOptions}");
                Debug.LogError(e);
            }
        }

        void SubscribeToFriendsEventCallbacks()
        {
            try
            {
                m_ManagedRelationshipService.RelationshipAdded += e =>
                {
                    RefreshRequests();
                    RefreshFriends();
                    Debug.Log($"create {e.GetRelationship()} EventReceived");
                };
                m_ManagedRelationshipService.MessageReceived += e =>
                {
                    RefreshRequests();
                    Debug.Log("MessageReceived EventReceived");
                };
                m_ManagedRelationshipService.PresenceUpdated += e =>
                {
                    RefreshFriends();
                    Debug.Log("PresenceUpdated EventReceived");
                };
                m_ManagedRelationshipService.RelationshipDeleted += e =>
                {
                    RefreshFriends();
                    Debug.Log($"delete {e.GetRelationship()} EventReceived");
                };
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log(
                    "An error occurred while performing the action. Code: " + e.Reason + ", Message: " + e.Message);
            }
        }

        /// <summary>
        /// Returns a list of members that are not blocked by the active user.
        /// </summary>
        /// <param name="relationships">The list of relationships to filter.</param>
        /// <returns>Filtered list of members.</returns>
        private List<Member> GetNonBlockedMembers(IList<Relationship> relationships)
        {
            var blocks = m_ManagedRelationshipService.Blocks;
            return relationships
                .Where(relationship => !blocks.Any(blockedRelationship => blockedRelationship.Member.Id == relationship.Member.Id))
                .Select(relationship => relationship.Member)
                .ToList();
        }
    }
}
