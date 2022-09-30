using UnityEngine;
using UnityEngine.UIElements;

public class LocalUserView : UIBaseView
{
    public override string ViewName => "LocalUserView";

    BasePlayerEntry m_LocalPlayerEntry;
    public LocalUserView(VisualElement viewRoot)
        : base(viewRoot)
    {
        m_LocalPlayerEntry = new BasePlayerEntry(viewRoot.Q("BasePlayerEntry"));
    }




    protected override void SetVisualElements()
    {
    }

    protected override void RegisterButtonCallbacks()
    {
        throw new System.NotImplementedException();
    }
}

