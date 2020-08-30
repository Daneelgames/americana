using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    enum BattleState
    {
        TurnStart, ActionStart, ActionFeedback, Null
    }

    private BattleState currentBattleState = BattleState.TurnStart;

    public static CombatManager instance;

    [SerializeField] private GameObject combatEnvironment;
    
    [SerializeField] List<CharacterStats> partyCharacters = new List<CharacterStats>();
    [SerializeField] List<CharacterStats> npcCharacters = new List<CharacterStats>();
    
    [SerializeField] private List<Transform> partyTransforms = new List<Transform>();
    [SerializeField] private List<Transform> npcTransforms = new List<Transform>();
    
    public List<CharacterStats> charactersInTurnOrder = new List<CharacterStats>();

    [SerializeField] private int currentCharacterIndex = 0;

    private GameManager gm;
    private CombatUiManager combatUiManager;
    private CombatActionsManager combatActionsManager;

    public Camera combatCamera;
    
    void Awake()
    {
        instance = this;
    }
    
    public IEnumerator StartCombat(List<CharacterStats> party, List<CharacterStats> npc)
    {
        gm = GameManager.instance;
        combatUiManager = CombatUiManager.instance;
        combatActionsManager = CombatActionsManager.instance;

        partyCharacters = party;
        npcCharacters = npc;
        
        combatEnvironment.SetActive(true);
        
        // move npcCharacters
        MoveCharactersToBattleTransforms(party, partyTransforms);
        MoveCharactersToBattleTransforms(npc, npcTransforms);
        combatUiManager.SetStatusBarsToCharacters(party, true);
        combatUiManager.SetStatusBarsToCharacters(npc, false);
        
        // current character index move in reversed order cuz sorting sort the list that way
        SortCharactersBySpeed();
        currentCharacterIndex = charactersInTurnOrder.Count - 1;
        
        yield return new WaitForSeconds(1);
        combatUiManager.UpdateStatusBars(true);
        combatUiManager.UpdateStatusBars(false);
        NewTurn();
    }

    void MoveCharactersToBattleTransforms(List<CharacterStats> characters, List<Transform> transforms)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            StartCoroutine(MoveCharacter(characters[i], transforms[i], 0.5f));
        }
    }

    IEnumerator MoveCharacter(CharacterStats character, Transform _transform, float time)
    {
        float timeCurrent = 0;
        while (timeCurrent < time)
        {
            timeCurrent += Time.deltaTime;
            character.transform.position = Vector3.Lerp(character.transform.position, _transform.position, timeCurrent / time);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, _transform.rotation, timeCurrent / time);
            yield return null;
        }

        character.transform.position = _transform.position;
        character.transform.rotation = _transform.rotation;
    }

    void SortCharactersBySpeed()
    {
        charactersInTurnOrder.Clear();

        for (int i = 0; i < partyCharacters.Count; i++)
        {
            charactersInTurnOrder.Add(partyCharacters[i]);
        }
        for (int i = 0; i < npcCharacters.Count; i++)
        {
            charactersInTurnOrder.Add(npcCharacters[i]);
        }
        
        charactersInTurnOrder.Sort(SortBySpeed);
    }

    void NewTurn()
    {
        currentBattleState = BattleState.TurnStart;
        
        // shows feedback on who's turn it is
        var color = Color.red;
        if (charactersInTurnOrder[currentCharacterIndex].inParty && charactersInTurnOrder[currentCharacterIndex].hp > 0 && charactersInTurnOrder[currentCharacterIndex].conscious > 0)
        {
            color = Color.cyan;   
            combatUiManager.ToggleActionButtons(true, charactersInTurnOrder[currentCharacterIndex]);
        }

        combatUiManager.NewCharactersTurn(charactersInTurnOrder[currentCharacterIndex].transform.position, color);
        
        combatUiManager.SetFeedbackText(charactersInTurnOrder[currentCharacterIndex].name[gm.language] + " " + combatUiManager.combatFeedbackTextsDatabase.isGonnaDoSomething[gm.language]);
        
        
        // click to go to the next step
    }

    void TurnAction()
    {
        currentBattleState = BattleState.ActionStart;
        // player chooses action before this method
        
        combatUiManager.ToggleActionButtons(false, null);

        if (!charactersInTurnOrder[currentCharacterIndex].inParty)
        {
            combatActionsManager.ChooseNpcAction(charactersInTurnOrder[currentCharacterIndex]);
        }
        
        // character acts here
        CharacterStats character = combatActionsManager.GetLastActingCharacter();
        CharacterStats target = combatActionsManager.GetLastTargetCharacter();
        CombatActionsManager.Actions action = combatActionsManager.GetLastAction();

        // generate feedback
        string resultString;
        if (action == CombatActionsManager.Actions.Run)
            resultString = charactersInTurnOrder[currentCharacterIndex].name[gm.language] + " " +
                           combatUiManager.combatFeedbackTextsDatabase.triesToRun[gm.language];
        else
        {
            string actionObjectString = "";

            if (action == CombatActionsManager.Actions.BareAttack)
                actionObjectString = combatUiManager.combatFeedbackTextsDatabase.fists[gm.language];
            else if (action == CombatActionsManager.Actions.WeaponAttack)
                actionObjectString = combatActionsManager.InventoryDatabase.weapons[character.inventory.charactersWeaponIndex].weaponName[gm.language];
            else if (action == CombatActionsManager.Actions.UseItem0)
                actionObjectString = combatActionsManager.InventoryDatabase.items[character.inventory.charactersItemsIndexes[0]].itemName[gm.language];
            else if (action == CombatActionsManager.Actions.UseItem1)
                actionObjectString = combatActionsManager.InventoryDatabase.items[character.inventory.charactersItemsIndexes[1]].itemName[gm.language];
            else if (action == CombatActionsManager.Actions.UseItem2)
                actionObjectString = combatActionsManager.InventoryDatabase.items[character.inventory.charactersItemsIndexes[2]].itemName[gm.language];
            else if (action == CombatActionsManager.Actions.UseTalent0)
                actionObjectString = combatActionsManager.InventoryDatabase.talents[character.inventory.charactersTalentIndexes[0]].talentName[gm.language];
            else if (action == CombatActionsManager.Actions.UseTalent1)
                actionObjectString = combatActionsManager.InventoryDatabase.talents[character.inventory.charactersTalentIndexes[1]].talentName[gm.language];
            else if (action == CombatActionsManager.Actions.UseTalent2)
                actionObjectString = combatActionsManager.InventoryDatabase.talents[character.inventory.charactersTalentIndexes[2]].talentName[gm.language];

            actionObjectString.ToLower();
            
            string targetString = combatUiManager.combatFeedbackTextsDatabase.self[gm.language];
            if (character != target)
                targetString = target.name[gm.language];
            
            resultString = charactersInTurnOrder[currentCharacterIndex].name[gm.language] + " " +
                           combatUiManager.combatFeedbackTextsDatabase.uses[gm.language] + " " + 
                           actionObjectString + " " + combatUiManager.combatFeedbackTextsDatabase.on[gm.language] + " " + targetString;
        }
        
        combatUiManager.SetFeedbackText(resultString);
        // click to go next
    }

    void TurnActionFeedback()
    {
        currentBattleState = BattleState.ActionFeedback;
        // action is over
        // player can read the feedback
        if (charactersInTurnOrder[currentCharacterIndex].hp <= 0)
            combatUiManager.SetFeedbackText(charactersInTurnOrder[currentCharacterIndex].name[gm.language] + " " + combatUiManager.combatFeedbackTextsDatabase.isDead[gm.language]);
        else if (charactersInTurnOrder[currentCharacterIndex].conscious <= 0)
            combatUiManager.SetFeedbackText(charactersInTurnOrder[currentCharacterIndex].name[gm.language] + " " + combatUiManager.combatFeedbackTextsDatabase.isUnconscious[gm.language]);
        else
        {
            // generate result text
            combatUiManager.SetFeedbackText(charactersInTurnOrder[currentCharacterIndex].name[gm.language] + " did some stuff with some result yo!");   
        }
        //click to start next turn
    }

    public void SkipPause()
    {
        print("here");
        if (charactersInTurnOrder[currentCharacterIndex].inParty && currentBattleState == BattleState.TurnStart && charactersInTurnOrder[currentCharacterIndex].hp > 0 && charactersInTurnOrder[currentCharacterIndex].conscious > 0)
            return;

        NextBattleStep();
    }


    public void NextBattleStep()
    {
        print("here2");
        switch (currentBattleState)
        {
            case BattleState.TurnStart:
                if (charactersInTurnOrder[currentCharacterIndex].hp <= 0 || charactersInTurnOrder[currentCharacterIndex].conscious <= 0 )
                    TurnActionFeedback();
                else
                    TurnAction();
                break;
            case BattleState.ActionStart:
                //TurnActionFeedback();
                
                LastStepInTurn();
                break;
            case BattleState.ActionFeedback:
                // next characters turn
                LastStepInTurn();
                break;
        }
    }

    void LastStepInTurn()
    {
        currentCharacterIndex--;
        if (currentCharacterIndex < 0 || currentCharacterIndex >= charactersInTurnOrder.Count)
            currentCharacterIndex = charactersInTurnOrder.Count - 1;

        bool partyAlive = false;
        bool npcAlive = false;

        for (int i = 0; i < partyCharacters.Count; i++)
        {
            if (partyCharacters[i].hp > 0 && partyCharacters[i].conscious > 0)
            {
                partyAlive = true;
                break;
            }
        }
        for (int i = 0; i < npcCharacters.Count; i++)
        {
            if (npcCharacters[i].hp > 0 && npcCharacters[i].conscious > 0)
            {
                npcAlive = true;
                break;
            }
        }

        if (partyAlive && npcAlive)
            NewTurn();
        else
        {
            StartCoroutine(gm.FinishCombat());
            currentBattleState = BattleState.Null;
            combatEnvironment.SetActive(false);
        }
    }

    static int SortBySpeed(CharacterStats p1, CharacterStats p2)
    {
        return p1.speed.CompareTo(p2.speed);
    }

    public CharacterStats GetCharacterInSlot(int index)// 0,1,2 - party, 3,4,5,6,7 - npcs 
    {
        CharacterStats returnChar = null;
        if (index < 3) // party
        {
            if (partyCharacters.Count > index)
                returnChar = partyCharacters[index];
        }
        else // npc
        {
            if (npcCharacters.Count > index - 3 )
                returnChar = npcCharacters[index - 3];
        }
        return returnChar;
    }

    public List<CharacterStats> GetParty()
    {
        return partyCharacters;
    }
    public List<CharacterStats> GetNpcs()
    {
        return npcCharacters;
    }
}