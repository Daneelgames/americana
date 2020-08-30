using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUiManager : MonoBehaviour
{
    public static CombatUiManager instance;

    public CombatFeedbackTextsDatabase combatFeedbackTextsDatabase;
    
    [SerializeField] private SpriteRenderer characterTurnFeedback;
    [SerializeField] private Animator characterTurnFeedbackAnim;
    //[SerializeField] private GameObject charactersTargetsCanvas;
    
    [Header("Buttons")]
    [SerializeField] private Animator actionButtonsAnim; 
    
    [SerializeField] private Animator atkButtonAnim;
    [SerializeField] private Animator actButtonAnim;
    [SerializeField] private Animator itmButtonAnim;
    [SerializeField] private Animator runButtonAnim;
    
    [SerializeField] private Animator attackSubButtonsAnim;
    [SerializeField] private GameObject wpnButton;
    [SerializeField] private GameObject bareButton;
    
    [SerializeField] private Animator talentsSubButtonsAnim;
    [SerializeField] private List<GameObject> talentButtons = new List<GameObject>();
    [SerializeField] private Animator inventorySubButtonsAnim;
    [SerializeField] private List<GameObject> inventoryButtons = new List<GameObject>();
    
    [Header("Sub buttons anims")]
    [SerializeField] private Animator wpnSubButtonAnim;
    [SerializeField] private Animator bareSubButtonAnim;
    [SerializeField] private List<Animator> actSubButtonsAnims;
    [SerializeField] private List<Animator> itemsSubButtonsAnims;

    [Header("Status bars")]
    [SerializeField] List<StatusBar> partyStatusBars = new List<StatusBar>();
    [SerializeField] List<StatusBar> npcStatusBars = new List<StatusBar>();
    
    private string updateString = "Update";
    private string activeString = "Active";

    private bool playerIsChoosingTargetToact = false;

    public CharacterStats currentCharacterInTurn;

    private GameManager gm;
    private CombatManager combatManager;
    private CombatActionsManager combatActionsManager;
    private PlayerClickInWorld pcw;

    private CombatActionsManager.Actions playersCurrentAction = CombatActionsManager.Actions.Run;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gm = GameManager.instance;
        combatManager = CombatManager.instance;
        combatActionsManager = CombatActionsManager.instance;
        pcw = PlayerClickInWorld.instance;
    }

    public void SetStatusBarsToCharacters(List<CharacterStats> characters, bool party)
    {
        npcStatusBars.Clear();
        
        List<StatusBar> tempBars = new List<StatusBar>();
        
        for (int i = 0; i < characters.Count; i++)
        {
            StatusBar newBar = new StatusBar();
            newBar.character = characters[i];
            newBar.healthbar = characters[i].visual.healthBar;
            newBar.consciousBar = characters[i].visual.consciousBar;
            tempBars.Add(newBar);
        }
        
        if (!party)
            npcStatusBars = new List<StatusBar>(tempBars);
        else
            partyStatusBars = new List<StatusBar>(tempBars);
    }
    
    public void UpdateStatusBars(bool party)
    {
        List<StatusBar> tempBars = partyStatusBars;
        if (!party)
        {
            tempBars = npcStatusBars;
        }
        
        for (int i = 0; i < tempBars.Count; i++)
        {
            if (tempBars[i].character)
            {
                tempBars[i].healthbar.transform.parent.parent.gameObject.SetActive(true);
                
                tempBars[i].healthbar.fillAmount =
                    tempBars[i].character.hp * 1f / tempBars[i].character.hpMax * 1f;
                
                tempBars[i].consciousBar.fillAmount =
                    tempBars[i].character.conscious * 1f / tempBars[i].character.consciousMax * 1f;
            }
            else
            {
                tempBars[i].healthbar.transform.parent.parent.gameObject.SetActive(false);
            }
        }
    }

    public void SetFeedbackText(string text)
    {
        /*
        feedbackTextField.text = text;
        feedbackTextAnim.SetTrigger(updateString);
        */
        LogWriterController.instance.NewLine(text);
    }

    public void ToggleActionButtons(bool active, CharacterStats character)
    {
        if (active && character)
        {
            currentCharacterInTurn = character;
            
            if (character.inventory.charactersTalentIndexes.Count == 0)
                actButtonAnim.gameObject.SetActive(false);
            else
                actButtonAnim.gameObject.SetActive(true);
            
            if (character.inventory.charactersItemsIndexes.Count == 0)
                itmButtonAnim.gameObject.SetActive(false);
            else
                itmButtonAnim.gameObject.SetActive(true);
        }
        //charactersTargetsCanvas.SetActive(active);
        actionButtonsAnim.SetBool(activeString, active);
    }

    public void NewCharactersTurn(Vector3 newPos, Color newColor)
    {
        characterTurnFeedback.transform.position = newPos;
        characterTurnFeedback.color = newColor;
        characterTurnFeedbackAnim.SetTrigger(updateString);
    }

    public void SelectNewCharacter(CharacterStats character)
    {
        ToggleTargetFeedback(false);
        pcw.characterTargetFeedbackAnim.transform.position = character.transform.position;
        ToggleTargetFeedback(true);
    }
    
    void ToggleSubButtons(bool attack, bool talents, bool items)
    {
        attackSubButtonsAnim.SetBool(activeString, attack);
        talentsSubButtonsAnim.SetBool(activeString, talents);
        inventorySubButtonsAnim.SetBool(activeString, items);
    }
    
    // SELECT BUTTONS
    public void AtkButtonSelect()
    {
        SetFeedbackText(combatFeedbackTextsDatabase.attackButtonFeedback[gm.language]);
        // if player has no weapon, hide WPN button
        // if player has wounded arm, hide BARE button
        //atkButtonAnim.SetTrigger(updateString);
        ToggleTargetFeedback(false);
        playerIsChoosingTargetToact = false;
        ToggleSubButtons(true, false, false);
    }

    public void WpnButtonSelect()
    {
        SetFeedbackText(combatFeedbackTextsDatabase.attackWeaponButtonFeedback[gm.language] +" "+ combatActionsManager.InventoryDatabase.weapons[currentCharacterInTurn.inventory.charactersWeaponIndex].weaponName[gm.language]);
    }
    
    public void BareButtonSelect()
    {
        SetFeedbackText(combatFeedbackTextsDatabase.attackBareButtonFeedback[gm.language]);
    }
    
    public void ActButtonSelect()
    {
        //actButtonAnim.SetTrigger(updateString);

        for (int i = 0; i < talentButtons.Count; i++)
        {
            if (currentCharacterInTurn.inventory.charactersTalentIndexes.Count > i)
                talentButtons[i].SetActive(true);
            else
                talentButtons[i].SetActive(false);
        }
        
        playerIsChoosingTargetToact = false;
        SetFeedbackText(combatFeedbackTextsDatabase.useTalentButtonFeedback[gm.language]);
        
        ToggleTargetFeedback(false);
        ToggleSubButtons(false, true, false);
    }
    
    public void TalentButtonSelect(int index)
    {
        var talent =
            combatActionsManager.InventoryDatabase.talents[
                currentCharacterInTurn.inventory.charactersTalentIndexes[index]];
        
        SetFeedbackText(talent.talentName[gm.language] + ". " + talent.talentDescription[gm.language]);
    }
    
    public void ItmButtonSelect()
    {
        for (int i = 0; i < inventoryButtons.Count; i++)
        {
            if (currentCharacterInTurn.inventory.charactersItemsIndexes.Count > i)
                inventoryButtons[i].SetActive(true);
            else
                inventoryButtons[i].SetActive(false);
        }

        //itmButtonAnim.SetTrigger(updateString);
        SetFeedbackText(combatFeedbackTextsDatabase.useItemsButtonFeedback[gm.language]);
        ToggleTargetFeedback(false);
        playerIsChoosingTargetToact = false;
        ToggleSubButtons(false, false, true);
    }
    public void ItemButtonSelect(int index)
    {
        var item =
            combatActionsManager.InventoryDatabase.items[
                currentCharacterInTurn.inventory.charactersItemsIndexes[index]];
        
        SetFeedbackText(item.itemName[gm.language] + ". " + item.itemDescription[gm.language]);
    }

    
    public void RunButtonSelect()
    {
        //runButtonAnim.SetTrigger(updateString);
        SetFeedbackText(combatFeedbackTextsDatabase.runButtonFeedback[gm.language]);
        ToggleTargetFeedback(false);
        playerIsChoosingTargetToact = false;
        ToggleSubButtons(false, false, false);
    }
    
    //show target feedback after choosing an action
    void ToggleTargetFeedback(bool active)
    {
        pcw.characterTargetFeedbackAnim.gameObject.SetActive(active);
    }

    
    // CLICK BUTTONS

    void ToggleSubButtonsAnims(bool wpn, bool bare, bool act0, bool act1, bool act2, bool itm0, bool itm1, bool itm2)
    {
        wpnSubButtonAnim.SetBool(activeString, wpn);
        bareSubButtonAnim.SetBool(activeString, bare);
        actSubButtonsAnims[0].SetBool(activeString, act0);
        actSubButtonsAnims[1].SetBool(activeString, act1);
        actSubButtonsAnims[2].SetBool(activeString, act2);
        itemsSubButtonsAnims[0].SetBool(activeString, itm0);
        itemsSubButtonsAnims[1].SetBool(activeString, itm1);
        itemsSubButtonsAnims[2].SetBool(activeString, itm2);
    }
    
    // player's actions
    public void ClickTargetCharacter(CharacterStats targetCharacter)
    {
        // if target feedback is active and index character is in battle
        if (!pcw.characterTargetFeedbackAnim.gameObject.activeInHierarchy || !playerIsChoosingTargetToact) return;
        
        if (targetCharacter != null)
            pcw.characterTargetFeedbackAnim.transform.position = targetCharacter.transform.position;
        
        // act on target
        if (playersCurrentAction == CombatActionsManager.Actions.WeaponAttack ||
            playersCurrentAction == CombatActionsManager.Actions.BareAttack)
        {
            combatActionsManager.Attack(currentCharacterInTurn, targetCharacter, playersCurrentAction);
        }
        else if (playersCurrentAction == CombatActionsManager.Actions.UseTalent0 ||
                 playersCurrentAction == CombatActionsManager.Actions.UseTalent1 ||
                 playersCurrentAction == CombatActionsManager.Actions.UseTalent2)
        {
            combatActionsManager.UseTalent(currentCharacterInTurn, targetCharacter, playersCurrentAction);
        }
        else if (playersCurrentAction == CombatActionsManager.Actions.UseItem0 ||
                 playersCurrentAction == CombatActionsManager.Actions.UseItem1 ||
                 playersCurrentAction == CombatActionsManager.Actions.UseItem2)
        {
            combatActionsManager.UseItem(currentCharacterInTurn, targetCharacter, playersCurrentAction);    
        }
        
        attackSubButtonsAnim.SetBool(activeString, false);
        talentsSubButtonsAnim.SetBool(activeString, false);
        inventorySubButtonsAnim.SetBool(activeString, false);
        playerIsChoosingTargetToact = false;
        ToggleSubButtonsAnims(false, false, false, false, false, false, false, false);
        ToggleTargetFeedback(false);
        
        // play action animations here
        
        // send info to combatActionsManager
        
        combatManager.NextBattleStep();
    }
    
    public void AtkButtonClick()
    {
        
    }
    
    public void WpnButtonClick()
    {
        //attackSubButtonsAnim.SetBool(activeString, false);
        playersCurrentAction = CombatActionsManager.Actions.WeaponAttack;
        playerIsChoosingTargetToact = true;
        ToggleTargetFeedback(true);
        ToggleSubButtonsAnims(true, false, false, false, false, false, false, false);
        //combatManager.NextBattleStep();
    }
    
    public void BareButtonClick()
    {
        //attackSubButtonsAnim.SetBool(activeString, false);
        playersCurrentAction = CombatActionsManager.Actions.BareAttack;
        playerIsChoosingTargetToact = true;
        ToggleSubButtonsAnims(false, true, false, false, false, false, false, false);
        ToggleTargetFeedback(true);
    }
    
    public void ActButtonClick()
    {
        
    }
    
    public void TalentButtonClick(int index)
    {
        //talentsSubButtonsAnim.SetBool(activeString, false);
        playerIsChoosingTargetToact = true;
        ToggleTargetFeedback(true);
        switch (index)
        {
            case 0:
                playersCurrentAction = CombatActionsManager.Actions.UseTalent0;
                ToggleSubButtonsAnims(false, false, true, false, false, false, false, false);
                break;
            case 1:
                playersCurrentAction = CombatActionsManager.Actions.UseTalent1;
                ToggleSubButtonsAnims(false, false, false, true, false, false, false, false);
                break;
            case 2:
                playersCurrentAction = CombatActionsManager.Actions.UseTalent2;
                ToggleSubButtonsAnims(false, false, false, false, true, false, false, false);
                break;
        }
        //combatManager.NextBattleStep();
    }
    
    public void ItmButtonClick()
    {
        
    }
    
    public void ItemButtonClick(int index)
    {
        //inventorySubButtonsAnim.SetBool(activeString, false);
        playerIsChoosingTargetToact = true;
        ToggleTargetFeedback(true);
        switch (index)
        {
            case 0:
                playersCurrentAction = CombatActionsManager.Actions.UseItem0;
                ToggleSubButtonsAnims(false, false, false, false, false, true, false, false);
                break;
            case 1:
                playersCurrentAction = CombatActionsManager.Actions.UseItem1;
                ToggleSubButtonsAnims(false, false, false, false, false, false, true, false);
                break;
            case 2:
                playersCurrentAction = CombatActionsManager.Actions.UseItem2;
                ToggleSubButtonsAnims(false, false, false, false, false, false, false, true);
                break;
        }
        //combatManager.NextBattleStep();
    }

    public void RunButtonClick()
    {
        //make character running
        combatActionsManager.Run(currentCharacterInTurn);
        playersCurrentAction = CombatActionsManager.Actions.Run;
        combatManager.NextBattleStep();
    }
}

[Serializable]
public class StatusBar
{
    public CharacterStats character;
    public Image healthbar;
    public Image consciousBar;
}