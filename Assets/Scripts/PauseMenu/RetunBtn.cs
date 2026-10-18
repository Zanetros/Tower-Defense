using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetunBtn : MonoBehaviour
{
    public Button BtnReturn;
    public GameObject PanelQuit;

    void Start () {
		Button btn = BtnReturn.GetComponent<Button>();
		btn.onClick.AddListener(Interact);
    }

	void Interact()
    {
            PanelQuit.SetActive(false);
	}
}
