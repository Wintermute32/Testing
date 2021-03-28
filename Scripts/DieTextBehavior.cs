using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieTextBehavior : MonoBehaviour
{
    public IEnumerator RandomizeSymbol()
    {
        GetComponent<TMP_Text>().text = Random.Range(0, 10).ToString();
        yield return new WaitForSeconds(3);
    }
   
}
