using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// візуальний індикатор зони телепорту: пульсує в спокої,
/// світиться зеленим під променем, спалахує в момент телепортації
[RequireComponent(typeof(TeleportationArea))]
public class TeleportPadVisual : MonoBehaviour
{
    [SerializeField] private Renderer padRenderer;                              // диск зони
    [SerializeField] private Color idleColor = new Color(0f, 0.8f, 1f, 0.45f); // блакитний — зона доступна
    [SerializeField] private Color hoverColor = new Color(0.2f, 1f, 0.4f, 0.7f); // зелений — сюди цілимось
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseMin = 0.3f;
    [SerializeField] private float pulseMax = 1.2f;
    [SerializeField] private float hoverEmission = 2.5f; // яскравість під променем
    [SerializeField] private float hoverScale = 1.1f;    // збільшення диска під променем
    [SerializeField] private float flashTime = 0.3f;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionId = Shader.PropertyToID("_EmissionColor");

    private TeleportationArea area;
    private Material padMaterial;
    private Vector3 baseScale;
    private bool hovered;
    private float flashTimer;

    private void Awake()
    {
        area = GetComponent<TeleportationArea>();
        if (padRenderer == null) padRenderer = GetComponent<Renderer>();
        padMaterial = padRenderer.material;      // окрема копія матеріалу для цієї зони
        padMaterial.EnableKeyword("_EMISSION");
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        area.firstHoverEntered.AddListener(OnHoverEntered);
        area.lastHoverExited.AddListener(OnHoverExited);
        area.teleporting.AddListener(OnTeleporting);
    }

    private void OnDisable()
    {
        area.firstHoverEntered.RemoveListener(OnHoverEntered);
        area.lastHoverExited.RemoveListener(OnHoverExited);
        area.teleporting.RemoveListener(OnTeleporting);
    }

    private void Update()
    {
        Color color = hovered ? hoverColor : idleColor;

        // у спокої — плавна пульсація, під променем — рівне яскраве світіння
        float glow = hovered
            ? hoverEmission
            : Mathf.Lerp(pulseMin, pulseMax, (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f);

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            glow += 3f; // короткий спалах після телепорту
        }

        padMaterial.SetColor(BaseColorId, color);
        padMaterial.SetColor(EmissionId, new Color(color.r, color.g, color.b) * glow);
    }

    // промінь телепорту навели на зону
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        hovered = true;
        transform.localScale = new Vector3(baseScale.x * hoverScale, baseScale.y, baseScale.z * hoverScale);
    }

    // промінь прибрали з зони
    private void OnHoverExited(HoverExitEventArgs args)
    {
        hovered = false;
        transform.localScale = baseScale;
    }

    // гравця щойно перенесло в цю зону
    private void OnTeleporting(TeleportingEventArgs args)
    {
        flashTimer = flashTime;
        Debug.Log($"Телепорт у зону: {name}");
    }
}