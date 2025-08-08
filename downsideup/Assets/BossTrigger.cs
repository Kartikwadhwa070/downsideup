using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject bossObject; // Drag your inactive boss here
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Activate boss
            bossObject.SetActive(true);

            // Tell boss to start chasing
            BossController bossController = bossObject.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.ActivateBoss(other.transform);
            }

            // Optional: disable trigger so it doesn't run again
            gameObject.SetActive(false);
        }
    }
}
