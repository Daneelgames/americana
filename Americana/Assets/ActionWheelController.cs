using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionWheelController : MonoBehaviour
{
    public ActionWheelDatabase actionWheelDatabase;
    
    public static ActionWheelController instance;
    
    public Animator actionWheelAnimator;
    public List<Image> actionImages = new List<Image>();
    public TextMeshProUGUI actionNameFeedbackField;

    private CharacterPlayerInteractor currentInteractor;
    public List<WheelAction> currentWheelActions = new List<WheelAction>();
    
    private string activeString = "Active";
    private string updateString = "Update";
    public bool wheelActive = false;
    private GameManager gm;

    private RoadManager roadManager;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gm = GameManager.instance;
        roadManager = RoadManager.instance;
    }
    
    public void ShowWheel(CharacterPlayerInteractor cpi)
    {
        actionNameFeedbackField.text = "";
        currentInteractor = cpi;
        actionWheelAnimator.gameObject.SetActive(false);
        actionWheelAnimator.transform.position = gm.activeCamera.WorldToScreenPoint(currentInteractor.transform.position);
        
        actionWheelAnimator.gameObject.SetActive(true);
        // configure wheel actions
        ConfigureWheelActions();
        actionWheelAnimator.SetBool(activeString, true);
        wheelActive = true;
    }

    void ConfigureWheelActions()
    {
        if (currentInteractor.character)
        {
            if (currentInteractor.character.hp > 0 && currentInteractor.character.conscious > 0) // alive
            {
                FillActionList(currentInteractor.aliveActions);
            }
            else // dead or unconscious
            {
                FillActionList(currentInteractor.deadActions);
            }
        }
        else if (currentInteractor.interactiveObject)
        {
            FillActionList(currentInteractor.aliveActions);
        }
    }

    void FillActionList(List<WheelAction.Action> actions)
    {
        currentWheelActions.Clear();
        
        for (int i = 0; i < actionImages.Count; i++)
        {
            if (i < actions.Count)
            {
                for (int a = 0; a < actionWheelDatabase.actions.Count; a++)
                {
                    if (actions[i] == actionWheelDatabase.actions[a].wheelAction)
                    {
                        actionImages[i].enabled = true;
                        actionImages[i].sprite = actionWheelDatabase.actions[a].spr;
                        currentWheelActions.Add(actionWheelDatabase.actions[a]);
                        break;
                    }
                }   
            }
            else
                actionImages[i].enabled = false;
        }
    }
    
    public void HideWheel()
    {
        actionWheelAnimator.SetBool(activeString, false);
        wheelActive = false;
    }

    public void SelectAction(int index)
    {
        actionNameFeedbackField.text = currentWheelActions[index].names[gm.language];
        actionWheelAnimator.SetTrigger(updateString);
    }
    
    public void ClickAction(int index)
    {
        var clickedAction = currentWheelActions[index].wheelAction;

        switch (clickedAction)
        {
            case WheelAction.Action.Drive:
                gm.StartDriving();
                break;
            case WheelAction.Action.HideBody:
                
                break;
            case WheelAction.Action.Talk:
                
                break;
            case WheelAction.Action.Assault:
                gm.StartCombat();
                break;
            case WheelAction.Action. Loot:
                
                break;
        }
    }
}
