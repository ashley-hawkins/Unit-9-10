using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraggable
{
    public abstract void BeginTouch(TouchDragManager.DragOptions opts, Touch touch, Vector3 worldPos);
    public abstract void ContinueTouch(TouchDragManager.DragOptions opts, Touch touch);
    public abstract void EndTouch(TouchDragManager.DragOptions opts, Touch touch);
}
