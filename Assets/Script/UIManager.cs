using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Singleton initialise
    public static UIManager Instance;
    
    [Header("Player GUI")]
    private Slider healthBar;
    public PlayerManager playerManager;
    [SerializeField] GameObject deathUI;
    [SerializeField] TMP_Text missionUI;
    [SerializeField] TMP_Text missionDescriptionUI;
    [SerializeField] TMP_Text playerCashIndicator;
    [SerializeField] TMP_Text playerScrapsIndicator;
    [SerializeField] GameObject interactPrompt;
    [SerializeField] Transform interactObjectTransform;
    public Vector3 interactObjectOffset;


    private void Update()
    {
        UpdateGUI();
        
        if(playerManager != null && (playerManager.collectible != null || playerManager.interactable != null))
        {
            if(playerManager.collectible == null)
                interactObjectTransform = playerManager.interactable.transform;
            else
                interactObjectTransform = playerManager.collectible.transform;

            ShowInteractivePrompt();
        }
        else if(interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    //Ensures that there is only 1 of this gameobject
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //For Scene Changes
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetCursorNMovement()
    {

        GameObject.Find("Player").GetComponent<PlayerManager>().canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisplayDeathUI()
    {
        deathUI.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "ShipScene" || scene.name == "PlanetScene")
        {
            //Gets the different component after entering game scenes
            healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
            deathUI = GameObject.Find("DeathScreen");
            playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
            missionUI = GameObject.Find("Mission").GetComponent<TMP_Text>();
            missionDescriptionUI = GameObject.Find("Description").GetComponent<TMP_Text>();
            playerCashIndicator = GameObject.Find("CashAmount").GetComponent<TMP_Text>();
            playerScrapsIndicator = GameObject.Find("ScrapsAmount").GetComponent<TMP_Text>();
            interactPrompt = GameObject.Find("InteractPrompt");

            if(deathUI != null)
                deathUI.SetActive(false);

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
            
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void UpdateGUI()
    {
        if (playerManager != null)
        {
            healthBar.maxValue = playerManager.playerMaxHealth;
            healthBar.value = playerManager.playerHealth;
        }

        if (missionUI != null && missionDescriptionUI != null)
        {
            missionUI.text = GameManager.Instance.objectives[GameManager.Instance.gameProgress];
            missionDescriptionUI.text = GameManager.Instance.objectiveDescription[GameManager.Instance.objectives[GameManager.Instance.gameProgress]];
        }

        if(playerCashIndicator != null && playerScrapsIndicator != null)
        {
            playerCashIndicator.text = "Cash: " + playerManager.cash;
            playerScrapsIndicator.text = "Scraps: " + playerManager.scraps;
        }
    }

    private void ShowInteractivePrompt()
    {
        interactPrompt.SetActive(true);
        interactPrompt.transform.position = interactObjectTransform.position + interactObjectOffset;

        interactPrompt.transform.LookAt(playerManager.playerCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
