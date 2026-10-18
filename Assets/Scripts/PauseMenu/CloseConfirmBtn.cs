using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseConfirmBtn : MonoBehaviour
{
    public Button BtnQuit;

    void Start () {
		Button btn = BtnQuit.GetComponent<Button>();
		btn.onClick.AddListener(Interact);
    }

	void Interact()
    {
            Application.Quit();
	}
}
