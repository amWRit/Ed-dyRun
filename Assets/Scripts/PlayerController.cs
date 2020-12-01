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

    private Animator playerAnim; 

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound; 
    public AudioClip crashSound; 
    public AudioClip eatSound;

    private AudioSource playerAudio; 

    private GameManagerX gameManagerController;

    private List<string> mathNumbers = new List<string>() {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}; 
    private List<string> mathOperations = new List<string>() {"add", "sub", "mul", "div"}; 


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier; 

        playerAnim = GetComponent<Animator>();

        playerAudio = GetComponent<AudioSource>();

        gameManagerController = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
        gameManagerController.CalculateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        bool gameOver = gameManagerController.gameOver;
        Vector3 startPos = transform.position;
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver){
        	playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        	isOnGround = false;
        	playerAnim.SetTrigger("Jump_trig");
        	dirtParticle.Stop();
        	playerAudio.PlayOneShot(jumpSound, 1.0f);
        }


        if (Input.GetKey(KeyCode.RightArrow) && !gameOver && transform.position.z > -6.5){
            //transform.position = Vector3.Lerp(startPos, startPos + transform.right * slideForce, 10 * Time.deltaTime);
            transform.Translate(Vector3.right * Time.deltaTime * slideForce);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !gameOver && transform.position.z < 6.5){
            transform.Translate(Vector3.right * Time.deltaTime * -slideForce);
        }

        if (Input.GetKey(KeyCode.DownArrow) && !gameOver && transform.position.z < 6.5){;
            transform.Translate(Vector3.forward * Time.deltaTime * -slideForce);
        }
    }

    private void OnCollisionEnter (Collision collision){
    	if (collision.gameObject.CompareTag("Ground")){
    		isOnGround = true;
    		dirtParticle.Play();
    	} else if (collision.gameObject.CompareTag("Obstacle")) {
    		gameManagerController.gameOver = true;
            gameManagerController.GameOver();
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
            gameManagerController.MathOperation(other.tag); 
        } 

        playerAudio.PlayOneShot(eatSound, 1.0f);
        Destroy(other.gameObject);
    }



}
