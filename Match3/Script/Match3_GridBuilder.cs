using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Match3_GridBuilder : MonoBehaviour
{
    //public GameObject cellPrefab; // Prefab for the UI element
    public Transform gridParent; // Parent transform for the grid
    public Vector3 cellSize = new Vector3(100f, 100f, 100f); // Size of each cell
    public Vector3 spacing = new Vector3(10f, 10f, 10f); // Spacing between cells

    // Example array dimensions
    public int[,] array2D = new int[3, 3];
    public Vector3Int array3DSize = new Vector3Int(3, 3, 3);

    // Element settings
    public GameObject[] cellPrefabs; //Prefabs for the UI elements

    int levelnum = 2;
    string fileName = "leveldata1_20.txt"; // Name of the text file
    public int[,] integers; // Variable to store the integers from the text file

    void LoadIntegersFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        StartCoroutine(LoadIntegersRoutine(filePath));
    }

    System.Collections.IEnumerator LoadIntegersRoutine(string filePath)
    {
        // Use different methods for different platforms
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            // For Android and iOS, use UnityWebRequest
            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    ProcessText(www.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("Failed to load file: " + www.error);
                }
            }
        }
        else
        {
            // For PC, use StreamReader
            StreamReader reader = new StreamReader(filePath);
            string text = reader.ReadToEnd();
            reader.Close();

            ProcessText(text);
        }
    }

    void ProcessText(string text)
    {
        // Split the text into lines
        string[] lines = text.Split('\n');

        int levelIndex =0;
        //Find level num
        for (int i = 0; i < lines.Length; i++)
        {
            int parsedInt;
            if (int.TryParse(lines[i], out parsedInt))
            {
                // Parsing successful
               // Debug.Log("Parsing successful: " + parsedInt);
                if(parsedInt == -(levelnum))
                {
                    levelIndex = i;
                    break;
                }
            }
            else
            {
                // Parsing failed
                //Debug.LogWarning("Parsing failed for: " + lines[i]);
            }


        }

        if (levelIndex != 0)
        {
            //Index to start reading grid parms and grid
            int readWidthLine = levelIndex + 1;
            int readLengthLine = levelIndex + 2;
            int readGridLine = levelIndex + 3;

            //Next two lines are width and length
            int width = int.Parse(lines[readWidthLine]);
            int length = int.Parse(lines[readLengthLine]);

            int readGridUpToLine = length + readGridLine;

            //3d Grid
            int on3d = 1;

            // Initialize the integers array first 2 lines are width and length
            integers = new int[length,width];

            for (int d = 0; d < on3d; d++)
            {
                int count = 0;
                //Debug.Log("readGridLine : " + readGridLine);
                //Debug.Log("readGridUpToLine: " + readGridUpToLine);

                // Parse each line as an integer and store it in the array
                for (int i = readGridLine; i < readGridUpToLine; i++)
                {
                    string[] separatedStrings = lines[i].Split(' ');

                    for (int j = 0; j < width; j++)
                    {
                        Debug.Log("Count: " + count + "");
                        Debug.Log(int.Parse(separatedStrings[j]));
                        integers[count, j] = int.Parse(separatedStrings[j]);

                    }
                    // integers[i] = int.Parse(lines[i]);
                    // Debug.Log(integers[i]);
                    count++;
                }


                int checkFor3d = readGridUpToLine; //readGridUpToLine + (length* d);
               // Debug.Log("checkFor3d: " + checkFor3d);
                string in3d = lines[checkFor3d];
               // Debug.Log("in3d : " + in3d);
                if (in3d[0] == 'd')
                {
                    Debug.Log("3d layer confirmed");
                    on3d++;
                    readGridLine = readGridUpToLine+1;
                    readGridUpToLine = readGridUpToLine + length+1;
                    
                }
                else
                {
                   
                    break;
                }
            }

            Debug.Log("Integers loaded successfully.");
        }
        else
        {
            Debug.Log("Level not found");
        }
    }
 

    void GenerateGridFromArray()
    {
        // Loop through the array dimensions
        for (int x = 0; x < array3DSize.x; x++)
        {
            for (int y = 0; y < array3DSize.y; y++)
            {
                for (int z = 0; z < array3DSize.z; z++)
                {
                    // Calculate position for each cell
                    Vector3 cellPosition = new Vector3(x * (cellSize.x + spacing.x),
                                                       y * (cellSize.y + spacing.y),
                                                       z * (cellSize.z + spacing.z));

                    // Instantiate cellPrefab
                    //GameObject cell = Instantiate(cellPrefab, gridParent);

                    // Set position
                   // cell.transform.localPosition = cellPosition;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadIntegersFromFile();
        //GenerateGridFromArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
