using UnityEngine;
using UnityEngine.UI;

/// Кнопка: вмикає / вимикає режим слідування об'єкта за камерою
public class FollowModeToggle : MonoBehaviour
{
    [SerializeField] private ARObjectSpawner spawner;   // звідки беремо розміщений об'єкт
    [SerializeField] private Button toggleButton;
    [SerializeField] private Text buttonLabel;

    private CameraFollower follower;                    // компонент слідування на розміщеному об'єкті

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleFollow); // обробник натискання
        UpdateLabel();
    }

    public void ToggleFollow()
    {
        if (spawner.SpawnedObject == null) return;      // об'єкт ще не розміщено

        follower = spawner.SpawnedObject.GetComponent<CameraFollower>();
        if (follower == null) return;

        follower.isFollowing = !follower.isFollowing;   // перемикаємо режим
        UpdateLabel();
    }

    void UpdateLabel()
    {
        bool following = follower != null && follower.isFollowing;
        buttonLabel.text = following ? "Закріпити" : "Слідувати";
    }
}