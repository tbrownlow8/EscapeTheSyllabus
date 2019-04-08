using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MonoBehaviour
	{
		public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
		public AudioClip moveSound1;				//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;				//2 of 2 Audio clips to play when player moves.
		public AudioClip eatSound1;					//1 of 2 Audio clips to play when player collects a food object.
		public AudioClip eatSound2;					//2 of 2 Audio clips to play when player collects a food object.
		public AudioClip drinkSound1;				//1 of 2 Audio clips to play when player collects a soda object.
		public AudioClip drinkSound2;				//2 of 2 Audio clips to play when player collects a soda object.
		public AudioClip gameOverSound;             //Audio clip to play when player dies.

        private Animator animator;					//Used to store a reference to the Player's animator component.
        public Rigidbody2D rb;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.
#endif


        //Start overrides the Start function of MovingObject
        void Start()
        {
            //Get a component reference to the Player's animator component
            animator = GetComponent<Animator>();
            for (int i = 1; i <= 6; i++)
            {
                if (PlayerLocation.enemies[i] == false)
                {
                    Destroy(GameObject.Find("Enemy" + i));
                }
            }

        }

        private void Awake()
        {

        }
        //This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable ()
		{

		}


		private void Update ()
		{
		    if (PlayerLocation.updatePlayer)
            {
                if (QuestionStats.wasCorrect)
                {
                    Debug.Log("was correct");
                    Destroy(GameObject.Find(PlayerLocation.enemy));
                    PlayerLocation.enemies[PlayerLocation.enemy[5] - '0'] = false;
                    QuestionStats.questionNumber++;
                    transform.position = new Vector3(PlayerLocation.x, PlayerLocation.y);
                }
                else
                {
                    transform.position = new Vector3(-7.47f, -3.34f);
                }
                PlayerLocation.updatePlayer = false;
            }
		}




        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.name == "Exit")
            {
                //this is saving state for quiz questions
                QuestionStats.questionNumber = 1;
                PlayerLocation.enemies = new bool[] { true, true, true, true, true, true, true };

                if (GameManager.instance.level == 3) {
                    //win
										GameObject.Find("Winner").GetComponent<Canvas>().enabled = true;
                } else {
                    GameManager.instance.level++;
                    DatabaseUtil.instance.updateCurrentLevel(FirebaseChecks.instance.GetUserId(), GameManager.instance.level);
                    SceneManager.LoadScene("Level " + GameManager.instance.level);

                }
            }

            for (int i = 1; i <= 6; i++)
            {
                if (other.name == "Enemy" + i)
                {
                    PlayerLocation.enemy = other.name;
                    Debug.Log("hit enemy " + i);
                    PlayerLocation.x = transform.position.x;
                    PlayerLocation.y = transform.position.y;
                    SceneManager.LoadScene("Level " + QuestionStats.currentLevel + " Questions");
                }
            }
		}


		//Restart reloads the scene when called.
		private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
            //and not load all the scene object in the current scene.
						// SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}


		//LoseFood is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.

	}
}
