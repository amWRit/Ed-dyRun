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

    public int maxHealth = 4;
    public int currentHealth;

    public bool isOnGround = true;  

    private Animator playerAnim; 

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound; 
    public AudioClip crashSound; 
    public AudioClip eatSound;

    private AudioSource playerAudio; 

    private GameManagerX gameManagerController;
    public HealthManager healthManager;

    private List<string> mathNumbers = new List<string>() {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}; 
    private List<string> mathOperations = new List<string>() {"add", "sub", "mul", "div"}; 

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthManager.SetMaxHealth(maxHealth);

        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier; 

        playerAnim = GetComponent<Animator>();

        playerAudio = GetComponent<AudioSource>();

        gameManagerController = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
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


        if (Input.GetKey(KeyCode.RightArrow) && !gameOver && transform.position.z > -7.5){
            transform.Translate(Vector3.right * Time.deltaTime * slideForce);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !gameOver && transform.position.z < 7.5){
            transform.Translate(Vector3.right * Time.deltaTime * -slideForce);
        }

        if (Input.GetKey(KeyCode.DownArrow) && !gameOver && transform.position.x > -2){;
            transform.Translate(Vector3.forward * Time.deltaTime * -slideForce);
        }

    }

    private void OnCollisionEnter (Collision collision){

    	if (collision.gameObject.CompareTag("Ground")){
    		isOnGround = true;
    		dirtParticle.Play();
    	} else if (collision.gameObject.CompareTag("Obstacle")) {
            TakeDamage(1);
            playerAudio.PlayOneShot(crashSound, 1.0f);
            explosionParticle.Play();

            if(currentHealth == 0){
        		gameManagerController.gameOver = true;
                gameManagerController.GameOver();
        		playerAnim.SetBool("Death_b", true);
        		playerAnim.SetInteger("DeathType_int", 1);
        		explosionParticle.Play();
        		dirtParticle.Stop();
            }
    	}
    }

    private void OnTriggerEnter(Collider other)
    {
        // perform maths or english operation based on the tag of the collided object
        gameManagerController.TargetOperation(other.tag);  
        playerAudio.PlayOneShot(eatSound, 1.0f);
        Destroy(other.gameObject);
    }

    private void TakeDamage(int damage){
        currentHealth -= damage;
        healthManager.SetHealth(currentHealth);
    }

    public void IncreaseHealth(int health){
        currentHealth += health;
        healthManager.SetHealth(currentHealth);
    }
}
