using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move enemy obstacles (boxes, bombs etc) across the road
public class MoveEnemy : MonoBehaviour
{
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
        	transform.Translate(Vector3.left * Time.deltaTime * gameManagerController.speed);
        }

        if(transform.position.x < leftBound){
        	Destroy(gameObject);
        }
    }
}
