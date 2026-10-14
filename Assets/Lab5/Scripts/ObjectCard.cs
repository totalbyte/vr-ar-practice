using UnityEngine;
using UnityEngine.UI;

/// картка одного об'єкта на екрані вибору (іконка + назва)
public class ObjectCard : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private Text label;
    [SerializeField] private Button button;
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.12f);       // звичайна картка
    [SerializeField] private Color selectedColor = new Color(0.25f, 0.75f, 0.35f, 0.85f); // вибрана — зелена

    public Button Button => button;

    // заповнюємо картку даними об'єкта
    public void Setup(string title, Sprite sprite)
    {
        label.text = title;
        icon.sprite = sprite;
    }

    // підсвічування вибраної картки
    public void SetSelected(bool selected)
    {
        background.color = selected ? selectedColor : normalColor;
    }
}