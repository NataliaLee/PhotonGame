using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetection : MonoBehaviour {	
	
	void Update () {
		DetectClick ();
	}

	void DetectClick(){
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				hit.transform.SendMessage("TryHit",SendMessageOptions.DontRequireReceiver);
			}
		}
	}

    void DetectTap()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    hit.transform.SendMessage("TryHit");
                }
            }
        }
    }
}
