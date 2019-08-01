using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Register : MonoBehaviour
{
	private TextMeshProUGUI TMPro;
    // Start is called before the first frame update
    void Start()
    {
        TMPro = transform.Find("Text (TMP)").gameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        TMPro.color = new Color32(255, 100, 100, 100);
		print("Working");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
		TMPro.color = new Color32(255, 255, 255, 255);
    }

}
