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
	public bool isNod(Vector3[] hposition, Vector3[] hforward)
	{
		int state = 0;
		//get length
		int lenp = hposition.Length;
		/*int lenf = hforward.Length;
		if (lenp <= lenf)
			lenf = lenp;
		else
			lenp = lenf;//choose the less one
		*/

		//at least five points
		if (lenp < 15)
			return false;

		//get x,y
		Vector3 origin = hposition [0];
		Vector3 axe_y = new Vector3(0,1,0);
		Vector3 axe_x = new Vector3(hforward[0].z,0,0-hforward[0].x);
		axe_x = axe_x.normalized;

		//new 2D array
		Vector3 cur3D = new Vector3(0,0,0);
		Vector3 offset = new Vector3(0,0,0);
		Vector2[] cur2D = new Vector2[lenp];

		// turn to 2D
		for (int i = 0; i < lenp; i++)
		{
			cur3D = hposition [i] + 20 * hforward [i];
			offset = cur3D - origin;
			cur2D[i].x = Vector3.Dot (offset, axe_x);
			cur2D[i].y = Vector3.Dot (offset, axe_y);
		}

		Vector2 start = cur2D [0];
		Vector2 end = cur2D [lenp - 1];
		int indexcos = -1;
		float leastcos = 1;

		//get lower y
		float y_low = 0;
		if(start.y > end.y)
			y_low = end.y;
		else
			y_low = start.y;
		
		// get least cos
		for (int i = 1; i < lenp - 1; i++) 
		{
			Vector2 come = (cur2D [i] - start);
			come /= come.magnitude;
			Vector2 go = (end - cur2D [i]);
			go /= go.magnitude;
			float cos = Vector2.Dot (come, go);
			if (cos < leastcos) {
				leastcos = cos;
				indexcos = i;
			}
		}
		
		//check cos
		if(leastcos < -0.2)// && cur2D[indexcos].y < y_low)
			state++;
		else
			return false;

		//check speet "slow to fast"
		float[] speed = new float[10];
		float[] acceler = new float[9];
		Vector2 speed_each = new Vector2(0,0);
		for (int k = indexcos - 5; k < intdexcos + 5; k++ )
		{
			speed_each = cur2D[k]-cur2D[k+1];
			speed[k-indexcos+5] = speed_each.magnitude;
		}
		for(int t = 0; t <= 8; t++ )
		{
			acceler[t] = speed[t+1]-speed[t]; 
		}
		acc_bf = (acceler[0]+acceler[1]+acceler[2]+acceler[3])/4;
		acc_af = (acceler[5]+acceler[6]+acceler[7]+acceler[8])/4;
		if(acc_bf < 0 && acc_af > 0)
			state++;
		else 
			return false;
		
		//final check
		if(state == 2)
		{
			//state = 0;
			return true;
		}
		else
			return false;

	}
    // Update is called once per frame
    void Update () {
		RaycastHit hit;

       if (StudyManager.Instance.CurrentStudy.replayLastRecord == false && 
           Physics.Raycast(head.transform.position, head.transform.forward, out hit, 20.0f))
		{
			if (hit.collider && hit.collider.name.Contains ("Cursor") == false) {
				transform.LookAt (head.transform);
				transform.RotateAround (transform.position, transform.up,90);
				transform.position = hit.point;
			}
        }
    }
}
