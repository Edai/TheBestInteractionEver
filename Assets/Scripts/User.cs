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
    [SerializeField]
    public int Familiarity_VR = 0;
    [SerializeField]
    public int Familiarity_AR = 0;
    [SerializeField]
    public int Familiarity_Head = 0;
    [SerializeField]
    public int Familiarity_Gesture = 0;
    [SerializeField]
    public int Sickness_VR = 0;

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