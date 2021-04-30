using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace MotorcadeGuard
{
    public class EnemyController : MonoBehaviour
    {
        private float movingSpeed;
        private EnemyManager enemyManager;

        public void SetDefault(float speed, EnemyManager enemy_manager)
        {
            movingSpeed = speed;
            this.enemyManager = enemy_manager;
 
        }


        private void Update()
        {
            if (GameManager.singleton.gameStatus == GameStatus.PLAYING) // if game started to being played 
            {
                transform.Translate(-transform.forward * movingSpeed * 0.8f * Time.deltaTime); //reduced the moving speed by multiplying with 0.8f

                if (transform.position.z <= -10) // if our vehicle is no more in the vision of our player, then we can deactivate it
                {
                    enemyManager.DeactivateEnemy(this); //because enemies are type of enemycontroller actually and in this way we are parsing specific enemy to be deactiavted

                }


            }
        }

        public void ActivateEnemy()
        {

            gameObject.SetActive(true); 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "guard")
            {
                DOTween.Kill(this);
                Destroy(this.gameObject); // destroy the attacker which collide to us

                DOTween.Kill(other);
                Rigidbody mybody_ = other.gameObject.GetComponent<Rigidbody>();
                mybody_.isKinematic = false;
                mybody_.useGravity = true;
                mybody_.AddForce(Random.insideUnitCircle.normalized * 100f); // inside Unit circle make sure that the force will be added only in x and y direction and we are normalizing it so that value will be between 0 and 1 
                other.isTrigger = true; // so our car wont fall down to under the ground after the collision

                






            }

            else if (other.gameObject.name=="attacker") // if the attacker type of enemy collides into the our guard, then both will be destroyed
            {
                Destroy(this.gameObject);
                Destroy(other.gameObject);
            
            }
        }

    }
}