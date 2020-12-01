using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private Rigidbody playerRb;
	public float jumpForce;
    public float gravityModifier;
    public float slideForce;

    public bool isOnGround = true;
    public bool gameOver = false;

    private Animator playerAnim; 

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound; 
    public AudioClip crashSound; 
    public AudioClip eatSound;

    private AudioSource playerAudio; 

    private SpawnManager spawnManagerController;
    public TextMeshProUGUI operationText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI scoreText;

    private List<string> mathNumbers = new List<string>() {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}; 
    private List<string> mathOperations = new List<string>() {"add", "sub", "mul", "div"}; 
    private float mathResult = 0.0f;
    public List<string> collectedItems = new List<string>();

    private string currentOperation;
    private string currentNumber;

    private float mathsTarget;
    private float score;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier; 

        playerAnim = GetComponent<Animator>();

        playerAudio = GetComponent<AudioSource>();

        spawnManagerController = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        CalculateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = transform.position;
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver){
        	playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        	isOnGround = false;
        	playerAnim.SetTrigger("Jump_trig");
        	dirtParticle.Stop();
        	playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !gameOver && transform.position.z > -6.5){
            //transform.position = Vector3.Lerp(startPos, startPos + transform.right * slideForce, 10 * Time.deltaTime);
            transform.Translate(Vector3.right * Time.deltaTime * slideForce);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !gameOver && transform.position.z < 6.5){
            transform.Translate(Vector3.right * Time.deltaTime * -slideForce);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !gameOver && transform.position.z < 6.5){;
            transform.Translate(Vector3.forward * Time.deltaTime * -slideForce);
        }
    }

    private void OnCollisionEnter (Collision collision){
    	if (collision.gameObject.CompareTag("Ground")){
    		isOnGround = true;
    		dirtParticle.Play();
    	} else if (collision.gameObject.CompareTag("Obstacle")) {
    		gameOver = true;
            spawnManagerController.GameOver();
    		Debug.Log("Game Over");
    		playerAnim.SetBool("Death_b", true);
    		playerAnim.SetInteger("DeathType_int", 1);
    		explosionParticle.Play();
    		playerAudio.PlayOneShot(crashSound, 1.0f);
    		dirtParticle.Stop();
    	}
    }

    private void OnTriggerEnter(Collider other)
    {
         
        if (mathOperations.Contains(other.tag) || mathNumbers.Contains(other.tag)){
            Debug.Log(other.tag);
            MathOperation(other.tag); 
        } 

        playerAudio.PlayOneShot(eatSound, 1.0f);
        Destroy(other.gameObject);
    }


    private void MathOperation(string tag){
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
                    score += 1;
                    scoreText.text = "Score: " + score;
                }
            }
        }    
    }

    private int GetMathNumber(string tag){
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

    private void CalculateTarget(){
        mathsTarget = Random.Range(10,20);
        targetText.text = "Target: " + mathsTarget;
    }
}
