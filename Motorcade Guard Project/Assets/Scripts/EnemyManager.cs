using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotorcadeGuard
{
    // responsible for activating and deactivating enemies 
    public class EnemyManager 
    {

        private List<EnemyController> deactivateEnemyList; // all deactive ones
        private Vector3[] enemySpawnPos = new Vector3[9];   // Possible positions array for spawning enemies, it is being filled below, with 9 vector3 positions because there are 9 lines
        private GameObject enemyHolder;
        private float moveSpeed;

        public EnemyManager(Vector3 spawnPos, float movingSpeed) //constructor
        {
            this.moveSpeed = movingSpeed;
            deactivateEnemyList = new List<EnemyController>();

            
           
            

            //it is being filled below, with 9 vector3 positions because there are 9 lines
            enemySpawnPos[0] = spawnPos - Vector3.right * 6; // spawn pos is position of the center line
            enemySpawnPos[1] = spawnPos - Vector3.right * 4.5f;
            enemySpawnPos[2] = spawnPos - Vector3.right * 3;
            enemySpawnPos[3] = spawnPos - Vector3.right * 1.5f;
            enemySpawnPos[4] = spawnPos;
            enemySpawnPos[5] = spawnPos + Vector3.right * 1.5f;
            enemySpawnPos[6] = spawnPos + Vector3.right * 3;
            enemySpawnPos[7] = spawnPos + Vector3.right * 4.5f;
            enemySpawnPos[8] = spawnPos + Vector3.right * 6;


            enemyHolder = new GameObject("EnemyHolder");
                

        }

        public void SpawnEnemies(GameObject[] vehicle_Prefabs) //spawning enemies but not activating them 
        {


            for (int i =0; i<vehicle_Prefabs.Length;i++)
            {
                GameObject enemy = Object.Instantiate(vehicle_Prefabs[i],enemySpawnPos[Random.Range(0,enemySpawnPos.Length)],Quaternion.identity);
                enemy.SetActive(false); //not using it just creating thats why false 
                enemy.transform.SetParent(enemyHolder.transform);
                enemy.name = "Enemy";
                enemy.AddComponent<EnemyController>();
                enemy.GetComponent<EnemyController>().SetDefault(moveSpeed,this); // taking it with getcomponent because we defined enemy as gameobject here, rather ten enemy controller type 

                Collider collidercomp = enemy.GetComponent<Collider>();
                collidercomp.isTrigger = true;
                deactivateEnemyList.Add(enemy.GetComponent<EnemyController>());
                

            }
        }

        public void InstantiateAttacker(GameObject [] attacker_list)
        {
          
            float x = 4.5f;
            float y = 4.5f;
            float z = 4.5f;
            if (1 > Random.Range(0, 4000))
            {
                 x = 4.5f;
                 y = 4.5f;
                 z = 4.5f;
            }
            Vector3 attacker_pos = new Vector3(4.5f, 0, 0);
            GameObject attacker = GameObject.Instantiate(attacker_list[0], attacker_pos,Quaternion.identity);
           
            attacker.transform.localRotation=Quaternion.Euler(0, 180, 0); // rotating the attacker car 180 degrees because the attackers will come from opposite direction
            attacker.name = "attacker";
            attacker.AddComponent<EnemyController>();

            attacker.GetComponent<EnemyController>().SetDefault(PlayerPrefs.GetInt("unlocked_level")*12.5f, this);// attackers get faster while user passes the levels by multiplying its velocity with current level number

            Collider collidercomp = attacker.GetComponent<Collider>();
            collidercomp.isTrigger = true;
            
        }
        public void ActivateEnemy()
        {
            if (deactivateEnemyList.Count>0) 
            {
                EnemyController enemy = deactivateEnemyList[Random.Range(0, deactivateEnemyList.Count)]; // randomly activating an enemy
                deactivateEnemyList.Remove(enemy);
                enemy.transform.position = enemySpawnPos[Random.Range(0, enemySpawnPos.Length)];
                enemy.ActivateEnemy();


            }


        }

        public void DeactivateEnemy(EnemyController enemy)
        {
            enemy.gameObject.SetActive(false);
            deactivateEnemyList.Add(enemy);


        }



    }

}