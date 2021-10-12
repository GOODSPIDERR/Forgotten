using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu]
public class DialogueBox : ScriptableObject
{
    public Ease portraitEaseMode = Ease.Linear;
    public Vector3 pOffset1, pOffset2;
    public Quaternion rOffset1, rOffset2;
    public Character character;
    [TextArea(10, 10)]
    public string text;

}

