
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Land_UI : MonoBehaviour
{

    public LandManager LandManager;
    public GameObject Timer;
    public Transform CanvesTr;
    private List<GameObject> _timers = new List<GameObject>();
    private List<float> _deltaTime = new List<float>();

    private void Start()
    {
        CreateTimer();
    }
    private void Update()
    {
        for (int nodeIndex = 0; nodeIndex < _deltaTime.Count; nodeIndex++)
        {
            ProgressTime(nodeIndex);
        }
    }
    private GameObject CreateTimerText(Vector3 timerpos, string time)
    {
        GameObject timerText = Instantiate(Timer,timerpos,Quaternion.identity);
        timerText.transform.parent = transform;
        timerText.transform.position = new Vector3(timerpos.x,timerpos.y,transform.position.z);
        timerText.transform.localScale = transform.localScale * 0.6f;
        timerText.GetComponent<TextMeshProUGUI>().text = time;
        return timerText;
    }
    public void CreateTimer()
    {
        ResetTimer();
        for (int timerIndex = 0; timerIndex < LandManager.NodeList.Count; timerIndex++)
        {
            _timers.Add(CreateTimerText(LandManager.NodeList[timerIndex].GetPosition(), LandManager.ConvertString(LandManager.NodeList[timerIndex].GetTimer())));
            float time = LandManager.NodeList[timerIndex].GetTimer();
            _deltaTime.Add(time);
        }
    }
    public void ResetTimer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        _timers.Clear();
        _deltaTime.Clear();
    }
    private void ProgressTime(int nodeIndex)
    {
        if (_deltaTime[nodeIndex] >= 0)
        {
            _deltaTime[nodeIndex] -= Time.deltaTime;
            LandManager.NodeList[nodeIndex].SetTimer((int)_deltaTime[nodeIndex]);
            LandManager.NodesList[nodeIndex / 9].Lands[nodeIndex % 9][1] = (int)_deltaTime[nodeIndex];
            _timers[nodeIndex].GetComponent<TextMeshProUGUI>().text = LandManager.ConvertString((int)_deltaTime[nodeIndex]);
        }
        else if (_deltaTime[nodeIndex] < 0 && LandManager.NodeList[nodeIndex].GetFoodSeq() != 0) 
        {
            LandManager.NodeList[nodeIndex].State = Node.NodeState.Cultivating;
            LandManager.NodeList[nodeIndex].SetNodeTile();
            LandManager.ChangeTile(LandManager.NodeList[nodeIndex]);
            LandManager.NodesList[nodeIndex / 9].Lands[nodeIndex % 9][2] = 1;

        }
       
    }


}
