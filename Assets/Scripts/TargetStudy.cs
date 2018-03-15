using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStudy : Study
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private int number;

    private List<GameObject> targets = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        Camera main = Camera.main;
        Vector3 playerPos = main.transform.position;
        Vector3 playerDirection = main.transform.forward;
        Vector3 spawnPos = playerPos + playerDirection * 1.5f;
        int i = 0;

        while (i < number)
        {
            var ex = Random.Range(-1.0f, 1f);
            var ex2 = Random.Range(-1.0f, 1f);
            Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y + ex, spawnPos.z + ex2);
            bool isGood = true;
            foreach (var obj in targets)
            {
                if (Vector3.Distance(obj.transform.position, newPos) < 0.3f)
                    isGood = false;
            }
            if (isGood)
            {
                GameObject t = Instantiate(target);
                t.transform.position = newPos;
                t.transform.LookAt(main.transform);
                t.transform.parent = transform;
                targets.Add(t);
                i++;
            }
	    }
    }
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
