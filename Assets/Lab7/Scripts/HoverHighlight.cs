using UnityEngine;

/// збільшує об'єкт, коли на нього наведено промінь контролера
public class HoverHighlight : MonoBehaviour
{
    [SerializeField] private float hoverScale = 1.15f; // у скільки разів збільшувати при наведенні

    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale; // запам'ятовуємо початковий розмір
    }

    // подія First Hover Entered — промінь навели
    public void HighlightOn()
    {
        transform.localScale = baseScale * hoverScale;
    }

    // подія Last Hover Exited — промінь прибрали
    public void HighlightOff()
    {
        transform.localScale = baseScale;
    }
}