using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour, IDraggable
{
    public float Radius = 100;
    public Vector2 Output { get { return output;  } }
    private Vector2 output;

    private Vector2 initialPosition = Vector2.zero;
    int lockedToFinger = -1;
    public void BeginTouch(TouchDragManager.DragOptions opts, Touch touch, Vector3 _)
    {
        if (lockedToFinger != -1) return;
        lockedToFinger = touch.fingerId;
        initialPosition = transform.position;
        UpdateJoystick(touch);
    }
    public void ContinueTouch(TouchDragManager.DragOptions opts, Touch touch)
    {
        if (lockedToFinger != touch.fingerId) return;
        UpdateJoystick(touch);
    }
    public void EndTouch(TouchDragManager.DragOptions x, Touch y)
    {
        lockedToFinger = -1;
        transform.position = initialPosition;
        output = Vector2.zero;
    }

    void UpdateJoystick(Touch touch)
    {
        var displacementVector = touch.position - initialPosition;
        displacementVector = Vector2.ClampMagnitude(displacementVector, Radius);
        print(displacementVector);
        transform.position = initialPosition + displacementVector;
        output = displacementVector / Radius;
    }
}
