using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace MotorcadeGuard
{

    public class UiManager : MonoBehaviour
    {
        public static UiManager instance;

        //thanks to serialize field, these objects can be editted from inspector
        [SerializeField] private GameObject mainMenuPanel, gameMenuPanel, gameOverPanel,WinPanel;
        [SerializeField] private GameObject selectPanelMenu, selectHolder, carHolder;

        [SerializeField] private Text distanceText;


        //getter for our distance text
        public Text DistanceText { get { return distanceText; }  }
        public void set_dist_text(string dist_Text_updated)
        {
            distanceText.text = dist_Text_updated.ToString();
        }
        
        private int currentCarIndex; //to tell us which car we have selected in our selectiom panel

        private Vector3 startCarHolderPos; //whenever our game starts, we are just going to set our car holder position 

       
        private void OnDisable()
        {
            //unsubscribing to callback
            InputManager.instance.swipeCallback -= ActionOnSwipe;

        }

        private void Awake()
        {
            if (instance == null) // if the scene is just (newly) loaded
            {
                instance = this;
            }
           

        }

        private void Start()
        {
            GameManager.singleton.gameStatus = GameStatus.NONE;
            InputManager.instance.swipeCallback += ActionOnSwipe;
            currentCarIndex = GameManager.singleton.currentCarIndex;
            startCarHolderPos = carHolder.transform.position;
            carHolder.transform.position -= Vector3.right * 8 * currentCarIndex;
            PopulateCars();
           
            

        }

        public void PlayButton()
        {
            WinPanel.SetActive(false);

            mainMenuPanel.SetActive(false); // when clicked on play button, then main menu panel should be deactivated
            gameMenuPanel.SetActive(true); // and game menu panel should set activated to true

            LevelManager.instance.GameStarted();

        }

        public void RetryButton() //retry button attached to button
        {
           
            gameOverPanel.SetActive(false);
            GameManager.singleton.gameStatus = GameStatus.PLAYING;
            LevelManager.instance.GameStarted();
            

        }

        public void GameOver() //gameover pnael
        {
            gameOverPanel.SetActive(true);
        }

        public void Win() //win panel
        {
            gameMenuPanel.SetActive(false);
            WinPanel.SetActive(true);
            GameManager.singleton.gameStatus = GameStatus.FAILED;
        }

        public void openSelectionPanel(bool val) //selection panel
        {
            if (val)
            {
                mainMenuPanel.SetActive(false);
                selectPanelMenu.SetActive(true);
                selectHolder.SetActive(true);
                
                SelectCarPos();

            }

            else
            {
                mainMenuPanel.SetActive(true);
                selectPanelMenu.SetActive(false);
                selectHolder.SetActive(false);


            }


        }

        void SelectCarPos() // car selection panel, iterating over the possible cars
        {
            float newXpos = startCarHolderPos.x - 8 * currentCarIndex;
            carHolder.transform.DOMoveX(newXpos, 0.5f);


        }

        public void ResetLevels() // reseting unlocked level informations and setting it 1, attached to button
        {

            PlayerPrefs.SetInt("unlocked_level", 1);
        }

        public void SelectCarButton() //car selection panel, select car button
        {
            GameManager.singleton.currentCarIndex = currentCarIndex;
            LevelManager.instance.PlayerController.SpawnVehicle(GameManager.singleton.currentCarIndex);
            openSelectionPanel(false);

        }

        void PopulateCars() // for showing cars in the car selection panel
        {
            for (int i =0; i< LevelManager.instance.vehicleprefabs.Length; i++)
            {
                GameObject car = Instantiate(LevelManager.instance.vehicleprefabs[i], carHolder.transform);
                car.transform.Rotate(new Vector3(0,-150,0));
                car.transform.localPosition = Vector3.right * i * 8;
            }
        
        }

        void ActionOnSwipe(SwipeType sw )
        {
            switch (sw)
            {
                case SwipeType.RIGHT:
                    if(currentCarIndex >0) 
                    {
                        currentCarIndex--;
                    }
                    break;
                case SwipeType.LEFT:
                    if (currentCarIndex < LevelManager.instance.vehicleprefabs.Length-1)
                    {
                        currentCarIndex++;
                    }
                    break;

            }
            SelectCarPos(); // will move our car holder to respected car's pos
                

        }

    }
}