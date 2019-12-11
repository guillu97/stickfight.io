using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MouseHover: MonoBehaviour
{
    //private TextMeshProUGUI tmpObj;

    //tmpObj.color = new Color32(50, 50, 50, 255);
    void Start(){
	    GetComponent<TextMeshPro>().color = Color.black;
    }

    void OnMouseEnter(){
        GetComponent<TextMeshPro>().color = Color.red;
    }

    void OnMouseExit() {
        GetComponent<TextMeshPro>().color = Color.black;
    }
}
