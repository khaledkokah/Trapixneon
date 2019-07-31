using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//== PlayerController contains all the movement and collision logic for the player, extended version of noobtuts
public class PlayerController : MonoBehaviour
{
    //Move speed
    public float speed = 0.1f;

    //Current movement
    Vector2 m_Move = Vector2.zero;

    //Player's death explosion particle
    public GameObject explosionPrefab;

    //Indicate if the player has won or is dead 
    bool isWin = false;
    bool isDead = false;

    //Move directions enum for WebGL movement
    private enum Direction { None, Up, Down, Left, Right };

    //Variable contains the desired direction for WebGL movement (default to Direction.None)
    private Direction m_Direction = Direction.None;

    //Indicate if the player currently moving
    public bool IsMoving()
    {
        return m_Move != Vector2.zero;
    }

    private void Start()
    {
        //Get movement buttons for WebGL movement
    #if UNITY_WEBGL 
        AddPointerEvents(GameObject.FindGameObjectWithTag("BtnUp").GetComponent<Button>(), "Up");
        AddPointerEvents(GameObject.FindGameObjectWithTag("BtnDown").GetComponent<Button>(), "Down");
        AddPointerEvents(GameObject.FindGameObjectWithTag("BtnLeft").GetComponent<Button>(), "Left");
        AddPointerEvents(GameObject.FindGameObjectWithTag("BtnRight").GetComponent<Button>(), "Right");
    #endif
    }

    //Method to add pojnterDown and pointerUp event entries for movement buttons
    private void AddPointerEvents(Button btn, string direction)
    {
        //Define pointerDown, pointerUp the entries for event trigger
        var pointerDown = new EventTrigger.Entry();
        var pointerUp = new EventTrigger.Entry();

        //Create the PointerDown Trigger
        EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => SetDir(direction));
    
        //Create the PointerUp Trigger
        trigger = btn.gameObject.AddComponent<EventTrigger>();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => SetDir("None"));

        //Add entries to the trigger
        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);

    }

    //FixedUpdate for physics and movement logic
    void FixedUpdate()
    {
        //Is player currently moving
        if (IsMoving())
        {
            //Remember the current position
            Vector2 pos = transform.position;

            //Move a bit further using the speed variable
            transform.position = Vector2.MoveTowards(pos, pos + m_Move, speed);

            //Subtract stepsize from move vector
            m_Move -= (Vector2)transform.position - pos;
        }
        // Otherwise allow for next move
        else
        {
            //Check if the game is not finished
            if (!GameManager.gameFinished)
            {
                //Check the axis values for horizontal and vertical movements
                //Also check the boundaries for the level before moving 
                if ((CrossPlatformInputManager.GetAxisRaw("Vertical") > 0 || m_Direction == Direction.Up) && Math.Round(transform.position.y) <= 5.5)
                    m_Move = Vector2.up; //Up

                else if ((CrossPlatformInputManager.GetAxisRaw("Horizontal") > 0 || m_Direction == Direction.Right) && Math.Round(transform.position.x) <= 3)
                    m_Move = Vector2.right; //Right

                else if ((CrossPlatformInputManager.GetAxisRaw("Vertical") < 0 || m_Direction == Direction.Down) && Math.Round(transform.position.y) >= -4.5)
                    m_Move = -Vector2.up; //Down

                else if ((CrossPlatformInputManager.GetAxisRaw("Horizontal") < 0 || m_Direction == Direction.Left) && Math.Round(transform.position.x) >= -3)
                    m_Move = -Vector2.right;//Left
               
                
            }
        }

        //Animation Parameters based on current player state
        if (isWin)
        {
            //Stop movement animation triggers and play win animation
            GetComponent<Animator>().SetFloat("X", 0);
            GetComponent<Animator>().SetFloat("Y", 0);
            GetComponent<Animator>().SetBool("Win", true);
        }
        else
        {
            //Continue movement animation triggers and apply animation speed
            GetComponent<Animator>().SetFloat("X", m_Move.x);
            GetComponent<Animator>().SetFloat("Y", m_Move.y);
            GetComponent<Animator>().speed = Convert.ToSingle(IsMoving());
        }
    }

    //Check tirgger collisions for the player 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player collided with the Exit object
        if (collision.name == "Exit")
        {
            //First check if the jewels count is zero
            if (GameManager.jewels == 0)
            {
                //If yes, then level won
                isWin = true;
                FindObjectOfType<AudioManager>().Play("Win");
                GameManager.instance.LevelWon();
            }
        }
        //If the player collided with enemy
        else if (collision.gameObject.name == "Enemy")
        {
            //Check to see if the player is not already dead
            if (!isDead)
            {
                //Set the player state to dead 
                isDead = true;
                
                //Instantiate the death particle prefab
                Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);

                //Destroy the player object as it's no longer needed
                Destroy(gameObject);

                //Play the player death sfx
                FindObjectOfType<AudioManager>().Play("Death");

                //Call LevelLose to handle levelLose events
                GameManager.instance.LevelLose();
            }
        }
        //If the player collided with poison
        else if (collision.gameObject.tag == "Poison")
        {
            //Check to see if the player is not already dead (same applies like enemy)
            if (!isDead)
            {
                isDead = true;
                Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
                FindObjectOfType<AudioManager>().Play("Death");
                GameManager.instance.LevelLose();
            }
        }
    }

    //Set direction function for WebGL movement
    public void SetDir(string dir)
    {
        //Output this to console when the Button2 is clicked
        switch (dir)
        {
            case "Up":
                m_Direction = Direction.Up;
                break;
            case "Down":
                m_Direction = Direction.Down;
                break;
            case "Left":
                m_Direction = Direction.Left;
                break;
            case "Right":
                m_Direction = Direction.Right;
                break;
            default:
                m_Direction = Direction.None;
                break;
        }
    }


}
