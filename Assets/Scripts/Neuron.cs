using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour
{
    static List<GameObject> connectors = new List<GameObject>();
    List<GameObject> connections = new List<GameObject>();
    
    public void AddConnection(GameObject neuron)
    {
        connections.Add(neuron);
    }

    public List<GameObject> FindNearest(List<GameObject> neurons)
    {
        List<GameObject> nearest_objects = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            float nearest_distance = -1;
            GameObject nearest = null;
            for (int j = 0; j < neurons.Count; j++)
            {
                float distance = Vector3.Distance(transform.position, neurons[j].transform.position);
                if ((distance < nearest_distance || nearest_distance == -1) && distance != 0f && !nearest_objects.Contains(neurons[j]))
                {
                    nearest_distance = distance;
                    nearest = neurons[j];
                }
            }
            if (nearest != null)
            {
                nearest_objects.Add(nearest);
            }
        }
        return nearest_objects;
    }

    public IEnumerator Connect(GameObject prefab)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            
            Vector3 direction = (connections[i].transform.position - transform.position).normalized;

            int length = (int)(Vector3.Distance(connections[i].transform.position, transform.position));
            List<GameObject> node_list = new List<GameObject>();
            
            for (int k = 0; k < 1; k++)
            {
                GameObject node = Instantiate(prefab, transform.position + direction * length/2 * (k + 1), Quaternion.identity);
                node.transform.localScale = new Vector3(0.5f, length, 0.5f);
                node_list.Add(node);
            }
            if (node_list.Count > 0)
            {
                for (int k = 0; k < node_list.Count - 1; k++)
                {
                    node_list[k].GetComponent<Node>().SetNext(node_list[k + 1]);
                }
                node_list[node_list.Count - 1].GetComponent<Node>().SetNext(connections[i]);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
