using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionScreen : MonoBehaviour
{
    [Serializable]
    public class PlaceableItem
    {
        public string title;
        public Sprite icon;
        public GameObject prefab;
    }

    [SerializeField] private GameObject screen;           // панель SelectionScreen
    [SerializeField] private Transform cardsParent;       // Content у Scroll View
    [SerializeField] private ObjectCard cardPrefab;
    [SerializeField] private List<PlaceableItem> items = new List<PlaceableItem>();

    private readonly List<ObjectCard> cards = new List<ObjectCard>();
    private int selectedIndex = -1;

    /// вибраний об'єкт (null, якщо нічого не вибрано)
    public PlaceableItem SelectedItem => selectedIndex >= 0 ? items[selectedIndex] : null;

    /// користувач вибрав інший об'єкт
    public event Action<PlaceableItem> SelectionChanged;

    void Start()
    {
        // створюємо картку для кожного об'єкта зі списку
        for (int i = 0; i < items.Count; i++)
        {
            ObjectCard card = Instantiate(cardPrefab, cardsParent);
            card.Setup(items[i].title, items[i].icon);

            int index = i;
            card.Button.onClick.AddListener(() => OnCardClicked(index));

            cards.Add(card);
        }

        if (items.Count > 0)
            Select(0);
    }

    // викликається кнопкою "Об'єкти"
    public void Open()
    {
        screen.SetActive(true);
    }

    // викликається кнопкою "Назад"
    public void Close()
    {
        screen.SetActive(false);
    }

    void OnCardClicked(int index)
    {
        Select(index);
        Close();
    }

    void Select(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < cards.Count; i++)
            cards[i].SetSelected(i == index);

        SelectionChanged?.Invoke(items[index]);
    }
}