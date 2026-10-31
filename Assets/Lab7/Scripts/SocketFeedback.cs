using UnityEngine;

/// реакція таці-сокета: підсвічується зеленим і грає звук, коли на неї поставили каву
[RequireComponent(typeof(AudioSource))]
public class SocketFeedback : MonoBehaviour
{
    [SerializeField] private Renderer trayRenderer;           // модель таці
    [SerializeField] private AudioClip placedClip;            // звук розміщення
    [SerializeField] private Color placedColor = new Color(0.3f, 0.8f, 0.4f); // зелений — предмет на місці

    private AudioSource audioSource;
    private Material trayMaterial;
    private Color baseColor;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        trayMaterial = trayRenderer.material;
        baseColor = trayMaterial.color;
    }

    // подія Select Entered сокета — предмет поставили на тацю
    public void OnPlaced()
    {
        trayMaterial.color = placedColor;
        audioSource.PlayOneShot(placedClip);
        Debug.Log("Каву поставлено на тацю");
    }

    // подія Select Exited сокета — предмет забрали
    public void OnRemoved()
    {
        trayMaterial.color = baseColor;
        Debug.Log("Каву забрали з таці");
    }
}