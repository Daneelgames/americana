using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public static CharacterGenerator instance;
    
    public CharacterStats tallCharacterPrefab;
    public CharacterStats shortCharacterPrefab;
    
    public List<Transform> npcRoadSpawners = new List<Transform>();

    private GameManager gm;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gm = GameManager.instance;
    }

    public void SpawnNewCharactersOnTheRoad(int amount)
    {
        // create number of characters and place them on spawners
        // these characters can be either hitchhikers or bandits
        for (int i = 0; i < amount; i++)
        {
            var newChar = CreateCharacter(true); 
            newChar.transform.position = npcRoadSpawners[i].position;
            newChar.transform.rotation = npcRoadSpawners[i].rotation;
            
            gm.npcCharacters.Add(newChar);
        }
    }
    
    CharacterStats CreateCharacter(bool npc)
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