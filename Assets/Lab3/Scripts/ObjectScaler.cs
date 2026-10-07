using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ObjectScaler : MonoBehaviour
{
    public ARObjectSpawner spawner;
    private GameObject selectedObject;
    private float initialDistance;          // початкова відстань між пальцями
    private Vector3 initialScale;           // початковий масштаб об'єкта

    void OnEnable() { ARTouchInput.Acquire(); }     // вмикає EnhancedTouch
    void OnDisable() { ARTouchInput.Release(); }

    void Update()
    {
        // для жесту масштабування кількість доторків дорівнює 2
        if (Touch.activeTouches.Count == 2)
        {
            selectedObject = spawner.SpawnedObject;
            if (selectedObject == null) return;

            Touch touchZero = Touch.activeTouches[0];
            Touch touchOne = Touch.activeTouches[1];

            // 1 із пальців щойно торкнувся екрану: зберігаємо початкові дані
            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.screenPosition, touchOne.screenPosition);
                initialScale = selectedObject.transform.localScale;
            }

            // під час руху пальців змінюємо масштаб
            if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
            {
                // поточна відстань між пальцями
                float currentDistance = Vector2.Distance(touchZero.screenPosition, touchOne.screenPosition);

                // пальці рухаються в одному напрямку — це обертання, а не щипок: масштаб не чіпаємо
                if (Vector2.Dot(touchZero.delta.normalized, touchOne.delta.normalized) > 0.9f)
                {
                    initialDistance = currentDistance;
                    initialScale = selectedObject.transform.localScale;
                    return;
                }

                if (Mathf.Approximately(initialDistance, 0))
                {
                    return;
                }

                // обчислюємо фактор масштабування
                float scaleFactor = currentDistance / initialDistance;

                // застосовуємо масштабування до об'єкта
                selectedObject.transform.localScale = initialScale * scaleFactor;
            }
        }
    }
}