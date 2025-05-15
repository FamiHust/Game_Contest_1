using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [SerializeField] private int maxHealth;
    [SerializeField] private int maxArmor;
    [HideInInspector] public int currentHealth;
    private int currentArmor;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;
    private Animator anim;
    private Color originalColor;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = 50;
        currentArmor = maxArmor;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentArmor > 0)
        {
            int remainingDamage = damage - currentArmor;
            currentArmor -= damage;
            if (currentArmor < 0) currentArmor = 0;
            
            if (remainingDamage > 0)
            {
                currentHealth -= remainingDamage;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth < 0) currentHealth = 0;
        
        UpdateUI();
        CheckDeath();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateUI();
    }

    public void AddArmor(int amount)
    {
        currentArmor += amount;

        if (currentArmor > maxArmor)
        {
            currentArmor = maxArmor;
        }
        UpdateUI();
    }

    public void ReduceArmor(int amount)
    {
        currentArmor -= amount;

        if (currentArmor < 0)
        {
            currentArmor = 0;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthText.text = currentHealth.ToString() + "%";
        armorText.text = currentArmor.ToString() + "%";
    }

    private void CheckDeath()
    {
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        anim.SetTrigger("isDie");
        PlayerController.Instance.enabled = false;
        GameManager.instance.GameOver();
        SoundManager.PlaySound(SoundType.GameOver);
    }

    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }
}