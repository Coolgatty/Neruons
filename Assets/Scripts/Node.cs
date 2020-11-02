using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject next = null;
    // Start is called before the first frame update

    public void SetNext(GameObject node)
    {
        next = node;
    }

    void Orientate()
    {
        if(next != null)
        {
            Vector3 direction = (next.transform.position - transform.position).normalized;
            transform.up = direction;
            //transform.GetComponent<ParticleSystem>().transform.rotation = transform.rotation;
        }
    }

    private void Start()
    {
        Orientate();
    }
}
