using UnityEngine;
using System.Collections;

public class WorldFlipper : MonoBehaviour
{
    [Header("Flip Settings")]
    public Transform worldParent;         // Parent object to rotate
    public float flipDuration = 1f;

    [Header("Snow Particle System")]
    public ParticleSystem snowParticle;   // Snow particle system

    public static bool worldHasFlipped = false; // <-- Global flip status

    private bool isFlipping = false;
    private bool isFlipped = false;
    private Collider myTrigger;

    private void Awake()
    {
        myTrigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Entered by: {other.name}");

        if (other.CompareTag("Player") && !isFlipping)
        {
            Debug.Log("Player detected. Starting world flip.");
            StartCoroutine(FlipWorldSmoothly(other));
        }
        else if (!other.CompareTag("Player"))
        {
            Debug.Log("Trigger entered by non-player object.");
        }
        else if (isFlipping)
        {
            Debug.Log("Flip is already in progress.");
        }
    }

    IEnumerator FlipWorldSmoothly(Collider playerCollider)
    {
        isFlipping = true;

        // Disable trigger permanently
        if (myTrigger != null)
        {
            myTrigger.enabled = false;
            Debug.Log("Trigger disabled permanently.");
        }

        // Smooth world rotation
        Quaternion startRotation = worldParent.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(180f, 0f, 0f);

        float elapsed = 0f;
        Debug.Log("Flipping started...");

        while (elapsed < flipDuration)
        {
            worldParent.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / flipDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        worldParent.rotation = targetRotation;
        Debug.Log("World flip complete.");

        // Move snow particle system down by 20 units
        if (snowParticle != null)
        {
            Vector3 currentPos = snowParticle.transform.position;
            snowParticle.transform.position = currentPos + new Vector3(0, -20f, 0);
            Debug.Log("Snow particle system moved down by 20 units.");
        }

        isFlipped = !isFlipped;
        isFlipping = false;

        // ✅ Set global flip flag
        worldHasFlipped = true;
        Debug.Log("Global worldHasFlipped = true");
    }
}
