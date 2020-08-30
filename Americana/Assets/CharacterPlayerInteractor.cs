using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerInteractor : MonoBehaviour
{
    private CombatManager combatManager;
    private CityManager cityManager;
    private CombatUiManager combatUiManager;
    private GameManager gm;
    
    public CharacterStats character;
    private LogWriterController lwc;
    public Canvas localCanvas;

    void Start()
    {
        gm = GameManager.instance;
        combatManager = CombatManager.instance;
        combatUiManager = CombatUiManager.instance;
        cityManager = CityManager.instance;
        lwc = LogWriterController.instance;
    }

    public void UpdateTargetCamera()
    {
        localCanvas.worldCamera = gm.activeCamera;
    }
    
    public void SelectCharacter()
    {
        print("select character");
        // if target feedback is active and index character is in battle
        // if (!characterTargetFeedbackAnim.gameObject.activeInHierarchy) return;

        combatUiManager.SelectNewCharacter(character);
        lwc.NewLine(character.name[gm.language]);
        
        // select character
    }

    public void ClickCharacter()
    {
        print("click character");
        combatUiManager.ClickTargetCharacter(character);
    } 
}
