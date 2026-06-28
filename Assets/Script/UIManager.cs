using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Singleton initialise
    public static UIManager Instance;

    public Slider healthBar;
    public PlayerManager playerManager;
    public GameObject deathUI;

    private void Update()
    {
        if (playerManager != null)
        {
            healthBar.maxValue = playerManager.playerMaxHealth;
            healthBar.value = playerManager.playerHealth;
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
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        deathUI = GameObject.Find("DeathScreen");
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

        deathUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
