using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
	private float speed = 30;
    private GameManagerX gameManagerController;
	private float leftBound = -10;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerController = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManagerController.gameOver == false){
        	transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        if(transform.position.x < leftBound){
        	Destroy(gameObject);
        }
    }
}
