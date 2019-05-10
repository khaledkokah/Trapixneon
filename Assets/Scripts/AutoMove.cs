using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//== AutoMove class creates automatic movement to the assigned object ==
namespace Assets.Scripts
{
    class AutoMove : MonoBehaviour
    {
        //Array containing all the waypoints
        public GameObject[] myWaypoints;

        //A slider in editor to limit move speed
        [Range(0.0f, 10.0f)]

        //Move speed
        public float moveSpeed = 5f;

        //How long to wait at a waypoint before moving to next waypoint
        public float waitAtWaypointTime = 1f;

        //Should it loop through the waypoints?
        public bool loop = true; 

        //Object transform
        Transform m_Transform;

        //Used as index for My_Waypoints
        int m_MyWaypointIndex = 0;       
        float m_MoveTime;

        //Detect if object is currently moving or not
        bool m_Moving = true;

        void Start()
        {
            m_Transform = gameObject.transform;
            m_MoveTime = 0f;
            m_Moving = true;
        }

        //Game loop
        void Update()
        {
            // if beyond _moveTime, then start moving
            if (Time.time >= m_MoveTime)
            {
                Movement();
            }
        }

        //Movement implementation 
        void Movement()
        {
            //If there isn't anything in my_Waypoints
            if ((myWaypoints.Length != 0) && (m_Moving))
            {
                // move towards waypoint
                m_Transform.position = Vector3.MoveTowards(m_Transform.position, myWaypoints[m_MyWaypointIndex].transform.position, moveSpeed * Time.deltaTime);

                // if the enemy is close enough to waypoint, make it's new target the next waypoint
                if (Vector3.Distance(myWaypoints[m_MyWaypointIndex].transform.position, m_Transform.position) <= 0)
                {
                    m_MyWaypointIndex++;
                    m_MoveTime = Time.time + waitAtWaypointTime;
                }

                // reset waypoint back to 0 for looping, otherwise flag not moving for not looping
                if (m_MyWaypointIndex >= myWaypoints.Length)
                {
                    if (loop)
                        m_MyWaypointIndex = 0;
                    else
                        m_Moving = false;
                }
            }
        }
    }
}