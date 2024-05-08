using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Engine;
using DG.Tweening;

public class Example6 : MonoBehaviour
{
    //Placement of the Elements on the Grid.
    //[Numbers are the array numeral position of BuildGrid.Elements].

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

    //Engages Single Swap
    void SingleSwap()
    {
        //Can Swap with Empty Spaces.
        BuildSwap.SingleSwap.CanSwapWithEmptySpaces = false;

        //Run Swap on Layer 2.
        BuildSwap.SingleSwap.EngageSwap(2, .4f);


    }

    //Sets up a custom match pattern to check at every space
    void CustomMatch()
    {

        /*
            The MatchMaker will check every space on the grid for matches. 
            According to the code below and given the example space (6,3) with Element 2, the following would be a confirmation of a match.

            Custom1
            Spaces checked is: (6,4) , (6,5) , (6,7)
            Expected Element Number*: 2 , 2 , 2

            Custom2
            Spaces checked is: (7,3) , (8,3) , (9,3)
            Expected Element Number*: 5 , 5 , 5

            [Element Number are Array numeral position of BuildGrid.Elements]

        */

        //Custom 1

        //This Array Sets which spaces to check for matches. (1,0) Checks Down , (1,0) Checks Up, (0,1) Checks Right, (0,-1) Check Left from any relative position on the grid.
        Vector2Int[] Seq = new Vector2Int[]
            {
                    new Vector2Int(0,1),    //1 Space Right
                    new Vector2Int(0,2),    //2 Spaces Right
                    new Vector2Int(0,3)     //3 Spaces Right
            };

        //This Array Sets what element to check for corresponding to spaces set. "same" Checks for the Same element number the space being checked.
        string[] Sequence_ElementNumber = new string[]
        {
                    "same",                 //Must be the Same element for the space checked (Will be 1 space right based on array position above)
                    "same",                 //Must be the Same element for the space checked (Will be 2 space right based on array position above)
                    "same"                  //Must be the Same element for the space checked (Will be 3 space right based on array position above)
        };

        //Custom 2

        Vector2Int[] Seq2 = new Vector2Int[]
            {
                    new Vector2Int(1,0),    //1 Space Down
                    new Vector2Int(2,0),    //2 Spaces Down
                    new Vector2Int(3,0)     //3 Spaces Down
            };

        string[] Sequence_ElementNumber2 = new string[]
        {
                    "5",                //Element Number 5
                    "5",                //Element Number 5
                    "5"                 //Element Number 5
        };

        //Sets the Custom Matches 
        BuildMatch.SetCustomMatch("Custom1", Seq, Sequence_ElementNumber, 1);
        BuildMatch.SetCustomMatch("Custom2", Seq2, Sequence_ElementNumber2, 1);
    }

    //Enable to find for matches.
    public bool findMatches = false;
    //Enable to use custom matches.
    public bool CheckForCustomMatches = false;

    //Check for matches
    public void GetMatches()
    {

        if (findMatches) //Enable to find for matches.
        {
            Debug.Log("Find Matches");

            /*
             *  MatchPositions outputs the Matched position. A list of Array of positions which match together.
             *  For Example, MatchPositions[0] may be Vector2Int{(0,0), (0,1), (0,2)} which position satisfies the match pattern. MatchNames[0] with be the corresponding name set by the SetCustomMatch.

            */
            List<Vector2Int[]> MatchPositions = null;
            //Output of the Names of the matches, corresponds with MatchPositions
            List<string> MatchNames;

            if (CheckForCustomMatches) //Sets whether to check for custom matches
            {
                //Looks for the Custom Match patterns
                BuildMatch.GetMatches(2, out MatchPositions, out MatchNames, null, new string[] { "Custom1", "Custom2" });
            }
            else
            {
                //Looks for Default Match Pattern. (3 or more of the same element in a row or column)
                BuildMatch.GetMatches(2, out MatchPositions, out MatchNames);
            }


            //List is not Empty. Matches are found.
            if (MatchPositions != null)
            {
                //Get Array of Matched Positions
                foreach (Vector2Int[] array in MatchPositions)
                {
                    //Get Each Match Element
                    foreach (Vector2Int pos in array)
                    {
                        //Gets the Element at the Space then Does a Dotween animation then deletes.
                        var doing = BuildGrid.Layer[2].GetElementAtSpace(pos.x, pos.y).transform.DOShakeScale(.5f);

                        //On completion of Dotween code will run.
                        doing.OnComplete(() =>
                        {
                            //Deletes Element
                            BuildGrid.Layer[2].DeleteElementAtSpace(pos.x, pos.y, 0);
                        });

                    }

                }
            }



            findMatches = false;
        }



    }

    //Customize Fall direction.
    public void CustomFall()
    {
        //Element will only fall where layer 1 elements exist.
        BuildMove.Fall.LayerBoundary = 1;

        /*
         * Sets the direction for element to fall.
         * (1,0) Falls Down , (1,0) Falls Up, (0,1) Falls Right, (0,-1) Falls Left from any relative position on the grid.
        */

        //At position [5,0] on the grid, the element will fall down or fall right on the grid.
        BuildMove.Fall.CustomFalldir[5, 0].Directions = new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(0, 1) };
    
    }

    //Enable falling.
    void Falling()
    {

        if (Fallf)
        {
            Debug.Log("Fall Along");
            Fallf = false; //Enable for Single frame

            //Enable falling on layer 2
            BuildMove.Fall.Engage(2, 5f);
        
        }

    }

    public bool Fallf = false; //Activate 

    //Sets where to create new grid elements
    void EmitObj()
    {
        for (int i = 0; i < 8; i++) //Each Column
        {
            //Create new get element on layer 2
            BuildMove.Emit.Create(2, 0, i, new int[] {  4, 5, 6 });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialise the Grid
        Build();
        //Sets the custom falling.
        CustomFall();
        //Sets the custom matching.
        CustomMatch();
        //Set object emission.
        EmitObj();
    }

    // Update is called once per frame
    void Update()
    {
        //Engage falling.
        Falling();
        //Get the matches.
        GetMatches();
        //Run single swap.
        SingleSwap();

    }
}
