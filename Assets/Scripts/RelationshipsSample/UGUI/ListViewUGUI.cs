using UnityEngine;

namespace Unity.Services.Toolkits.Friends.UGUI
{
    public abstract class ListViewUGUI : MonoBehaviour
    {
        public void Show()
        {
            Refresh();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public abstract void Refresh();
    }
}