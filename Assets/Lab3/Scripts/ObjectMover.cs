using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ObjectMover : MonoBehaviour
{
    public Camera arCamera;
    public ARRaycastManager raycastManager;     // для визначення площини
    private GameObject selectedObject;
    private Vector2 touchPosition;
    private float dragThreshold = 30f;      // скільки пікселів треба пройти, щоб це вважалось перетягуванням
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();     // список для результатів raycast

    void OnEnable() { ARTouchInput.Acquire(); }     // вмикає EnhancedTouch
    void OnDisable() { ARTouchInput.Release(); }

    void Update()
    {
        if (Touch.activeTouches.Count == 1)
        {
            // отримуємо позицію дотику
            Touch touch = Touch.activeTouches[0];
            touchPosition = touch.screenPosition;

            // при початковому дотику перевіряємо, чи натиснули на об'єкт
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject) && hitObject.collider.GetComponentInParent<ARPlane>() == null)
                {
                    selectedObject = hitObject.transform.root.gameObject;
                }
            }

            // якщо дотик перетягування і об'єкт існує — змінюємо позицію
            if (touch.phase == TouchPhase.Moved && selectedObject != null
                && (touch.screenPosition - touch.startScreenPosition).magnitude > dragThreshold)
            {
                // Raycast визначає нову позицію на площині
                if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;
                    selectedObject.transform.position = hitPose.position;
                }
            }

            // якщо дотик завершено — обнуляємо вибраний об'єкт
            if (touch.phase == TouchPhase.Ended)
            {
                selectedObject = null;
            }
        }
    }
}