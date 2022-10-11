using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Character;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

	public int Damage = 80;
	public EnemyHealth Enemyhealth;

	void OnTriggerEnter (Collider PlayerCollider)
	{


		if (PlayerCollider.tag.Equals ("Player") && Enemyhealth.health > 0) {
			var health = PlayerCollider.GetComponentInParent<UltimateCharacterLocomotion>();
			if(health != null) {
				health.GetComponent<CharacterHealth>().Damage(10);
			}

			// CapsuleColliderPositioner player_capsule = PlayerCollider.gameObject.GetComponent<CapsuleColliderPositioner> ();
			// Transform player = player_capsule.FirstEndCapTarget;
			// Health	playerHealth = PlayerCollider.gameObject.GetComponent<Health> ();
			
			// playerHealth.SetDamage (Damage);

		}

	}



}
