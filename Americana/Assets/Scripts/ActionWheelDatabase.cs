using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionWheelDatabase", menuName = "ScriptableObjects/ActionWheelDatabase", order = 1)]
public class ActionWheelDatabase : ScriptableObject
{
    public List<WheelAction> actions = new List<WheelAction>();
}

[Serializable]
public class WheelAction
{
    public enum Action
    {
        Drive, HideBody, Talk, Assault, Loot
    }

    public Action wheelAction = Action.Drive;
    public List<string> names = new List<string>();
    public Sprite spr;
}