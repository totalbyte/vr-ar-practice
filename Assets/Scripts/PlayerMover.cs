using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    public float moveForce = 10f;
    public float jumpImpulse = 5f;
    private Rigidbody rb;

    void Start() { rb = GetComponent<Rigidbody>(); }

    void FixedUpdate()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        Vector3 dir = Vector3.zero;
        if (kb.wKey.isPressed) dir += Vector3.forward;
        if (kb.sKey.isPressed) dir += Vector3.back;
        if (kb.aKey.isPressed) dir += Vector3.left;
        if (kb.dKey.isPressed) dir += Vector3.right;
        rb.AddForce(dir * moveForce);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
    }
}