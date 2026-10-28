using System.Collections;
using UnityEngine;

/// дзвіночок на прилавку: звук і короткий спалах кольору при активації
[RequireComponent(typeof(AudioSource))]
public class BellRinger : MonoBehaviour
{
    [SerializeField] private AudioClip ringClip;            // звук дзвінка
    [SerializeField] private Renderer bellRenderer;         // модель дзвіночка
    [SerializeField] private Color flashColor = Color.white; // колір спалаху
    [SerializeField] private float flashTime = 0.15f;       // тривалість спалаху, с

    private AudioSource audioSource;
    private Material bellMaterial;
    private Color baseColor;
    private int ringCount;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        bellMaterial = bellRenderer.material;
        baseColor = bellMaterial.color;
    }

    // подія Select Entered — навели промінь і натиснули Grip
    public void Ring()
    {
        audioSource.PlayOneShot(ringClip);
        StopAllCoroutines();
        StartCoroutine(Flash());
        ringCount++;
        Debug.Log($"Дзвіночок: замовлення №{ringCount}");
    }

    private IEnumerator Flash()
    {
        bellMaterial.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        bellMaterial.color = baseColor;
    }
}