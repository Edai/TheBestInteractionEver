using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : ClickableObject
{
    [SerializeField]

    public int targetType;
    private Renderer r;

    private TargetStudy study;

    private bool isHeadClicking;

    private Color c;

	private Vector3[] headpos;
	private Vector3[] headforwards;
	private int record_length = 120;
    
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

    public override void Click()
    {
        Recorder.Instance.Stop();
        Recorder.Instance.SaveFile();
        TargetRecorder.Instance.Stop();
        TargetRecorder.Instance.SaveFile();
		transform.parent.gameObject.SetActive(false);
    }

	public void OnPress()
	{

		if (isHeadClicking == true) {
			Recorder.Instance.Launch ();

		}
			
	}

	public void OnRelease()
	{
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
	 

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Cursor"))
        {
            c = r.material.color;
            //r.material.color = new Color32(0xFF, 0x0, 0x0, 0xFF);
            r.material.color = c / 1.2f + new Color(0, 0, 0, 0.5f);
            TargetRecorder.Instance.Enter();
        }
    }

    void OnTriggerExit(Collider col)
    {
        r.material.color = c;
    }
}
