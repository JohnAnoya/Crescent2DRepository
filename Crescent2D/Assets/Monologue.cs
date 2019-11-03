using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monologue 
{
    public string NPCName;
    [TextArea(4, 10)]
    public string[] Sentences; 
}
