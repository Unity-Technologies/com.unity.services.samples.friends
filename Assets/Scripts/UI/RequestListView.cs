using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RequestListView : UIBaseView
{
    // Start is called before the first frame update
    public override string ViewName => "RequestListView";
    public RequestListView(VisualElement viewRoot)
        : base(viewRoot) { }

    protected override void SetVisualElements()
    {
    }

    protected override void RegisterButtonCallbacks()
    {
    }
}
