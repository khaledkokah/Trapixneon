using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//== Platform game object than can carry player based on https://noobtuts.com/unity
public class Platform : MonoBehaviour
{
    //Called as long as something stays on the platform's Collider
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Make the player gameobject a child of platform
        if (collision.name == "Player")
            collision.transform.parent = transform;
    }

    //Called whenever something leaves the platform's Collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.parent = null;
    }
}
