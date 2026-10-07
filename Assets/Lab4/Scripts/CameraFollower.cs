using UnityEngine;

/// Об'єкт "слідує" за камерою: залишається в певній позиції відносно неї
public class CameraFollower : MonoBehaviour
{
    public Transform arCamera;
    public Vector3 offset = new Vector3(0f, -0.1f, 0.4f);   // зміщення: x — вправо, y — вгору, z — вперед (у метрах)
    public bool isFollowing = false;

    private float lastCameraYaw;                            // кут повороту камери навколо осі Y у попередньому кадрі

    void Start()
    {
        // префаб не може зберігати посилання на об'єкт сцени, тому камеру знаходимо при створенні об'єкта
        if (arCamera == null)
            arCamera = Camera.main.transform;

        lastCameraYaw = arCamera.eulerAngles.y;
    }

    // LateUpdate викликається після всіх Update: положення камери в цьому кадрі вже оновлене
    void LateUpdate()
    {
        if (arCamera == null) return;

        // на скільки градусів камера повернулась навколо вертикалі з минулого кадру
        float currentYaw = arCamera.eulerAngles.y;
        float deltaYaw = Mathf.DeltaAngle(lastCameraYaw, currentYaw);
        lastCameraYaw = currentYaw;

        if (!isFollowing) return;

        // Позиція: розміщуємо об'єкт відносно камери (формула з методички)
        transform.position = arCamera.position + arCamera.forward * offset.z
                           + arCamera.right * offset.x
                           + arCamera.up * offset.y;

        // Обертання: повертаємо об'єкт навколо вертикальної осі на той самий кут, що й камеру
        transform.rotation = Quaternion.AngleAxis(deltaYaw, Vector3.up) * transform.rotation;
    }
}