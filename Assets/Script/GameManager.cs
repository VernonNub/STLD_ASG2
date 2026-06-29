using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton initialise
    public static GameManager Instance;

    public int gameProgress = 0;

    public List<string> objectives = new List<string>
    {
        "Where Am I?"
    };
    public Dictionary<string, string> objectiveDescription = new Dictionary<string, string>
    {
        {"Where Am I?", "Investigate the spaceship for clues."}
    };

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
    }
}
