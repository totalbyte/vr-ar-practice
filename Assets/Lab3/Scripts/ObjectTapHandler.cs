using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ObjectTapHandler : MonoBehaviour
{
    public Camera arCamera;
    public ARObjectSpawner spawner;
    public Texture[] textures;               // текстури, які змінюються по колу

    private float maxTapDuration = 0.3f;     // короткий дотик (секунди)
    private float maxTapMovement = 30f;      // і майже без зсуву пальця (пікселі)
    private float multiTapInterval = 0.35f;  // макс. пауза між тапами однієї серії (секунди)

    private int tapCount;                    // скільки тапів у поточній серії
    private float lastTapTime;               // час останнього тапу
    private int textureIndex;                // яку текстуру ставити наступною
    public float spinSpeed = 90f;            // швидкість анімації (градусів за секунду)
    private bool animating;                  // чи запущена анімація

    void OnEnable() { ARTouchInput.Acquire(); }     // вмикає EnhancedTouch
    void OnDisable() { ARTouchInput.Release(); }

    void Update()
    {
        // анімація: об'єкт постійно обертається навколо вертикальної осі
        if (animating && spawner.SpawnedObject != null)
        {
            spawner.SpawnedObject.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }

        // якщо після останнього тапу минула пауза, серія завершилась: виконуємо дію за кількістю тапів
        if (tapCount > 0 && Time.unscaledTime - lastTapTime > multiTapInterval)
        {
            OnTaps(tapCount);
            tapCount = 0;
        }

        // тап рахуємо в момент, коли палець піднято
        if (Touch.activeTouches.Count == 1 && Touch.activeTouches[0].phase == TouchPhase.Ended)
        {
            Touch touch = Touch.activeTouches[0];

            // тап = короткий дотик без руху
            float duration = (float)(touch.time - touch.startTime);
            float movement = (touch.screenPosition - touch.startScreenPosition).magnitude;
            if (duration > maxTapDuration || movement > maxTapMovement)
            {
                tapCount = 0;
                return;
            }

            // тап має бути саме по віртуальному об'єкту (Raycast)
            if (!IsObjectHit(touch.screenPosition))
            {
                tapCount = 0;
                return;
            }

            tapCount++;
            lastTapTime = Time.unscaledTime;
        }
    }

    // чи влучає промінь з точки дотику у віртуальний об'єкт
    private bool IsObjectHit(Vector2 screenPosition)
    {
        GameObject target = spawner.SpawnedObject;
        if (target == null) return false;

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit) && hit.transform.root.gameObject == target;
    }

    // що робити після серії тапів
    private void OnTaps(int taps)
    {
        if (taps == 2)
        {
            ChangeTexture();           // подвійний тап: змінити текстуру
        }
        else if (taps == 3)
        {
            animating = !animating;    // потрійний тап: запустити або зупинити анімацію
        }
    }

    // підставляємо наступну текстуру в матеріал об'єкта
    private void ChangeTexture()
    {
        if (textures == null || textures.Length == 0) return;

        Renderer objectRenderer = spawner.SpawnedObject.GetComponentInChildren<Renderer>();
        objectRenderer.material.mainTexture = textures[textureIndex];
        textureIndex = (textureIndex + 1) % textures.Length;
    }
}