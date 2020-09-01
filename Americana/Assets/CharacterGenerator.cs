using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public static CharacterGenerator instance;
    
    public CharacterStats tallCharacterPrefab;
    public CharacterStats shortCharacterPrefab;

    void Awake()
    {
        instance = this;
    }
    
    public CharacterStats CreateCharacter(bool npc)
    {
        CharacterStats newCharacter;

        if (Random.value > 0.5f)
            newCharacter = Instantiate(tallCharacterPrefab, Vector3.forward * 500, Quaternion.identity);
        else
            newCharacter = Instantiate(shortCharacterPrefab, Vector3.forward * 500, Quaternion.identity);
        
        // randomize his stats based on players resources and game state
        
        // also randomize his visuals

        return newCharacter;
    }
}