using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBtn : MonoBehaviour
{
    public Button BtnPause;
    public GameObject PanelPause;

    void Start () {
		Button btn = BtnPause.GetComponent<Button>();
		btn.onClick.AddListener(Interact);
    }

	void Interact()
    {
        PanelPause.SetActive(true);
        Time.timeScale = 0;
	}
}
