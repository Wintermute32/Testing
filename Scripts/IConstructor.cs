using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConstructor
{
    void ConstructBlockCluster(Block[] chosenBlocks);
    Block[] ChooseBlocks(float totalClusterValue);



}
