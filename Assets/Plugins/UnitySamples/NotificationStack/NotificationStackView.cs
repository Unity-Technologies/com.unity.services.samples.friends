using UnityEngine;

namespace Unity.Samples.UX
{
    public class NotificationStackView : MonoBehaviour
    {
        [SerializeField] NotificationView m_NotificationView;
        [SerializeField] Transform m_StackParent;

        public void CreateNotification(float lifeTime, string playerName, string notificationContent,
            Sprite image = null)
        {
            var notificationInstance = Instantiate(m_NotificationView, m_StackParent);
            notificationInstance.Init(lifeTime, playerName, notificationContent, image);
        }
    }
}