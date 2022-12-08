
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class Player : Actor
{
    [Header("Essentials")]
    static Player instance;
    InputControls _controls;
    Camera mainCamera;

    Vector3 lastLevelSpawn;
    Vector3 playerRespawn = new Vector3(18, -11, 0);
    Vector3 cameraRespawn = new Vector3(18, -10, -20);

    [SerializeField]
    private int coins;

    bool canShift = true;

    public int Coins { get => coins; set => coins = value; }

    public static Player GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        mainCamera = Camera.main;

        movementController.MovementVector = Vector2.zero;
        battleController.AttackVector = Vector2.zero;

        lastLevelSpawn = playerRespawn;

        InitializeActor();
        ControlsAwake();
    }

    private void ControlsAwake()
    {
        _controls = new InputControls();

        _controls.Controls.Move.performed += ctx => movementController.MovementVector = ctx.ReadValue<Vector2>();
        _controls.Controls.Move.canceled += ctx => movementController.MovementVector = Vector2.zero;

        _controls.Controls.Attack.performed += ctx => battleController.AttackVector = ctx.ReadValue<Vector2>();
        _controls.Controls.Move.canceled += ctx => battleController.AttackVector = Vector2.zero;

        _controls.Controls.Switch.performed += ctx => battleController.SwitchWeapon();

        _controls.Controls.Shift.performed += ctx => ShiftAction();

        if (movementController == null)
        {
            _controls.Controls.Disable();
        }
    }
    
    private void FixedUpdate()
    {
        Attack();
        Move();
    }

    void Move()
    {
        if(movementController != null)
        {
            movementController.Move();
        }
    }

    public void Loot(Loot loot)
    {
        if(loot.Type == LootType.Coin)
        {
            Coins++;
        }
        else if(loot.Type == LootType.Weapon)
        {
            GameObject w = Instantiate(loot.WeaponPrefab, battleController.transform);
            w.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            battleController.SetWeapon();
        }
        else if(loot.Type == LootType.Heal)
        {
            battleController.Heal();
        }
        else if(loot.Type == LootType.Heart)
        {
            battleController.MaxHealthPoints++;
            battleController.Heal();
        }
    }

    void Attack()
    {
        if (battleController.AttackVector.magnitude > 0.1f)
        {
            battleController.Attack();
        }
    }

    void ShiftAction()
    {
        if (canShift)
        {
            movementController.GetComponent<ControledMovement>().Shift();
        }
    }
    public override void SetDead()
    {
        animationController.AnimateDeath();
        movementController.Deactivate();
    }

    public void ReloadAfterDeath()
    {
        mainCamera.transform.position = cameraRespawn;
        transform.position = playerRespawn;
        movementController.ResetAfterDeath();
        battleController.ResetAfterDeath();
        animationController.Anim.SetTrigger("Spawn");
    }

    public void RestoreLevelSpawn()
    {
        transform.position = lastLevelSpawn;
        animationController.Anim.SetTrigger("Respawn");
    }

    private void OnEnable()
    {
        _controls.Controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Controls.Disable();
    }
    
    public PlayerSaveState GetSaveState()
    {
        return new PlayerSaveState(this);
    }

    public void RestoreSaveState(PlayerSaveState pss)
    {
        coins = pss.Coins;
        battleController.MaxHealthPoints = pss.MaxHealth;

        foreach(Transform t in battleController.availableWeapons.GetComponentsInChildren<Transform>())
        {
            Destroy(t.gameObject);
        }

        List<GameObject> prefabWeapons = pss.GetWeapons();

        foreach(GameObject prefabWeapon in prefabWeapons)
        {
            GameObject w = Instantiate(prefabWeapon, battleController.availableWeapons);
            w.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public void SetLevelSpawn(Vector3 pos)
    {
        lastLevelSpawn = pos;
    }
}
