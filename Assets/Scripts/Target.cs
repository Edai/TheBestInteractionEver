using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : ClickableObject
{
    public int targetType;
    private GameObject controller;
    private SteamVR_TrackedObject tracked;
    private SteamVR_Controller.Device device;

    [SerializeField]
    private Renderer r;

    private GameObject cursor;
    private Camera head;

    private TargetStudy study;

    private bool isHeadClicking;

    private Color c;

	private Vector3[] headpos;
	private Vector3[] headforwards;
    private List<Vector2> cursors_inside;
    private List<Vector2> cursors_overshoot;
    private List<Vector2> cursors_saveinside;
    private List<Vector2> cursors_savebefore;
    private List<Vector2> cursors_before;
    private bool isovershooting;
    private int overshoot_threshold = 6;
    private int overshoot_count = 0;

    private int before_threthold = 50;
    private int before_num = -1;
    private int after_num = -1;
    private int after_count = 0;
	private int record_length = 120;
    private int timer_count = 0;
    private int durationPass = 20;
    private int durationDwell = 90;
    private bool timeout = false;
    private bool counting_after = false;
    private bool isinside = false;
    public string index_controller;

    float alpha_before = 0.2f;
	float beta_before = 22;
    float alpha_after = 0.3f;
    float beta_after = 9;
    float[] aves = { 0.00080164f, 0.00089076f, 0.00097964f, 0.00106336f, 0.00114061f, 0.0012242f, 0.00131345f };
    float[] stds = { 0.00044953f, 0.00047326f, 0.0005009f, 0.00052903f, 0.00055412f, 0.00058841f, 0.00063059f};

    void Start()
    {
        
        study = GameObject.FindObjectOfType<TargetStudy>();
        cursor = GameObject.Find("Cursor");
        head = Camera.main;
        r = GetComponent<Renderer>();
        c = r.material.color;
		isHeadClicking = true;
		if (isHeadClicking == false) {
			Recorder.Instance.Launch ();
		}
        cursors_before = new List<Vector2>();
        cursors_saveinside = new List<Vector2>();
        cursors_inside = new List<Vector2>();
        cursors_overshoot = new List<Vector2>();
        isovershooting = false;
    }

    private void Update()
    {
        //Debug.Log(isovershooting);
        // controller event
        if (controller == null)
        {
            controller = GameObject.Find("Controller (right)");
        }
        else
        {
            tracked = controller.GetComponent<SteamVR_TrackedObject>();
            
            device = SteamVR_Controller.Input((int)tracked.index);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && study.typenow == 1)
            {
                ActionStart();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && study.typenow == 1)
            {
                ActionEnd();
            }
        }
        Vector3 cursor_3d = cursor.transform.position - head.transform.position;
        float radius = 2;
        float phi = Mathf.Atan2(cursor_3d.z, cursor_3d.x);
        float alpha = Mathf.Asin(cursor_3d.y / radius);
        // for recognition
        // record the cursor positions in 2d when it is inside the target
        if (isinside == true)
        {
            cursors_inside.Add(new Vector2(alpha, phi));
        }
        // record the cursor before entering the target, for a fixed amount
        else
        {
            if (cursors_before.Count < before_threthold)
            {
                cursors_before.Add(new Vector2(alpha, phi));
            }
            else
            {
                cursors_before.RemoveAt(0);
                cursors_before.Add(new Vector2(alpha, phi));
            }
            // wait the overshoot to come back to the target in a fixed amount of frames of time
            if (isovershooting == true)
            {
                if(overshoot_count < overshoot_threshold)
                {
                    overshoot_count += 1;
                }
                else
                {
                    overshoot_count = 0;
                    isovershooting = false;
                }
            }
        }




        //timer event
        if (timer_count > 0)
        {
            if(isinside == false)
            {
                timer_count = 0;
                return;
            }
            timer_count--;
            if (timer_count == 0)
            {
                timeout = true;
                r.material.color = Color.black;
            }
        }
    }

    public override void Click()
    {
        transform.parent.gameObject.SetActive(false);
        Recorder.Instance.Stop();
        Recorder.Instance.SaveFile();
        TargetRecorder.Instance.Stop();
        TargetRecorder.Instance.SaveFile();
		
    }

    public void ActionStart()
    {
        if (isHeadClicking == true)
        {
            Recorder.Instance.Launch();
            TargetRecorder.Instance.Launch();
        }
    }

    public void ActionEnd()
    {
        bool islegal = true;
        if (isHeadClicking)
        {
            //islegal = Recorder.Instance.isNod ();
            islegal = true;
        }
        if (islegal)
        {
            transform.parent.gameObject.SetActive(false);
            Recorder.Instance.SaveFile();
            TargetRecorder.Instance.SaveFile();
            
        }
        //Debug.Log(islegal);
        Recorder.Instance.Stop();
        TargetRecorder.Instance.Stop();
    }

	public void OnPress()
	{
        /*
        Debug.Log("Press");
		if (isHeadClicking == true) {
			Recorder.Instance.Launch ();

		}*/			
	}

	public void OnRelease()
	{
        /*
		bool islegal = true;
		if (isHeadClicking) {
            //islegal = Recorder.Instance.isNod ();
            islegal = true;
		}
		if (islegal) {
			Recorder.Instance.SaveFile();
			TargetRecorder.Instance.SaveFile();
			transform.parent.gameObject.SetActive(false);
		}
		Debug.Log (islegal);
		Recorder.Instance.Stop();
	    TargetRecorder.Instance.Stop();
	    */

    }

    bool checkBefore()
    {
        int num_check = aves.Length;
        Vector2[] before = cursors_before.ToArray();
        int start_before = before.Length - before_num;
        if (start_before < 0)
            start_before = 0;
        if (before.Length - start_before - 1 < 1)
            return false;
        float[] speeds = new float[before.Length - start_before-1];
        for(int i = start_before+1;i < before.Length;i++)
        {
            speeds[i - start_before - 1] = (before[i] - before[i - 1]).magnitude;
        }
        for (int i = 4; i < 4+ num_check;i++)
        {
            float lowest_high = 10000;
            for (int j = 0; j < speeds.Length-i;j++)
            {
                float high = 0;
                for(int k = 0;k < i;k++)
                {
                    if (high<speeds[j+k])
                    {
                        high = speeds[j + k];
                    }
                }
                if (high < lowest_high)
                {
                    lowest_high = high;
                }
            }
            if (lowest_high > aves[i-4]+stds[i-4]*3)
            {
                Debug.Log(lowest_high);
                Debug.Log(i);
                return false;
            }
        }
        return true;

    }

    IEnumerator SendContent()
    {
        // for recognition
        Debug.Log("sendcon");
        string content = "";
        Vector2[] inside = cursors_inside.ToArray();
        if (isovershooting == true)
        {
            Debug.Log("overshoot");
            foreach(var cursor in cursors_saveinside)
            {
                content += cursor.x.ToString() + "_" + cursor.y.ToString() + "_";
            }
            foreach(var cursor in cursors_overshoot)
            {
                content += cursor.x.ToString() + "_" + cursor.y.ToString() + "_";
            }
            isovershooting = false;
        }

        for (int i = 0; i < inside.Length; i++)
        {
            content += inside[i].x.ToString() + "_" + inside[i].y.ToString() + "_";
        }
        string url = "http://127.0.0.1/?" + content;
        using (WWW www = new WWW(url))
        {
            yield return www;
            Debug.Log(www.text);
            switch (www.text)
            {
                case "1":
                    r.material.color = Color.red;
                    break;
                case "0":
                    r.material.color = Color.blue;
                    isovershooting = true;
                    break;
                case "2":
                    r.material.color = Color.green;
                    break;
                default:
                    break;

            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Cursor"))
        {
            isinside = true;
            c = r.material.color;
            r.material.color = c / 1.2f + new Color(0, 0, 0, 0.5f);
            TargetRecorder.Instance.Enter();
            if (study.typenow == 2) // dwell task
            {
                timer_count = durationDwell;
            }
            cursors_saveinside.Clear();
            foreach(var cursor in cursors_inside)
            {
                cursors_saveinside.Add(cursor);
            }
            cursors_inside.Clear();
        }
    }

    //IEnumerator OnTriggerExit(Collider col)
    void OnTriggerExit(Collider col)
    {
        Debug.Log(isovershooting);
        r.material.color = c;
        isinside = false;
        TargetRecorder.Instance.Leave();
        //timeout for pass
        if (timeout == true || study.typenow == 3)
        {
            transform.parent.gameObject.SetActive(false);
            Recorder.Instance.SaveFile();
            TargetRecorder.Instance.SaveFile();

            timeout = false;
        }
        int inside_num = cursors_inside.Count;
        before_num = (int)(alpha_before * inside_num + beta_before);
        if (before_num > before_threthold)
        {
            Debug.Log("too long");
            before_num = before_threthold;
        }
        //after_num = (int)(alpha_after * inside_num + beta_after);
        // if isovershooting is true means it has been checked "before" last time.
        if (isovershooting == true || checkBefore() == true)
        {
            StartCoroutine(SendContent());
        }
        else
        {
            r.material.color = Color.blue;
        }
        
        before_num = -1;
        cursors_before.Clear();



    }

    bool checkNod(Vector2[] coors)
    {
        int num = coors.Length;
        float distance = Vector2.Distance(coors[0], coors[num - 1]);
        Vector2 inDirection = coors[num / 2] - coors[0];
        Vector2 outDirection = coors[num / 2] - coors[num - 1];
        float cos = Vector2.Dot(inDirection, outDirection) / inDirection.magnitude / outDirection.magnitude;
        float length = (inDirection.magnitude + outDirection.magnitude) / 2;

        return true;
    }
}
