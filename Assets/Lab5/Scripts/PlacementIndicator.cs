using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject visual;          // диск, який показуємо/ховаємо

    private readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    /// чи є зараз під центром екрана площина
    public bool IsValid { get; private set; }

    /// позиція та орієнтація, у яку буде поставлено об'єкт
    public Pose PlacementPose { get; private set; }

    void Update()
    {
        // промінь з центру екрана — туди, куди дивиться камера
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        IsValid = raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        if (IsValid)
        {
            Pose pose = hits[0].pose;    // найближча точка перетину з площиною

            // повертаємо індикатор лише навколо вертикалі так, щоб він дивився на камеру
            Vector3 toCamera = arCamera.transform.position - pose.position;
            toCamera.y = 0;
            if (toCamera.sqrMagnitude > 0.0001f)
                pose.rotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);

            PlacementPose = pose;
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }

        // площина є — показуємо диск, як немає ховаємо
        if (visual.activeSelf != IsValid)
            visual.SetActive(IsValid);
    }
}