using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f); // градусів за секунду
    public Space space = Space.Self; // Self = Local, World = Global

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, space);
    }
}