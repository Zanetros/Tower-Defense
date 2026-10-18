using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueBtn : MonoBehaviour
{
    public Button BtnContinue;
    public GameObject PanelPause;

    void Start () {
		Button btn = BtnContinue.GetComponent<Button>();
		btn.onClick.AddListener(Interact);
    }

	void Interact()
    {
            PanelPause.SetActive(false);
            Time.timeScale = 1;
	}
}
