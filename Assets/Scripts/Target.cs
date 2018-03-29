using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : ClickableObject
{
    [SerializeField]
    private Renderer r;

    private TargetStudy study;

    private bool isHeadClicking;

    private Color c;
    
    void Start()
    {
        study = GameObject.FindObjectOfType<TargetStudy>();
        c = r.material.color;
		isHeadClicking = false;
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
        gameObject.SetActive(false);
    }

	public void OnPress()
	{

		if (isHeadClicking == true) {
			Recorder.Instance.Launch ();
		}
			
	}

	public void OnRelease()
	{

		Recorder.Instance.Stop();
		Recorder.Instance.SaveFile();
	    TargetRecorder.Instance.Stop();
	    TargetRecorder.Instance.SaveFile();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Cursor"))
        {
            r.material.color = new Color32(0xFF, 0x0, 0x0, 0xFF);
            TargetRecorder.Instance.Launch();
        }
    }

    void OnTriggerExit(Collider col)
    {
        r.material.color = c;
    }
}
