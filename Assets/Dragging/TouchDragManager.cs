using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchDragManager : MonoBehaviour
{
    [System.Serializable]
    public struct DragOptions
    {
        public float ForceMultiplier;
        public bool DragFromOffsetPoint;
    };
    [Header("Options")]
    public DragOptions Options = new()
    {
        ForceMultiplier = 10,
        DragFromOffsetPoint = true
    };
    Camera cam;
    Dictionary<int, IDraggable> currentTouches;
    void Start()
    {
        currentTouches = new();
    }

    // Update is called once per frame
    void Update()
    {
        cam = Camera.main;

        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var touchPos = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                var pointer = new PointerEventData(EventSystem.current);
                pointer.position = touchPos;
                print($"Touch began with ID {touch.fingerId}");

                List<RaycastResult> results = new();
                EventSystem.current.RaycastAll(pointer, results);
                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent<IDraggable>(out var draggable))
                    {
                        currentTouches[i] = draggable;
                        draggable.BeginTouch(Options, touch, Vector3.zero);
                        return;
                    }
                }
                var worldPoint = cam.ScreenToWorldPoint(touchPos);
                var overlappingColliders = Physics2D.OverlapPointAll(worldPoint);
                foreach (var collider in overlappingColliders)
                {
                    var obj = collider.gameObject;
                    print($"Processing object: {obj}");
                    var draggable = obj.GetComponent<IDraggable>();
                    if (draggable != null)
                    {
                        currentTouches[touch.fingerId] = draggable;
                        draggable.BeginTouch(Options, touch, worldPoint);
                        break;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (currentTouches.ContainsKey(touch.fingerId))
                {
                    currentTouches[touch.fingerId].ContinueTouch(Options, touch);
                }
            }
            else
            {
                if (!currentTouches.ContainsKey(touch.fingerId)) return;
                print($"Touch ended with ID {touch.fingerId}");
                currentTouches[touch.fingerId].EndTouch(Options, touch);
                currentTouches.Remove(touch.fingerId);
            }
        }
    }
}
