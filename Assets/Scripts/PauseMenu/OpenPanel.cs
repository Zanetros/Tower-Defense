using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanel : MonoBehaviour
{
    public Button BtnReturn;
    public GameObject PanelObj;

    void Start () {
		Button btn = BtnReturn.GetComponent<Button>();
		btn.onClick.AddListener(Interact);
    }

	void Interact()
    {
            PanelObj.SetActive(true);
	}
}
