using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpScript : MonoBehaviour
{
    GameObject player;
    Rigidbody2D playerBody;
    Collider2D playerCollider;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBody = player.GetComponent<Rigidbody2D>();
        playerCollider = player.GetComponent<Collider2D>();
    }
    IEnumerator OnTriggerEnter2D()
    {
        if (gameObject.tag.Equals("speed"))
        {
            Debug.Log("Entered");
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);//WE MAKE IT DISSAPEAR VISUALLY AT LEAST
            Singleton.Instance.playerSpeed *= 2f;
            yield return new WaitForSeconds(3);
            Singleton.Instance.playerSpeed /= 2f;
        }
        else if(gameObject.tag.Equals("intan"))
        {
            Debug.Log("intangible");
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            
            playerCollider.isTrigger = true;//TO MAKE IT 'PHASE' THROUGH EVERYTHING
            //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -9);
            playerBody.gravityScale = 0;
            switch(SceneManager.GetActiveScene().buildIndex){
                case 1: playerBody.velocity = new Vector2(playerBody.velocity.x, -2); break;
                case 2: playerBody.velocity = new Vector2(playerBody.velocity.x, 2); break;
            }
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0);
            playerBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            Singleton.Instance.keysEnabled = false;
            
            yield return new WaitForSeconds(3);

            playerCollider.isTrigger = false;
            //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            playerBody.gravityScale = 1.5f;
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);//REMOVE THE Y VELOCITY
            playerBody.constraints = RigidbodyConstraints2D.None;
            Singleton.Instance.keysEnabled = true;
            
        }
		else if(gameObject.tag.Equals("point"))
			Singleton.Instance.currentPlayer.playerScore += 10;
        
        Destroy(gameObject);//WHEN DESTROY IS CALLED THE SCRIPT IS ALSO DESTROYED SINCE IT IS ATTACHED TO THIS GAMEOBJECT
    }
}



