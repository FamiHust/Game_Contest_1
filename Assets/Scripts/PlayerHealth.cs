using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxArmor = 50;
    private int currentHealth;
    private int currentArmor;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentArmor = maxArmor;
        UpdateUI();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     TakeDamage(10);
        // }
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

    private void UpdateUI()
    {
        healthText.text = currentHealth.ToString() + "%";
        armorText.text = currentArmor.ToString() + "%";
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }
}
