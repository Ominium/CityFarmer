using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Inventory _inventory;
    private Mongo _mongoDB;

    public List<Item> PlayerItemList { get; set; }
    public List<Food> PlayerFoodList { get; set; }

    public void Init()
    {
        GameObject _gameObject = InfoManager.Instance.gameObject;
        _inventory = _gameObject.GetComponent<Inventory>();
        _mongoDB = _gameObject.GetComponent<Mongo>();
        _mongoDB.MongoDBConnection();
        LoadInventory();
    }

    public void LoadInventory()
    {
        Debug.Log("Load Inventory Start");
        _inventory.UserSeq = UserInfo.UserSeq;

        if (_mongoDB.LoadMongo("Inventory").Count > 0)
        {
            BsonDocument bson = _mongoDB.LoadMongo("Inventory")[0];
            Inventory inventory = BsonSerializer.Deserialize<Inventory>(bson);
            _inventory.UserSeq = inventory.UserSeq;
            _inventory.FoodSeqs = inventory.FoodSeqs;
            _inventory.ItemSeqs = inventory.ItemSeqs;
            _inventory.FoodValues = inventory.FoodValues;
            _inventory.ItemValues = inventory.ItemValues;
            List<Item> items = InfoManager.Instance.Items;
            List<Food> foods = InfoManager.Instance.Foods;

            PlayerItemList = new List<Item>();
            PlayerFoodList= new List<Food>();

            for (int inventoryIndex = 0; inventoryIndex < _inventory.ItemSeqs.Count; inventoryIndex++)
            {
                Item item = InfoManager.Instance.FindBySeq(items, _inventory.ItemSeqs[inventoryIndex]);
                
                item.ItemValue = _inventory.ItemValues[inventoryIndex];
                PlayerItemList.Add(item);

            }

            for (int inventoryIndex = 0; inventoryIndex < _inventory.FoodSeqs.Count; inventoryIndex++)
            {
                Food food = InfoManager.Instance.FindBySeq(foods, _inventory.FoodSeqs[inventoryIndex]);

                food.FoodValue = _inventory.FoodValues[inventoryIndex];
                PlayerFoodList.Add(food);
            }
        }
        else
        {
            Mongo.InitMongoInventory(_inventory);
        }

    }
    public void SaveInventory()
    {
        Mongo.UpdateMongo<Inventory>(_inventory, null, _inventory);
    }
}
