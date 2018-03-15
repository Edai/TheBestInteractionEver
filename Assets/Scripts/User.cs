using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Singleton<User>
{
    [SerializeField] private string name = "Mike";
    [SerializeField] private int age = 20;
    [SerializeField] private int height = 175;
    [SerializeField] private Gender gender = Gender.MALE;
    [SerializeField] private Laterality laterality = Laterality.RIGHT_HANDED;
    [SerializeField] private bool HaveYouEverTriedVR = true;
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