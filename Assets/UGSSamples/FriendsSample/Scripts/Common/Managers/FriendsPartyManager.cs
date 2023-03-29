using System;
using Unity.Services.Friends;
using Unity.Services.Friends.Notifications;
using UnityEngine;
using Unity.Samples.UI;

namespace Unity.Services.Samples.Friends
{
    /// <summary>
    /// Collection of functions for enabling Party functionality in the Friends Sample.
    /// Also an example of
    /// </summary>
    public class FriendsPartyManager
    {
        [System.Serializable]
        public class PartyInviteMessage
        {
            public string InviteMessage;
            public string SenderID;
            public string partyCode;
        }

        PlayerProfile m_LoggedPlayerProfile;

        public FriendsPartyManager(PlayerProfile localProfile)
        {
            m_LoggedPlayerProfile = localProfile;
            FriendsService.Instance.MessageReceived += ProcessPartyInvite;
        }

        //Player has been invited by a friend to party
        void TryJoinParty(string partyCode)
        {
            LobbyEvents.RequestJoinLobby?.Invoke(partyCode);
        }

        public async void SendPartyInvite(string targetPlayerID, string myPartyCode)
        {
            var party = new PartyInviteMessage()
            {
                InviteMessage = $"{m_LoggedPlayerProfile.Name} has invited you to a party, accept?",
                SenderID = m_LoggedPlayerProfile.Id,
                partyCode = myPartyCode
            };
            await FriendsService.Instance.MessageAsync(targetPlayerID, party);
        }

        void ProcessPartyInvite(IMessageReceivedEvent messageEvent)
        {
            try
            {
                var inviteMessage = messageEvent.GetMessageAs<PartyInviteMessage>();
                if (inviteMessage != null)
                {
                    // Pop up that a party request happened with Accept/decline options
                    //On Accept, Join party and Set In Party
                    PartyInvitePopupAsync(inviteMessage);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error getting PartyInviteMessage from {messageEvent.UserId}\n-{ex}");
            }
        }

        async void PartyInvitePopupAsync(PartyInviteMessage inviteMessage)
        {
            Debug.Log($"Got Party Invite: {inviteMessage.InviteMessage}\n" +
                $" -From: {inviteMessage.SenderID}\n" +
                $" -Code: {inviteMessage.partyCode}");
            int choice = await PopUpEvents.ShowPopup(inviteMessage.InviteMessage, "Join", "Cancel");
            Debug.Log($"Popup choice selected {choice}");

            if (choice == 0)
            {
                TryJoinParty(inviteMessage.partyCode);
            }
        }
    }
}