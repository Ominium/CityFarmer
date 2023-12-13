using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class Node_UI : MonoBehaviour
{

    public  LandManager LandManager;
    private List<Transform> _transforms;
    private float[] _deltaTime = new float[9];


    private void OnEnable()
    {

        _transforms = new List<Transform>();
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            _transforms.Add(transform.GetChild(childIndex));
        }

        ShowButton(LandManager.LandSeq);
    }
    public void ShowButton(int LandSeq)
    {

        for (int nodeIndex = 0; nodeIndex < 9; nodeIndex++)
        {
            Node node = LandManager.NodeList[LandSeq * 9 + nodeIndex];
            int foodSeq = node.GetFoodSeq();
            _deltaTime[nodeIndex] = node.GetTimer();
            Tile tile = node.GetStateNodeTile();
            Debug.Log(_transforms[nodeIndex].name);
            _transforms[nodeIndex].GetComponent<Image>().sprite = tile.sprite;

        }

    }
    private void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (_deltaTime[i] > 0)
            {
                _deltaTime[i] -= Time.deltaTime;
            }

            _transforms[i].GetComponentInChildren<TextMeshProUGUI>().text = LandManager.ConvertString((int)_deltaTime[i]);
        }

    }

    //TODO : 버튼 이닛 진우 작업
}
