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
    public float rotationSpeed = 90f;

    [Header("End Game UI")]
    public GameObject endGameCanvas; // Assign in Inspector
    public Text endGameText; // Assign the Text UI element here

    [TextArea(3, 10)]
    public string victoryMessage = "Congratulations! You've fixed the flipping abnormalities of the Axis Flip. The distorted realms are stabilizing, and balance is restored... for now.";

    private bool isActive = false;
    private Vector3 initialRotation;
    private Coroutine typewriterCoroutine; // Track the coroutine

    public void ActivateBoss(Transform playerTransform)
    {
        player = playerTransform;
        isActive = true;
        initialRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if (!isActive || player == null) return;

        Vector3 moveDirection = (player.position - transform.position).normalized;
        moveDirection.y = 0;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

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
        isActive = false; // Stop boss movement

        if (endGameCanvas != null)
        {
            endGameCanvas.SetActive(true);

            // Check if endGameText is assigned
            if (endGameText != null)
            {
                // Start coroutine and destroy after it completes
                StartCoroutine(TypewriterEffectThenDestroy());
            }
            else
            {
                Debug.LogError("endGameText is not assigned in the Inspector!");
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogError("endGameCanvas is not assigned in the Inspector!");
            Destroy(gameObject);
        }
    }

    private IEnumerator TypewriterEffectThenDestroy()
    {
        if (endGameText == null)
        {
            Debug.LogError("endGameText is null!");
            Destroy(gameObject);
            yield break;
        }

        endGameText.text = ""; // Clear the text

        foreach (char letter in victoryMessage)
        {
            if (endGameText == null) // Check if still valid
            {
                Destroy(gameObject);
                yield break;
            }

            endGameText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        // Only destroy after typewriter effect is complete
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Stop the coroutine if the object is destroyed
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
    }
}