using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBtn : MonoBehaviour
{
    public Button BtnPause;
    public bool pause = false;
    public Sprite img1;
    public Sprite img2;
    // Start is called before the first frame update
    void Pause()
    {

    }
    void Start () {
		Button btn = BtnPause.GetComponent<Button>();
		btn.onClick.AddListener(Interact);
    }
    // Update is called once per frame
    void Update()
    {
        if(pause == false)
        {
            BtnPause.GetComponent<Image>().sprite = img1;
        }
        else
        {
            BtnPause.GetComponent<Image>().sprite = img2;
        }
      
    }
	void Interact()
    {
        if (pause == true)
        {
            pause = false;
            Time.timeScale = 1;
        }
        else
        {
            pause = true;
            Time.timeScale = 0;
        }
	}
}
