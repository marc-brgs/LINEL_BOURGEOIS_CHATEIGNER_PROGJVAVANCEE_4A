using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class RandomAgent : MonoBehaviour
{
    private bool actionExecute = true;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(doAction());
    }

    // Update is called once per frame
    void Update()
    {
        if (actionExecute)
            doAction();
    }

    private IEnumerator doAction()
    {
        actionExecute = false;
        Debug.Log("Action");
        yield return new WaitForSeconds(2f);
        actionExecute = true;
    }
}
