using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    private enum State
    {
        PREPARE_TO_MOVE,
        MOVE,
        PREPARE_TO_GAME,
        GAME,
        GAME_OVER
    }

    private State state = State.PREPARE_TO_MOVE;
    public static SpawnManager instance;
    public float moveDurationTime = 3;
    private float moveBeginTime;
    public GameObject enemyPrefab;
    private float spawnLimitXLeft = 11;
    private float spawnLimitXRight = 13;
    private float spawnLimitZLeft = -3;
    private float spawnLimitZRight = 4;
    public int maxEnemiesCount = 3;

    public GameObject background;
    public GameObject ground;
    public GameObject player;
    public GameObject gameOverScreen;
    public GameObject abilities;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        /*background = GameObject.FindGameObjectWithTag("Background");
        ground = GameObject.FindGameObjectWithTag("Ground");
        player = GameObject.FindGameObjectWithTag("Player");*/
    }

    private void Update()
    {
        switch (state)
        {
            case State.PREPARE_TO_MOVE:
                background.GetComponent<MoveLeft>().enabled = true;//  gameObject.SetActive(true);
                ground.GetComponent<MoveLeft>().enabled = true; //gameObject.SetActive(true);
                player.GetComponent<Player>().SetRun();
                moveBeginTime = Time.time;
                state = State.MOVE;
                break;
            case State.MOVE:
                if (Time.time - moveBeginTime > moveDurationTime)
                {
                    background.GetComponent<MoveLeft>().enabled = false; //gameObject.SetActive(false);
                    ground.GetComponent<MoveLeft>().enabled = false; //gameObject.SetActive(false);
                    player.GetComponent<Player>().SetStop();
                    state = State.PREPARE_TO_GAME;
                }
                break;
            case State.PREPARE_TO_GAME:
                for (int i = 0; i < Random.Range(1, maxEnemiesCount); i++)
                {
                    Vector3 spawnPos = new Vector3(Random.Range(spawnLimitXLeft, spawnLimitXRight), 0, Random.Range(spawnLimitZLeft, spawnLimitZRight));
                    Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);
                }
                state = State.GAME;
                break;
            case State.GAME:
                int enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
                if (enemiesCount == 0)
                    state = State.PREPARE_TO_MOVE;
                break;
            case State.GAME_OVER:
                abilities.SetActive(false);
                gameOverScreen.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void GameOver()
    {
        state = State.GAME_OVER;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
