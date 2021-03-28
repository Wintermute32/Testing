using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockConstructor : MonoBehaviour
{
    [SerializeField] Block[] blockPrefabs;
    [SerializeField] float totalClusterValue;
    [SerializeField] GameObject spawnParentInvis; //parent obj for block cluster

    //cached
    Vector3[] vertexArr;
    Block currentBlock;

    public void ConstructBlockCluster()
    {
        ArrangeBlocks(blockPrefabs);
    }

    private Block[] ChooseBlocks(float totalClusterValue)
    {
        //random collectin algorithm here to pick blocks.
        return null;
    }

    private void ArrangeBlocks(Block[] chosenBlocks)
    {
        Vector3 spawnPos;
        Block coreBlock;

        for (int i = 0; i < chosenBlocks.Length; i++)
        {
            currentBlock = chosenBlocks[i];

            if (i == 0)
            {
                //currentBlock.transform.localScale = spawnParentInvis.transform.localScale;
                coreBlock = Instantiate(currentBlock, spawnParentInvis.transform.position, transform.rotation, spawnParentInvis.transform);
                vertexArr = BlockBounds(coreBlock);
            }

            if (i > 0)
            {
                Vector3 startCorner = vertexArr[Random.Range(0, 4)];
                Vector3 stopCorner = vertexArr[Random.Range(0, 4)];

                //confirm we haven't picked same corner twice or opposite corners
                while (startCorner == stopCorner || (startCorner.x - stopCorner.x != 0 && startCorner.y - stopCorner.y != 0))
                {
                    startCorner = vertexArr[Random.Range(0, 4)];
                    stopCorner = vertexArr[Random.Range(0, 4)];
                }


                //if corners lie on same y value, pick random value for x.
                if (startCorner.y - stopCorner.y == 0)
                {
                    spawnPos = new Vector3(Random.Range(startCorner.x, stopCorner.x), startCorner.y, 0);

                    //offset the chosen y value to align edges
                    float yOffset = 0f;

                    if (spawnPos.y < spawnParentInvis.transform.position.y)
                        yOffset = -currentBlock.GetComponent<BoxCollider2D>().size.y / 2 * currentBlock.transform.localScale.y;

                    if (spawnPos.y > spawnParentInvis.transform.position.y)
                        yOffset = currentBlock.GetComponent<BoxCollider2D>().size.y / 2 * currentBlock.transform.localScale.y;

                    spawnPos = new Vector3(spawnPos.x, spawnPos.y + yOffset, 0); //adding offset y value
                }

                //if chosen corners lie on same x value, we pick a random y.
                else if (startCorner.x - stopCorner.x == 0)
                {
                    spawnPos = new Vector3(startCorner.x, Random.Range(startCorner.y, stopCorner.y));

                    float xOffset = 0f;

                    if (spawnPos.x < spawnParentInvis.transform.position.x)
                        xOffset = -currentBlock.GetComponent<BoxCollider2D>().size.x / 2 * currentBlock.transform.localScale.x;
                    if (spawnPos.x > spawnParentInvis.transform.position.x)
                        xOffset = currentBlock.GetComponent<BoxCollider2D>().size.x / 2 * currentBlock.transform.localScale.x;

                    spawnPos = new Vector3(spawnPos.x + xOffset, spawnPos.y, 0);
                }

                else
                {
                    //this should never be called if prior logic works
                    spawnPos = new Vector3(1, 1, 0);
                }


                var newestBlock = Instantiate(currentBlock, spawnPos, transform.rotation, spawnParentInvis.transform);

                //AlignBlock(newestBlock);

            }

        }

    }
    public Vector3[] BlockBounds(Block coreBlock)
    {
     //gets bounds on specificed block
     Vector3 spawnPosCenter = coreBlock.GetComponent<BoxCollider2D>().bounds.center;
     Vector3 spawnPosBounds = Vector3.Scale((coreBlock.GetComponent<BoxCollider2D>().size / 2), coreBlock.transform.localScale);


     Vector3 topLeft = new Vector3(spawnPosCenter.x - spawnPosBounds.x, spawnPosCenter.y + spawnPosBounds.y, 0);
     Vector3 topRight = new Vector3(spawnPosCenter.x + spawnPosBounds.x, spawnPosCenter.y + spawnPosBounds.y, 0);
     Vector3 bottomLeft = new Vector3(spawnPosCenter.x - spawnPosBounds.x, spawnPosCenter.y - spawnPosBounds.y, 0);
     Vector3 bottomRight = new Vector3(spawnPosCenter.x + spawnPosBounds.x, spawnPosCenter.y - spawnPosBounds.y, 0);

     var vecCornerArr = new Vector3[4] {topLeft, topRight, bottomLeft, bottomRight};
        return vecCornerArr;

    }

    /*private void AlignBlock(Block block)
   {

       /*

       Collider2D blockCollider = block.GetComponent<BoxCollider2D>();


       Collider2D[] hitColliders = Physics2D.OverlapCircleAll(validityChecker.position, radius);

       blockCollider.ClosestPoint()

       blockCollider.IsTouching()

       Vector3.Distance()

   }*/

}
