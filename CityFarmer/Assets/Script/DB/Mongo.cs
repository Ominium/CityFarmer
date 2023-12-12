using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using UnityEngine;

public class Mongo : MonoBehaviour
{
    public static MongoClient client;
    public static IMongoDatabase database;

    public void MongoDBConnection()
    {
        string connectionString = "mongodb+srv://myUser:dnflskfk1@mydb.qgumh09.mongodb.net/MyDB?retryWrites=true&appName=AtlasApp";
        // MongoClient 생성
        client = new MongoClient(connectionString);
        // 데이터베이스 선택
        database = client.GetDatabase("MyDB");
    }

    // 여기에서 쿼리 실행 등 MongoDB 작업 수행
    public List<BsonDocument> LoadMongo(string DB)
    {
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(DB);
        var filter = Builders<BsonDocument>.Filter.Eq("UserSeq", UserInfo.UserSeq);
        List<BsonDocument> result = collection.Find(filter).ToList();
        return result;
    }

    public static void InitMongoInventory(Inventory inventory)
    {
        Debug.Log("Start InitMongoInventory");

        var collection = database.GetCollection<BsonDocument>(inventory.GetType().Name);

        var document = new BsonDocument { { "UserSeq", inventory.UserSeq }, { "ItemSeqs", new BsonArray( inventory.ItemSeqs) },{"FoodSeqs",new BsonArray(inventory.FoodSeqs )},
            {"ItemValues",new BsonArray(inventory.ItemValues) },{"FoodValues",new BsonArray(inventory.FoodValues) }  };
        collection.InsertOneAsync(document);
    }
    public static void InitMongoEncyclopedia(Encyclopedia encyclopedia)
    {
        Debug.Log("Start InitMongoEncyclopedia");

        var collection = database.GetCollection<BsonDocument>(encyclopedia.GetType().Name);

        var document = new BsonDocument { { "UserSeq", encyclopedia.UserSeq }, { "FoodSeqs", new BsonArray(encyclopedia.FoodSeqs) } };
        collection.InsertOneAsync(document);

    }
    public static void InitMongoNodes()
    {
        Debug.Log("Start InitMongoNodes");

        var collection = database.GetCollection<BsonDocument>("Node");
        List<List<int>> Lands = new List<List<int>>();

        for (int nodeIndex = 0; nodeIndex < 9; nodeIndex++)
        {
            List<int> node = new List<int>();
            for (int resetIndex = 0; resetIndex < 3; resetIndex++)
            {
                node.Add(0);
            }
            Lands.Add(node);
        }
        var document = new BsonDocument { { "UserSeq", UserInfo.UserSeq }, { "LandSeq", InfoManager.Instance.UserInfo.UserLandLevel }, { "Lands", new BsonArray(Lands) } };
        collection.InsertOneAsync(document);
    }
    public static void UpdateMongo<T>(Inventory inventory, Encyclopedia encyclopedia, T type)
    {
        string typetext = type.GetType().ToString();
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(typetext);
        var filter = Builders<BsonDocument>.Filter.Eq("UserSeq", UserInfo.UserSeq);
        
        switch (typetext)
        {
            case "Inventory":
                List<int> itemSeqs = new();
                List<int> itemValues = new();
                List<int> foodSeqs = new();
                List<int> foodValues = new();
                for (int i = 0; i < GameManager.InventoryManager.PlayerItemList.Count; i++)
                {
                    itemSeqs.Add(GameManager.InventoryManager.PlayerItemList[i].ItemSeq);
                    itemValues.Add(GameManager.InventoryManager.PlayerItemList[i].ItemValue);
                }
                for (int i = 0; i < GameManager.InventoryManager.PlayerFoodList.Count; i++)
                {
                    foodSeqs.Add(GameManager.InventoryManager.PlayerFoodList[i].FoodSeq);
                    foodValues.Add(GameManager.InventoryManager.PlayerFoodList[i].FoodValue);
                }

                var InventoryItemupdate = Builders<BsonDocument>.Update.Set("ItemSeqs", itemSeqs);
                var InventoryFoodupdate = Builders<BsonDocument>.Update.Set("FoodSeqs", foodSeqs);
                var InventoryItemValueupdate = Builders<BsonDocument>.Update.Set("ItemValues", itemValues);
                var InventoryFoodValueupdate = Builders<BsonDocument>.Update.Set("FoodValues", foodValues);
                // 데이터 저장
                var inventoryItemResult = collection.UpdateOne(filter, InventoryItemupdate);
                var inventoryFoodResult = collection.UpdateOne(filter, InventoryFoodupdate);
                var inventoryItemValueResult = collection.UpdateOne(filter, InventoryItemValueupdate);
                var inventoryFoodValueResult = collection.UpdateOne(filter, InventoryFoodValueupdate); break;
            case "Encyclopedia":
                var EncyclopediaFoodupdate = Builders<BsonDocument>.Update.Set("FoodSeqs", encyclopedia.FoodSeqs);
                var EncyclopediaFoodresult = collection.UpdateOne(filter, EncyclopediaFoodupdate);
                break;
        }

        Debug.Log("Data Update successfully!");
    }
    public static void UpdateMongoNodes(Nodes node)
    {
       
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Node");
        var builder = Builders<BsonDocument>.Filter;
        var filter = builder.Eq("UserSeq", UserInfo.UserSeq) & builder.Eq("LandSeq", node.LandSeq);
        var LandUpdate = Builders<BsonDocument>.Update.Set("Lands", node.Lands);

        // 데이터 저장
        var LandResult = collection.UpdateOne(filter, LandUpdate);


        Debug.Log("Data Update successfully!");
    }

    public BsonDocument JsonToBson(string jsonString)
    {
        // JSON 문자열을 BsonDocument로 변환
        using (JsonReader reader = new JsonReader(jsonString))
        {
            BsonDocument bsonDocument = BsonSerializer.Deserialize<BsonDocument>(reader);
            return bsonDocument;
        }
    }
}
