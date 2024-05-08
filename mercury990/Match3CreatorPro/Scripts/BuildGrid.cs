using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Match3Engine
{
    /// <summary>Creates a grid and set other basic settings.</summary>
    public class BuildGrid : GridSetting
    {

        #region BuildGrid

        /// <summary>Setup a Grid at a Parent position.</summary>
        /// <param name="Row">Total Amount of Rows on the Grid</param>
        /// <param name="Column">Total Amount of Columns on the Grid</param>
        /// <param name="SizeDelta">Length and Width of Each Cell.</param>
        /// <param name="Parent_Base">Parent Transform of the Grid; Parent Holds All of the Grid Elements.</param>
        public static void Create(int Row, int Column, Vector2 SizeDelta, Transform Parent_Base)
        {
            Canvas C = Parent_Base.root.GetComponent<Canvas>();

            if (C != null)
            {
                DOTween.SetTweensCapacity(500, 50);
                BasicSettings(Row, Column, SizeDelta, Parent_Base, C);
            }
            else
            {
                Debug.LogError("Match3 Creator : GameObject is not in UI Canvas");
            }

            
        }

        /// <summary>An Array of GameObjects to appear on Grid. Array Numeral Position Represents Element Reference Number.</summary>
        public static GameObject[] Elements = new GameObject[100];

        /// <summary>Layer of grid. </summary>
        public static GridLayer[] Layer = new GridLayer[100];

        /// <summary>Container of grid elements of a certain layer.</summary>
        static GameObject[] _OBJ_Layers = new GameObject[100];

        /// <summary>Container of grid elements of a certain layer.</summary>
        public static GameObject[] OBJ_Layers
        {
            get { return _OBJ_Layers; }
        }

        /// <summary>Switches the elements at given spaces (Does not move it just switches space reference)</summary>
        /// <param name="LayerNum1">First grid layer number.</param>
        /// <param name="LayerNum2">Second grid layer number.</param>
        /// <param name="Row1">First row location.</param>
        /// <param name="Column1">First column location.</param>
        /// <param name="Row2">Second row location.</param>
        /// <param name="Column2">Second column location.</param>
        public static void SwitchElementsRef(int LayerNum1, int Row1, int Column1, int LayerNum2, int Row2, int Column2)
        {
            GameObject Ele1 = null;
            GameObject Ele2 = null;
            Ele1 = Layer[LayerNum1].GetElementAtSpace(Row1, Column1);
            Ele2 = Layer[LayerNum2].GetElementAtSpace(Row2, Column2);
            int ele1num = Layer[LayerNum1].GetElementNumAtSpace(Row1, Column1);
            int ele2num = Layer[LayerNum2].GetElementNumAtSpace(Row2, Column2);

            Layer[LayerNum1].Spaces[Row1, Column1].Element = Ele2;
            Layer[LayerNum2].Spaces[Row2, Column2].Element = Ele1;

            Layer[LayerNum1].Spaces[Row1, Column1].Elementnum = ele2num;
            Layer[LayerNum2].Spaces[Row2, Column2].Elementnum = ele1num;

        }

        /// <summary>Places element(Doesn't switch) at given space (Doesn't move it just places space reference)</summary>
        /// <param name="LayerNum1">First grid layer number.</param>
        /// <param name="LayerNum2">Second grid layer number.</param>
        /// <param name="Row1">First row location.</param>
        /// <param name="Column1">First column location.</param>
        /// <param name="Row2">Second row location.</param>
        /// <param name="Column2">Second column location.</param>
        public static void PlaceElementsRef(int LayerNum1, int Row1, int Column1, int LayerNum2, int Row2, int Column2)
        {
         
            int ele1num = Layer[LayerNum1].GetElementNumAtSpace(Row1, Column1);

            Layer[LayerNum1].Spaces[Row1, Column1].Element = null;
            Layer[LayerNum1].Spaces[Row1, Column1].Elementnum = -1;

            Layer[LayerNum2].Spaces[Row2, Column2].Element = Layer[LayerNum1].GetElementAtSpace(Row1, Column1);
            Layer[LayerNum2].Spaces[Row2, Column2].Elementnum = ele1num;

        }

        /// <summary>Places new element at given space (Doesn't move it just places space reference)</summary>
        /// <param name="Element">New element to place.</param>
        /// <param name="ElementNum">New element reference number.</param>
        /// <param name="LayerNum">Layer to place element.</param>
        /// <param name="Row">Row location.</param>
        /// <param name="Column">Column location.</param> 
        public static void PlaceElementsGameObjectRef(GameObject Element, int ElementNum, int LayerNum, int Row, int Column)
        {

            Layer[LayerNum].Spaces[Row, Column].Element = Element;
            Layer[LayerNum].Spaces[Row, Column].Elementnum = ElementNum;

        }

        /// <summary>Creates new layer.</summary>
        /// <param name="LayerNum">The layer number (higher number = closer to screen)</param>
        protected static void CreateGridLayer(int LayerNum)
        {
            if (GridContainer != null)
            {
                _OBJ_Layers[LayerNum] = new GameObject("Empty");
                _OBJ_Layers[LayerNum].AddComponent(typeof(RectTransform));
                _OBJ_Layers[LayerNum].name = "GridLayer_" + LayerNum;
                _OBJ_Layers[LayerNum].transform.SetParent(GridContainer.transform);
                _OBJ_Layers[LayerNum].transform.localPosition = new Vector3(0, 0, -LayerNum);
                _OBJ_Layers[LayerNum].transform.localScale = new Vector3(1, 1, 1);
                Layer[LayerNum] = new GridLayer();
                Layer[LayerNum].Layernum = LayerNum;

            }
            else
            {
                Debug.LogError("Match3 Creator : GridBase not found / Use CreateGridBase before creating a Layer");
            }
        }

        /// <summary>Creates a Layer and places the gameobjects by the BuildGrid.Elements array numeral position.</summary>
        /// <param name="LayerNum">Select grid layer. </param>
        /// <param name="GridElementPosition">Set the Grid elements by BuildGrid.Elements array numeral position.</param>
        public static void PlaceElements(int LayerNum , int[,] GridElementPosition)
        {
            GameObject[] Elements = BuildGrid.Elements;
            if (Layer[LayerNum] == null)
            {
                CreateGridLayer(LayerNum);
                Layer[LayerNum].Spaces = new Space[Rows, Columns];
            }
            if (Layer[LayerNum] != null)
            {
               
                //Must be in the cell bounds
                if (GridElementPosition.GetLength(0) == Rows && GridElementPosition.GetLength(1) == Columns)
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Columns; j++)
                        {
                           
                            SetSpaceClass(LayerNum, i, j, null, -1);
                            for (int ele = 0; ele < Elements.GetLength(0); ele++)
                            {
                                if (GridElementPosition[i, j] == ele)
                                {
                                    if (Elements[ele] != null)
                                    {
                                        GameObject p = Instantiate(Elements[ele]);
                                        p.transform.SetParent(_OBJ_Layers[LayerNum].transform);
                                        p.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                                        p.transform.localPosition = SpaceLocationPos(i, j);
                                        SetSpaceClass(LayerNum, i, j, p, ele);
                                    }
                                    else
                                    {
                                        SetSpaceClass(LayerNum, i, j, null, -1);
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("GridElementPosition - error/ int[,] must equal new int[Rows,Columns]");
                }
            }
            else
            {
                Debug.LogError("GridBase not found / Use CreateGridBase before creating a Layer");
            }
        }
        #endregion

        #region Make Part Invisible

        /// <summary>Make certain layers or elements invisible. Doesn't make unclickable.</summary>
        public static class MakeInvisible
        {

            /// <summary>Change all elements in layer transparency. Doesn't make Unclickable.</summary>
            /// <param name="LayerNumber">Select Grid Layer</param>
            /// <param name="alpha">Set Transparent(0.0 - 1.0f).</param>
            public static void DOLayer(int LayerNumber,float alpha)
            {
                GameObject[,] AllElements = Layer[LayerNumber].AllElementsInLayer();

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        GameObject Element = AllElements[i, j];
                        if (Element != null)
                        {
                            
                            if (Element.GetComponent<Image>() != null)
                            {
                    


                                Color _InitialColor = Element.GetComponent<Image>().color;
                                Element.GetComponent<Image>().color = new Color(_InitialColor.r, _InitialColor.g, _InitialColor.b, alpha);
                            }
                            else if (Element.GetComponent<SpriteRenderer>() != null)
                            {
                                Color _InitialColor = Element.GetComponent<SpriteRenderer>().color;
                                Element.GetComponent<SpriteRenderer>().color = new Color(_InitialColor.r, _InitialColor.g, _InitialColor.b, alpha);
                            }
                            else
                            {
                                Debug.LogError("Space Gameobject has no Image or SpriteRenderer (SetClickable)");
                            }
                        }
                    }
                }
            }

            /// <summary>Change certain element in layer transparency. Doesn't make Unclickable.</summary>
            /// <param name="LayerNumber">Select grid layer to make invisible.</param>
            /// <param name="Row">Row Position.</param>
            /// <param name="Column">Colunm Position.</param>
            /// <param name="alpha">Set Transparent(0.0 - 1.0f).</param>
            public static void DOLayerSpace(int LayerNumber,int Row, int Column, float alpha)
            {
               GameObject Element = Layer[LayerNumber].GetElementAtSpace(Row, Column);

                        if (Element != null)
                        {
                            if (Element.GetComponent<Image>() != null)
                            {
                               Color _InitialColor = Element.GetComponent<Image>().color;
                               Element.GetComponent<Image>().color = new Color(_InitialColor.r, _InitialColor.g, _InitialColor.b, alpha);
                               
                            }
                            else if (Element.GetComponent<SpriteRenderer>() != null)
                            {
                               Color _InitialColor = Element.GetComponent<SpriteRenderer>().color;
                               Element.GetComponent<SpriteRenderer>().color = new Color(_InitialColor.r, _InitialColor.g, _InitialColor.b, alpha);
                            }
                            else
                            {
                                Debug.LogError("Space Gameobject has no Image or SpriteRenderer (SetClickable)");
                            }
                        }
              
            }

            /// <summary>Change All element on a space transparency. Doesn't make unclickable.</summary>
            /// <param name="Row">Row Postion.</param>
            /// <param name="Column">Colunm Postion.</param>
            /// <param name="alpha">Set Transparent(0.0 - 1.0f)</param>
            public static void DOSpace(int Row, int Column, float alpha)
            {

                foreach(GridLayer L in Layer)
                {
                    if (L != null)
                    {
                        GameObject Element = L.GetElementAtSpace(Row, Column);

                        if (Element != null)
                        {
                            if (Element.GetComponent<Image>() != null)
                            {
                                Color _InitialColor = Element.GetComponent<Image>().color;
                                Element.GetComponent<Image>().color = new Color(_InitialColor.r, _InitialColor.g, _InitialColor.b, alpha);
                                
                            }
                            else if (Element.GetComponent<SpriteRenderer>() != null)
                            {
                                Color _InitialColor = Element.GetComponent<SpriteRenderer>().color;
                                Element.GetComponent<SpriteRenderer>().color = new Color(_InitialColor.r, _InitialColor.g, _InitialColor.b, alpha);
                            }
                            else
                            {
                                Debug.LogError("Space Gameobject has no Image or SpriteRenderer (SetClickable)");
                            }
                        }

                    }
                }

                   
                

            }


        }

        #endregion

        #region Partition
        /// <summary>Separate the grid into parts.</summary>
        public static class Partition
        {

            /// <summary>Class for the grid part.</summary>
            private static Part[] Parts = new Part[100];

            /// <summary>Dotween for partition transition.</summary>
            public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> DotweenCore;

            /// <summary>Total amount of partitioned parts.</summary>
            public static int PartitionedParts;

            /// <summary>Separate the grid into parts. Must be lower than total grid rows and columns. </summary>
            /// <param name="Max_X">Amount of rows to partition. </param>
            /// <param name="Max_Y">Amount of columns to partition. </param>
            public static void PartitionGrid(int Max_X, int Max_Y)
            {

                //Must be Smaller than Grid
                if (Max_X <= Rows && Max_Y <= Columns)
                {

                    //Calculate Amount of X and Y partition
                    int PartitionX()
                    {
                        int whole = Rows / Max_X;
                        int remain = Rows % Max_X;

                        if (remain > 0) { return (whole + 1); }
                        else { return whole; }
                    }

                    int PartitionY()
                    {
                        int whole = Columns / Max_Y;
                        int remain = Columns % Max_Y;

                        if (remain > 0) { return (whole + 1); }
                        else { return whole; }
                    }

                    int partTotal = PartitionX() * PartitionY();
                    Parts = new Part[partTotal];
                    PartitionedParts = partTotal;

                    //Getting top Right position
                    int p = 0;
                    for (int xp = 0; xp < PartitionX(); xp++)
                    {
                        for (int yp = 0; yp < PartitionY(); yp++)
                        {
                            Parts[p] = new Part();
                            Parts[p].TopRightPosition = new Vector2Int(xp * Max_X, yp * Max_Y);
                            p++;
                        }

                    }

                    //Loop for each part
                    for (int pt = 0; pt < partTotal; pt++)
                    {
                        int PosAmount = Max_X * Max_Y;
                        int count = 0;
                        Vector2Int[] positions = new Vector2Int[PosAmount];
                        //Finding All Positions
                        for (int x = 0; x < Max_X; x++)
                        {
                            for (int y = 0; y < Max_Y; y++)
                            {
                                Vector2Int TRP = Parts[pt].TopRightPosition;
                                if (TRP.x + x < Rows && TRP.y + y < Columns)
                                {
                                    positions[count] = new Vector2Int(TRP.x + x, TRP.y + y);
                                    Parts[pt].BottomLeftPosition = new Vector2Int(TRP.x + x, TRP.y + y);
                                    count++;
                                }
                                else
                                {
                                    positions[count] = new Vector2Int(-1, -1);
                                }

                            }
                        }

                        Parts[pt].BottomLeftPosition = positions[count - 1];
                        //Enter Part Values
                        Parts[pt].positions = new Vector2Int[count];
                        Parts[pt].totalPositions = count;

                        for (int c = 0; c < count; c++)
                        {
                            Parts[pt].positions[c] = new Vector2Int();
                            Parts[pt].positions[c] = positions[c];
                        }
                    }
                }

              
            }

            /// <summary>Part Class.</summary>
            public class Part
            {
                /// <summary>Returns positions in part.</summary>
                public Vector2Int[] positions;
                /// <summary>Return total amount of spaces in part.</summary>
                public int totalPositions;
                /// <summary>Returns the TopRight most position of the part.</summary>
                public Vector2Int TopRightPosition;
                /// <summary>Returns the BottomLeft most position of the part.</summary>
                public Vector2Int BottomLeftPosition;
            }

            /// <summary>Enable/Disable clickability of part. </summary>
            /// <param name="part">Part partitioned. </param>
            /// <param name="on">Enable or disable if clickable. </param>
            public static void SetPartClickable(int part, bool on)
            {
                
                if (part < PartitionedParts)
                {
                    foreach (Vector2Int p in Parts[part].positions)
                    {
                        foreach (GridLayer L in Layer)
                        {
                            if (L != null)
                            {
                                L.Spaces[p.x, p.y].SetClickable(on);
                                L.Spaces[p.x, p.y].SetMatchable(on);
                            }
                        }
                    }
                }
            }

            /// <summary>Current part active.</summary>
            public static int OnPart = -1;

            /// <summary>Transition to different parts of the grid.</summary>
            /// <param name="part">Part to transition to.</param>
            /// <param name="Timer">Transition time. </param>
            /// <param name="InActiveUnclickable">Make unactive part spaces unclickable</param>
            /// <param name="InActiveTransparent">Make unactive part spaces transparent</param>
            public static void TransitionPartPosition(int part, float Timer,  bool InActiveUnclickable, bool InActiveTransparent)
            {

                DG.Tweening.Ease EaseType = DG.Tweening.Ease.InOutSine;
                if (part != OnPart && part < Parts.GetLength(0))
                {
                    Vector3 temp_GridContainer = GridContainer.transform.localPosition;
                    GridContainer.transform.localPosition = GridContainer_LocalPosition;

                    Vector3 PosTR = SpaceLocationPos(Parts[part].TopRightPosition.x, Parts[part].TopRightPosition.y);
                    Vector3 PosBL = SpaceLocationPos(Parts[part].BottomLeftPosition.x, Parts[part].BottomLeftPosition.y);

                    Vector3 PosCenter = new Vector3(((PosTR.x - PosBL.x) / 2) + PosBL.x, ((PosTR.y - PosBL.y) / 2) + PosBL.y, 0);

                    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] Temp_TransitionPart_DGTweenMove = new DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[10];
                    int LCount = 0;
                    foreach (GameObject L in _OBJ_Layers)
                    {
                        if (L != null)
                        {
                            Vector3 Lpos = L.transform.localPosition;
                            LCount++;

                        }
                    }

                    

                    Vector3 posCenterWorld = _OBJ_Layers[0].transform.TransformPoint(new Vector3(-PosCenter.x, -PosCenter.y, 0));

                    Vector3 GridPosCenter = GridContainer.transform.parent.InverseTransformPoint(posCenterWorld);
                    GridContainer.transform.localPosition = temp_GridContainer;
                    DotweenCore = GridContainer.transform.DOLocalMove(GridPosCenter, Timer, false).SetEase(EaseType);


                    //Set Other Part Unclickable or visible
                    if (InActiveUnclickable || InActiveTransparent)
                    {
                        for (int p = 0; p < PartitionedParts; p++)
                        {
                            //Part is the spaces Centered on
                            if (part == p)
                            {
                                    SetPartClickable(p, true);
                                
                                //Make Visible
                                if (InActiveTransparent)
                                {
                                    foreach (Vector2Int pos in Parts[p].positions)
                                    {
                                        MakeInvisible.DOSpace(pos.x, pos.y, 1f);
                                    }
                                }
                            }
                            else
                            {
                                if (InActiveUnclickable)
                                {
                                    SetPartClickable(p, false);
                                }
                                //Make Inivisible
                                if (InActiveTransparent)
                                {
                                    foreach (Vector2Int pos in Parts[p].positions)
                                    {
                                        MakeInvisible.DOSpace(pos.x, pos.y, .4f);
                                    }
                                }
                            }
                        }
                    }

                    OnPart = part;
                }
            }

        }
        #endregion

        #region Layer/Space Settings
        /// <summary>Set Space Presets</summary>
        static void SetSpaceClass(int LayerNum, int ipos, int jpos, GameObject Element, int Elementnum)
        {

            Layer[LayerNum].Spaces[ipos, jpos] = new Space();
            Layer[LayerNum].Spaces[ipos, jpos].Element = Element;
            Layer[LayerNum].SetElementNumAtSpace(ipos, jpos, Elementnum);
            Layer[LayerNum].Spaces[ipos, jpos].posx = ipos;
            Layer[LayerNum].Spaces[ipos, jpos].posy = jpos;
            Layer[LayerNum].Spaces[ipos, jpos].OnLayer = LayerNum;
        }

        /// <summary>Space Class</summary>
        public class Space
        {

            /// <summary>Sets if clickable.</summary>
            private bool _IsClickable = true;
            /// <summary>Sets if matchable.</summary>
            private bool _IsMatchable = true;

            /// <summary>Initial color of gameobject.</summary>
            private Color _InitialColor;
            private static Image _InitialImage;
            private static SpriteRenderer _InitialSpriteRenderer;

            /// <summary>Element gameobject.</summary>
            public GameObject Element = null;

            /// <summary>Layer object is on</summary>
            public int OnLayer;
            /// <summary>Element number of gameobject.</summary>
            public int Elementnum;

            /// <summary>Row position on grid.</summary>
            public int posx = -1;
            /// <summary>Column position on grid.</summary>
            public int posy = -1;

            /// <summary>Return if element exist at position.</summary>
            public bool Exist()
            {
                if (Element == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>Set if space is clickable.</summary>
            public void SetClickable(bool On)
            {

                if (On && !_IsClickable )
                {
                    _IsClickable = true;

                }
                else if (!On && _IsClickable )
                {
                    _IsClickable = false;
                }
            }

            /// <summary>Set if space is matchable.</summary>
            public void SetMatchable(bool On)
            {
                _IsMatchable = On;
   
            }

            /// <summary>Returns if space can be clicked.</summary>
            public bool IsClickable
            {
                get { return _IsClickable; }
            }

            /// <summary>Returns if space can be matched.</summary>
            public bool IsMatchable
            {
                get { return _IsMatchable; }
            }

            /// <summary>Resets space to empty.</summary>
            public void Reset(float Timer)
            {
                _IsClickable = true;
                _IsMatchable = true;
                Elementnum = -1;

                if(Element != null) { Destroy(Element, Timer); }
            }
        }

        /// <summary>Class of a Layer.</summary>
        public class GridLayer
        {
            /// <summary>Layer Number.</summary>
            public int Layernum;

            /// <summary>Layer gameobject parent.</summary>
            private GameObject OBJ_GridLayer
            {
                get { return _OBJ_Layers[Layernum]; }
            }

            /// <summary>Space class of grid.</summary>
            public Space[,] Spaces = new Space[Rows, Columns];

            /// <summary>Returns all elements in layer.</summary>
            public GameObject[,] AllElementsInLayer()
            {
                GameObject[,] All  = new GameObject[Rows, Columns];

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        All[i, j] = Spaces[i, j].Element;
                    }
                }

                return All;
            }

            /// <summary>Returns all element numbers in layer.</summary>
            public int[,] AllElementsNumbersInLayer()
            {
                int[,] All = new int[Rows, Columns];

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (Spaces[i, j] != null)
                        {
                            All[i, j] = Spaces[i, j].Elementnum;
                        }
                        else
                        {
                            All[i, j] = -1;
                        }
                        
                    }
                }

                return All;
            }

            /// <summary>Returns element at space in layer.</summary>
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Colunm position.</param>
            public GameObject GetElementAtSpace(int Row, int Column)
            {
                return Spaces[Row, Column].Element;
            }

            /// <summary>Get element number at space in layer.</summary
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Colunm position.</param>
            public int GetElementNumAtSpace(int Row, int Column)
            {
                return Spaces[Row, Column].Elementnum;
            }

            /// <summary>Set element number at space in layer.</summary>
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Colunm position.</param>
            /// <param name= "Number">Number to set (Only Changes Number Reference).</param>
            public void SetElementNumAtSpace(int Row, int Column, int Number)
            {
                Spaces[Row, Column].Elementnum = Number;
            }

            /// <summary>Set clickability at space in layer.</summary>
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Colunm position.</param>
            /// <param name="Set">Enable clickability.</param>
            public void SetClickableAtSpace(int Row, int Column , bool Set)
            {
                Spaces[Row, Column].SetClickable(Set);
            }

            /// <summary>Delete element at space. Resets space.</summary>
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Colunm position.</param>
            /// <param name="Timer">Wait time to reset space.</param>
            public void DeleteElementAtSpace(int Row, int Column, float Timer)
            {
                Spaces[Row, Column].Reset(Timer);
            }

            /// <summary>Gets all empty spaces in layer.</summary>
            public Vector2Int [] GetAllEmptySpaces()
            {
                List<Vector2Int> Empty = new List<Vector2Int>();
                int count = 0;

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if(!Spaces[i, j].Exist())
                        {
                            Empty.Add(new Vector2Int(i, j));
                            count++;
                        }
                    }
                }

                Vector2Int[] E = new Vector2Int[count];
                int temp = 0;
                foreach(Vector2Int pos in Empty)
                {
                    E[temp] = pos;
                    temp++;
                }

                return E;
            }
        }
        #endregion

    }
}