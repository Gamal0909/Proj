using System;
using UnityEngine;
using TMPro;
public class MoneyManager : MonoBehaviour
{
    
    [SerializeField]private int currentMoney = 0;
    [SerializeField] private TMP_Text moneyText; // Link to UI text
    public static MoneyManager Instance;

    public int CurrentMoney
    {
        get
        {
            return currentMoney;
        }
        set
        {
            currentMoney = value;
        }
    }
    private void Awake()
    { 
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
   
    public void AddMoney(int amount) 
    { 
        currentMoney += amount; 
        UpdateUI();
    }

    public void LossMoney(int amount)
    {
        if (currentMoney - amount >= 0) currentMoney -= amount;
        else Debug.LogWarning("currentMoney will be negative :(");
        UpdateUI();
    }

    void UpdateUI()
    {
        moneyText.text = currentMoney.ToString();
    }
}
