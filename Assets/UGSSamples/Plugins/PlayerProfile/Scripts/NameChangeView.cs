using System;
using TMPro;
using UnityEngine;
namespace Unity.Services.Samples.Parties
{
    public class NameChangeView : MonoBehaviour
    {
        public event Action<string> OnNameChanged;
        [SerializeField] TMP_InputField m_NameField;

        public void SetName(string newName)
        {
            m_NameField.SetTextWithoutNotify(newName);
        }
        public void Init(string startname)
        {
            SetName(startname);
            m_NameField.onEndEdit.AddListener((s) => OnNameChanged?.Invoke(s));
        }
    }
}
