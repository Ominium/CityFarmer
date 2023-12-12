using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class Shop_UI : MonoBehaviour
{
    public ShopManager ShopManager;
    public GameObject ShopButtonPrefab;
    private int _height = 0;
    private int _weight = 0;
    public void CreateItemButton(List<Shop> shops, Transform parent)
    {
        for(int shopIndex = 0; shopIndex <shops.Count; shopIndex++)
        {
            GameObject button = Instantiate(ShopButtonPrefab);
            button.transform.SetParent(parent.transform, false);

            RectTransform buttonPosition = button.GetComponent<RectTransform>();
            if((shopIndex + 1) % 2 == 0)
            {
                _height = 1;
            }else 
            {
                _height = 0;
            }
            if ((shopIndex + 1) % 3 == 0)
            {
                _weight++;
            }
            buttonPosition.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (300 * _height), 200);
            buttonPosition.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, (250 * _weight), 200);
            
            Shop shop = shops[shopIndex];
            int shopSeq = shop.ShopSeq;
            button.transform.GetChild(1).GetComponent<Image>().sprite = shop.ShopSprite;
            button.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = shop.ShopName;
            button.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = shop.ShopPrice.ToString();
            button.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = shop.ShopMoneySprite();
            button.GetComponent<Button>().onClick.AddListener(() => ShopManager.ClickBuyButton(shopSeq));
        }
       
    }

}
