using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingProjectiles : MonoBehaviour
{
    public List<GameObject> projectiles;
    private float FirstSpawnTime = 5.0f;
    public float SpawnInterval = 2.0f;
    public int ProjectilesToSpawn = 3;

    private void Start()
    {
        InvokeRepeating("SpawnProjectiles", FirstSpawnTime, SpawnInterval);
    }

    private void SpawnProjectiles()
    {
        for (int i = 0; i < ProjectilesToSpawn; i++)
        {
            int rngNumber = Random.Range(0, projectiles.Count);
            var direction = Random.insideUnitCircle.normalized;
            GameObject go = GameObject.Instantiate(projectiles[rngNumber], this.transform.position, this.transform.rotation);
            go.GetComponent<Projectile>().dir = direction;
            go.GetComponent<Projectile>().GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
            GameObject.Destroy(go, 5.0f);
        }
    }
}
