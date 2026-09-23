using UnityEngine;

public class ColorOnCollision : MonoBehaviour
{
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        rend.material.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
        Debug.Log(gameObject.name + " зіткнувся з " + collision.gameObject.name);
    }
}