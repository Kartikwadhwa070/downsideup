using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    public float moveSpeed = 3f;
    public float health = 100f;
    public float maxHealth = 100f;
    public Transform player;
    public float rotationSpeed = 90f; // degrees per second for X-axis rotation

    [Header("UI On Death")]
    public Canvas victoryCanvas;         // Assign in Inspector
    public Text victoryText;              // Assign in Inspector
    [TextArea] public string message = "You have defeated the boss!";

    private bool isActive = false;
    private Vector3 initialRotation;

    public void ActivateBoss(Transform playerTransform)
    {
        player = playerTransform;
        isActive = true;
        initialRotation = transform.eulerAngles; // store starting rotation
    }

    private void Update()
    {
        if (!isActive || player == null) return;

        // Move toward player
        Vector3 moveDirection = (player.position - transform.position).normalized;
        moveDirection.y = 0;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Rotate only in X axis
        float xRotation = initialRotation.x + (Mathf.Sin(Time.time * rotationSpeed * Mathf.Deg2Rad) * 30f);
        transform.rotation = Quaternion.Euler(xRotation, initialRotation.y, initialRotation.z);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Boss Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss Defeated!");
        isActive = false;

        // Show victory UI
        if (victoryCanvas != null)
        {
            victoryCanvas.gameObject.SetActive(true);
            if (victoryText != null)
            {
                victoryText.text = "";
                StartCoroutine(TypeText(message));
            }
        }

        Destroy(gameObject);
    }

    private IEnumerator TypeText(string textToType)
    {
        foreach (char letter in textToType)
        {
            victoryText.text += letter;
            yield return new WaitForSeconds(0.05f); // typing speed
        }
    }
}
