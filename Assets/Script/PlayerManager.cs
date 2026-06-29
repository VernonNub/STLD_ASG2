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

    [Header("Inventory")]
    public int cash = 0;
    public int scraps = 0;
    public List<string> inventory = new List<string>();

    [Header("Player Components")]
    [SerializeField] Camera playerCamera;
    public CharacterController cc;
    public GameObject checkPoint;
    public StarterAssetsInputs starterAssetsInputs;

    [Header("Collectible & Interactable")]
    [SerializeField] GameObject collectible;
    [SerializeField] CollectibleManager collectibleManager;
    [SerializeField] GameObject interactable;
    [SerializeField] InteractibleManager interactableManager;

    private void OnEnable()
    {
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        cc = gameObject.GetComponent<CharacterController>();
        starterAssetsInputs = gameObject.GetComponent<StarterAssetsInputs>();
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

        cc.enabled = false;
        gameObject.transform.position = checkPoint.transform.position;
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
}
