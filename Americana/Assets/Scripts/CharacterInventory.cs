using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [Header("0 - knife, 1 - chain, 2 - bat, 3 - pistol, 4- rev, 5 - shotti")]
    public int charactersWeaponIndex = 0;
    
    [Header("0 - TakeHostage, 1 - Rob, 2 - Intimidate, 3 - Seduce")]
    public List<int> charactersTalentIndexes = new List<int>();
    
    [Header("0 - Money, 1 - PistolAmmo, 2 - RevolverAmmo, 3 - ShotgunAmmo, 4 - gasoline, 5 - coffee")]
    public List<int> charactersItemsIndexes = new List<int>();
}
