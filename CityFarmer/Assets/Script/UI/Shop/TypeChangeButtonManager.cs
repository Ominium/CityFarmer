using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TypeChangeButtonManager : MonoBehaviour
{

    public GameObject[] panels;
    private void Start()
    {
        Button[] changeButton = new Button[3];
        for(int childIndex =0; childIndex < changeButton.Length; childIndex++)
        {
            int ramdaIdx = childIndex;
            changeButton[childIndex] = transform.GetChild(childIndex).GetComponent<Button>();
            changeButton[childIndex].onClick.AddListener(() => OnChangeButton(ramdaIdx, panels,changeButton));
        }   
    }
    private void OnChangeButton(int childIndex,GameObject[] panels,Button[] button)
    {
        for(int panelIndex = 0; panelIndex< panels.Length; panelIndex++)
        {
            panels[panelIndex].SetActive(false);
            ChangeColor(button[panelIndex], Color.white);
        }
        panels[childIndex].SetActive(true);
        ChangeColor(button[childIndex], Color.red);

    }
    //이후 추가 작성시 따로 cs를 만들어서 관리
    private void ChangeColor(Button button, Color color)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.selectedColor = color;
        colorBlock.normalColor = color;
        button.colors = colorBlock;
    }
}
