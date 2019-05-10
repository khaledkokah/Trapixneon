using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//== All logic to a jewel gameobject in game
public class Jewel : MonoBehaviour
{
    //Detect trigger collisions with jewel object
    void OnTriggerEnter2D(Collider2D other)
    {
        //If collided with the player
        if (other.name == "Player")
        {
            //Play jewel collected sfx
            FindObjectOfType<AudioManager>().Play("Jewel");

            //Minimize the total jewels count from scene
            GameManager.jewels -= 1;

            //Destroy jewel game object as it is no longer needed
            Destroy(gameObject);

            //== Optional, you can instantiate a simple explosion prefab for collected jewel

            //Check if this was the last jewel, if yes, then enable the exit
            if (GameManager.jewels ==0)
            {
                //Exit revealed logic
                GameManager.instance.EnableExit();
            }
        }
    }
}
