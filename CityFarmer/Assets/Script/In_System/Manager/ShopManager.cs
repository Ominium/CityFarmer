using UnityEngine;

public class ShopManager : MonoBehaviour
{

    private InventoryManager _inventory;
    public LandManager LandManager;
    private Money_UI _money_UI;

    private void Awake()
    {
        _inventory = GameManager.InventoryManager;
        _money_UI = transform.Find("GoodsPopUp").GetComponent<Money_UI>();
    }
    private void OnEnable()
    {
        LandManager.OnNodePopUp = false;
    }
    private void OnDisable()
    {
        LandManager.OnNodePopUp = true;
    }
    public void ClickBuyButton(int shopSeq)
    {
        Shop shop = InfoManager.Instance.FindBySeq(InfoManager.Instance.Shops, shopSeq);
        Debug.Log(shop.ShopSeq);
        
        if (shop.ShopMoney)
        {
            if(InfoManager.Instance.Money.moneyRuby>= shop.ShopPrice)
            {
                ShopTypeCheck(shop);
            }
            else
            {
                Debug.Log("금액이 부족합니다.");
            }
        }
        else
        {
            if (InfoManager.Instance.Money.moneyGold >= shop.ShopPrice)
            {
                ShopTypeCheck(shop);
            }
            else
            {
                Debug.Log("금액이 부족합니다.");
            }
        }
        _money_UI.UpdateMoney();

    }
    private void ShopTypeCheck(Shop shop)
    {
        switch (shop.shopType)
        {
            case Shop.ShopType.Land: BuyLand(shop); break;
            case Shop.ShopType.Item: BuyItem(shop); break;
            case Shop.ShopType.Money: BuyMoney(shop); break;
            case Shop.ShopType.Other: BuyOther(shop); break;
        }
    }
    public void SellFood(int foodSeq, int value)
    {
        Food food = _inventory.PlayerFoodList.Find(x => x.FoodSeq == foodSeq);
        int foodIndex = _inventory.PlayerFoodList.FindIndex(x => x.FoodSeq == foodSeq);
        for (int i = 0; i < value; i++)
        {
            InfoManager.Instance.Money.moneyGold += food.FoodPrice;

        }
        if(food.FoodValue > value)
        {
            food.FoodValue -= value;
            _inventory.PlayerFoodList[foodIndex] = food;
        }
        else
        {
            _inventory.PlayerFoodList.RemoveAt(foodIndex);
        }

        _inventory.SaveInventory();
        InfoManager.Instance.UpdateSQL(InfoManager.Instance.MoneyUpdateString());
        InfoManager.Instance.UpdateSQL(InfoManager.Instance.UserUpdateString());
        _money_UI.UpdateMoney();
    }
    public void BuyItem(Shop shop)
    {
        UseMoney(shop);
        Item item = _inventory.PlayerItemList.Find(x => x.ItemSeq == shop.ItemSeq);
        int itemIndex = _inventory.PlayerItemList.FindIndex(x => x.ItemSeq == shop.ItemSeq);
        if (item == null)
        {
            item = InfoManager.Instance.FindBySeq(InfoManager.Instance.Items, shop.ItemSeq);
            item.ItemValue = shop.ShopValue;
        }
        else
        {
            item.ItemValue += shop.ShopValue;
            _inventory.PlayerItemList.RemoveAt(itemIndex);
        }
        _inventory.PlayerItemList.Add(item);
        _inventory.SaveInventory();

    }
    public void BuyMoney(Shop shop)
    {
        UseMoney(shop);
        if (!shop.ShopMoney)
        {
            InfoManager.Instance.Money.moneyRuby += shop.ShopValue;
        }
        else
        {
            InfoManager.Instance.Money.moneyGold += shop.ShopValue;
        }

        InfoManager.Instance.UpdateSQL(InfoManager.Instance.MoneyUpdateString());
    }
    public void BuyLand(Shop shop)
    {
        UseMoney(shop);
        InfoManager.Instance.UserInfo.UserLandLevel++;
        Mongo.InitMongoNodes();
        InfoManager.Instance.UpdateSQL(InfoManager.Instance.MoneyUpdateString());
        InfoManager.Instance.UpdateSQL(InfoManager.Instance.UserUpdateString());
        LandManager.Init();
        LandManager.LoadLand();
    }
    public void UseMoney(Shop shop)
    {
        if (shop.ShopMoney)
        {
            InfoManager.Instance.Money.moneyRuby -= shop.ShopPrice;
        }
        else
        {
            InfoManager.Instance.Money.moneyGold -= shop.ShopPrice;
        }

        InfoManager.Instance.UpdateSQL(InfoManager.Instance.MoneyUpdateString());
    }


    public void BuyOther(Shop shop)
    {
        UseMoney(shop);
    }
}
