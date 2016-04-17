using ProgressBar;
using System.Collections;
using UnityEngine;

public class ProgressScript : MonoBehaviour
{
    ProgressBarBehaviour BarBehaviour;
    float UpdateDelay = 2f;

    IEnumerator Start()
    {
        BarBehaviour = GetComponent<ProgressBarBehaviour>();
        while (true)
        {
            yield return new WaitForSeconds(UpdateDelay);
            BarBehaviour.Value = Random.value * 100;
            print("new value: " + BarBehaviour.Value);
        }
    }
}
