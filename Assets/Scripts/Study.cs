using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Study : MonoBehaviour
{
    [SerializeField]
    private string name = "Study";

    public string Name
    {
        get { return name; }
    }

    [SerializeField]
    public bool replayLastRecord = false;
   
}
