using UnityEngine;
using TMPro;
using System.Collections;

public class WorldFlipper : MonoBehaviour
{
    [Header("Flip Settings")]
    public Transform worldParent;         // Parent object to rotate
    public float flipDuration = 1f;

    [Header("Snow Particle System")]
    public ParticleSystem snowParticle;   // Snow particle system

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;      // Panel at the bottom of the screen
    public TextMeshProUGUI dialogueText;  // Text element inside the panel
    public float typingSpeed = 0.05f;     // Seconds per character
    public float messageDisplayTime = 3f; // Time to keep text on screen after typing
    [TextArea(2, 5)]
    public string messageToDisplay;       // Set your custom message in Inspector

    public static bool worldHasFlipped = false;

    private bool isFlipping = false;
    private bool isFlipped = false;
    private Collider myTrigger;

    private void Awake()
    {
        myTrigger = GetComponent<Collider>();
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false); // Hide dialogue at start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFlipping)
        {
            if (myTrigger != null)
                myTrigger.enabled = false;

            StartCoroutine(FlipWorldSmoothly());
        }
    }

    IEnumerator FlipWorldSmoothly()
    {
        isFlipping = true;

        // Smooth world rotation
        Quaternion startRotation = worldParent.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(180f, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < flipDuration)
        {
            worldParent.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / flipDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        worldParent.rotation = targetRotation;

        // Move snow particle system down by 20 units
        if (snowParticle != null)
        {
            Vector3 currentPos = snowParticle.transform.position;
            snowParticle.transform.position = currentPos + new Vector3(0, -20f, 0);
        }

        isFlipped = !isFlipped;
        isFlipping = false;

        // Set global flag
        worldHasFlipped = true;

        // Show custom message after flip
        if (dialoguePanel != null && dialogueText != null && !string.IsNullOrEmpty(messageToDisplay))
        {
            StartCoroutine(ShowTypewriterMessage(messageToDisplay));
        }
    }

    IEnumerator ShowTypewriterMessage(string message)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "";

        // Typewriter effect
        foreach (char letter in message.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait before hiding
        yield return new WaitForSeconds(messageDisplayTime);

        dialoguePanel.SetActive(false);
    }
}
