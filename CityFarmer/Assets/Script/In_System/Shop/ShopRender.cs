using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRender : MonoBehaviour
{
    public Transform CanvasTr;
    private void OnMouseDown()
    {
        Transform parentPopUp = CanvasTr.transform.Find(transform.parent.name + "PopUp");
        Transform mainUIPopUp = CanvasTr.transform.Find("Farm_MainUI");
        Debug.Log(transform.parent.name);
        parentPopUp.gameObject.SetActive(true);
        transform.parent.parent.gameObject.SetActive(false);
        mainUIPopUp.gameObject.SetActive(false);
        
    }

}
