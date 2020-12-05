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

	// 1 = Maths, 2 = English
	public int currentGameType;
	public int level = 1;

	public float speed = 20;

    public TextMeshProUGUI operationText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI levelText;

    private List<string> mathNumbers = new List<string>() {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}; 
    private List<string> mathOperations = new List<string>() {"add", "sub", "mul", "div"};
    private List<string> letters = new List<string>() {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p",
														"q", "r", "s", "t", "u", "v", "w", "x", "y", "z"}; 
	private List<string> englishTargets = new List<string>() {"apple", "ball", "cat", "dog", "egg", "fan", "goat", "hen", "iron", "jug", "key", "lamp", "mouse", "nose", "orange", "puppy",
														"queen", "rat", "snake", "tiger", "uncle", "van", "watch", "xray", "yak", "zebra"};

    private float mathResult = 0.0f;
    private string englishResult;

    private string currentOperation;
    private string currentNumber;

    private float mathsTarget;
    private string englishTarget = "";
    private float score;

    // used to store literals separately from the target word
    //E.g. if target is DOG, this list containts {"D", "O", "G"}
    public List<char> englishTargetLiterals = new List<char>(){};

    // index of the alphabet : E.g. 4 for D, 10 for J etc. 
    // this index used to get the specific prefab from prefab array
    public int alphabetIndex;

    // integer value used to get char from englishTargetLiterals list
    public int literalIndex;

    public TextMeshProUGUI gameOverText;
    public GameObject titleScreen;
    public Button restartButton;
    public GameObject healthBar;

	private AudioSource playerAudio; 
    public AudioClip scoreSound;
    public ParticleSystem scoreParticle;

	public GameObject[] numberPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] letterPrefabs;
    public GameObject[] mathOperationPrefabs;

	private Vector3 spawnPos = new Vector3(40, 2, 0);

	private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        speedText.text = "Speed: " + speed;
        levelText.text = "Level: " + level;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKey(KeyCode.C) && !gameOver){
        	//reset results
            englishResult = "";
            resultText.text = "Result: ";
            literalIndex = 0;
            mathResult = 0;
        }
    }

    // 1 = maths, 2 = english
    public void StartGame(int gameType){
    	currentGameType = gameType;
    	CalculateTarget();
        isGameActive = true;

        titleScreen.SetActive(false);

        // set the Game play screen UI elements visible
        targetText.gameObject.SetActive(true);
        resultText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        speedText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        healthBar.SetActive(true);

        if(gameType == 1){
        	operationText.gameObject.SetActive(true);
        }
        else{
        	operationText.gameObject.SetActive(false);
        }

        if(level == 1)
        	literalIndex = 0; //literal index required only for English(2) game type
        StartCoroutine(SpawnObstacle());
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // spawn friend or enemy obstacles based on level and game type
    public IEnumerator SpawnObstacle(){
        while(true){
            float waitTime = Random.Range(1,2);
            yield return new WaitForSeconds(waitTime);
            int obstacleIndex; //index to get specific prefab from array
            GameObject obstacle; //store obstacle prefab; friendly or enemy
            int spawnRandom = Random.Range(1,7);


            //spawn numbers, only in Maths game type -- highest probability
            if( spawnRandom >= 4 && currentGameType == 1){
                obstacleIndex = Random.Range(0, numberPrefabs.Length);
                obstacle = numberPrefabs[obstacleIndex];
                spawnPos = obstacle.transform.position;
                spawnPos.z = Random.Range(-10,5);
            } 

            // spawn math operations, only in Maths game type -- medium probability
            else if(spawnRandom > 1 && spawnRandom < 4 && currentGameType == 1) {
            	if(level < 5){
                	obstacleIndex = Random.Range(0, level);
            	} else{
            		obstacleIndex = Random.Range(0, mathOperationPrefabs.Length);
            	}
                obstacle = mathOperationPrefabs[obstacleIndex];
                spawnPos = obstacle.transform.position;
                spawnPos.z = Random.Range(-9,6);
            }

            // spawn letters, only in English game type -- highest probability
            else if(spawnRandom >= 3 && currentGameType == 2) {
            	// level 1: spawn only alphabets in target word. For e.g. only spawn D, O, G in correct order
            	//level 2: spawn only alphabets in target word but randomly. For e.g. only spawn D, O, G in random order
            	// level 3: spawn all 26 alphabets in random order

            	if(level == 1){
            		// convert alphabet into its corresponding integer value - e.g. D = 4, J = 10
            		alphabetIndex = (int) englishTargetLiterals[literalIndex] % 32;
            		obstacleIndex = alphabetIndex - 1;
            	} else if( level == 2){
            		literalIndex = Random.Range(0, englishTargetLiterals.Count);
            		alphabetIndex = (int) englishTargetLiterals[literalIndex] % 32;
            		obstacleIndex = alphabetIndex - 1;
            	}
            	else {
                	obstacleIndex = Random.Range(0, letterPrefabs.Length);
            	}
                obstacle = letterPrefabs[obstacleIndex];
                spawnPos = obstacle.transform.position;
            }

            //spawn enemy obstacles -- lowest probability
            else {
                obstacleIndex = Random.Range(0, enemyPrefabs.Length);
                obstacle = enemyPrefabs[obstacleIndex];
                spawnPos = obstacle.transform.position;
                spawnPos.z = Random.Range(-6,10);
            }
   
            spawnPos.x = Random.Range(80,90);
            if(gameOver == false){
                Instantiate(obstacle, spawnPos, obstacle.transform.rotation);
            }
        }
    }

    // when player picks up a friendly obstacle (number, operator, alphabet), choose corresponding operation on it
    public void TargetOperation(string tag){
    	if (mathOperations.Contains(tag) || mathNumbers.Contains(tag)){
            MathOperation(tag); 
        }
        else if(letters.Contains(tag)){
        	LetterOperation(tag);
        }
    }

    // if number is picked, do operation on it and calculate result
    // if operator is picked, replace previous operator with current one
    // if result number is equal to target number, increase score and generate new target

    public void MathOperation(string tag){
        // if collected item is an operation, replace previous operator with current one
        if (mathOperations.Contains(tag)){
            operationText.text = "Opr: " + tag.ToUpper();
            currentOperation = tag;
        } 

        // if collected item is a number
        else if (mathNumbers.Contains(tag)){
            // if no current operation, don't do any operation on the number
            if(currentOperation == null || mathResult == 0){
                mathResult = GetMathNumber(tag);
                resultText.text = "Result: " + mathResult;
            }
            else {
                //do math operation based on current operation
                CalculateMathResult(GetMathNumber(tag));
                resultText.text = "Result: " + mathResult;

                if(mathResult == mathsTarget){
                	Score();
                	levelChange();
                	CalculateTarget();
    				mathResult = 0;
    				resultText.text = "Result: ";
                }
            }
        }    
    }

    // returns integer value of the picked number prefab based on its tag
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

    // perform arithmetic operation based on current active operator
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

    // calculate maths or english target
    public void CalculateTarget(){

    	// for maths target, generate a random integer from 10 to 20
        if(currentGameType == 1){
        	mathsTarget = Random.Range(10,21);
        	targetText.text = "Target: " + mathsTarget;
        } 

        // for english target, pick any word from the list of words, each word is from A to Z
        else if(currentGameType == 2){
        	englishTarget = englishTargets[Random.Range(0,englishTargets.Count)];
        	targetText.text = "Target: " + englishTarget;
        	englishTargetLiterals = new List<char>(){};

        	// prepares a list of characters/alphabets present in the target
        	// For e.g. if target is DOG, this list has {"D", "O", "G"}
        	for(int i=0; i < englishTarget.Length; i++){
        		englishTargetLiterals.Add(englishTarget[i]);
        	}
        }
    }

    // if player picks alphabet, add to english result word
    // if result word is equal to target word, increase score and generate new target
    private void LetterOperation(string tag){
    	englishResult += tag;
    	resultText.text = "Result: " + englishResult;
    	ChangeLiteralIndex(tag);
    	Debug.Log(englishResult);
    	if(string.Equals(englishTarget, englishResult)){
    		Score();
    		levelChange();
    		literalIndex = 0;
    		CalculateTarget();
    		englishResult = "";
    		resultText.text = "Result: ";
    	}
    }

    // For level 1, when player picks an alphabet, increment the index so that the next alphabet is spawned
    private void ChangeLiteralIndex(string tag){
    	if(string.Equals(englishTargetLiterals[literalIndex].ToString(), tag)){
    		if(level == 1 && literalIndex < englishTargetLiterals.Count - 1)
    			literalIndex += 1;
    	}
    }

    private void Score(){
    	playerAudio.PlayOneShot(scoreSound, 1.0f);
    	scoreParticle.Play();
        score += 1 * level;
        scoreText.text = "Score: " + score;
        speed += 5;
        speedText.text = "Speed: " + speed;               
    }

    private void levelChange(){
    	int scoreLimit = 0;
    	scoreLimit = currentGameType == 1 ? 0 : 1;
    	if(score > scoreLimit){
    	 	level += 1;
    	 	levelText.text = "Level: " + level;

    	 	if(playerController.currentHealth < playerController.maxHealth){
    	 		playerController.IncreaseHealth(1);
    	 	} 
    	}
    }
}
