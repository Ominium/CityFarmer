using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FoodSellPanel : MonoBehaviour
{
    private Inventory_UI _inventory_UI;
    public GameObject SellPanel;
    private Slider _foodValueSlider;
    private TextMeshProUGUI _foodValue;
    private Button _sellButton;
    private Image _foodImage;
    private int _currentValue;
    private int _maxValue;

    private void OnEnable()
    {
        Init();
        SellPanelInit();
    }
    private void Init()
    {
        _inventory_UI = transform.parent.parent.Find("InventoryPopUp").GetComponent<Inventory_UI>();
        _inventory_UI.ShowButtonFood(transform);

    }
    private void SellPanelInit()
    {
        _foodValue = SellPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _sellButton = SellPanel.transform.GetChild(1).GetComponent<Button>();
        _foodValueSlider = SellPanel.transform.GetChild(2).GetComponent<Slider>();
        _foodImage = SellPanel.transform.GetChild(3).GetComponent<Image>();
        if(transform.childCount <= GameManager.InventoryManager.PlayerFoodList.Count)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int foodIndex = i;
                Food food = GameManager.InventoryManager.PlayerFoodList[foodIndex];
                transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => OnFoodInfo(food));
            }
        }
        else
        {
            for (int i = 0; i < GameManager.InventoryManager.PlayerFoodList.Count; i++)
            {
                int foodIndex = i;
                Food food = GameManager.InventoryManager.PlayerFoodList[foodIndex];
                transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => OnFoodInfo(food));
            }
        }
       
    }
    private void OnFoodInfo(Food food)
    {
        SellPanel.SetActive(true);
        _currentValue = 1;
        _maxValue = food.FoodValue;
        FoodText();
        _foodImage.sprite = food.foodSprite();
        _foodValueSlider.maxValue = _maxValue;
        _foodValueSlider.minValue = 1;
        _foodValueSlider.value = 1;
        Debug.Log(food.FoodSeq);
        _sellButton.onClick.RemoveAllListeners();
        _sellButton.onClick.AddListener(() => SellFoodButton(food.FoodSeq, _currentValue));
       
        _foodValueSlider.onValueChanged.AddListener(delegate { CurrentValue(); });
    }
    private void CurrentValue()
    {
        _currentValue = (int)_foodValueSlider.value;
        FoodText();

    }
    private void FoodText()
    {
        _foodValue.text = _currentValue + "/" + _maxValue;
    }
    private void SellFoodButton(int foodSeq, int currentValue)
    {

        ShopManager shopManager = transform.parent.GetComponent<ShopManager>();
        shopManager.SellFood(foodSeq, currentValue);
        SellPanel.SetActive(false);
        Init();

    }
}
