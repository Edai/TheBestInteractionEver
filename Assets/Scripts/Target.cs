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

    private TargetStudy study;

    private bool isHeadClicking;

    private Color c;

	private Vector3[] headpos;
	private Vector3[] headforwards;
	private int record_length = 120;
    private int timer_count = 0;
    private int durationPass = 20;
    private int durationDwell = 90;
    private bool timeout = false;
    
    void Start()
    {
        
        study = GameObject.FindObjectOfType<TargetStudy>();
        r = GetComponent<Renderer>();
        c = r.material.color;
		isHeadClicking = true;
		if (isHeadClicking == false) {
			Recorder.Instance.Launch ();
		}
    }

    private void Update()
    {
        // controller event
        if (controller == null)
        {
            controller = GameObject.Find("Controller (left)");
        }
        else
        {
            tracked = controller.GetComponent<SteamVR_TrackedObject>();
            device = SteamVR_Controller.Input((int)tracked.index);
            if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && study.typenow == 1)
            {
                ActionStart();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && study.typenow == 1)
            {
                ActionEnd();
            }
        }

        //timer event
        if(timer_count > 0)
        {
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
        Recorder.Instance.Stop();
        Recorder.Instance.SaveFile();
        TargetRecorder.Instance.Stop();
        TargetRecorder.Instance.SaveFile();
		transform.parent.gameObject.SetActive(false);
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
            Recorder.Instance.SaveFile();
            TargetRecorder.Instance.SaveFile();
            transform.parent.gameObject.SetActive(false);
        }
        Debug.Log(islegal);
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

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Cursor"))
        {
            c = r.material.color;
            r.material.color = c / 1.2f + new Color(0, 0, 0, 0.5f);
            TargetRecorder.Instance.Enter();
            if (study.typenow == 3) // pass task
            {
                timer_count = durationPass;
            }
            else if (study.typenow == 2) // dwell task
            {
                timer_count = durationDwell;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        r.material.color = c;
        TargetRecorder.Instance.Leave();
        //timeout for pass and dwell
        if (timeout == true)
        {
            Recorder.Instance.SaveFile();
            TargetRecorder.Instance.SaveFile();
            transform.parent.gameObject.SetActive(false);
            timeout = false;
        }
    }
}
