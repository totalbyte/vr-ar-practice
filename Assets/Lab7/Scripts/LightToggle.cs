using UnityEngine;

/// вмикає/вимикає лампу при дистанційній активації променем
public class LightToggle : MonoBehaviour
{
    [SerializeField] private Light targetLight;        // джерело світла лампи
    [SerializeField] private Renderer shadeRenderer;   // плафон (світиться, коли лампа увімкнена)
    [SerializeField] private Color onColor = new Color(1f, 0.85f, 0.5f);   // теплий жовтий
    [SerializeField] private Color offColor = new Color(0.25f, 0.25f, 0.25f); // сірий
    [SerializeField] private float emissionIntensity = 2f;

    private Material shadeMaterial;

    private void Awake()
    {
        shadeMaterial = shadeRenderer.material;  // окрема копія матеріалу для цього плафона
        shadeMaterial.EnableKeyword("_EMISSION"); // вмикаємо світіння матеріалу
        ApplyVisual();
    }

    // подія Select Entered — навели промінь і натиснули Grip
    public void Toggle()
    {
        targetLight.enabled = !targetLight.enabled;
        ApplyVisual();
        Debug.Log($"Лампа {(targetLight.enabled ? "увімкнена" : "вимкнена")}");
    }

    // плафон світиться, коли лампа увімкнена
    private void ApplyVisual()
    {
        bool on = targetLight.enabled;
        shadeMaterial.color = on ? onColor : offColor;
        shadeMaterial.SetColor("_EmissionColor", on ? onColor * emissionIntensity : Color.black);
    }
}