using System;
using Unity.Services.Friends.Models;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRelationshipBarView
    {
        Action onShowFriends { get; set; }
        Action onShowRequests { get; set; }
        Action onShowBlocks { get; set; }
        Action onShowRequestFriend { get; set; }
    }


}

