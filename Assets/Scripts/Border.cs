using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//== A border is an object that can be used to destroy anything that collides with it ==
public class Border : MonoBehaviour {

    //Destroy anything that collides with the borders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
