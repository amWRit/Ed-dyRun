using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
	public GameObject[] obstaclePrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] mathOperationPrefabs;
	private Vector3 spawnPos = new Vector3(40, 2, 0);

	private PlayerController playerControllerScript;

    private bool isGameActive = true;

    //public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    //public TextMeshProUGUI targetText;
    public GameObject titleScreen;
    public Button restartButton;

   

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnObstacle", starDelay, repeatRate);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator SpawnObstacle(){
        while(true){
            float waitTime = Random.Range(1,2);
            yield return new WaitForSeconds(waitTime);
            int obstacleIndex;
            GameObject obstacle;
            int spawnRandom = Random.Range(1,5);
            //spawn numbers
            if( spawnRandom >= 4){
                obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
                obstacle = obstaclePrefabs[obstacleIndex];
                spawnPos = obstacle.transform.position;
                // spawnPos.z = Random.Range(-6,10);
            } 
            else if(spawnRandom > 1 && spawnRandom < 4) {
                obstacleIndex = Random.Range(0, mathOperationPrefabs.Length);
                obstacle = mathOperationPrefabs[obstacleIndex];
                spawnPos = obstacle.transform.position;
            }
            //spawn enemy
            else {
                obstacleIndex = Random.Range(0, enemyPrefabs.Length);
                obstacle = enemyPrefabs[obstacleIndex];
                spawnPos = obstacle.transform.position;
                spawnPos.z = Random.Range(-6,10);
            }
   
            spawnPos.x = Random.Range(60,80);
            // spawnPos.y = Random.Range(-6,10);
            if(playerControllerScript.gameOver == false){
                Instantiate(obstacle, spawnPos, obstacle.transform.rotation);
            }
        }
    }

    // 1 = maths, 2 = english
    public void StartGame(int gameType){
        isGameActive = true;
        //StartCoroutine(SpawnTarget());

        //UpdateScore(0);
        titleScreen.SetActive(false);

        //timerText.text = "Timer: " + seconds;
        //timerRoutine = StartCoroutine(time());

        //CalculateTarget();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        StartCoroutine(SpawnObstacle());
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        //StopCoroutine(timerRoutine);
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
