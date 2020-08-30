using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatFeedbackTextsDatabase", menuName = "ScriptableObjects/CombatFeedbackTextsDatabase", order = 1)]
public class CombatFeedbackTextsDatabase : ScriptableObject
{
    public List<string> attackButtonFeedback = new List<string>();
    public List<string> attackWeaponButtonFeedback = new List<string>();
    public List<string> attackBareButtonFeedback = new List<string>();
    public List<string> useTalentButtonFeedback = new List<string>();
    public List<string> useItemsButtonFeedback = new List<string>();
    public List<string> runButtonFeedback = new List<string>();
    
    [Header("Characters actions")]
    public List<string> isGonnaDoSomething = new List<string>();
    public List<string> isDead = new List<string>();
    public List<string> isUnconscious = new List<string>();
    public List<string> uses = new List<string>();
    public List<string> fists = new List<string>();
    public List<string> on = new List<string>();
    public List<string> self = new List<string>();
    public List<string> triesToRun = new List<string>();
}
