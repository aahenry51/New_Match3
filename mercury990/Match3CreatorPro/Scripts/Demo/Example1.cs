using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Engine;

public class Example1 : MonoBehaviour
{
    //Placement of the Elements on the Grid.
    //[Numbers are Array numeral position set by BuildGrid.Elements].

    //Layer 0
    private int[,] GridOutline = new int[10, 8]
    {
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3}

    };

    //Layer 1
    private int[,] GridBackground = new int[10, 8]
    {
        {1, 2, 1, 2, 1, 2, 1, 2},
        {2, 1, 2, 1, 2, 1, 2, 1},
        {1, 2, 1, 2, 1, 2, 1, 2},
        {2, 1, 2, 1, 2, 1, 2, 1},
        {1, 2, 1, 2, 1, 2, 1, 2},
        {2, 1, 2, 1, 2, 1, 2, 1},
        {1, 2, 1, 2, 1, 2, 1, 2},
        {2, 1, 2, 1, 2, 1, 2, 1},
        {1, 2, 1, 2, 1, 2, 1, 2},
        {2, 1, 2, 1, 2, 1, 2, 1}

    };

    //Layer 2
    private int[,] GridItems = new int[10, 8]
    {   
        {4, 5, 4, 5, 4, 6, 5, 6},
        {5, 5, 4, 4, 5, 6, 6, 5},
        {4, 6, 5, 4, 6, 5, 5, 5},
        {5, 6, 5, 5, 6, 4, 6, 4},
        {6, 5, 6, 6, 4, 4, 6, 4},
        {4, 5, 6, 5, 6, 6, 4, 5},
        {5, 4, 4, 6, 5, 5, 6, 4},
        {4, 6, 6, 5, 5, 6, 6, 5},
        {4, 5, 5, 4, 6, 5, 4, 5},
        {6, 5, 4, 6, 5, 4, 5, 4}

    };

    //Parent GameObject of the Grid.
    public GameObject GridParent;

    //Grid Elements
    public GameObject outline;
    public GameObject background1;
    public GameObject background2;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    //Creation of the Grid.
    void Build()
    {

        //Creates the Grid.
        BuildGrid.Create(10, 8, background1.GetComponent<RectTransform>().sizeDelta, GridParent.transform);

        //Sets the GameObjects to be used on the Grid.
        BuildGrid.Elements[0] = null;
        BuildGrid.Elements[1] = background1;
        BuildGrid.Elements[2] = background2;
        BuildGrid.Elements[3] = outline;
        BuildGrid.Elements[4] = item1;
        BuildGrid.Elements[5] = item2;
        BuildGrid.Elements[6] = item3;

        //Places the Elements on the Grid Using Layer and the int Array.
        BuildGrid.PlaceElements(0, GridOutline);
        BuildGrid.PlaceElements(1, GridBackground);
        BuildGrid.PlaceElements(2, GridItems);

    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialise the Grid
        Build();
    }




}
