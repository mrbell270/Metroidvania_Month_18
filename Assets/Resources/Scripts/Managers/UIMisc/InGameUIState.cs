using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIState : UIState
{
    Player player;

    const int maxHpFill = 10;
    [SerializeField]
    Image hpMax;
    [SerializeField]
    Image hpCur;

    [SerializeField]
    TextMeshProUGUI coinsAmount;

    [SerializeField]
    List<Image> weaponBox = new();
    [SerializeField]
    int currentWeaponIdx;
    [SerializeField]
    Texture2D weaponBoxTexture;
    Dictionary<string, Sprite> weaponToSprite = new();

    public override void OnStateEnter()
    {
        player = Player.GetInstance();

        base.OnStateEnter();
    }

    public override void OnStateStay()
    {
        hpMax.fillAmount = (float)player.battleController.MaxHealthPoints / maxHpFill;
        hpCur.fillAmount = (float)player.battleController.CurHealthPoints / maxHpFill;
        coinsAmount.text = player.Coins.ToString();

        if (currentWeaponIdx != player.battleController.CurrentWeaponIdx)
        {
            currentWeaponIdx = player.battleController.CurrentWeaponIdx;
            WeaponBoxUpdate();
        }
    }

    void WeaponBoxUpdate()
    {
        int openedWeaponsCnt = player.battleController.availableWeapons.childCount;
        if (openedWeaponsCnt > weaponBox.Count) openedWeaponsCnt = weaponBox.Count;
        for (int i = 0; i < openedWeaponsCnt; i++)
        {
            weaponBox[i].gameObject.SetActive(true);
            int weaponSackIdx = (currentWeaponIdx + i) % openedWeaponsCnt;
            string weaponTypeName = player.battleController.availableWeapons.GetComponentsInChildren<Weapon>()[weaponSackIdx].GetType().Name;
            Sprite sp;
            weaponToSprite.TryGetValue(weaponTypeName, out sp);
            if(sp != null) weaponBox[i].sprite = sp;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Image im in weaponBox)
        {
            im.gameObject.SetActive(false);
        }
        currentWeaponIdx = -1;
        Sprite[] weaponSprites = Resources.LoadAll<Sprite>("Sprites/skins/weapons/weaponbox");
        weaponToSprite.Add("FistWeapon", weaponSprites[0]);
        weaponToSprite.Add("FastSlashWeapon", weaponSprites[1]);
        weaponToSprite.Add("RangedWeapon", weaponSprites[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
