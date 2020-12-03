using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
	public Vector3 startPos;
	private float repeatWidth;
    private GameManagerX gameManagerController;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerController = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x/2;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManagerController.gameOver == false){
            transform.Translate(Vector3.forward * Time.deltaTime * -gameManagerController.speed);
        }

        if (transform.position.x < startPos.x - repeatWidth){
        	transform.position = startPos;
		}
    }
}
