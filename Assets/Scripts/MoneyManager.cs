using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public int CurrentMoney { get; private set; }
    public UnityEvent<int> OnMoneyChanged;
    public TextMeshProUGUI CurrentMoneyText;

    private const string MONEY_KEY = "PLAYER_MONEY";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadMoney();
    }

    private void Update() 
    {
        CurrentMoneyText.text = CurrentMoney.ToString();
    }

    private void LoadMoney()
    {
        CurrentMoney = PlayerPrefs.GetInt(MONEY_KEY, 0);
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(MONEY_KEY, CurrentMoney);
        PlayerPrefs.Save();
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        SaveMoney();
        OnMoneyChanged?.Invoke(CurrentMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            SaveMoney();
            OnMoneyChanged?.Invoke(CurrentMoney);
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }
}
