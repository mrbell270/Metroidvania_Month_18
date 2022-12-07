using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveState
{
    [SerializeField]
    int maxHealth;
    [SerializeField]
    int coins;
    [SerializeField]
    List<string> weapons = new();

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Coins { get => coins; set => coins = value; }
    public List<string> Weapons { get => weapons; set => weapons = value; }

    public PlayerSaveState(Player pl)
    {
        MaxHealth = pl.battleController.MaxHealthPoints;
        Coins = pl.Coins;
        foreach(Weapon w in pl.battleController.availableWeapons.GetComponentsInChildren<Weapon>())
        {
            Weapons.Add(w.gameObject.name);
        }
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this); ;
    }

    public static PlayerSaveState PlayerSaveStateFromJson(string json)
    {
        PlayerSaveState pss = JsonUtility.FromJson<PlayerSaveState>(json);
        return pss;
    }

    public List<string> GetWeaponsList() => weapons;

    public List<GameObject> GetWeapons()
    {
        List<GameObject> weapons = LoadWeaponPrefabs();

        return weapons;
    }

    List<GameObject> LoadWeaponPrefabs()
    {
        List<GameObject> allWeapons = new(Resources.LoadAll<GameObject>("Prefabs/Weapons/Player/Dagger"));

        return allWeapons;
    }
}