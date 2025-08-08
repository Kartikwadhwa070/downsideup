using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTriggerLoader : MonoBehaviour
{
    [Header("UI")]
    public GameObject eButtonUI;

    [Header("Scene")]
    public string sceneToLoad;

    private bool isPlayerInZone = false;

    void Start()
    {
        if (eButtonUI != null)
            eButtonUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInZone && WorldFlipper.worldHasFlipped && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed. Loading scene...");
            LoadNextScene();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!WorldFlipper.worldHasFlipped)
            {
                Debug.Log("Player entered scene trigger, but world hasn't flipped yet.");
                return;
            }

            isPlayerInZone = true;
            if (eButtonUI != null)
                eButtonUI.SetActive(true);

            Debug.Log("Player entered scene trigger zone. Press E to continue.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (eButtonUI != null)
                eButtonUI.SetActive(false);
            Debug.Log("Player exited scene trigger zone.");
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex + 1);
        }
    }
}
