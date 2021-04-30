using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotorcadeGuard
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager singleton;

        //hide inspector for we dont want them to change from inspector
        [HideInInspector]public GameStatus gameStatus = GameStatus.NONE;
        [HideInInspector]public int currentCarIndex = 0;
        private void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
                DontDestroyOnLoad(gameObject);  //default method, it ensures that the object that this scripts attached will not get destroyed 
            }
            else
            {

                Destroy(gameObject);

            }
        }
    }
}