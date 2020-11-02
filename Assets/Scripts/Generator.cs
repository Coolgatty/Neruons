using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using SimpleFileBrowser;

public class Generator : MonoBehaviour
{
    public string filePath;
    string fileName;
    public Button fileNameBox;
    public static byte[] bytes;

    public GameObject neuronPrefab;
    public GameObject nodePrefab;
    bool systemCreated = false;
    List<GameObject> neuronList = new List<GameObject>();
    int BSI = 1;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == BSI && !systemCreated)
        {
            CreateNervousSystem();
            systemCreated = true;
        }
    }

    void UpdateText()
    {
        fileNameBox.GetComponentInChildren<Text>().text = fileName;
    }

    public void OnButtonClicked()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void StartButtonClicked()
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            SceneManager.LoadSceneAsync(BSI);
        }
    }

    private void CreateNervousSystem()
    {
        bytes = File.ReadAllBytes(filePath);
        List<int> byteList = new List<int>();
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(Vector3.one);
        int dim;
        if (bytes.Length < 1000000)
        {
            dim = 1000;
        }
        else if (bytes.Length < 1000000000)
        {
            dim = 1000000;
        }
        else
        {
            dim = 1000000000;
        }

        for (int i = 0; i < bytes.Length / dim; i++)
        {
            byteList.Add(ByteArraySum(SliceByteArray(bytes, dim * i, dim * (i + 1))));
        }

        for (int i = 0; i < byteList.Count; i++)
        {
            positionList.Add(new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)) * byteList[i]);
        }
        for (int i = 0; i < positionList.Count; i++)
        {
            neuronList.Add(Instantiate(neuronPrefab, positionList[i], Quaternion.identity));
        }
        
       for (int i = 0; i < neuronList.Count; i++)
        {
            List<GameObject> nearest = neuronList[i].GetComponent<Neuron>().FindNearest(neuronList);
            for (int j = 0; j < nearest.Count; j++)
            {
                neuronList[i].GetComponent<Neuron>().AddConnection(nearest[j]);
            }
        }
        for (int i = 0; i < neuronList.Count; i++)
        {
           StartCoroutine(neuronList[i].GetComponent<Neuron>().Connect(nodePrefab));
        }
    }

    byte[] SliceByteArray(byte[] array, int startIndex, int endIndex)
    {
        if (endIndex > array.Length)
        {
            endIndex = array.Length;
        }
        byte[] bytes = new byte[endIndex-startIndex];
        int j = 0;
        for (int i = startIndex; i < endIndex; i++)
        {
            bytes[j] = array[i];
            j++;
        }
        return bytes;
    }

    int ByteArraySum(byte[] array)
    {
        int total = 0;
        for (int i = 0; i < array.Length; i++)
        {
            total += array[i];
        }
        return total%1000;
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, true, null, "Load File", "Load");

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);
            filePath = FileBrowser.Result[0];
        }
        fileName = Path.GetFileName(filePath);
        if (!string.IsNullOrEmpty(fileName))
        {
            UpdateText();
        }
    }
}
