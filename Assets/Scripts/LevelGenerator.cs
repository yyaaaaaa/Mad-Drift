using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
     private float areaWidth = 6000f;
     private float areaHeight = 6000f;
    [SerializeField] private int numberOfObjects = 80;
     private float minScale = 10f;
     private float maxScale = 11f;
    public GameObject coin;
    private int coinsToSpawn = 1000; 
    private void Start()
    {
        for(int i = 0; i < coinsToSpawn; i++)
        {
            float x = Random.Range(-areaWidth / 2, areaWidth / 2);
            float y = Random.Range(-areaHeight / 2, areaHeight / 2);
            Instantiate(coin,new Vector3(x,3f,y),Quaternion.identity);
        }
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Choose a random prefab from the array
            int index = Random.Range(0, prefabs.Length);
            GameObject prefab = prefabs[index];

            // Choose a random position within the area
            float x = Random.Range(-areaWidth / 2, areaWidth / 2);
            float y = Random.Range(-areaHeight / 2, areaHeight / 2);
            Vector3 position = transform.position + new Vector3(x, 0f, y);

            // Choose a random scale and rotation
            float scale = Random.Range(minScale, maxScale);
            Vector3 rotation = new Vector3(0f, Random.Range(0f, 360f), 0f);

            // Instantiate the prefab at the chosen position with random scale and rotation
            GameObject newObject = Instantiate(prefab, position, Quaternion.Euler(rotation));
            newObject.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaWidth, 0f, areaHeight));
    }
}
