using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlasticGui.Configuration.CloudEdition.Welcome;
using Unity.Services.Authentication;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class RelationshipsManager : MonoBehaviour
    {
        [Tooltip("Reference a GameObject that has a component extending from IRelationshipsUIController.")]
        [SerializeField]
        GameObject m_RelationshipsUIControllerGameObject;
        IRelationshipsUIController m_RelationshipsUIController;

        List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();
        List<PlayerProfile> m_RequestsEntryDatas = new List<PlayerProfile>();
        List<PlayerProfile> m_BlockEntryDatas = new List<PlayerProfile>();

        string m_LoggedPlayerName;
        ILocalPlayerView m_LocalPlayerView;
        IAddFriendView m_AddFriendView;
        IFriendsListView m_FriendsListView;
        IRequestListView m_RequestListView;
        IBlockedListView m_BlockListView;
        ISocialProfileService m_SocialProfileService;
        IManagedRelationshipService m_ManagedRelationshipService;

        string LoggedPlayerId => AuthenticationService.Instance.PlayerId;

        public async Task Init(string currentPlayerName, ISocialProfileService profileService)
        {
            m_SocialProfileService = profileService;
            UIInit();

            await LogInAsync(LoggedPlayerId);

            m_LoggedPlayerName = currentPlayerName;
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            await SubscribeToFriendsEventCallbacks();
            RefreshAll();
        }

        void UIInit()
        {
            if (m_RelationshipsUIControllerGameObject == null)
            {
                Debug.LogError($"Missing GameObject in {name}",gameObject);
                return;
            }

            m_RelationshipsUIController = m_RelationshipsUIControllerGameObject.GetComponent<IRelationshipsUIController>();
            if (m_RelationshipsUIController == null)
            {
                Debug.LogError($"No Component extending IRelationshipsUIController {m_RelationshipsUIControllerGameObject.name}",
                    m_RelationshipsUIControllerGameObject );
                return;
            }

            m_RelationshipsUIController.Init();
            m_LocalPlayerView = m_RelationshipsUIController.LocalPlayerView;
            m_AddFriendView = m_RelationshipsUIController.AddFriendView;

            //Bind Lists
            m_FriendsListView = m_RelationshipsUIController.FriendsListView;
            m_FriendsListView.BindList(m_FriendsEntryDatas);
            m_RequestListView = m_RelationshipsUIController.RequestListView;
            m_RequestListView.BindList(m_RequestsEntryDatas);
            m_BlockListView = m_RelationshipsUIController.BlockListView;
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

        public async void LogIn(string playerName)
        {
            await LogInAsync(playerName);
        }
        
        public async Task LogInAsync(string playerName)
        {
            await UASUtils.SwitchUser(playerName);
            m_LoggedPlayerName = playerName;
            if (m_ManagedRelationshipService != null)
            {
                m_ManagedRelationshipService.Dispose();
                // Want to make sure wire has a chance to shutdown (we need a dispose async method!)
                await Task.Delay(500);
            }
            m_ManagedRelationshipService = await ManagedRelationshipService.CreateManagedRelationshipServiceAsync();
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            RefreshAll();
            Debug.Log($"Logged in as {playerName} id: {LoggedPlayerId}");
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
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, status.activity, status.presence);
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
            var totalFriends = 0;

            var friends = GetFriends();
            if (friends == null)
                return;
            AddFriends(friends);

            void AddFriends(List<Member> friends)
            {
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
                        Name = m_SocialProfileService.GetName(friend.Id),
                        Id = friend.Id,
                        Availability = friend.Presence.Availability,
                        Activity = activityText
                    };
                    m_FriendsEntryDatas.Add(info);
                    totalFriends++;
                }
                m_RelationshipsUIController.RelationshipBarView.Refresh();
            }
        }

        void RefreshRequests()
        {
            m_RequestsEntryDatas.Clear();
            var requests = GetRequests();
            if (requests == null)
                return;
            AddRequests(requests);


            void AddRequests(List<Member> requests)
            {
                foreach (var request in requests)
                {
                    m_RequestsEntryDatas.Add(new PlayerProfile(m_SocialProfileService.GetName(request.Id), request.Id));
                }
                m_RelationshipsUIController.RelationshipBarView.Refresh();
            }
        }

        void RefreshBlocks()
        {
            m_BlockEntryDatas.Clear();
            var blocks = GetBlocks();
            if (blocks == null)
                return;
            AddBlocks(blocks);

            void AddBlocks(List<Member> blocks)
            {
                foreach (var block in blocks)
                {
                    m_BlockEntryDatas.Add(new PlayerProfile(m_SocialProfileService.GetName(block.Id), block.Id));
                }
                m_RelationshipsUIController.RelationshipBarView.Refresh();
            }
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
        /// Get am amount of friends (including presence data).
        /// </summary>
        /// <returns>List of friends.</returns>
        List<Member> GetFriends()
        {            
            return getNonBlockedMembers(m_ManagedRelationshipService.Friends);
        }

        /// <summary>
        /// Get an amount of Requests. The friends SDK maintains relationships unless explicitly deleted, even those
        /// towards blocked players. We don't want to show blocked players' requests, so we filter them out.
        /// </summary>
        /// <returns>List of players.</returns>
        List<Member> GetRequests()
        {
            return getNonBlockedMembers(m_ManagedRelationshipService.IncomingFriendRequests);
        }

        /// <summary>
        /// Get an amount of Blocks.
        /// </summary>
        /// <returns>List of players.</returns>
        List<Member> GetBlocks()
        {
            return m_ManagedRelationshipService.Blocks.Select(x => x.Member).ToList();
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

        async Task SubscribeToFriendsEventCallbacks()
        {
            try
            {
              
                // callbacks.RelationshipsEventConnectionStateChanged += e =>
                // {
                //     RefreshFriends();
                //     Debug.Log("FriendsEventConnectionStateChanged EventReceived");
                // };
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
        List<Member> getNonBlockedMembers(IList<Relationship> relationships)
        {
            var blocks = GetBlocks();
            return relationships
                .Select(x => x.Member)
                .Where(x => !blocks.Any(y => x.Id == y.Id))
                .ToList();
        }
    }
}
