using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDatabase", menuName = "ScriptableObjects/InventoryDatabase", order = 1)]
public class InventoryDatabase : ScriptableObject
{
    [Header("knife, chains, bat, pistol, revolver, shotgun")]
    public List<Weapon> weapons = new List<Weapon>();
    [Header("hostage, rob, intimidate, seduce")]
    public List<Talent> talents = new List<Talent>();
    [Header("money, pistolAmmo, revAmmo, shotgunAmmo, gasolin, coffee, sandwich, nuts, potatoChips")]
    public List<Item> items = new List<Item>();
}

[Serializable]
public class Weapon
{
    public enum WeaponType {Knife, Chains, Bat, Pistol, Revolver, Shotgun}
    public WeaponType weaponType = WeaponType.Chains;
    public List<string> weaponName = new List<string>();
    public int damage = 10;
    public int consciousDamage = 10;
    public int consciousDamageToEveryone = 10;
}

[Serializable]
public class Talent
{
    public enum Talents {TakeHostage, Rob, Intimidate, Seduce}
    public List<string> talentName = new List<string>();
    public List<string> talentDescription = new List<string>();

}

[Serializable]
public class Item
{
    public enum Items {Money, PistolAmmo, RevolverAmmo, ShotgunAmmo, Gasoline, Coffee, Sandwich, Nuts, PotatoChips}
    public List<string> itemName = new List<string>();
    public List<string> itemDescription = new List<string>();
}