using System;
using System.Collections;
using System.Collections.Generic;
using CurvedVRKeyboard;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardStudy : Study
{
    [SerializeField] private Text textField;
    [SerializeField] private Text instruction;

    [SerializeField] private List<string> words;

    [SerializeField] private int index;

    [SerializeField]
    KeyboardRaycaster keyboard = null;

    // Use this for initialization
    void Start ()
	{
	    if (index >= words.Count)
	    {
	        Debug.LogError("Start index is incorrect, please check it out.");
	        index = 0;
	    }
	    if (keyboard == null)
	        keyboard = GameObject.Find("Keyboard").GetComponent<KeyboardRaycaster>();
        instruction.text = String.Format("ENTER \"{0}\" WITH THE KEYBOARD", words[index]);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (textField.text == words[index])
	    {
	        index += 1;
	        textField.text = "";
	        if (index >= words.Count)
	            index = 0;
	        instruction.text = String.Format("ENTER \"{0}\" WITH THE KEYBOARD", words[index]);
	    }
	}
    
}
