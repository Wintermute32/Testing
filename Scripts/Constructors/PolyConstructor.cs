using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyConstructor : MonoBehaviour
{
    [Tooltip("Determines whether blocks spawn in linear order, or spawn pos is randomized between instantiated blocks")]
    [SerializeField] bool endToEndSpawn;
   
    //cached
    Vector2[] vertexArr;
    Block currentBlock;
    Block newestBlock;
    List<Block> existingBlocks = new List<Block>();
    ContactFilter2D noFilter = new ContactFilter2D().NoFilter();

    //Takes a block array and parent object. Spawns chosenBlocks ontop of spawnParent
    public void ConstructBlockCluster(Block[] chosenBlocks, GameObject spawnParentInvis)
    {
        Vector3 spawnPos;

        for (int i = 0; i < chosenBlocks.Length; i++)
        {
            currentBlock = chosenBlocks[i];

            if (i == 0)
            {
                newestBlock = Instantiate(currentBlock, spawnParentInvis.transform.position, transform.rotation, spawnParentInvis.transform);
                vertexArr = BlockBounds(newestBlock);
                existingBlocks.Add(newestBlock);
            }

            if (i > 0)
            {
                Vector2[] validCorners = GetValidCorners(vertexArr);
                spawnPos = GetSpawnPos(validCorners);
                newestBlock = Instantiate(currentBlock, spawnPos, transform.rotation, spawnParentInvis.transform);
                //ResetForOverlap(newestBlock);

                existingBlocks.Add(newestBlock);
                SpawnFormat();
            }
        }
    }
    private void SpawnFormat()
    {
        if (endToEndSpawn)
            vertexArr = BlockBounds(newestBlock);
        else
           vertexArr = BlockBounds(existingBlocks[Random.Range(0, existingBlocks.Count)]);
    }
    private void ResetForOverlap(Block newestBlock)
    {
        int overlapCount = GetOverlapCount(newestBlock);
        while (overlapCount > 3)
        {
            vertexArr = BlockBounds(existingBlocks[Random.Range(0, existingBlocks.Count)]);
            Vector2[] validCorners = GetValidCorners(vertexArr);
            Vector3 spawnPos = GetSpawnPos(validCorners);

            Debug.Log("Moving Block");
            newestBlock.transform.position = spawnPos;
            overlapCount = GetOverlapCount(newestBlock);
        }
    }
    private int GetOverlapCount(Block newestBlock)
    {
        Collider2D[] overlapArr = new Collider2D[existingBlocks.Count];
        PolygonCollider2D newestColider = newestBlock.GetComponent<PolygonCollider2D>();
        int count = newestColider.OverlapCollider(noFilter, overlapArr);
        return count;
    }

    private Vector3 GetSpawnPos(Vector2[] validCorners)
    {
        Vector3 spawnPos;
        var startCorner = validCorners[0];
        var stopCorner = validCorners[1];

        var polyCollider = currentBlock.GetComponent<PolygonCollider2D>();

        float xValue = 0;
        float yValue = 0;

        if (startCorner.y - stopCorner.y == 0)
        {
            float yOffset = 0f;

            if (startCorner.y < newestBlock.transform.position.y)
                yOffset = -(polyCollider.points[0].y * currentBlock.transform.localScale.y); //index 0 has pos y value

            if (startCorner.y > newestBlock.transform.position.y)
                yOffset = (polyCollider.points[0].y * currentBlock.transform.localScale.y);

             xValue = Random.Range(startCorner.x, stopCorner.x);
             yValue = startCorner.y + yOffset;
        }

        else if (startCorner.x - stopCorner.x == 0)
        {
            float xOffset = 0f;

            if (startCorner.x < newestBlock.transform.position.x)
                xOffset = -(polyCollider.points[2].x * currentBlock.transform.localScale.x); //index 2 has pos x value
            if (startCorner.x > newestBlock.transform.position.x)
                xOffset = (polyCollider.points[2].x) * currentBlock.transform.localScale.x;

            yValue = Random.Range(startCorner.y, stopCorner.y);
            xValue = startCorner.x + xOffset;
        }

        else
        {
            //this should never be called if prior logic works
            Debug.Log("Errored GetSpawnPos Default values called.");
            return spawnPos = new Vector3(1, 1, 0);
        }
        return spawnPos = new Vector3(xValue, yValue);
    }
    private Vector2[] GetValidCorners(Vector2[] vectorArr)
    {
        Vector2 startCorner = vectorArr[Random.Range(0, vectorArr.Length)];
        Vector2 stopCorner = vectorArr[Random.Range(0, vectorArr.Length)];

        //confirm we haven't picked same corner twice or opposite corners
        while (startCorner == stopCorner || (startCorner.x - stopCorner.x != 0 && startCorner.y - stopCorner.y != 0))
        {
            startCorner = vectorArr[Random.Range(0, vectorArr.Length)];
            stopCorner = vectorArr[Random.Range(0, vectorArr.Length)];
        }
        Vector2[] validCorners = new Vector2[2] { startCorner, stopCorner };
        return validCorners;
    }
    public Vector2[] BlockBounds(Block coreBlock)
    {
     PolygonCollider2D blockPolyCollider = coreBlock.GetComponent<PolygonCollider2D>();
     Vector2[] cornerVectors = blockPolyCollider.points;
     
     for (int i = 0; i < cornerVectors.Length; i++)
       cornerVectors[i] = coreBlock.transform.TransformPoint(cornerVectors[i]);
   
        return cornerVectors;
    }
    public Collider2D[] OverlappingBlocks(Block coreBlock)
    {
        //get array of overlapping blocks after 
        Collider2D[] colliderArray = new Collider2D[7];
        Collider2D overlap = coreBlock.GetComponent<PolygonCollider2D>();
        overlap.OverlapCollider(new ContactFilter2D(), colliderArray);
        return colliderArray;
    } 
}


/*public void GetTouchingColliders(Block block)
    {

        var corePos = block.transform.position;
        var blockCollider = block.GetComponent<PolygonCollider2D>();
        var overLapArr = Physics.OverlapBox(block.transform.position, blockCollider.bounds.extents, Quaternion.identity);

        Vector3.Distance(coreBlock.transform.position,)


        int minDistance ;
        Block closestBlock;

        foreach (var x in overLapArr)
        {
            var coreCenter = coreBlock.GetComponent<PolygonCollider2D>().ClosestPoint(corePos);

            if ( < minDistance)
                closestBlock = 

                Vector3.Distance(coreBlock.transform.position, corePos);

        }
  



    }   */

/*private void AlignBlock(Block block)
{

   /*

   Collider2D blockCollider = block.GetComponent<PolygonCollider2D>();


   Collider2D[] hitColliders = Physics2D.OverlapCircleAll(validityChecker.position, radius);

   blockCollider.ClosestPoint()

   blockCollider.IsTouching()

   Vector3.Distance()

}*/