using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class RandomAgent : MonoBehaviour
{
    bool actionExecute = true;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (actionExecute)
        {
            StartCoroutine(doAction());
        }
            
    }

    private IEnumerator doAction()
    {
        actionExecute = false;
        rb.velocity = new Vector3( Random.Range(-0.6f, 0.8f), 0, Random.Range(-0.8f, 0.8f)) * 30f;

        if (FrisbeeController.instance.lastHolder == "Ennemy")
        {
            if (Random.Range(0, 10) > 3)
            {
                FrisbeeController.instance.Shoot();
            }
        }
        yield return new WaitForSeconds(1f);
        actionExecute = true;
    }
}
