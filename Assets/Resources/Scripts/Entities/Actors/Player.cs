
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

    [SerializeField]
    private int coins;

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

        InitializeActor();
        ControlsAwake();

        coins = 0;
    }

    private void ControlsAwake()
    {
        _controls = new InputControls();

        _controls.Controls.Move.performed += ctx => movementController.MovementVector = ctx.ReadValue<Vector2>();
        _controls.Controls.Move.canceled += ctx => movementController.MovementVector = Vector2.zero;

        _controls.Controls.Attack.performed += ctx => battleController.AttackVector = ctx.ReadValue<Vector2>();
        _controls.Controls.Move.canceled += ctx => battleController.AttackVector = Vector2.zero;

        _controls.Controls.Switch.performed += ctx => battleController.SwitchWeapon(); ;

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
            coins++;
        }
        else if(loot.Type == LootType.Weapon)
        {
            GameObject w = Instantiate(loot.WeaponPrefab, battleController.transform);
            w.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            battleController.SetWeapon();
        }
        else if(loot.Type == LootType.Heart)
        {
            battleController.Heal();
        }
        else if(loot.Type == LootType.Buff)
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

    public override void SetDead()
    {
        Debug.Log(gameObject.name + " is dead");
        // restart level?checkpoint. Reset coins, interactables. Restore HP
    }

    public void LoadNextLevel()
    {

    }

    private void OnEnable()
    {
        _controls.Controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Controls.Disable();
    }
}
