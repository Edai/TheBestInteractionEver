using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Singleton<User>
{
    [SerializeField] public string name = "Mike";
    [SerializeField] public int age = 20;
    [SerializeField] public int height = 175;
    [SerializeField] public Gender gender = Gender.MALE;
    [SerializeField] public Laterality laterality = Laterality.RIGHT_HANDED;
    [SerializeField] public bool HaveYouEverTriedVR = true;
}

public enum Gender
{
    FEMALE,
    MALE
}

public enum Laterality
{
    RIGHT_HANDED,
    LEFT_HANDED,
    AMBIDEXTROUS
}