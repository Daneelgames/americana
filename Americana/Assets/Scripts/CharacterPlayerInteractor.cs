using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerInteractor : MonoBehaviour
{
    public Canvas localCanvas;
    public float targetFeedbackScale = 1;
    
    private CombatManager combatManager;
    private CityManager cityManager;
    private CombatUiManager combatUiManager;
    private GameManager gm;
    
    public CharacterStats character;
    public InteractiveObject interactiveObject;
    private LogWriterController lwc;

    private ActionWheelController awc;
    
    public List<WheelAction.Action> aliveActions = new List<WheelAction.Action>();
    public List<WheelAction.Action> deadActions = new List<WheelAction.Action>();

    void Start()
    {
        gm = GameManager.instance;
        combatManager = CombatManager.instance;
        combatUiManager = CombatUiManager.instance;
        cityManager = CityManager.instance;
        lwc = LogWriterController.instance;
        awc = ActionWheelController.instance;
    }

    public void UpdateTargetCamera()
    {
        if (localCanvas)
            localCanvas.worldCamera = gm.activeCamera;
    }
    
    public void SelectCharacter()
    {
        combatUiManager.SelectNewTarget(transform, targetFeedbackScale);
        if (character)
        {
            lwc.NewLine(character.name[gm.language]);   
        }
        else if (interactiveObject)
        {
            lwc.NewLine(interactiveObject.name[gm.language]);
        }
        
        // select character
    }

    public void ClickInteractor()
    {
        if (character)
        {
            if (combatManager.currentBattleState != CombatManager.BattleState.Null)
            {
                combatUiManager.ClickTargetCharacter(character);
                awc.HideWheel();   
            }
            else
            {
                awc.ShowWheel(this);
            }
        }
        else if (interactiveObject)
        {
            awc.ShowWheel(this);   
        }
    } 
}
