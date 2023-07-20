using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] fruitPrefab;

    [SerializeField] float minTras;

    [SerializeField] float maxTras;

    [SerializeField] float secondSpawn = 2f;


    private void Start()
    {
        StartCoroutine(FruitSpawn());
    }


    IEnumerator FruitSpawn()
    {
        while (true)
        {
            var wanted = Random.Range(minTras, maxTras);
            var position = new Vector3(wanted, transform.position.y, transform.position.z);
            GameObject newFruit = Instantiate(fruitPrefab[Random.Range(0, fruitPrefab.Length)], position, Quaternion.identity);


            yield return new WaitForSeconds(secondSpawn);
            //Destroy(newFruit, 5f);
        }
    }
}
