using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  


namespace MotorcadeGuard
{
    [RequireComponent(typeof(Rigidbody))]  // ensures that whenever we attach this script to any game object, rigidbody component will be attached to it
    public class PlayerController : MonoBehaviour
    {
        private float endXPos = 0; //storing the new x position of our car 
        private Rigidbody mybody; //to store our rigidbody
        private Collider colliderComponent;
        [HideInInspector] public List<GameObject> Convoy_List= new List<GameObject>(); //for providing joint movement to the convoy 
        

        private void OnDisable() 
        {
            InputManager.instance.swipeCallback -= SwipeMethod;

        }

        void Start()
        {
           
            mybody = gameObject.GetComponent<Rigidbody>();
            mybody.isKinematic = true; 
            mybody.useGravity = false; // we dont want any gravity or any external force that will acted to our game object in game play
            SpawnVehicleFirst(GameManager.singleton.currentCarIndex); // when we were creating our game manager, we added this variable to it    
        }

        public void gamestarted() // will be called by our level manager 
        {
            InputManager.instance.swipeCallback += SwipeMethod; // so we are subscribing to swipecallback event 
        }

        public void SpawnVehicle(int index) // our player controller game object will have only one child  that will be the 3d model of our car  and  as we are going to have only one child, we are going to get reference to child at index 0 
        {
            if (transform.childCount > 0)

            {
                Destroy(transform.GetChild(0).gameObject);
            }

            GameObject child = Instantiate(LevelManager.instance.vehicleprefabs[index],transform); // we need reference our level manager vehicle prefab array as well 
            // make sures that the new game object which is called, will be the child of this gama object.

            colliderComponent = child.GetComponent<Collider>();
            colliderComponent.isTrigger = true; // so we can detect enemy cars in the game if a collision will occur in that collider's component then it will go into ontrigger method
            

            
        }

        public void SpawnVehicleFirst(int index)
        {
            if (transform.childCount > 0)

            {
                Destroy(transform.GetChild(0).gameObject);
            }

            GameObject child = Instantiate(LevelManager.instance.vehicleprefabs[index], transform); // we need reference our level manager vehicle prefab array as well 
            // make sures that the new game object which is called, will be the child of this gama object.

            colliderComponent = child.GetComponent<Collider>();
            colliderComponent.isTrigger = true; // so we can detect enemy cars in the game if a collision will occur in that collider's component then it will go into ontrigger method



            Vector3 pos_limo = child.transform.position;
            Vector3 a = new Vector3(pos_limo.x + 1.5f, pos_limo.y , pos_limo.z+2);
            Vector3 b = new Vector3(pos_limo.x + 1.5f, pos_limo.y , pos_limo.z-2);
            Vector3 c = new Vector3(pos_limo.x - 1.5f, pos_limo.y , pos_limo.z+2);
            Vector3 d = new Vector3(pos_limo.x - 1.5f, pos_limo.y, pos_limo.z-2);
            Vector3 e = new Vector3(pos_limo.x+  1, pos_limo.y , pos_limo.z+5);
            Vector3 f = new Vector3(pos_limo.x - 1, pos_limo.y , pos_limo.z+5);

            GameObject child_guard = Instantiate(LevelManager.instance.vehicleprefabs[LevelManager.instance.vehicleprefabs.Length - 1], a, Quaternion.identity); // we need reference our level manager vehicle prefab array as well 
            GameObject child_guard1 = Instantiate(LevelManager.instance.vehicleprefabs[LevelManager.instance.vehicleprefabs.Length - 1], b, Quaternion.identity); // we need reference our level manager vehicle prefab array as well 
            GameObject child_guard2 = Instantiate(LevelManager.instance.vehicleprefabs[LevelManager.instance.vehicleprefabs.Length - 1], c, Quaternion.identity); // we need reference our level manager vehicle prefab array as well 
            GameObject child_guard3 = Instantiate(LevelManager.instance.vehicleprefabs[LevelManager.instance.vehicleprefabs.Length - 1], d, Quaternion.identity); // we need reference our level manager vehicle prefab array as well 
            GameObject child_guard4 = Instantiate(LevelManager.instance.vehicleprefabs[LevelManager.instance.vehicleprefabs.Length - 1], e, Quaternion.identity); // we need reference our level manager vehicle prefab array as well 
            GameObject child_guard5 = Instantiate(LevelManager.instance.vehicleprefabs[LevelManager.instance.vehicleprefabs.Length - 1], f, Quaternion.identity); // we need reference our level manager vehicle prefab array as well 

            Convoy_List.Add(child_guard);
            Convoy_List.Add(child_guard1);
            Convoy_List.Add(child_guard2);
            Convoy_List.Add(child_guard3);
            Convoy_List.Add(child_guard4);
            Convoy_List.Add(child_guard5);

            for (int i = 0; i < Convoy_List.Count; i++)
            {
                Collider k;
                k = Convoy_List[i].GetComponent<Collider>();
                k.isTrigger = true; // so we can detect enemy cars in the game if a collision will occur in that collider's component then it will go into ontrigger method
                Convoy_List[i].name = "guard";
                Rigidbody mybody_ = Convoy_List[i].GetComponent<Rigidbody>();
                mybody_.isKinematic = true;
                //mybody_.useGravity = true;


            }

        }



        void SwipeMethod(SwipeType sw)// determine if user moves car to right or left
        {
            int direction =0;
            switch (sw)
            {

    
                case SwipeType.RIGHT:
                    endXPos = transform.position.x + 1.5f;
                    direction = +1;
                    break;
                case SwipeType.LEFT:
                    endXPos = transform.position.x - 1.5f;
                    direction = -1;
                    break;
             
            }



            //endXPos = Mathf.Clamp(endXPos, -3, 3);
            if (-6 <= endXPos &&  endXPos <= 6)
            {
                transform.DOMoveX(endXPos, 0.15f);
                for (int i = 0; i < Convoy_List.Count; i++)
                {
                    if ((Convoy_List[i].transform.position.x + (direction * 1.5f) > 6 || -6  >Convoy_List[i].transform.position.x + (direction * 1.5f))) // if the guards moves out from the road then it will be destroyed
                    {
                        DOTween.Kill(Convoy_List[i].gameObject); // killing the guard
                        Destroy(Convoy_List[i].gameObject); //killing the guard 
                        Convoy_List[i] = null; //  gameobject which just destroyed, is made equal to null for ease in rearranging the guard array

                        List<GameObject> new_convoy_list = new List<GameObject>();
                        
                        foreach( var game_obj in Convoy_List) // rearrange the guard array
                        {
                            if (game_obj != null)
                            {

                                new_convoy_list.Add(game_obj);
                            }

                            


                        }

                        Convoy_List.Clear();
                        Convoy_List = new_convoy_list;

                    }

                    else { // if the guard's next position which it tries to go, is not in out of the road, then it will move that position
                        Convoy_List[i].transform.DOMoveX(Convoy_List[i].transform.position.x + (direction * 1.5f), 0.15f); 

                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other) // to check for our enemy 
        {
            if (other.GetComponent<EnemyController>())
            {
                if (GameManager.singleton.gameStatus == GameStatus.PLAYING) // we should check if our game status is playing or not
                {
                    DOTween.Kill(this); // any dotween which is happening on this game object will get killed 
                    LevelManager.instance.Gameover();
                    
                    mybody.isKinematic = false;
                    mybody.useGravity = true;
                    mybody.AddForce(Random.insideUnitCircle.normalized * 100f); // inside Unit circle make sure that the force will be added only in x and y direction and we are normalizing it so that value will be between 0 and 1 
                    colliderComponent.isTrigger = false; // so our car wont fall down to under the ground after the collision
                }

            }

            else if (Convoy_List.Contains(this.gameObject))
            {

                DOTween.Kill(this);
            }
            
        }

       
    }

    

    //whenever our input manager detects a swipe from inputmanager.cs, it calls swipecallback and will parse the swipe type 
    // and as our playercontroller is subscribed to it, it will listen to that event and it will call the SwipeMethod from InputManager.instance.swipeCallback += SwipeMethod; statement

}