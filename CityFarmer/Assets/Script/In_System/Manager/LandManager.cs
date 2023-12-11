using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandManager : MonoBehaviour
{
    public Tilemap Tilemap;
    public Grid grid;
    public List<Vector3Int> Vector3MinPostion = new();
    public List<Vector3Int> Vector3MaxPostion = new();
    public Dictionary<Vector3Int, int> Vector3Node = new Dictionary<Vector3Int, int>();
    public List<Node> NodeList;
  
    public List<Nodes> NodesList = new();
    public Nodes ClickNodes;
    private Mongo _mongoDB;
    public int LandSeq;
    public GameObject _nodePopUp;
    public bool OnNodePopUp = true;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        GameObject _gameObject = InfoManager.Instance.gameObject;

        _mongoDB = _gameObject.GetComponent<Mongo>();
        _mongoDB.MongoDBConnection();
        NodesList.Clear();
        Vector3MinPostion.Clear();
        Vector3MaxPostion.Clear();
        for (int i = 0; i < _mongoDB.LoadMongo("Node").Count; i++)
        {
            BsonDocument bson = _mongoDB.LoadMongo("Node")[i];
            Nodes nodes = BsonSerializer.Deserialize<Nodes>(bson);
            NodesList.Add(nodes);
        }
        // 재배 가능 영역 불러오기
        for (int i = 0; i < 8; i++)
        {
            Vector3Int minPosition = new(-8 + (i * 4), -3, 0);
            Vector3Int maxPosition = new(-6 + (i * 4), -1, 0);
            Vector3MinPostion.Add(minPosition);
            Vector3MaxPostion.Add(maxPosition);
        }

    }
    private void Start()
    {
        LoadLand();

    }
    private void Update()
    {
        // 이후 리팩토링 과정을 거쳐 업데이트문에서 삭제
        if (OnNodePopUp)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3Int mousePos = GetMousePosition();
                if (Vector3Node.ContainsKey(mousePos))
                {
                    LandSeq = Vector3Node[mousePos];
                    _nodePopUp.SetActive(true);
                    OnNodePopUp = false;

                }

            }
        }
       
    }
    public Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
    public void LoadLand()
    {
        NodeList.Clear();
        Vector3Node.Clear();
        for (int currentland = 0; currentland < NodesList.Count; currentland++)
        {
            coordinate(currentland);
        }
    }
    public void SaveLand()
    {
       
        for (int nodeIndex = 0; nodeIndex < NodesList.Count; nodeIndex++)
        {
            Mongo.UpdateMongoNodes(NodesList[nodeIndex]);
        }

    }
    private void coordinate(int land)
    {
     
        
       Vector3Int minPosition = Vector3MinPostion[land]; // 특정 영역의 최소 좌표
        Vector3Int maxPosition = Vector3MaxPostion[land]; // 특정 영역의 최대 좌표
        List<Vector3Int> vector3s = new List<Vector3Int>();
        for (int x = minPosition.x; x <= maxPosition.x; x++)
        {
            for (int y = minPosition.y; y <= maxPosition.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                vector3s.Add(tilePosition);
            }
        }
        for (int tileCount = 0; tileCount< vector3s.Count; tileCount++)
        {
            Node node = new Node(vector3s[tileCount], NodesList[land].Lands[tileCount][0], NodesList[land].Lands[tileCount][1]);
            node.State = (Node.NodeState)NodesList[land].Lands[tileCount][2];
            node.SetNodeTile();
            NodeList.Add(node);
            Vector3Node.Add(node.GetPosition(), land);
            ChangeTile(node); // 타일 변경
        }
    }
    public void ChangeTile(Node node)
    {
        Tilemap.SetTile(node.GetPosition(), node.GetStateNodeTile());
    }
    public int ConvertTimer(string inputTime)
    {
        // "PT3H40M20S" 형식의 문자열을 TimeSpan으로 파싱
        TimeSpan timeSpan = XmlConvert.ToTimeSpan(inputTime);

        // 시간 형식 "3:40:20"으로 변환
        int timer = (int)timeSpan.TotalHours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;

        return timer;
    }
    public string ConvertString(int inputTime)
    {
        string timer = "";
        int h, m, s;
        h = inputTime / 3600;
        m = inputTime / 60 %60;
        s = inputTime % 60;
        timer += h.ToString()+":";
        timer += m.ToString()+ ":";
        timer += s.ToString();
        return timer;
    }
    public string ConvertTimeString(string inputTime)
    {
        TimeSpan timeSpan = XmlConvert.ToTimeSpan(inputTime);
        string formattedTime = $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
        return formattedTime;
    }
   

}
