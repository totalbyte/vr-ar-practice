using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private PlacementIndicator indicator;
    [SerializeField] private ObjectSelectionScreen selectionScreen;
    [SerializeField] private Button placeButton;
    [SerializeField] private Text selectedLabel;

    void OnEnable()
    {
        // підписуємось на подію вибору об'єкта з екрана вибору
        selectionScreen.SelectionChanged += OnSelectionChanged;
    }

    void OnDisable()
    {
        selectionScreen.SelectionChanged -= OnSelectionChanged;
    }

    void Update()
    {
        // кнопка активна лише тоді коли під індикатором є площина і вибрано об'єкт
        placeButton.interactable = indicator.IsValid && selectionScreen.SelectedItem != null;
    }

    // викликається кнопкою "Розмістити"
    public void PlaceSelected()
    {
        ObjectSelectionScreen.PlaceableItem item = selectionScreen.SelectedItem;
        if (item == null || !indicator.IsValid) return;

        // створюємо об'єкт у позиції та з орієнтацією допоміжного елемента
        Pose pose = indicator.PlacementPose;
        Instantiate(item.prefab, pose.position, pose.rotation);
    }

    void OnSelectionChanged(ObjectSelectionScreen.PlaceableItem item)
    {
        selectedLabel.text = "Вибрано: " + item.title;
    }
}