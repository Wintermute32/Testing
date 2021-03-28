using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeConstructor : MonoBehaviour
{
    [SerializeField] Block[] blockPrefabs;
    [SerializeField] float totalClusterValue;
    [SerializeField] GameObject spawnParentInvis; //parent obj for block cluster

    //cached
    Vector2[] vectorArr;
    Block currentBlock;
    Block coreBlock;
    Vector3 spawnPos;

    public void ConstructBlockCluster()
    {
        ArrangeBlocks(blockPrefabs);
    }
   
    private Block[] ChooseBlocks(float totalClusterValue)
    {
        //random collectin algorithm here to pick blocks.
        return null;
    }

    //https://answers.unity.com/questions/1456235/how-to-get-specific-path-in-a-composite-collider2d.html
    private void ArrangeBlocks(Block[] chosenBlocks)
    {
        for (int i = 0; i < chosenBlocks.Length; i++)
        {
            currentBlock = chosenBlocks[i];

            if (i == 0)
            {
                coreBlock = Instantiate(currentBlock, spawnParentInvis.transform.position, transform.rotation, spawnParentInvis.transform);
                vectorArr = GetCompositePoints(spawnParentInvis.gameObject);
            }

            if (i > 0)
            {
                    vectorArr = GetCompositePoints(spawnParentInvis.gameObject);
                    var validCorners = GetValidCorners(vectorArr);
                    var spawnPos = DetermineOffset(validCorners, currentBlock);
                    var newBlock = Instantiate(currentBlock, spawnPos, transform.rotation, spawnParentInvis.transform);
            }
        }

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

    private Vector3 DetermineOffset(Vector2[] validCorners, Block currentBlock) //takes valid corners and adds offset, returning final spawn pos
    {
        Vector2 startCorner = validCorners[0];
        Vector2 stopCorner = validCorners[1];

        var centerBounds = spawnParentInvis.GetComponent<CompositeCollider2D>().bounds.center;

        Debug.Log("Chosen Values :" + startCorner + stopCorner);

        //if corners lie on same y value, pick random value for x.
        if (startCorner.y - stopCorner.y == 0)
        {
            spawnPos = new Vector3(Random.Range(startCorner.x, stopCorner.x), startCorner.y, 0);

            //offset the chosen y value to align edges
            float yOffset = 0f;

            if (spawnPos.y < centerBounds.y)
            {
                //yOffset = -(currentBlock.GetComponent<PolygonCollider2D>().bounds.size.y/2 * currentBlock.transform.localScale.y);
                yOffset = -(currentBlock.GetComponent<PolygonCollider2D>().points[0].y) * (currentBlock.transform.localScale.y) + .001f; //added slight .001 offest to make sure compostie collider = 1 path;
           
            }

            if (spawnPos.y > centerBounds.y)
            {
                yOffset = (currentBlock.GetComponent<PolygonCollider2D>().points[0].y) * (currentBlock.transform.localScale.y) - .001f;
     
            }

            spawnPos = new Vector3(spawnPos.x, spawnPos.y + yOffset, 0); //adding offset y value
        }

        //if chosen corners lie on same x value, we pick a random y.
        else if (startCorner.x - stopCorner.x == 0)
        {
            spawnPos = new Vector3(startCorner.x, UnityEngine.Random.Range(startCorner.y, stopCorner.y));

            float xOffset = 0f;

            if (spawnPos.x < centerBounds.x)
            {
                xOffset = -(currentBlock.GetComponent<PolygonCollider2D>().points[2].x) * (currentBlock.transform.localScale.x) + .001f; //index 2 has pos x value
       
            }
            if (spawnPos.x > centerBounds.x)
            {
                xOffset = (currentBlock.GetComponent<PolygonCollider2D>().points[2].x) * (currentBlock.transform.localScale.x) - .001f;
            }

            spawnPos = new Vector3(spawnPos.x + xOffset, spawnPos.y, 0);
           
        }
        else
        {
            //this should never be called if prior logic works
            spawnPos = new Vector3(0, 0, 0);
        }

        return spawnPos;
    }


    public Vector2[] GetCompositePoints(GameObject parentObject)
    {
        int totalPoints = 0;

        CompositeCollider2D compoColldier = parentObject.GetComponent<CompositeCollider2D>();
  

        Vector2 spawnPosCenter = compoColldier.bounds.center;
        Debug.Log("Center bounds on object " + spawnPosCenter);

        compoColldier.transform.position = parentObject.transform.position;

        Vector2[] points = new Vector2[compoColldier.GetPathPointCount(0)];
        totalPoints = compoColldier.GetPath(0, points);

        Debug.Log("Total Collider Points :" + totalPoints);

        for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + spawnPosCenter;

        foreach (var x in points)
            Debug.Log("Point values for object vertexes include : " + x);


        return points;
    }

}



//What is it I want to do now?

//1) Generate the total dimensions of the composite collider after the first block placement.
//2) Save those Vector values in an array.
//3) Run that array through our ArrangeBlock method to make sure we're picking the correct edge values.
//4) Ideally, this instantiates blocks according to random edge value.
//5) this won't prevent overlapping but might make interesting arrangements.


//Return from AZ notes:
// ATM, this is doing the same that the other constructor scripts do. Should spawn all blocks, with no overlap.
//But when trying to re-input the new collider to get the updated points, the blocks go haywire. Need to fix/figure out
//how to update the composite colider with it's newly added values and spawn the next block along that range of new values.




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