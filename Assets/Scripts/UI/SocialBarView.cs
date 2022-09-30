
using UnityEngine;
using UnityEngine.UIElements;

public class SocialBarView : UIBaseView
{
    public override string ViewName => "SocialBarView";

    public SocialBarView(VisualElement viewRoot)
        : base(viewRoot) { }

    protected override void SetVisualElements() { }

    protected override void RegisterButtonCallbacks() { }
}