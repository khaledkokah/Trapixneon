using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//== This class contains all general methods needed to access objects or other functions ==
namespace Assets.Scripts.Helper
{
   public static class GeneralMethods
    {
        //Get child by name 
        public static GameObject GetChildWithName(GameObject obj, string name)
        {
            Transform trans = obj.transform;
            Transform childTrans = trans.Find(name);
            if (childTrans != null)
            {
                return childTrans.gameObject;
            }
            else
            {
                return null;
            }
        }

        //Add listener to a button with sound effect
        public static void AssignSoundToButton(GameObject btn, string clickSFX)
        {
            btn.GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<AudioManager>().Play(clickSFX));
        }

    }
}
