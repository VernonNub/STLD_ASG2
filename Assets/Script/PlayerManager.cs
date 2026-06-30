using System.Collections.Generic;
using NUnit.Framework;
using StarterAssets;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Values")]
    public float playerHealth = 100;
    public float playerMaxHealth = 100;
    public float raycastLength = 10;
    private int mask = (1 << 6) | (1 << 7);
    public int playerDeath;
    public bool canMove = true;

    [Header("Inventory")]
    public int cash = 0;
    public int scraps = 0;
    public List<string> inventory = new List<string>();

    [Header("Player Components")]
    public Camera playerCamera;
    public CharacterController cc;
    public GameObject checkPoint;
    public ThirdPersonController tpc;

    [Header("Collectible & Interactable")]
    public GameObject collectible;
    [SerializeField] CollectibleManager collectibleManager;
    public GameObject interactable;
    [SerializeField] InteractibleManager interactableManager;

    private void OnEnable()
    {
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        cc = gameObject.GetComponent<CharacterController>();
        tpc = gameObject.GetComponent<ThirdPersonController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth <= 0)
        {
            HandleDeath();
        }

        HandleRayCast();

        if(canMove)
        {
            cc.enabled = true;
        }
        else if (!canMove)
        {
            cc.enabled = false;
        }
    }

    private void HandleDeath()
    {
        playerDeath += 1;

        UIManager.Instance.DisplayDeathUI();

        ResetPlayer();
    }

    private void ResetPlayer()
    {
        playerHealth = 100;
        playerMaxHealth = 100;

        canMove = false;
        gameObject.transform.position = checkPoint.transform.position;
        Cursor.lockState = CursorLockMode.None;
    }
    
    private void HandleRayCast()
    {
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, raycastLength, mask))
        {
            if(hit.collider.gameObject.layer == 6)
            {
                collectible = hit.collider.gameObject;
            }
            else if(hit.collider.gameObject.layer == 7)
            {
                interactable = hit.collider.gameObject;
            }
            
        }
        else
        {
            collectible = null;
            interactable = null;
            collectibleManager = null;
            interactableManager = null;
        }
    }

    private void HandleInteraction()
    {
        //Get respective manager script & run interaction
        if (collectible != null)
        {
            collectibleManager = collectible.GetComponent<CollectibleManager>();
            collectibleManager.CollectItems(this);
        }
        else if (interactable != null)
        {
            interactableManager = interactable.GetComponent<InteractibleManager>();
        }
    }

    void OnInteraction()
    {
        HandleInteraction();
    }

    //For taking damage, maxhealth boosts, healing etc. Parameters --> Health change - amount of health given/minus, maxhealthchange - amount of health given/minus
    public void UpdateHealth(float HealthChange, float MaxHealthChange)
    {
        //Ensure that health does not go past MaxHealth (Doesnt give extra health)
        playerHealth = Mathf.Min(playerHealth + HealthChange, playerMaxHealth);

        //Ensure player max health is not 0 or does not become negative
        if (playerMaxHealth + MaxHealthChange <= 0)
        {
            playerMaxHealth = 1;
        }
        else
        {
            playerMaxHealth += MaxHealthChange;
        }

        if (MaxHealthChange < 0 && playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        else if (MaxHealthChange > 0)
        {
            playerHealth += MaxHealthChange;
        }
    }
}
