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
    private RoadManager roadManager;
    
    List<Transform> partyTransforms = new List<Transform>();
    List<Transform> npcTransforms = new List<Transform>();

    [SerializeField] private Transform charactersTempTransformsParent;

    public int language = 0;
    public Camera activeCamera;

    private ActionWheelController acw;
    
    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        combatManager = CombatManager.instance;
        cityManager = CityManager.instance;
        roadManager = RoadManager.instance;
        acw = ActionWheelController.instance;

        cityManager.ToggleCity(true);
        
        yield return new WaitForSeconds(2);
        StartCombat();
    }

    void StartCombat()
    {
        activeCamera = combatManager.combatCamera;
        
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

    void SetActiveCamera(Camera newCam)
    {
        activeCamera = newCam;

        foreach (var ch in npcCharacters)
        {
            ch.interactor.UpdateTargetCamera();
        }
        foreach (var ch in partyCharacters)
        {
            ch.interactor.UpdateTargetCamera();
        }
    }

    public IEnumerator FinishCombat()
    {
        yield return null;
        cityManager.ToggleCity(true);
        activeCamera = cityManager.cityCamera;
    }

    public void StartDriving()
    {
        // hide party inside the car
        for (int i = partyCharacters.Count - 1; i >= 0; i--)
        {
            if (partyCharacters[i].hp <= 0)
                partyCharacters.RemoveAt(i);
            else
            {
                partyCharacters[i].visual.HideCharacter();
            }
        }
        
        roadManager.StartDriving();
        acw.HideWheel();
    }

    public void StopDriving()
    {
        roadManager.StopDriving();

        // unhide party
        for (int i = partyCharacters.Count - 1; i >= 0; i--)
        {
            if (partyCharacters[i].hp <= 0)
                partyCharacters.RemoveAt(i);
            else
            {
                partyCharacters[i].visual.UnhideCharacter();
            }
        }
    }
}