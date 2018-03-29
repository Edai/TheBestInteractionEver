using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TargetStudy : Study
{
    public GameObject currentTarget;

    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private int numberTasks;

    [SerializeField]
    private int numberSessions;

    [SerializeField]
    private Text textInstruction;


    private int current_task = 0;
    private int current_session = 0;
    
    private string[] contentFile = null;
    private int indexContent = 0;
    

    // Use this for initialization
    void Start ()
    {
        textInstruction.gameObject.SetActive(true);
        currentTarget = Instantiate(currentTarget);
        if (replayLastRecord == true)
        {
            string path = Application.persistentDataPath + "target_" + User.Instance.name + ".csv";
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string fileContent = (reader.ReadToEnd());
                    contentFile = fileContent.Split('\n');
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
                replayLastRecord = false;
            }
        }
        
    }

    void PlayRecord()
    {
        if (indexContent < contentFile.Length && String.IsNullOrEmpty(contentFile[indexContent]) == false)
        {
            var line = contentFile[indexContent].Split(',');
            currentTarget.gameObject.SetActive(true);
            currentTarget.transform.LookAt(Camera.main.transform);
            currentTarget.transform.position = new Vector3(
                float.Parse(line[0], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[1], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[2], CultureInfo.InvariantCulture.NumberFormat));

            cursor.transform.position = new Vector3(
                float.Parse(line[3], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[4], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[5], CultureInfo.InvariantCulture.NumberFormat));
        
            indexContent++;
        }
        else
        {
            replayLastRecord = false;
            currentTarget.gameObject.SetActive(false);
        }
    }

	// Update is called once per frame
	void Update ()
	{
	    if (replayLastRecord == true)
	    {
	        PlayRecord();
	    }
        //keyEvent();
        if  (replayLastRecord == false && current_task < numberTasks && current_session < numberSessions && currentTarget == null)
        {
            Camera main = Camera.main;
            Vector3 playerPos = main.transform.position;
            Vector3 playerDirection = main.transform.forward;
            Vector3 spawnPos = playerPos + playerDirection * 1.5f;

            var ex = Random.Range(-1.0f, 1f);
	        var ex2 = Random.Range(-1.0f, 1f);
	        Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y + ex, spawnPos.z + ex2);

            currentTarget.gameObject.SetActive(true);
            currentTarget.transform.position = newPos;
            currentTarget.transform.LookAt(main.transform);
            currentTarget.transform.parent = transform;

            current_task++;
            textInstruction.text = "Session: " + current_session.ToString() + " - Task: " + current_task.ToString();
            if (current_task >= numberTasks)
            {
                if(current_session >= numberSessions)
                {
                    current_task = 0;
                    current_session = 0;
                    textInstruction.text = "Done";
                }
                else
                {
                    current_session++;
                    current_task = 0;
                }
            }
            
        }
    }

    //for debug
    void keyEvent()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            currentTarget.gameObject.SetActive(false);
        }
    }
}
