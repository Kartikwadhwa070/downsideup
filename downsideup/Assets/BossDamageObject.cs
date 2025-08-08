using UnityEngine;

public class BossDamageObject : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageToBoss = 10f;

    [Header("Hover Settings")]
    public float floatSpeed = 2f;      // How fast it bobs
    public float floatHeight = 0.3f;   // How high it moves up/down

    [Header("Rotation Settings")]
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f); // Degrees per second

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Bob up and down
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // Rotate constantly
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossController boss = FindObjectOfType<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(damageToBoss);
            }
        }
    }
}
