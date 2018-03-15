using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : ClickableObject
{
    [SerializeField]
    private Renderer r;

    private Color c;
    
    void Start()
    {
        c = r.material.color;
    }

    public override void Click()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Cursor"))
        {
            r.material.color = new Color32(0xFF, 0x0, 0x0, 0xFF);
        }
    }

    void OnTriggerExit(Collider col)
    {
        r.material.color = c;
    }
}
