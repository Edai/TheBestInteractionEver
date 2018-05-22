using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TargetRecorder : Singleton<TargetRecorder> {
    
    [SerializeField] private GameObject cursor;
    [SerializeField] private TargetStudy study;
    private Camera head;
    private List<string> data;
    private bool recording = false;
    private bool isinside = false;

    void Start()
    {
        data = new List<string>();
        head = Camera.main;
    }

    public void Launch()
    {
        recording = true;
    }
    public void Enter()
    {
        isinside = true;
    }
    public void Stop()
    {
        recording = false;
        isinside = false;
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "target_" + User.Instance.name + ".csv";
        Debug.Log("File created : " + destination);
        using (StreamWriter writetext = new StreamWriter(destination, true))
        {
            foreach (var s in data)
                writetext.WriteLine(s);
            writetext.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* absolute position
                data.Add(String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                study.currentTarget.transform.position.x, study.currentTarget.transform.position.y, study.currentTarget.transform.position.z,
                cursor.transform.position.x, cursor.transform.position.y, cursor.transform.position.z));
                */
        // relative position
        Vector3 targetPositionRelative = study.currentTarget.transform.position - head.transform.position;
        Vector3 cursorPositionRelative = cursor.transform.position - head.transform.position;

        data.Add(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                targetPositionRelative.x, targetPositionRelative.y, targetPositionRelative.z,
                cursorPositionRelative.x, cursorPositionRelative.y, cursorPositionRelative.z, Convert.ToInt32(recording), Convert.ToInt32(isinside)));

            

    }
}
