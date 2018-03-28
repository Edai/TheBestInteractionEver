using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetStudy : Study
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private int numberTasks;

    [SerializeField]
    private int numberSessions;

    [SerializeField]
    private Text textInstruction;


    private int current_task = 0;
    private int current_session = 0;
    

    public static GameObject CurrentTarget = null;

    // Use this for initialization
    void Start ()
    {
      
    }
	
	// Update is called once per frame
	void Update ()
	{
        keyEvent();
        if  (current_task < numberTasks && current_session < numberSessions && CurrentTarget == null)
        {
            Camera main = Camera.main;
            Vector3 playerPos = main.transform.position;
            Vector3 playerDirection = main.transform.forward;
            Vector3 spawnPos = playerPos + playerDirection * 1.5f;

            var ex = Random.Range(-1.0f, 1f);
	        var ex2 = Random.Range(-1.0f, 1f);
	        Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y + ex, spawnPos.z + ex2);

            CurrentTarget = Instantiate(target);
            CurrentTarget.transform.position = newPos;
            CurrentTarget.transform.LookAt(main.transform);
            CurrentTarget.transform.parent = transform;


            current_task++;
            textInstruction.text = "Session: " + current_session.ToString() + " Task: " + current_task.ToString();
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
            Destroy(CurrentTarget);
        }
    }
}
