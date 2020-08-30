using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats: MonoBehaviour
{
    public bool inParty = false;
    
    public List<string> name = new List<string>();
    
    // characters health
    public int hp = 100;
    public int hpMax = 100;

    // consciousness depletes 
    public int conscious = 100;
    public int consciousMax = 100;
    public float consciousScaler = 1;

    //physical stats
    public int bareDamage = 5;
    public int bareConsciousDamage = 5;
    public int bareConsciousDamageToEveryone = 5;
    public int speed = 5;
    public float critChance = 0.1f;
    public int accuracy = 5;
    
    // mental stats
    public int diplomacy = 5;
    public int dreadness = 5;
    public int charm = 5;

    [Header("Links")] 
    public CharacterInventory inventory;
    public CharacterVisualController visual;
}