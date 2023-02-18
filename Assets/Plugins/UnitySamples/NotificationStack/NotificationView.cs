using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Samples.UX
{
    public class NotificationView : MonoBehaviour
    {
        [SerializeField] Image m_NotificationImage;
        [SerializeField] TMP_Text m_NotificationTitle;
        [SerializeField] TMP_Text m_NotificationText;
        [SerializeField] CanvasGroup m_NotificationGroup;
        [SerializeField] float m_FadeTime = 1;

        float m_LifeTime = 1;

        public void Init(float lifeTime, string nameText, string notificationText, Sprite image = null)
        {
            if (image != null)
            {
                m_NotificationImage.gameObject.SetActive(true);
                m_NotificationImage.sprite = image;
            }
            else
            {
                m_NotificationImage.gameObject.SetActive(false);
            }

            m_LifeTime = lifeTime;
            m_NotificationTitle.text = nameText;
            m_NotificationText.text = notificationText;
            StartCoroutine(NotificationLifeTime());
        }

        IEnumerator NotificationLifeTime()
        {
            yield return new WaitForSeconds(m_LifeTime);

            var fadeTime = m_FadeTime;
            while (fadeTime >= 0)
            {
                m_NotificationGroup.alpha = fadeTime / m_FadeTime;
                yield return new WaitForEndOfFrame();
                fadeTime -= Time.deltaTime;
            }

            Destroy(gameObject);
        }
    }
}