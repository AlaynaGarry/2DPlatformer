using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    enum State
    {
        TITLE,
        PLAYER_START,
        GAME,
        GAMEOVER,
        GAMEWIN
    }

    //[SerializeField] GameObject playerPrefab;
    [SerializeField] PlayerMovement player;
    //[SerializeField] Transform playerSpawn;
    //[SerializeField] BoxSpawner boxSpawner;
    //[SerializeField] GameObject world;
    //[SerializeField] GameObject gameWorld;
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameWinScreen;
    [SerializeField] TMP_Text scoreUI;
    //[SerializeField] TMP_Text livesIU;
    //[SerializeField] Slider healthBarUI;

    //public float playerHealth { set { healthBarUI.value = value; } }

    public delegate void GameEvent();

    public event GameEvent startGameEvent;
    public event GameEvent stopGameEvent;

    int score = 0;
    int lives = 1;
    State state = State.TITLE;
    float stateTimer;
    //float gameTimer = 0;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreUI.text = score.ToString();
        }
    }

   public int Lives
    {
        get { return lives; }
        set
        {
            lives = value;
            //livesIU.text = "Lives: " + lives.ToString();
        }
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.TITLE:

                //OnStartTitle();
                break;
            case State.PLAYER_START:
                startGameEvent?.Invoke();

                state = State.GAME;
                break;
            case State.GAME:

                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                {
                    stateTimer = 5;
                    state = State.GAMEWIN;
                    gameWinScreen.SetActive(true);

                    player.gameObject.SetActive(false);
                }
                break;
            case State.GAMEWIN:
                if (stateTimer <= 0)
                {
                    /* //DestroyAllEnemies();
                     state = State.TITLE;
                     gameOverScreen.SetActive(false);
                     titleScreen.SetActive(false);
                    */
                    //SceneManager.LoadScene(0);
                }
                break;
            case State.GAMEOVER:
                if (stateTimer <= 0)
                {
                    /*//DestroyAllEnemies();
                    gameOverScreen.SetActive(false);
                    titleScreen.SetActive(false);*/

                    //SceneManager.LoadScene(0);
                    //Debug.Log("reload");
                    state = State.TITLE;
                }

                break;
            default:
                break;
        }
    }

    public void OnStartGame()
    {
        state = State.PLAYER_START;
        Score = 0;
        Lives = 1;
        //gameTimer = 0;

        foreach (GameObject enemy in enemies) {
            enemy.SetActive(true);
            enemy.GetComponent<Health>().ResetHealth();
        }

        player.gameObject.SetActive(true);
        player.GetComponent<Health>().ResetHealth();
        //player.enabled = true;
        //world.SetActive(false);
        titleScreen.SetActive(false);
        //gameWorld.SetActive(true);
    }

    public void OnPlayerDead()
    {
        Lives -= 1;
        if (player.GetComponent<Health>().dead == true)
        {
            state = State.GAMEOVER;
            stateTimer = 5;
            gameOverScreen.SetActive(true);
            player.enabled = false;
        }
        else
        {
            //state = State.GAMEWIN;
            stateTimer = 5;
        }
        stopGameEvent?.Invoke();
    }

    public void OnStartTitle()
    {
        state = State.TITLE;
        titleScreen.SetActive(true);
        gameWinScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        player.enabled = false;
        stopGameEvent?.Invoke();
    }

    /*    private void DestroyAllEnemies()
        {
            // destroy all enemies
            *//*var spaceEnemies = FindObjectsOfType<CharacterPlayer>();
            foreach (var spaceEnemy in spaceEnemies)
            {
                Destroy(spaceEnemy.gameObject);
            }*//*
        }*/
}