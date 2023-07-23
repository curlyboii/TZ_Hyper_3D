using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] fruitPrefab;
    [SerializeField] float minTras;
    [SerializeField] float maxTras;
    [SerializeField] float secondSpawn = 2f;
    [SerializeField] GameObject spawnEffectPrefab; // Particle System Prefab

    private Coroutine fruitSpawnCoroutine; // Store the reference to the coroutine

    public void StartFruitSpawn()
    {
        if (fruitSpawnCoroutine == null)
        {
            fruitSpawnCoroutine = StartCoroutine(FruitSpawn());
        }
    }

    public void StopFruitSpawn()
    {
        if (fruitSpawnCoroutine != null)
        {
            StopCoroutine(fruitSpawnCoroutine);
            fruitSpawnCoroutine = null;
        }
    }

    IEnumerator FruitSpawn()
    {
        while (true)
        {
            if (!GameStartManager.instance.isTaskCompletedGet)
            {
                var wanted = Random.Range(minTras, maxTras);
                var position = new Vector3(transform.position.x, transform.position.y, wanted);

                if (fruitPrefab != null && fruitPrefab.Length > 0)
                {
                    GameObject newFruit = Instantiate(fruitPrefab[Random.Range(0, fruitPrefab.Length)], position, Quaternion.identity);
                    if (spawnEffectPrefab != null)
                    {
                        Instantiate(spawnEffectPrefab, position, Quaternion.identity);
                    }
                }

                yield return new WaitForSeconds(secondSpawn);
            }
            else
            {
                // Task is completed, stop spawning fruits
                yield break;
            }
        }
    }
}
