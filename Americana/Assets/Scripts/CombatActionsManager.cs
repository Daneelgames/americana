using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class manages what units do and how stats changes with that
public class CombatActionsManager : MonoBehaviour
{
    public InventoryDatabase InventoryDatabase;
    public static CombatActionsManager instance;

    private CombatManager combatManager;
    private CombatUiManager combatUiManager;

    private CharacterStats lastActingCharacter;
    private CharacterStats lastTargetCharacter;
    private Actions lastAction;
    
    public enum Actions
    {
        WeaponAttack, BareAttack, UseTalent0, UseTalent1, UseTalent2, UseItem0, UseItem1, UseItem2, Run
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        combatManager = CombatManager.instance;
        combatUiManager = CombatUiManager.instance;
    }

    public void ChooseNpcAction(CharacterStats character)
    {
        // only use weapon or bare for now
        float r = Random.value;
        
        //choose target
        List<CharacterStats> partyCharactersTemp = new List<CharacterStats>(combatManager.GetParty());
        //List<CharacterStats> npcsCharactersTemp = new List<CharacterStats>(combatManager.GetNpcs());

        for (int i = partyCharactersTemp.Count - 1; i >= 0; i--)
        {
            if (partyCharactersTemp[i].hp <= 0 || partyCharactersTemp[i].conscious <= 0)
                partyCharactersTemp.RemoveAt(i);
        }

        CharacterStats target = partyCharactersTemp[Random.Range(0, partyCharactersTemp.Count)];
        
        if (r > 0.5f) // weapon
        {
            Attack(character, target, Actions.WeaponAttack);
        }
        else // bare
        {
            Attack(character, target, Actions.BareAttack);
        }
    }
    
    public void Attack(CharacterStats character, CharacterStats target, Actions action)
    {
        var damage = 0;
        var consciousDamage = 0;
        var consciousDamageToEveyone = 0;

        SaveLastAction(character, target, action);
        
        if (action == Actions.WeaponAttack)
        {
            damage = InventoryDatabase.weapons[character.inventory.charactersWeaponIndex].damage;
            consciousDamage = InventoryDatabase.weapons[character.inventory.charactersWeaponIndex].consciousDamage;
            consciousDamageToEveyone = InventoryDatabase.weapons[character.inventory.charactersWeaponIndex].consciousDamageToEveryone;
        }
        else if (action == Actions.BareAttack)
        {
            damage = character.bareDamage;
            consciousDamage = character.bareConsciousDamage;
            consciousDamageToEveyone = character.bareConsciousDamageToEveryone;
        }
        
        //damage everyones consious
        foreach (var c in combatManager.charactersInTurnOrder)
        {
            if (c != character && c != target && c.hp > 0 && c.conscious > 0)
            {
                c.conscious -= consciousDamageToEveyone;

                if (c.conscious <= 0)
                {
                    // character is down feedback
                    // play uncoiscious anim
                }
            }
        }
            
        //damage target
        if (target.conscious > 0)
        {
            if (Random.value <= character.critChance)
            {
                consciousDamage *= 2;
                // crit feedback
            }
            
            target.conscious -= consciousDamage;   
        }

        if (target.hp > 0)
        {
            if (Random.value <= character.critChance)
            {
                damage *= 2;
                // crit feedback
            }

            target.hp -= damage;   
        }
        
        if (target.conscious <= 0)
        {
            // character is down feedback
            // play uncoiscious anim
        }
        
        if (target.hp <= 0)
        {
            // character is dead feedback
            // play dead anim
        }

        combatUiManager.UpdateStatusBars(true);
        combatUiManager.UpdateStatusBars(false);
    }

    public void UseTalent(CharacterStats character, CharacterStats target, Actions action)
    {
        SaveLastAction(character, target, action);
    }

    public void UseItem(CharacterStats character, CharacterStats target, Actions action)
    {
        SaveLastAction(character, target, action);
    }

    public void Run(CharacterStats character)
    {
        // running logic
        SaveLastAction(character, null, Actions.Run);
    }

    void SaveLastAction(CharacterStats character, CharacterStats target, Actions action)
    {
        lastAction = action;
        lastActingCharacter = character;
        lastTargetCharacter = target;
    }

    public CharacterStats GetLastActingCharacter()
    {
        return lastActingCharacter;
    }
    public CharacterStats GetLastTargetCharacter()
    {
        return lastTargetCharacter;
    }
    
    public Actions GetLastAction()
    {
        return lastAction;
    }
}
