using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] RectTransform healthBar;

    public float health, maxHealth, width, height;

    public void SetHaxHealth(float pMaxHealth)
    {
        maxHealth = pMaxHealth;
    }

    public void SetHealth(float playerHealth)
    {
        health = playerHealth;
        float newWidth = (health / maxHealth) * width;

        healthBar.sizeDelta = new Vector2(newWidth, height);
    }
}
