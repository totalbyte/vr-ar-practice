using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ObjectRotator : MonoBehaviour
{
    public ARObjectSpawner spawner;
    private GameObject selectedObject;
    private float rotationSpeed = 0.2f;     // швидкість обертання

    void OnEnable() { ARTouchInput.Acquire(); }     // вмикає EnhancedTouch
    void OnDisable() { ARTouchInput.Release(); }

    void Update()
    {
        // для жесту обертання кількість активних доторків дорівнює 2
        if (Touch.activeTouches.Count == 2)
        {
            selectedObject = spawner.SpawnedObject;
            if (selectedObject == null) return;

            Touch touch1 = Touch.activeTouches[0];
            Touch touch2 = Touch.activeTouches[1];

            // перевіряємо, чи обидва дотики рухаються, та обчислюємо їх зміщення
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 delta1 = touch1.delta;
                Vector2 delta2 = touch2.delta;

                // пальці рухаються в одному напрямку
                if (Vector2.Dot(delta1.normalized, delta2.normalized) > 0.9f)
                {
                    // середнє зміщення по осях X та Y для обох пальців
                    float averageDeltaX = (delta1.x + delta2.x) / 2;
                    float averageDeltaY = (delta1.y + delta2.y) / 2;

                    // обертання навколо вертикальної осі (Y)
                    selectedObject.transform.Rotate(0, -averageDeltaX * rotationSpeed, 0);

                    // обертання навколо горизонтальної осі (X)
                    // selectedObject.transform.Rotate(averageDeltaY * rotationSpeed, 0, 0);
                }
            }
        }
    }
}