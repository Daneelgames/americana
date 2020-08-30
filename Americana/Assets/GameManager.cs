using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<CharacterStats> partyCharacters = new List<CharacterStats>();
    public List<CharacterStats> npcCharacters = new List<CharacterStats>();

    private CombatManager combatManager;
    private CityManager cityManager;
    
    List<Transform> partyTransforms = new List<Transform>();
    List<Transform> npcTransforms = new List<Transform>();

    [SerializeField] private Transform charactersTempTransformsParent;

    public int language = 0;
    
    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        combatManager = CombatManager.instance;
        cityManager = CityManager.instance;

        cityManager.ToggleCity(true);
        
        yield return new WaitForSeconds(2);
        StartCombat();
    }

    void StartCombat()
    {
        cityManager.ToggleCity(false);
        
        // need to find characters who participate in combat
        //
        // remember characters positions in level
        SaveCharactersTransforms(partyCharacters, partyTransforms);
        SaveCharactersTransforms(npcCharacters, npcTransforms);
        
        StartCoroutine(combatManager.StartCombat(partyCharacters, npcCharacters));
    }

    void SaveCharactersTransforms(List<CharacterStats> characters, List<Transform> transforms)
    {
        for (int j = transforms.Count - 1; j >= 0; j--)
        {
            if (transforms[j] != null)
                Destroy(transforms[j].gameObject);
            
            transforms.RemoveAt(j);
        }
        
        for (int i = 0; i < characters.Count; i++)
        {
            var newTransform = new GameObject().transform;
            newTransform.position = characters[i].transform.position;
            newTransform.rotation = characters[i].transform.rotation;
            newTransform.parent = charactersTempTransformsParent;
            transforms.Add(newTransform);
        }
    }

    public IEnumerator FinishCombat()
    {
        yield return null;
        cityManager.ToggleCity(true);
        
        for (int i = 0; i < partyCharacters.Count; i++)
        {
            partyCharacters[i].transform.position = partyTransforms[i].transform.position;
            partyCharacters[i].transform.rotation = partyTransforms[i].transform.rotation;
        }
        
        for (int i = 0; i < npcCharacters.Count; i++)
        {
            npcCharacters[i].transform.position = npcTransforms[i].transform.position;
            npcCharacters[i].transform.rotation = npcTransforms[i].transform.rotation;
        }
    }
}