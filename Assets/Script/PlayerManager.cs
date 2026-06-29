using System.Collections.Generic;
using NUnit.Framework;
using StarterAssets;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Values")]
    public float playerHealth = 100;
    public float playerMaxHealth = 100;
    public float raycastLength = 3;
    private int mask = (1 << 6) | (1 << 7);
    public int playerDeath;

    [Header("Inventory")]
    public int cash = 0;
    public int scraps = 0;
    public List<string> inventory = new List<string>();

    [Header("Player Components")]
    private Camera camera;
    public CharacterController cc;
    public GameObject checkPoint;
    public StarterAssetsInputs starterAssetsInputs;

    private void OnEnable()
    {
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();
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
}
