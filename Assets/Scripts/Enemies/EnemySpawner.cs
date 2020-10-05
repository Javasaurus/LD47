using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public AnimationCurve enemySpawnValues;
    public BasicEnemy enemyPrefab;
    // Start is called before the first frame update
    private float spawnDelay = 1.5f;
    private float spawnTime;

    public int minSpawn = 3;
    public int maxSpawn = 15;
    public int TimeTillMaxSpawn = 600;

    public int maxCount = 75;
    public List<BasicEnemy> enemies = new List<BasicEnemy>();

    public float spawnRange = 10;
    private void Start()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, spawnRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime)
        {
            int amountToSpawn = (int)(minSpawn + (Random.Range(0, (maxSpawn - minSpawn)) * enemySpawnValues.Evaluate(Time.time / TimeTillMaxSpawn)));
            if (Random.Range(0f, 100f) <= 35f)
            {
                for (int i = 0; i < amountToSpawn; i++)
                {
                    if (enemies.Count >= maxCount)
                    {
                        break;
                    }
                    BasicEnemy instance = GameObject.Instantiate(enemyPrefab);
                    instance.transform.position = transform.position + Random.insideUnitSphere.normalized * (spawnRange);
                    instance.startPosition = instance.transform.position;
                    instance.targetTransform = transform;
                    instance.currentState = BasicEnemy.EnemyState.CHASE;
                    enemies.Add(instance);

                }
            }
            spawnTime = Time.time + spawnDelay;
        }

        enemies.RemoveAll(enemy => enemy.expired);

    }
}
