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
    public GameObject nextTarget;
    

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
    private int[] order_task;
    private int[] size_session;

    private Vector3 offset_Target_Head;
    private Vector3 offset_Next_Head;
    private Vector3 target_size_basic = new Vector3(0.12f, 0.02f, 0.12f);
    private Vector2[] target_positions;
    private int num_positions = 36; // number of different positions in the space, depends on the size of the space
    private enum targetType:int
    {
        Click = 1,
        Pass = 2,
        Dwell = 3
    };

    public int typenow;
    public int sizenow;

    public void moveNext() //to inform the position of the next target
    {
        if (current_session == numberSessions - 1 && current_task == numberTasks - 1)
            return;
        Debug.Log(nextTarget.name);
        nextTarget.transform.localScale = new Vector3(0.1f, 0.1f, 0.01f);
        setNextPosition(nextTarget, current_session * numberTasks + current_task + 1);
        
    }
    


    void setTaskParameter()
    {
        StreamReader orderfile = new StreamReader("python/order.txt");
        string orders = orderfile.ReadLine();
        order_task = new int[numberSessions * numberTasks];

        string[] order_string = orders.Split(' ');
        for(int i = 0;i < numberSessions*numberTasks;i++)
        {
            order_task[i] = Convert.ToInt32(order_string[i]);
        }

        size_session = new int[numberSessions];
        string[] size_string = orderfile.ReadLine().Split(' ');
        for (int i = 0; i< numberSessions;i++)
        {
            size_session[i] = Convert.ToInt32(size_string[i]);
        }

        target_positions = new Vector2[num_positions];
        int index_pos = Random.Range(1, 20);
        StreamReader posfile = new StreamReader("python/position" + index_pos.ToString() + ".txt");
        string[] xs = posfile.ReadLine().Split(' ');
        string[] ys = posfile.ReadLine().Split(' ');
        for(int i = 0; i< num_positions;i++)
        {
            target_positions[i].x = Convert.ToSingle(xs[i]);
            target_positions[i].y = Convert.ToSingle(ys[i]);
        }

    }

    void setTargetType()
    {
        typenow = order_task[current_session * numberTasks + current_task];
        //typenow = Random.Range(1, 4);
        switch (typenow){
            case (int)targetType.Click:
                currentTarget.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case (int)targetType.Dwell:
                currentTarget.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case (int)targetType.Pass:
                currentTarget.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.blue;
                break;
            default:
                Debug.Log("give type error");
                break;

        }
    }

    void setNextPosition(GameObject target, int indexnow)
    {
        Camera main = Camera.main;
        Vector3 playerPos = main.transform.position;
        //float phi = Random.Range(-30, 30);
        //float alpha = Random.Range(-15, 45);
        float phi = (target_positions[indexnow % num_positions].x + target_positions[(indexnow-1) % num_positions].x);
        float alpha = (target_positions[indexnow % num_positions].y + target_positions[(indexnow - 1) % num_positions].y);

        phi = phi / 180 * Mathf.PI;
        alpha = alpha / 180 * Mathf.PI;
        float radius = 2;
        Vector3 newPos = playerPos + new Vector3(radius * Mathf.Cos(alpha) * Mathf.Cos(phi), radius * Mathf.Sin(alpha), radius * Mathf.Cos(alpha) * Mathf.Sin(phi));
        offset_Next_Head = newPos - playerPos;

        target.gameObject.SetActive(true);
        target.transform.position = newPos;
        target.transform.LookAt(main.transform,Vector3.left);
        target.transform.parent = transform;
    }

    void setTargetPosition(GameObject target, int indexnow)
    {
        Camera main = Camera.main;
        Vector3 playerPos = main.transform.position;
        //float phi = Random.Range(-30, 30);
        //float alpha = Random.Range(-15, 45);
        float phi = target_positions[indexnow % num_positions].x;
        float alpha = target_positions[indexnow % num_positions].y;
        phi = phi / 180 * Mathf.PI;
        alpha = alpha / 180 * Mathf.PI;
        float radius = 2;
        Vector3 newPos = playerPos + new Vector3(radius * Mathf.Cos(alpha) * Mathf.Cos(phi), radius * Mathf.Sin(alpha), radius * Mathf.Cos(alpha) * Mathf.Sin(phi));
        offset_Target_Head = newPos - playerPos;

        target.gameObject.SetActive(true);
        target.transform.position = newPos;
        target.transform.LookAt(main.transform);
        target.transform.parent = transform;
    }

    // Use this for initialization
    void Start ()
    {
        textInstruction.gameObject.SetActive(true);
        currentTarget = Instantiate(currentTarget);

        //read task order and session size
        setTaskParameter();

        // initiate the position of the target
        setTargetPosition(currentTarget,0);

        // initiate the type of the target
        setTargetType();

        //target size
        currentTarget.transform.GetChild(0).transform.localScale = target_size_basic * (0.5f+ size_session[current_session]*0.5f);
        sizenow = size_session[current_session];

        //next pos
        offset_Next_Head = new Vector3(1000, 1000, 1000);

        if (replayLastRecord == true)
        {
            string path = Application.persistentDataPath + "target_" + User.Instance.name + ".csv";
            currentTarget.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
            nextTarget.transform.localScale = new Vector3(0, 0, 0);

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
            Camera main = Camera.main;
            var line = contentFile[indexContent].Split(',');
            currentTarget.gameObject.SetActive(true);
            currentTarget.transform.LookAt(Camera.main.transform);
            offset_Target_Head = new Vector3(
                float.Parse(line[3], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[4], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[5], CultureInfo.InvariantCulture.NumberFormat));// + main.transform.position;

            cursor.transform.position = new Vector3(
                float.Parse(line[6], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[7], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(line[8], CultureInfo.InvariantCulture.NumberFormat)) + main.transform.position;

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
		if  (replayLastRecord == false && current_task < numberTasks && current_session < numberSessions && currentTarget.activeInHierarchy == false)
        {
            current_task++;
            Camera main = Camera.main;
            Vector3 playerPos = main.transform.position;
            Vector3 playerDirection = main.transform.forward;
            Vector3 spawnPos = playerPos + playerDirection * 1.5f;

            /* random in a cubic region
            var ex = Random.Range(-1.0f, 1f);
	        var ex2 = Random.Range(-1.0f, 1f);
	        Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y + ex, spawnPos.z + ex2);
            */

            // random on the sphere surface
            setTargetPosition(currentTarget, current_session * numberTasks + current_task);

            //type
            setTargetType();
            /*if(typenow == 3)
            {
                moveNext();
            }
            else
            {
                offset_Next_Head = new Vector3(1000,1000,1000);
            }*/

            currentTarget.transform.GetChild(0).transform.localScale = target_size_basic * size_session[current_session];
            sizenow = size_session[current_session];
            
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
        else // just keep object relatively fixed to the head
        {
            currentTarget.transform.position = Camera.main.transform.position + offset_Target_Head;
            nextTarget.transform.position = Camera.main.transform.position + offset_Next_Head;
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
