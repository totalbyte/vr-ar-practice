using UnityEngine;
using UnityEngine.UI;

/// Кнопки керування слідуванням: увімкнути/вимкнути режим і змінити offset
public class FollowModeToggle : MonoBehaviour
{
    [SerializeField] private ARObjectSpawner spawner;
    [SerializeField] private Text buttonLabel;
    [SerializeField] private Text offsetLabel;

    // варіанти зміщення відносно камери: x — вправо, y — вгору, z — вперед (у метрах)
    [SerializeField]
    private Vector3[] offsets =
    {
        new Vector3(0f, -0.1f, 0.4f),     // трохи нижче центру
        new Vector3(0.15f, 0f, 0.4f),     // праворуч
        new Vector3(0f, -0.2f, 0.6f)      // нижче і далі
    };

    private int offsetIndex;
    private CameraFollower follower;    // компонент слідування на розміщеному об'єкті

    void Start()
    {
        UpdateLabels();
    }

    // викликається кнопкою "Слідувати" (On Click в інспекторі)
    public void ToggleFollow()
    {
        if (!FindFollower()) return;
        follower.isFollowing = !follower.isFollowing;   // перемикаємо режим
        UpdateLabels();
    }

    // викликається кнопкою "Offset" (On Click в інспекторі)
    public void NextOffset()
    {
        if (!FindFollower()) return;
        offsetIndex = (offsetIndex + 1) % offsets.Length;
        follower.offset = offsets[offsetIndex];         // новий offset одразу використовується у формулі
        UpdateLabels();
    }

    bool FindFollower()
    {
        if (spawner.SpawnedObject == null) return false; // об'єкт ще не розміщено
        follower = spawner.SpawnedObject.GetComponent<CameraFollower>();
        return follower != null;
    }

    void UpdateLabels()
    {
        bool following = follower != null && follower.isFollowing;
        buttonLabel.text = following ? "Закріпити" : "Слідувати";
        offsetLabel.text = "Offset " + offsets[offsetIndex].ToString("F2");
    }
}