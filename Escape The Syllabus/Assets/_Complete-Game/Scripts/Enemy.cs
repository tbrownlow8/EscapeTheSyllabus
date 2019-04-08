using UnityEngine;
using System.Collections;

namespace Completed
{
	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
	public class Enemy : MonoBehaviour
	{
		public int playerDamage; 							//The amount of food points to subtract from the player when attacking.
		public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
		public AudioClip attackSound2;						//Second of two audio clips to play when attacking the player.
		
		
		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
		
		
		//Start overrides the virtual Start function of the base class.
		void Start ()
		{
			
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//Find the Player GameObject using it's tag and store a reference to its transform component.
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			
		}
        private void Awake()
        {
            
        }






    }
}
