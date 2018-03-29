using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyManager : Singleton<StudyManager>
{
    [SerializeField]
    private List<Study> studies;

    [SerializeField]
    private Study currentStudy;

    public Study CurrentStudy
    {
        get { return currentStudy; }
    }

    // Use this for initialization
    void Start ()
    {
        currentStudy.gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}