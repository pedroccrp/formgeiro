using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ant", menuName = "Scriptable Objects/Ant")]
public class Ants : ScriptableObject
{
    [TextArea]
    public string description;

    [TextArea]
    public string pickupText;

    public Sprite sprite;
}
