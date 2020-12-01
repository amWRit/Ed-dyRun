using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
	public bool gameOver = false;
	private bool isGameActive = true;

	public float speed = 20;

    public TextMeshProUGUI operationText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;

    private List<string> mathNumbers = new List<string>() {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}; 
    private List<string> mathOperations = new List<string>() {"add", "sub", "mul", "div"}; 
    private float mathResult = 0.0f;
    //public List<string> collectedItems = new List<string>();

    private string currentOperation;
    private string currentNumber;

    private float mathsTarget;
    private float score;

    public TextMeshProUGUI gameOverText;
    public GameObject titleScreen;
    public Button restartButton;

	private AudioSource playerAudio; 
    public AudioClip scoreSound;
    public ParticleSystem scoreParticle;

	public GameObject[] obstaclePrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] mathOperationPrefabs;
	private Vector3 spawnPos = new Vector3(40, 2, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        speedText.text = "Speed: " + speed;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

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

    public IEnumerator SpawnObstacle(){
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
            if(gameOver == false){
                Instantiate(obstacle, spawnPos, obstacle.transform.rotation);
            }
        }
    }

    public void MathOperation(string tag){
        // if collected item is an operation
        if (mathOperations.Contains(tag)){
            operationText.text = "Opr: " + tag.ToUpper();
            currentOperation = tag;
        } 

        // if collected item is a number
        else if (mathNumbers.Contains(tag)){
            // if no current operation
            if(currentOperation == null || mathResult == 0){
                mathResult = GetMathNumber(tag);
                resultText.text = "Result: " + mathResult;
            }
            else {
                //do math operation based on current operation
                CalculateMathResult(GetMathNumber(tag));
                resultText.text = "Result: " + mathResult;

                if(mathResult == mathsTarget){
                	playerAudio.PlayOneShot(scoreSound, 1.0f);
                	scoreParticle.Play();
                    score += 1;
                    scoreText.text = "Score: " + score;
                    speed += 5;
                    speedText.text = "Score: " + speed;
                }
            }
        }    
    }

    public int GetMathNumber(string tag){
        if(tag == "zero"){
            return 0;
        } 
        else if(tag == "one"){
            return 1;
        }
        else if(tag == "two"){
            return 2;
        }
        else if(tag == "three"){
            return 3;
        }
        else if(tag == "four"){
            return 4;
        }
        else if(tag == "five"){
            return 5;
        }
        else if(tag == "six"){
            return 6;
        }
        else if(tag == "seven"){
            return 7;
        }
        else if(tag == "eight"){
            return 8;
        }
        else {
            return 9;
        }
    }

    private float CalculateMathResult(int currentNumber){
        if(currentOperation == "add"){
            return mathResult += currentNumber;
        } 
        else if(currentOperation == "sub"){
            return mathResult -= currentNumber;
        }
        else if(currentOperation == "mul"){
            return mathResult *= currentNumber;
        }
        else {
            if (currentNumber == 0)
                return mathResult = 0;
            else
                return mathResult = Mathf.RoundToInt(mathResult / currentNumber);
        }
    }

    public void CalculateTarget(){
        mathsTarget = Random.Range(10,20);
        targetText.text = "Target: " + mathsTarget;
    }
}
