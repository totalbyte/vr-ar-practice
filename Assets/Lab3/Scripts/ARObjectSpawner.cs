using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;      // ARRaycastManager, ARRaycastHit
using UnityEngine.XR.ARSubsystems;      // TrackableType
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

/// Створює віртуальний об'єкт при першому дотику до виявленої площини.
/// Наступні дотики об'єкт не переставляють: ним керують жести.
[RequireComponent(typeof(ARRaycastManager))]
public class ARObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;

    private ARRaycastManager raycastManager;
    private readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject spawnedObject;

    /// Об'єкт, який зараз стоїть у сцені (null, поки його не розміщено)
    public GameObject SpawnedObject => spawnedObject;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void OnEnable() { ARTouchInput.Acquire(); }
    void OnDisable() { ARTouchInput.Release(); }

    void Update()
    {
        if (spawnedObject != null) return;   // об'єкт уже є — далі працюють жести

        foreach (Touch touch in Touch.activeTouches)
        {
            if (touch.phase != TouchPhase.Began) continue;                       // лише початок дотику
            if (ARTouchInput.IsPointerOverUI(touch.screenPosition)) continue;    // не по кнопці UI

            // Промінь з точки дотику -> перетин з виявленими площинами
            if (!raycastManager.Raycast(touch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
                continue;

            Pose hitPose = hits[0].pose;     // найближча точка перетину (позиція + орієнтація)
            spawnedObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
            return;
        }
    }
}