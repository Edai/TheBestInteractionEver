using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStudy : Study
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private int number;
    int i = 0;

    public static GameObject CurrentTarget = null;

    // Use this for initialization
    void Start ()
    {
      
    }
	
	// Update is called once per frame
	void Update ()
	{
        if  (i < number && CurrentTarget == null)
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
	    }
    }
}
