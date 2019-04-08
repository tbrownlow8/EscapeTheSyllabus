﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Completed
{
	using System.Collections.Generic;		//Allows us to use Lists.
	using UnityEngine.UI;					//Allows us to use UI.

	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.1f;							//Delay between each Player turn.
		public int playerFoodPoints = 100;						//Starting value for Player food points.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		[HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.


		private Text levelText;									//Text to display current level number.
		private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		public int level = 1;									//Current level number, expressed in game as "Day 1".
		public int levelsCompleted = 0;
		public int correctAnswers = 0;
		public int incorrectAnswers = 0;
		public int deaths = 0;
		private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.


		//Awake is always called before any Start functions
		void Awake()
		{
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);


			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();

			//Call the InitGame function to initialize the first level
			InitGame();

		}



        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        // static public void CallbackInitialization()
        // {
        //     //register the callback to be called everytime the scene is loaded
        //     SceneManager.sceneLoaded += OnSceneLoaded;
				//
        // }

        //This is called each time a scene is loaded.
//         static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
//         {
//             if (arg0.name.Equals("EscapeTheSyllabusUI"))
//             {
//
// 								if (instance.levelsCompleted < instance.level) {
// 									instance.levelsCompleted = instance.level;
// 									DatabaseUtil.instance.updateLevelsCompleted(FirebaseChecks.instance.GetUserId(), instance.levelsCompleted);
// 								}
// 								instance.level++;
//
//                 // updates level in database
//                 DatabaseUtil.instance.updateCurrentLevel(FirebaseChecks.instance.GetUserId(), instance.level);
//                 if (instance.level > 1)
//                 {
//                     GameObject.Find("Screens").SetActive(false);
//                 }
//
//                 instance.InitGame();
// }
//         }


		//Initializes the game for each level.
		void InitGame()
		{
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;

			//Get a reference to our image LevelImage by finding it by name.
			if (levelImage == null) {
				levelImage = GameObject.Find("LevelImage");

			}
			if (levelText == null) {
				levelText = GameObject.Find("LevelText").GetComponent<Text>();

			}


			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			// levelText = GameObject.Find("LevelText").GetComponent<Text>();

			//Set the text of levelText to the string "Day" and append the current level number.
			// levelText.text = "Day " + level;
			levelText.text = "Level " + level;

			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);

			// only enable canvas is game is active
			if (GameObject.Find("Screens") == null) {
				levelImage.transform.parent.gameObject.GetComponent<Canvas>().enabled=(true);
			}

			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);


			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
		}




		//Hides black image used between levels
		void HideLevelImage()
		{
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);

			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		}

		//Update is called every frame.
		void Update()
		{
			
		}

		


		
		
	}
}
