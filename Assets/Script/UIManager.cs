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
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject deathUI;
    [SerializeField] TMP_Text missionUI;
    [SerializeField] TMP_Text missionDescriptionUI;
    [SerializeField] TMP_Text playerCashIndicator;
    [SerializeField] TMP_Text playerScrapsIndicator;


    private void Update()
    {
        UpdateGUI();
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
        Cursor.lockState = CursorLockMode.Locked;
        playerManager.cc.enabled = true;
        Cursor.visible = false;
        playerManager.starterAssetsInputs.cursorLocked = true;
    }

    public void DisplayDeathUI()
    {
        deathUI.SetActive(true);
        playerManager.starterAssetsInputs.cursorLocked = false;
        //Cursor.lockState = CursorLockMode.None;
       // Cursor.visible = true;
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

            if(deathUI != null)
            {
                deathUI.SetActive(false);
            }
            
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
}
