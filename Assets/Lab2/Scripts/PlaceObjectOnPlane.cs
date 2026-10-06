using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;          // новий Input System
using UnityEngine.XR.ARFoundation;      // ARRaycastManager, ARRaycastHit
using UnityEngine.XR.ARSubsystems;      // TrackableType
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;   // префаб FoodAR

    private ARRaycastManager raycastManager;
    private readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject spawnedObject;                    // вже розміщений об'єкт
    private readonly List<RaycastResult> uiHits = new List<RaycastResult>();

    public GameObject SpawnedObject => spawnedObject;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // 1. Чи був новий дотик до екрана саме в цьому кадрі?
        var touchscreen = Touchscreen.current;
        if (touchscreen == null || !touchscreen.primaryTouch.press.wasPressedThisFrame)
            return;

        Vector2 touchPosition = touchscreen.primaryTouch.position.ReadValue();
        if (IsPointerOverUI(touchPosition))
            return;

        // 2. Промінь з точки дотику: шукаємо перетин з виявленими площинами
        if (!raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            return;

        // 3. Найближча точка перетину (позиція + орієнтація)
        Pose hitPose = hits[0].pose;

        // 4. Перший тап — створюємо, наступні — переміщуємо той самий об'єкт
        if (spawnedObject == null)
            spawnedObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
        else
            spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
    }

    bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;
        var pointerData = new PointerEventData(EventSystem.current) { position = screenPosition };
        uiHits.Clear();
        EventSystem.current.RaycastAll(pointerData, uiHits);
        return uiHits.Count > 0;   // під пальцем є UI-елемент
    }
}