using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    private static PlayerManager _instance;

    public static PlayerManager instance
    {
        get
        {
            return _instance;
        }
    }

    public List<PlayerController> localPlayers;
    public List<PlayerController> players;

    // Use this for initialization
    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        players = new List<PlayerController>();
        localPlayers = new List<PlayerController>();
    }

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") Destroy(gameObject);
    }
    
    void KillPlayer(PlayerController player) {
        
    }
}
