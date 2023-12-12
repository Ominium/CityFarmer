using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Money_UI : MonoBehaviour
{
    private TextMeshProUGUI _goldText;
    private TextMeshProUGUI _rubyText;
    private Money _money;

    private int _goldIndex = 2;
    private int _rubyIndex = 3;

    private void Awake()
    {   
        InitGoods();
        
    }
    
    private void OnEnable()
    {
        
        UpdateMoney();
    }
    
    public void UpdateMoney()
    {
        
        _goldText.text = _money.moneyGold.ToString();
        _rubyText.text = _money.moneyRuby.ToString();
    }

    private void InitGoods()
    {
        _goldText = transform.GetChild(_goldIndex).GetChild(0).GetComponent<TextMeshProUGUI>();
        _rubyText = transform.GetChild(_rubyIndex).GetChild(0).GetComponent<TextMeshProUGUI>();
        _money = InfoManager.Instance.Money;
    }
}
