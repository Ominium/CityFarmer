using MongoDB.Bson;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using UnityEngine;



public class InfoManager : MonoBehaviour
{
    public UserInfo UserInfo;
    public Money Money;

    public List<Shop> Shops = new List<Shop>();
    public List<Item> Items = new List<Item>();
    public List<Food> Foods = new List<Food>();
    public string MoneyUpdateQuery = "";
    public string MoneyInsertQuery = "";
    public string UserUpdateQuery = "";
    private static InfoManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static InfoManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(InfoManager)) as InfoManager;

                if (_instance == null)
                {
                    Debug.Log("no Singleton obj and create Info Manager");
                    //Init();
                }
            }
            return _instance;
        }
    }
    private static void Init()
    {
        GameObject gameObject = new GameObject("InfoManager");
        _instance = gameObject.AddComponent<InfoManager>();
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }
    public string MoneyUpdateString()
    {
        MoneyUpdateQuery = "UPDATE MONEY SET MONEY_GOLD = '" + Money.moneyGold + "', MONEY_RUBY = '" + Money.moneyRuby + "' WHERE USER_SEQ = '" + UserInfo.UserSeq + "';";
        return MoneyUpdateQuery;
    }
    public string MoneyInsertString()
    {
        MoneyInsertQuery = "INSERT INTO MONEY ( USER_SEQ, MONEY_GOLD,MONEY_RUBY )VALUES('" + UserInfo.UserSeq + "',0,0)";
        return MoneyInsertQuery;
    }
    public string UserUpdateString()
    {
        UserUpdateQuery = "UPDATE USER SET USER_LANDLEVEL = '" + UserInfo.UserLandLevel + "', USER_LEVEL = '" + UserInfo.UserLevel + "', USER_EXP = '" + UserInfo.UserExp + "' WHERE USER_SEQ ='" + UserInfo.UserSeq + "';";
        return UserUpdateQuery;
    }
    public void UpdateSQL(string query)
    {
        StartSQL();

        Maria.OnInsertOrUpdateRequest(query);
        Maria.SqlConnection.Close();
    }
    public void InsertSQL(string query)
    {
        StartSQL();

        Maria.OnInsertOrUpdateRequest(query);
        Maria.SqlConnection.Close();
    }
    public void LoadMoney()
    {
        StartSQL();
        string query = "SELECT * FROM MONEY WHERE USER_SEQ = '" + UserInfo.UserSeq + "'";
        DataSet dataSet = Maria.OnSelectRequest(query, "MONEY");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());
        if (xmlDocument != null)
        {
            XmlNodeList data = xmlDocument.SelectNodes("NewDataSet/MONEY");


            foreach (XmlNode node in data)
            {
                Money.moneyGold = System.Convert.ToInt32(node.SelectSingleNode("MONEY_GOLD").InnerText);
                Money.moneyRuby = System.Convert.ToInt32(node.SelectSingleNode("MONEY_RUBY").InnerText);
            }
        }
        Maria.SqlConnection.Close();
    }
    public T FindBySeq<T>(List<T> TList, int Seq)
    {
        // LINQ를 사용하여 itemSeq와 일치하는 Item 찾기
        T found = TList.FirstOrDefault(t => TypeSeq(t) == Seq);
        return found;
    }
    public int TypeSeq<T>(T t)
    {

        string type = t.ToJson().Split("Name")[0];
        string typeSeq = "";

        for (int i = 0; i < type.Length; i++)
        {
            char ch = type[i];
            if ('0' <= ch && ch <= '9')
            {
                typeSeq += ch;
            }
        }
        return int.Parse(typeSeq);
    }

    public void LoadShop()
    {
        StartSQL();
        Shops.Clear();
        XmlDocument xmlDocument = Instance.OnLoadData("SHOP");

        if (xmlDocument != null)
        {
            XmlNodeList data = xmlDocument.SelectNodes("NewDataSet/SHOP");

            foreach (XmlNode node in data)
            {
                Shop shop = new Shop();
                shop.ShopSeq = System.Convert.ToInt32(node.SelectSingleNode("SHOP_SEQ").InnerText);
                shop.ShopName = node.SelectSingleNode("SHOP_NAME").InnerText;
                shop.ShopPrice = System.Convert.ToInt32(node.SelectSingleNode("SHOP_PRICE").InnerText);
                shop.ShopLevel = System.Convert.ToInt32(node.SelectSingleNode("SHOP_LEVEL").InnerText);
                shop.ShopSpriteString = node.SelectSingleNode("SHOP_SPRITE").InnerText;
                shop.ShopSprite = shop.shopSprite();
                shop.ShopValue = System.Convert.ToInt32(node.SelectSingleNode("SHOP_VALUE").InnerText);
                shop.ItemSeq = System.Convert.ToInt32(node.SelectSingleNode("ITEM_SEQ").InnerText);
                string type = node.SelectSingleNode("SHOP_TYPE").InnerText;
                switch (type)
                {
                    case "Land": shop.shopType = Shop.ShopType.Land; break;
                    case "Money": shop.shopType = Shop.ShopType.Money; break;
                    case "Item": shop.shopType = Shop.ShopType.Item; break;
                    case "Other": shop.shopType = Shop.ShopType.Other; break;
                }
                Shops.Add(shop);
            }
        }
        Maria.SqlConnection.Close();
    }
    public XmlDocument OnLoadData(string DB)
    {
        StartSQL();
        string query = "SELECT * FROM " + DB;
        DataSet dataSet = Maria.OnSelectRequest(query, DB);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());
        return xmlDocument;
    }
    public void StartSQL()
    {
        try
        {
            Maria.SqlConnection = new MySqlConnection(Maria.strConnection);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    public void LoadFood()
    {
        Foods.Clear();
        XmlDocument xmlDocument = OnLoadData("FOOD");

        if (xmlDocument != null)
        {
            XmlNodeList data = xmlDocument.SelectNodes("NewDataSet/FOOD");
            int foodCount = 0;

            foreach (XmlNode node in data)
            {
                foodCount++;
                Food food = new Food();
                food.FoodSeq = System.Convert.ToInt32(node.SelectSingleNode("FOOD_SEQ").InnerText);
                food.FoodName = node.SelectSingleNode("FOOD_NAME").InnerText;
                food.FoodText = node.SelectSingleNode("FOOD_TEXT").InnerText;
                food.FoodPrice = System.Convert.ToInt32(node.SelectSingleNode("FOOD_PRICE").InnerText);

                string time = node.SelectSingleNode("FOOD_TIME").InnerText;
                food.FoodTime = time.Split("T")[1];
                food.FoodLevel = System.Convert.ToInt32(node.SelectSingleNode("FOOD_LEVEL").InnerText);
                food.FoodSpriteString = node.SelectSingleNode("FOOD_SPRITE").InnerText;
                food.FoodSprite = food.foodSprite();

                string type = node.SelectSingleNode("FOOD_TYPE").InnerText;
                switch (type)
                {
                    case "Plant": food.foodtype = Food.Foodtype.Plant; break;
                    case "Meat": food.foodtype = Food.Foodtype.Meat; break;
                }
                Foods.Add(food);
            }
        }
        Maria.SqlConnection.Close();
    }

    public void LoadItem()
    {
        Items.Clear();
        XmlDocument xmlDocument = OnLoadData("ITEM");

        if (xmlDocument != null)
        {
            XmlNodeList data = xmlDocument.SelectNodes("NewDataSet/ITEM");
            int itemCount = 0;

            foreach (XmlNode node in data)
            {
                itemCount++;
                Item item = new Item();
                item.ItemSeq = System.Convert.ToInt32(node.SelectSingleNode("ITEM_SEQ").InnerText);
                item.ItemName = node.SelectSingleNode("ITEM_NAME").InnerText;
                item.ItemText = node.SelectSingleNode("ITEM_TEXT").InnerText;
                item.ItemPrice = System.Convert.ToInt32(node.SelectSingleNode("ITEM_PRICE").InnerText);
                item.ItemSpriteString = node.SelectSingleNode("ITEM_SPRITE").InnerText;
                item.ItemSprite = item.itemSprite();

                string type = node.SelectSingleNode("ITEM_TYPE").InnerText;

                switch (type)
                {
                    case "disposable": item.itemType = Item.ItemType.Disposable; break;
                    case "costume": item.itemType = Item.ItemType.Costume; break;

                }

                item.ItemMoney = System.Convert.ToBoolean(node.SelectSingleNode("ITEM_MONEY").InnerText);
                Items.Add(item);
            }
        }
        Maria.SqlConnection.Close();
    }

    public bool Login(string InfoUserId, string InfoUserPassword)
    {
        StartSQL();
        UserInfo.UserSeq = 0;
        //로그인 쿼리
        string query = "SELECT * FROM USER WHERE USER_ID = '" + InfoUserId + "' AND USER_PASSWORD = '" + InfoUserPassword + "'";
        DataSet dataSet = Maria.OnSelectRequest(query, "USER");

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        if (xmlDocument != null)
        {
            XmlNodeList user = xmlDocument.SelectNodes("NewDataSet/USER");

            foreach (XmlNode node in user)
            {

                UserInfo.UserSeq = System.Convert.ToInt32(node.SelectSingleNode("USER_SEQ").InnerText);
                UserInfo.UserLevel = System.Convert.ToInt32(node.SelectSingleNode("USER_LEVEL").InnerText);
                UserInfo.UserLandLevel = System.Convert.ToInt32(node.SelectSingleNode("USER_LANDLEVEL").InnerText);
                UserInfo.UserId = node.SelectSingleNode("USER_ID").InnerText;
                UserInfo.UserName = node.SelectSingleNode("USER_NAME").InnerText;
                UserInfo.UserReg = node.SelectSingleNode("USER_REG").InnerText;
            }
        }
        Maria.SqlConnection.Close();
        if (UserInfo.UserSeq == 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public void SignUp(string InfoUserId, string InfoUserPassword, string InfoUserName)
    {
        StartSQL();
        string query = "SELECT * FROM USER WHERE USER_ID = '" + InfoUserId + "'";
        DataSet dataSet = Maria.OnSelectRequest(query, "USER");

        if (dataSet.GetXml().IndexOf("SEQ") >= 0)
        {
            Debug.Log("이미 가입된 아이디입니다.");
        }
        else
        {
            //회원가입 쿼리
            query = "INSERT INTO USER ( USER_ID, USER_LEVEL, USER_NAME, USER_PASSWORD )VALUES('" + InfoUserId + "', '1', '" + InfoUserName + "','" + InfoUserPassword + "')";
            if (Maria.OnInsertOrUpdateRequest(query))
            {
                Debug.Log("회원가입에 성공하였습니다.");
            }
            else
            {
                Debug.Log("회원가입에 실패하였습니다.");
            }
        }
    }
}
