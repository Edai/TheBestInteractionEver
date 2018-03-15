using System.Collections;
using System.Collections.Generic;
using CurvedVRKeyboard;
using UnityEngine;

public class KeyClickable : ClickableObject
{
    public override void Click()
    {
        KeyboardRaycaster keyboard = FindObjectOfType<KeyboardRaycaster>();
        keyboard.Click();
    }
}
