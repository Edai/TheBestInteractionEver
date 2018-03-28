using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CurvedVRKeyboard;

public class CursorMovement : MonoBehaviour {

	[SerializeField]
	GameObject head;
    
	// Use this for initialization
	void Start () {
	
	}

	/*
    void OnTriggerStay(Collider col)
    {
        ClickableObject o = col.gameObject.GetComponent<ClickableObject>();
        if (Input.GetKeyDown("space"))
        {
            if (o != null)
                o.Click();
        }
    }
    */

	void OnTriggerStay(Collider col)
	{
		Target o = col.gameObject.GetComponent<Target>();
		if (Input.GetKeyDown("space"))
		{

			if (o != null)
				o.OnPress();
		}
		if (Input.GetKeyUp ("space")) {

			if (o != null)
				o.OnRelease();
		}

	}

    // Update is called once per frame
    void Update () {
		RaycastHit hit;

		if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, 20.0f))
		{
			if (hit.collider && hit.collider.name.Contains ("Cursor") == false) {
				transform.LookAt (head.transform);
				transform.RotateAround (transform.position, transform.up,90);
				transform.position = hit.point;
			}
        }
    }
}
