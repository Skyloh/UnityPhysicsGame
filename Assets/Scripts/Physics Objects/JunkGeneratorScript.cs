using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkGeneratorScript : MonoBehaviour
{

    [SerializeField] private GameObject junk;

    public int num = 10;

    // Start is called before the first frame update
    void Start()
    {

        Vector3 spawnloc = Vector3.up;

        Vector3 randomScale = Vector3.one;

        GameObject temp;

        for (int i = 0; i < num; i++)
        {
            spawnloc.x = Random.Range(-10f, 10f);
            spawnloc.z = Random.Range(-10f, 10f);

            temp = Instantiate(junk, spawnloc, Quaternion.identity);

            temp.transform.localScale = getRandomVectorScale();
        }
    }

    Vector3 getRandomVectorScale()
    {
        return new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
    }
}
