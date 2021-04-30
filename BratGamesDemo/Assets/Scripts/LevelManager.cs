using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MotorcadeGuard
{
    public class LevelManager : MonoBehaviour 
    {
        public static LevelManager instance;
        [SerializeField] private float movespeed; //move our environment with a some constant speed 
        [SerializeField]private GameObject roadPrefab; // to store our road prefab reference
        [SerializeField]private GameObject[] vehiclePrefabs; // to store our all vehicle prefabs 
        [SerializeField] private GameObject[] attacker_prefabs; // to store our all vehicle prefabs 
        public int total_meter = 0;


        private List<GameObject> roadList;
        private Vector3 nextRoadPos = Vector3.zero;
        private GameObject roadHolder; // the sprite which provides us the movement 
        private PlayerController playercontroller;
        private int roadAtLastIndex, roadAtTopIndex;
        private EnemyManager enemy_manager;

        public PlayerController PlayerController { get { return playercontroller; } }
        public GameObject[] vehicleprefabs { get { return vehiclePrefabs; } }
        private void Awake()
        {
            if (instance == null) // if the scene is just (newly) loaded
            {
                instance = this;
            }

            else 
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            roadHolder = new GameObject("RoadHolder"); // the sprite which provides us the movement 
            roadList = new List<GameObject>();
            

            for (int i = 0; i<5;i++)
            {
                GameObject road = Instantiate(roadPrefab,nextRoadPos,Quaternion.identity );  //prefab whatevevr rotation it  has will maintained thanks to quarternion.identity
                road.name = "Road " + i.ToString();
                road.transform.SetParent(roadHolder.transform);//making the road game object child of the road holder game object
                nextRoadPos += Vector3.forward * 10; //all roadholder objects has 10 length 
                roadList.Add(road);
            }

            enemy_manager = new EnemyManager(nextRoadPos, movespeed); 

            SpawnPlayer(); // spawn the player car

            enemy_manager.SpawnEnemies(vehiclePrefabs); //spawn the enemies but not the attacker type ones, the enemies are the ones which exists in the traffic, moves same direction with our convoy

           


        }

        private void SpawnPlayer()
        {

            GameObject player = new GameObject("Player"); //spawning the player 
            player.transform.position = Vector3.zero;
            player.AddComponent<PlayerController>();
            playercontroller = player.GetComponent<PlayerController>();
        }

        private void MoveRoad() // it will be called in update 
        {
            // our road includes 5 roadholder prefab like 0-1-2-3-4 (roadList list includes all them and they are being instantiated in start for loop),
            //whenever a moving vehicle moves into the next prefab in list like from 0th to 1st, the new list will be 1-2-3-4-0 
            for (int i = 0; i < roadList.Count; i++)
            {
                roadList[i].transform.Translate(-transform.forward * movespeed * Time.deltaTime); //negative transform.forward because we need to our road in backward direction
            }

            /////////so at the move the last one which is just left by our vehicle will be the new top one

         
                if (roadList[roadAtLastIndex].transform.position.z <= -10) // distance between center points of two roadholder prefabs is 10 
                {
                    roadAtTopIndex = roadAtLastIndex - 1;

                    if (roadAtTopIndex < 0) // the topindex is 4 at the beginning and the last index is 0, the last index indicates the roadholder prefab which vehicle currently exists and the last index is will be the 
                    {
                        roadAtTopIndex = roadList.Count - 1;
                    }

                    roadList[roadAtLastIndex].transform.position = roadList[roadAtTopIndex].transform.position + Vector3.forward * 10;
                    roadAtLastIndex++;
                    if (GameManager.singleton.gameStatus == GameStatus.PLAYING)
                    {
                        total_meter = total_meter + 10;
                       string level = "level " + PlayerPrefs.GetInt("unlocked_level")+" |"+total_meter.ToString(); //the string that will exists in the game playing panel
                        UiManager.instance.set_dist_text(level);
                    }
                    if (roadAtLastIndex >= roadList.Count)
                    {
                        roadAtLastIndex = 0;

                    }

                }


                if (1> Random.Range(0, 2500)) //instantiating attacker type enemies randomly. But their velocity is not random and while user passes the levels, velocity increases
                {
                    enemy_manager.InstantiateAttacker(attacker_prefabs);
                

            }

            if (total_meter == 200*PlayerPrefs.GetInt("unlocked_level")) //all levels are 200 meter, whenever user reaches currentLevel*200 meter, then it means it win the level and ready for the next
            {


                PlayerPrefs.SetInt("unlocked_level", (PlayerPrefs.GetInt("unlocked_level")+1));
                UiManager.instance.Win();
            }
            


        }

        private void Update()
        {
            if (GameManager.singleton.gameStatus != GameStatus.FAILED) //as long as it is not in failed status, continue to move
            { 
            MoveRoad();
            
            }

           
        }

        public void GameStarted()
        {
            GameManager.singleton.gameStatus = GameStatus.PLAYING;
            enemy_manager.ActivateEnemy();
            playercontroller.gamestarted();

            if (!PlayerPrefs.HasKey("unlocked_level")) // if there is no unlocked level exist then initialize 1
            {

                PlayerPrefs.SetInt("unlocked_level", 1);

            }
            total_meter = 100 * (PlayerPrefs.GetInt("unlocked_level")-1);
            string level = "level " + PlayerPrefs.GetInt("unlocked_level") + " |" + total_meter.ToString();
            
            UiManager.instance.set_dist_text(level);
        }

        private void OnTriggerEnter(Collider other) // there exists invisible enemycontroller type object and whenever our player hits it, randomly  enemies and attacker types enemies are spawning into the map and gets activated 
        {
            
            if (2>Random.Range(0, 6)) //can be used for making harder 
            {
                enemy_manager.ActivateEnemy();
            }
            
            if (other.GetComponent<EnemyController>())
            {

                enemy_manager.ActivateEnemy();
                
            }

        }

        public void Gameover()
        {
            GameManager.singleton.gameStatus = GameStatus.FAILED;

            Camera.main.transform.DOShakePosition(1f, Random.insideUnitCircle.normalized, 5, 10, false, true).OnComplete  // FOR CAMERA SHAKE
                (
                 () => UiManager.instance.GameOver()

            );

        }

        

    }

}