using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float interval = 2f;   // інтервал між появами, сек
    public float areaSize = 4f;   // розмір зони появи
    public float lifetime = 10f;  // через скільки секунд видалити копію

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval && prefab != null)
        {
            timer = 0f;
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-areaSize, areaSize), 0f, Random.Range(-areaSize, areaSize));
            GameObject obj = Instantiate(prefab, pos, Random.rotation);
            Destroy(obj, lifetime);
        }
    }
}