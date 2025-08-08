using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public BossController boss;  // Drag the boss here
    public RawImage hpBar;       // Drag the UI RawImage here
    public float maxWidth = 300f; // Original bar width

    private void Start()
    {
        if (hpBar != null)
            maxWidth = hpBar.rectTransform.sizeDelta.x;
    }

    private void Update()
    {
        if (boss == null || hpBar == null) return;

        // Calculate health ratio
        float healthRatio = Mathf.Clamp01(boss.health / 100f); // Assuming boss max HP = 100
        hpBar.rectTransform.sizeDelta = new Vector2(maxWidth * healthRatio, hpBar.rectTransform.sizeDelta.y);
    }
}
