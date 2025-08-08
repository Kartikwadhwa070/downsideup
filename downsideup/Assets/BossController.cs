using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    public float moveSpeed = 3f;
    public float health = 100f;
    public Transform player;
    public float rotationSpeed = 90f; // degrees per second for X-axis rotation

    private bool isActive = false;
    private Vector3 initialRotation;

    public void ActivateBoss(Transform playerTransform)
    {
        player = playerTransform;
        isActive = true;
        initialRotation = transform.eulerAngles; // store starting rotation as euler angles
    }

    private void Update()
    {
        if (!isActive || player == null) return;

        // Move directly toward player without rotating
        Vector3 moveDirection = (player.position - transform.position).normalized;
        moveDirection.y = 0; // no vertical movement
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Rotate only on X-axis while keeping Y and Z from initial rotation
        float xRotation = initialRotation.x + (Mathf.Sin(Time.time * rotationSpeed * Mathf.Deg2Rad) * 30f);
        transform.rotation = Quaternion.Euler(xRotation, initialRotation.y, initialRotation.z);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Boss Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss Defeated!");
        Destroy(gameObject);
    }
}