using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Recorder : Singleton<Recorder>
{
    [SerializeField] private User user;
    [SerializeField] private Camera main;
    private List<string> cameraData;
    private bool recording = false;

    void Start () {
        cameraData = new List<string>(500);
    }

    public void Launch()
    {
        recording = true;
    }
    public void Stop()
    {
        recording = false;
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + user.name + ".csv";
        Debug.Log("File created : " + destination);
        using (StreamWriter writetext = new StreamWriter(destination, false))
        {
            writetext.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                user.name, user.age, user.gender, user.height, user.laterality.ToString(), user.HaveYouEverTriedVR.ToString()).ToLower());
            foreach (var s in cameraData)
                writetext.WriteLine(s);
            writetext.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (recording == true)
        {
            cameraData.Add(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 
                            main.transform.position.x, main.transform.position.y, main.transform.position.z,
                            main.transform.rotation.eulerAngles.x, main.transform.rotation.eulerAngles.y, main.transform.rotation.eulerAngles.z,
                            main.transform.forward.x, main.transform.forward.y, main.transform.forward.z));
        }
    }
}
