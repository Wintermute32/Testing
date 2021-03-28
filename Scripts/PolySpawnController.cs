using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolySpawnController : MonoBehaviour
{
    [SerializeField] Block[] blockSpawnObjects;
    [SerializeField] float totalClusterValue;
    [SerializeField] float countDownToRespawn;

    [Tooltip("This Object should contain our animator/rigidbody for the cluster")]
    [SerializeField] GameObject blockClusterParentObject;

    //cached refs
    GameObject spawnParentInvis;
    PolyConstructor polyConstructor;
    float timeCounter;

    private void Start()
    {
        polyConstructor = GetComponent<PolyConstructor>();
    }

    void Update()
    {
       if (CheckClusterRespawn())
        {
            SpawnParentInvis();
            polyConstructor.ConstructBlockCluster(blockSpawnObjects, spawnParentInvis);
        }
    }
    public Block[] ChooseBlocks(float totalClusterValue)
    {

        return null;
    }
    public void SpawnParentInvis()
    {
        spawnParentInvis = Instantiate(blockClusterParentObject, transform.position, transform.rotation);
    }

    public bool CheckClusterRespawn()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= countDownToRespawn)
        {
            timeCounter = 0;
            return true;
        }
        return false;
    }
}
