using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockedListView : UIBaseView
{
    public override string ViewName => "BlockedListView";
    public BlockedListView(VisualElement viewRoot)
        : base(viewRoot) { }

    protected override void SetVisualElements()
    {
        throw new System.NotImplementedException();
    }

    protected override void RegisterButtonCallbacks()
    {
        throw new System.NotImplementedException();
    }
}
