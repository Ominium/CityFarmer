using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemBuyPanel : MonoBehaviour
{
    private Shop_UI _shop_UI;
    private ShopData _shopData;
    private void Start()
    {
        Transform parentTr = transform.parent;
        _shop_UI = parentTr.GetComponent<Shop_UI>();
        _shopData = parentTr.GetComponent<ShopData>();
        _shop_UI.CreateItemButton(_shopData.ItemShop,transform);
    }

    
}
