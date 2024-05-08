using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


namespace Match3Engine
{
    /// <summary>Move elements around the grid. Emit elements.</summary>
    public class BuildMove : MonoBehaviour
    {
        /// <summary>Switch the slements given the spaces (Moves the Element and switches Ref) [Returns DoMove Tween for both element]</summary>
        /// <param name="LayerNum1">Select grid first layer. </param>
        /// <param name="LayerNum2">Select grid second layer. </param>
        /// <param name="pos1">First space position.</param>
        /// <param name="pos2">Second space position.</param>
        /// <param name="speed">Move time duration. </param>
        /// <param name="snapping">Snap to position. </param>
        public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[]
            SwitchElements(int LayerNum1, Vector2Int pos1, int LayerNum2, Vector2Int pos2, float speed, bool snapping)
        {
            DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> E1 = null;
            DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> E2 = null;

            GameObject ele1 = BuildGrid.Layer[LayerNum1].Spaces[pos1.x, pos1.y].Element;
            GameObject ele2 = BuildGrid.Layer[LayerNum2].Spaces[pos2.x, pos2.y].Element;

            //Debug.Log()
            if (ele1 != null)
            {
                Vector3 pos = GridSetting.SpaceLocationPos(pos2.x, pos2.y);
                E1 = ele1.transform.DOLocalMove(new Vector3(pos.x, pos.y, -LayerNum2), speed, snapping);

            }

            if (ele2 != null)
            {
                Vector3 pos = GridSetting.SpaceLocationPos(pos1.x, pos1.y);
                E2 = ele2.transform.DOLocalMove(new Vector3(pos.x, pos.y, -LayerNum1), speed, snapping);
            }

            BuildGrid.SwitchElementsRef(LayerNum1, pos1.x, pos1.y, LayerNum2, pos2.x, pos2.y);

            return new DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] { E1, E2 };
        }


        /// <summary>Make element fall on grid.</summary>
        public class Fall : Emit
        {

            #region User Interface
            /// <summary>Fall on positions only where given layer elements exist.</summary>
            public static int LayerBoundary = -1;
            /// <summary>Exclude elements numbers from falling.</summary>
            public static int[] ExcludeElements = null;
            /// <summary>Do not fall elements that share a position with these element numbers regardless of layer.</summary>
            public static int[] ElementPositionRestriction = null;
            /// <summary>Dotween Path Core of all falling elements.</summary>
            public static List<DG.Tweening.Core.TweenerCore<Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions>> DotweenPaths;

            /// <summary>Amount of elements in fall animation. </summary>
            static int ActiveFalling = 0;

            /// <summary>Returns if falling is active. </summary>
            public static bool FallingActive()
            {
                if(ActiveFalling > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>Engage falling of elements on layer. Default fall direction is down.</summary>
            /// <param name="Layer">Layer to engage fall.</param>
            /// <param name="Speed">Speed at which elements fall.</param>
            public static void Engage(int Layer, float Speed)
            {
                if (!BuildSwap.Preset.AnySwapEngaged())
                {
                    GetFallMoves(Layer, Speed);
                }
                else
                {
                    Debug.LogWarning("Match3 Creator : BuildMove - Cannot Fall While Swapping");
                }
            }

            /// <summary>Create custom fall directions. </summary>
            public static FallPattern[,] CustomFalldir = null;
            #endregion 

            #region Module
            /// <summary>Checks if Element Can Move over elements in FallElementsBoundary.</summary>
            static bool LayerFallBoundConditionsMet(Vector2Int Position)
            {

                if (LayerBoundary != -1)
                {
                    if (BuildGrid.Layer[LayerBoundary].GetElementAtSpace(Position.x, Position.y) != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return true;
                }
            }

            /// <summary>Return if can move ElementNumbersPositionRestriction true/false at position regardless of layer. </summary>
            static bool FoundElementNumbersPositionRestriction(int Layer, Vector2Int Position)
            {
                bool check = false;

                if (ElementPositionRestriction != null)
                {
                    foreach (BuildGrid.GridLayer L in BuildGrid.Layer)
                    {
                        if (L != null && L.Layernum != Layer)
                        {
                            foreach (int num in ElementPositionRestriction)
                            {
                                if (L != null && L.GetElementNumAtSpace(Position.x, Position.y) == num)
                                {
                                    check = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    check = false;
                }

                return check;
            }

            /// <summary>Check for Exclude Fall Positions.</summary>
            protected static bool ElementAtPositionCanFall(int Layer, int ElementNum)
            {
                bool can = true;
 
                    if (ExcludeElements != null)
                    {
                        foreach (int e in ExcludeElements)
                        {
                            if (ElementNum == e)
                            {
                                can = false;
                            }
                        }
                    }
             

               // }

               // Debug.Log(can);
                return can;
            }

            /// <summary>Checks for Emission At Position.</summary>
            static bool CheckForEmitionAtPosition (int Layer, Vector2Int Position)
            {
                if (Position.x != -1)
                {
                    
                    if (Emit.EmitLayer[Layer] != null && Emit.EmitLayer[Layer].Space[Position.x, Position.y] != null && Emit.EmitLayer[Layer].Space[Position.x, Position.y].EmitObj != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }

            /// <summary>Class of fall directions.</summary>
            public class FallPattern
            {
                public Vector2Int [] Directions;
            }

            /// <summary>Gets fall direction from position.</summary>
            protected static Vector2Int findPositionFromFallPattern(Vector2Int Direction, int row, int column)
            {
                Vector2Int dir = new Vector2Int(-1, -1);
                dir = GridSetting.SpaceNextToPos(row, column, Direction.x, 0, 0, Direction.y);
                return dir;
            }

            /// <summary>Checks if can fall to position.</summary>
            static bool CheckForObstructions(int Layer, int Row, int Column)
            {
                bool _LayerFallBoundConditionsMet = LayerFallBoundConditionsMet(new Vector2Int(Row, Column));

                if (
                    _LayerFallBoundConditionsMet 
                    )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>Checks if position can fall.</summary>
            static bool CheckIfElementAtPositionCanMove(int Layer, int Row, int Column, int Elementnum)
            {

                bool _FoundElementNumbersPositionRestriction = FoundElementNumbersPositionRestriction(Layer, new Vector2Int(Row, Column));
                bool _ElementAtPositionCanFall = ElementAtPositionCanFall(Layer, Elementnum);
                bool _LayerFallBoundConditionsMet = LayerFallBoundConditionsMet(new Vector2Int(Row, Column));

                if (
                     _ElementAtPositionCanFall
                    && _LayerFallBoundConditionsMet
                    && !_FoundElementNumbersPositionRestriction

                    )
                {

                    return true;

                }
                else
                {
                    return false;
                }
            }

            /// <summary>Class of fall path.</summary>
            protected class ArrayListClass
            {
                public List<Vector2Int> Pathways = new List<Vector2Int>();
            }

            /// <summary>Get fall path.</summary>
            static ArrayList FallPaths2 (int Layer, int Row, int Column)
            {
                Vector2Int FirstPos = new Vector2Int(Row, Column);
                Vector2Int CurrentPos = new Vector2Int(Row, Column);
                bool[,] PathwaysChecked = new bool[GridSetting.Rows, GridSetting.Columns];
                ArrayList Path = new ArrayList();

                int TotalPaths = 1;


                PathwaysChecked[FirstPos.x, FirstPos.y] = true;

                //Start Gettting Path
                if (true)
                {
                    //Run through each path
                    for (int pcount = 0; pcount < TotalPaths; pcount++)
                    {
                        int totalLoops = 1;

                        PathwaysChecked = new bool[GridSetting.Rows, GridSetting.Columns]; //Prevent Infinite Loops
                        PathwaysChecked[FirstPos.x, FirstPos.y] = true;


                        if(pcount > 0)
                        {
                            ArrayListClass array1 = (ArrayListClass)Path[pcount];
                            CurrentPos = array1.Pathways[array1.Pathways.Count - 1];
                        }
                        else
                        {
                            Path.Add(new ArrayListClass());
                            ArrayListClass array1 = (ArrayListClass)Path[pcount];
                            array1.Pathways.Add(FirstPos);
                        }

                        //Get All next positions
                        for (int loop = 0; loop < totalLoops; loop++)
                        {
                            //Has Direction Set at Path
                            if (CustomFalldir[CurrentPos.x, CurrentPos.y].Directions != null)
                            {
                                //Get Direction
                                Vector2Int [] Dir = CustomFalldir[CurrentPos.x, CurrentPos.y].Directions;
                                int DirCount = Dir.Length;
                                Vector2Int NextCurrentPos = new Vector2Int(-1, -1);

                                for (int Dcount = 0; Dcount < DirCount; Dcount++)
                                {
                                 
                                    //Translate to Next pos 
                                    Vector2Int NextPos = findPositionFromFallPattern(Dir[Dcount], CurrentPos.x, CurrentPos.y);

                                    //Check if next Position meets standards
                                    if (NextPos.x != -1 && !PathwaysChecked[NextPos.x, NextPos.y])
                                    {
                                        //Priority is First Direction
                                        if (Dcount == 0)
                                        {

                                            PathwaysChecked[NextPos.x, NextPos.y] = true;

                                            ArrayListClass array = (ArrayListClass)Path[pcount];
                                            array.Pathways.Add(NextPos);
                                            NextCurrentPos = NextPos;

                                              totalLoops++; //Increase Loops when next position is found
                                        }
                                        else
                                        {
                                            Path.Insert(pcount + 1, new ArrayListClass());
                                            ArrayListClass array = (ArrayListClass)Path[pcount + 1];

                                            ArrayListClass arraycurrent  = (ArrayListClass)Path[pcount];
                                            //Add all previous paths 
                                            for (int add = 0; add < arraycurrent.Pathways.Count-1; add++)
                                            {

                                                array.Pathways.Add(arraycurrent.Pathways[add]);
                                            }

                                            array.Pathways.Add((Vector2Int)NextPos);
                                            TotalPaths++; //New paths to go down

                                        }
                                    }

                                    

                                }

                                CurrentPos = NextCurrentPos;
                            }
                        }

                       
                    }
                        
                }

                return Path;

            }

            /// <summary>Fake element class.</summary>
            protected class fakeElement
            {
                public bool ElementExist = false;
                public int Obj_id ;
                public int ElementNum = -1;

            }

            /// <summary>Fake grid.</summary>
            protected static fakeElement[,] FakeGrid;

            /// <summary>Setup fake grid and find emission positions.</summary>
            static void Setup(int Layer)
            {
                FakeGrid = new fakeElement[GridSetting.Rows, GridSetting.Columns];
                for (int i = 0; i < GridSetting.Rows; i++)
                {
                    for (int j = 0; j < GridSetting.Columns; j++)
                    {
                        bool _ElementAtSpace = BuildGrid.Layer[Layer].GetElementAtSpace(i, j);
                        FakeGrid[i, j] = new fakeElement();
                       
                        FakeGrid[i, j].ElementExist = _ElementAtSpace;
                        FakeGrid[i, j].ElementNum = BuildGrid.Layer[Layer].GetElementNumAtSpace(i, j);
                        if (_ElementAtSpace) {FakeGrid[i, j].Obj_id = BuildGrid.Layer[Layer].GetElementAtSpace(i, j).gameObject.GetInstanceID();}

                        
                        if(CheckForEmitionAtPosition(Layer, new Vector2Int(i, j)))
                        {
                            EmitPos[i,j] = new EmissionPosition();
                        }

                    }
                }


            }

            /// <summary>Class to track element movement.</summary>
            protected class ElementMovement
            {
                public int ElementNumber = -1;
                public bool Emitted;
                public GameObject New_Element;
                public int Emitnum = 0; //Emittion Count 

                public Vector2Int OrigPosition; //Or emition position if Emitted
                public List<Vector2Int> MovePositions = new List<Vector2Int>();
                public int Id_Objs = new int();
            }

            /// <summary>Find element id</summary>
            protected static int FindElementID_MoveSpaceCount(int Element_ID)
            {

                int count = -1;

                for(int i = 0; i < MoveSpace.Count; i++)
                {
                    if(MoveSpace[i].Id_Objs == Element_ID)
                    {
                        count = i;
                        
                    }
                }

                return count;
            }

            /// <summary>Initialise the emission positions.</summary>
            protected static GameObject Create_Emission(int Layer, Vector2Int Pos, out int ElementNumber)
            {
                EmitPos[Pos.x, Pos.y].AmountEmitted++;
                Emit.EmitSpace Space = Emit.EmitLayer[Layer].Space[Pos.x, Pos.y];

                //Get new Gameobject
                int ElementNumberToEmit = Space.EmitElementNum[Space.RandomElementNum()];
                bool CanCreateEmittion = true;
                foreach (int num in EmitLayer[Layer].Space[Pos.x, Pos.y].EmitElementNum)
                {
                    if (!ElementAtPositionCanFall(Layer, num))
                    {
                        CanCreateEmittion = false;
                        Debug.LogError("Match3 Creator : BuildMove - Cannot Emit Element With a Fall Restriction");
                        break;
                    }
                }


                GameObject Element = null;
                GameObject Element_new = null;
                if (CanCreateEmittion) { Element = BuildGrid.Elements[ElementNumberToEmit]; Element_new = GameObject.Instantiate(Element); }
                else { Element_new = GameObject.Instantiate(new GameObject("name")); }
            

               

                Element_new.transform.localScale = new Vector3(1, 1, 1);
                Element_new.transform.SetParent(Space.EmitObj.transform);
                Element_new.transform.localScale = new Vector3(1, 1, 1);
                int PhantomSpaceCount = EmitPos[Pos.x, Pos.y].AmountEmitted; //Spaces that do not exist but are place holders

                Element_new.transform.position = BuildGrid.OBJ_Layers[Layer].transform.TransformPoint(GridSetting.SpaceLocationPos(Pos.x, Pos.y) + new Vector3(0, GridSetting.SpaceSize.y * PhantomSpaceCount, 0));

                EmitPos[Pos.x, Pos.y].Phantom_TransformPos.Add(Element_new.transform.position);

                ElementNumber = ElementNumberToEmit;

                return Element_new;
            }

            /// <summary>Emission position class.</summary>
            protected class EmissionPosition
            {
                public int AmountEmitted = 0;
                public List<Vector3> Phantom_TransformPos = new List<Vector3>();
                public List<Vector2Int> PosToEmitTo = new List<Vector2Int>();
            }

            /// <summary>Array of emission positions.</summary>
            protected static EmissionPosition[,] EmitPos;

            /// <summary>List of spaces for element to move to.</summary>
            protected static List<ElementMovement> MoveSpace = new List<ElementMovement>();

            /// <summary>Find the falling path, all emissions, and intialise fall.</summary>
            static void GetFallMoves(int Layer, float Speed)
            {

                bool[,] CheckedSpace = new bool[GridSetting.Rows, GridSetting.Columns];
                MoveSpace = new List<ElementMovement>();
                EmitPos = new EmissionPosition[GridSetting.Rows, GridSetting.Columns];

                Setup(Layer);

                //NormalGrid
                for (int i = 0; i < GridSetting.Rows; i++)
                {
                    for (int j = 0; j < GridSetting.Columns; j++)
                    {

                        if (!CheckedSpace[i, j])//&& i ==0 && j == 0)
                        {

                            ArrayList Pathways = FallPaths2(Layer, i, j);

                            if (Pathways != null)
                            {
                                for (int c = 0; c < Pathways.Count; c++)
                                {
                                    if (Pathways[c] != null)
                                    {
                                        ArrayListClass array = (ArrayListClass)Pathways[c]; //Array is a one path

                                        for (int add = array.Pathways.Count - 1; add >= 0; add--)
                                        {
                                  
                                            
                                            bool Movable = CheckIfElementAtPositionCanMove(Layer, array.Pathways[add].x, array.Pathways[add].y, BuildGrid.Layer[Layer].Spaces[array.Pathways[add].x, array.Pathways[add].y].Elementnum);
                                            bool Exist = FakeGrid[array.Pathways[add].x, array.Pathways[add].y].ElementExist;
                                            CheckedSpace[array.Pathways[add].x, array.Pathways[add].y] = true;

                                            #region Normal Position
                                            if (Exist && Movable)
                                            {
                                                int MoveSpaceCount = FindElementID_MoveSpaceCount(FakeGrid[array.Pathways[add].x, array.Pathways[add].y].Obj_id);

                                                if(MoveSpaceCount == -1)
                                                {
                                                    MoveSpace.Add(new ElementMovement());
                                                    MoveSpace[MoveSpace.Count - 1].Id_Objs = FakeGrid[array.Pathways[add].x, array.Pathways[add].y].Obj_id;
                                                    MoveSpace[MoveSpace.Count - 1].OrigPosition = array.Pathways[add];
                                                    MoveSpaceCount = MoveSpace.Count - 1;
                                                }

                                                Vector2Int LastPos = new Vector2Int(-1, -1);

                                                bool HitObstruction = false;
                                                //Check Elements in path
                                                for (int p = add+1; p < array.Pathways.Count; p++)
                                                {
                                                    
                                                    Vector2Int pospath = array.Pathways[p];
                                             
                                                    
                                                    //Element nonexistant ; No emitted element ; No Obstruction
                                                    if (!FakeGrid[pospath.x, pospath.y].ElementExist && !HitObstruction && !CheckForObstructions(Layer, pospath.x, pospath.y) && !CheckForEmitionAtPosition(Layer, new Vector2Int(pospath.x, pospath.y)))
                                                    {
                                 
                                                        MoveSpace[MoveSpaceCount].MovePositions.Add(pospath);
                                                        LastPos = pospath;
                                                        HitObstruction = FoundElementNumbersPositionRestriction(Layer, new Vector2Int(pospath.x, pospath.y));
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }

                                                }

                                                //Switch Element ref
                                                if(LastPos.x != -1)
                                                {
                                                    FakeGrid[array.Pathways[add].x, array.Pathways[add].y].ElementExist = false; //Previous
                                                    FakeGrid[LastPos.x, LastPos.y].ElementExist = true; //Next

                                                    FakeGrid[LastPos.x, LastPos.y].ElementNum = FakeGrid[array.Pathways[add].x, array.Pathways[add].y].ElementNum;
                                                    FakeGrid[array.Pathways[add].x, array.Pathways[add].y].ElementNum = -1;

                                                    FakeGrid[LastPos.x, LastPos.y].Obj_id = FakeGrid[array.Pathways[add].x, array.Pathways[add].y].Obj_id;
                                                    FakeGrid[array.Pathways[add].x, array.Pathways[add].y].Obj_id = 0;
                                                }


                                            }
                                            #endregion

                                            #region Emittion Position
                                            //Emittion Position
                                            if (EmitPos[array.Pathways[add].x, array.Pathways[add].y] != null)
                                            {
                                                EmissionPosition Epos = EmitPos[array.Pathways[add].x, array.Pathways[add].y];
                                                Vector2Int LastPos = new Vector2Int(-1, -1);
                                                
                                                bool Exist2 = FakeGrid[array.Pathways[add].x, array.Pathways[add].y].ElementExist;

                                                //Element not at position ; Element not Emitted 
                                                if (!Exist2 )
                                                {

                                                    List<Vector2Int> PosToEmit = new List<Vector2Int>();

                                                    bool HitObstruction = false;
                                                    //Check Elements in path
                                                    for (int p = add ; p < array.Pathways.Count; p++)
                                                    {

                                                        Vector2Int pospath = array.Pathways[p];
                                                        bool EmitPosFound = false;
                                                        if (CheckForEmitionAtPosition(Layer, new Vector2Int(pospath.x, pospath.y)) &&
                                                            new Vector2Int(pospath.x, pospath.y) != new Vector2Int(array.Pathways[add].x, array.Pathways[add].y)
                                                            ) { EmitPosFound = true; }

                                                        if (!FakeGrid[pospath.x, pospath.y].ElementExist && !HitObstruction && !CheckForObstructions(Layer, pospath.x, pospath.y) && !EmitPosFound)
                                                        {
 
                                                            PosToEmit.Add(pospath);
                                                            HitObstruction = FoundElementNumbersPositionRestriction(Layer, new Vector2Int(pospath.x, pospath.y));

                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }

                                                    for(int E = PosToEmit.Count-1; E >= 0; E--)
                                                    {

                                                        int ElementNum;
                                                        GameObject new_Element = Create_Emission(Layer, array.Pathways[add],out ElementNum);


                                                        //Add to Fake Grid
                                                       FakeGrid[PosToEmit[E].x, PosToEmit[E].y].ElementExist = true; //Next

                                                       FakeGrid[PosToEmit[E].x, PosToEmit[E].y].Obj_id = new_Element.GetInstanceID();

                                                        //Add to Moving Elements List
                                                        MoveSpace.Add(new ElementMovement());
                                                        MoveSpace[MoveSpace.Count - 1].Id_Objs = new_Element.GetInstanceID();
                                                        MoveSpace[MoveSpace.Count - 1].OrigPosition = new Vector2Int(array.Pathways[add].x, array.Pathways[add].y);//PosToEmit[E];
                                                        MoveSpace[MoveSpace.Count - 1].Emitted = true;
                                                        MoveSpace[MoveSpace.Count - 1].New_Element = new_Element;
                                                        MoveSpace[MoveSpace.Count - 1].Emitnum = EmitPos[array.Pathways[add].x, array.Pathways[add].y].AmountEmitted;
                                                        MoveSpace[MoveSpace.Count - 1].ElementNumber = ElementNum;

                                                        //Get path
                                                        for (int L = 0; L <= E; L++)
                                                        {
                                                            MoveSpace[MoveSpace.Count - 1].MovePositions.Add(PosToEmit[L]);
                                                        }

                                                    }
                                                   

                                                }
                                            }
                                            #endregion


                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                //Tween Elements
                DotweenPaths = new List<DG.Tweening.Core.TweenerCore<Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions>>();
                
                DotweenFall(Layer, null, Speed, MoveSpace);

            }

            /// <summary>Start the fall animation.</summary>
            static void DotweenFall(int Layer, List<Vector2Int[]> MoveToSpace, float Speed, List<ElementMovement> MoveSpace)
            {

                foreach (ElementMovement E in MoveSpace)  
                {
                    if (E != null && E.MovePositions.Count > 0)
                    {
                        #region Setup / Get Element
                        List<Vector2Int> Pos = E.MovePositions;
                        Vector2Int FirstPos = E.OrigPosition;
                        Vector2Int LastPos = E.OrigPosition;
                        GameObject ele1 = null;

                        if (E.Emitted) //Emittion Element
                        {
                            ele1 = E.New_Element;

                        }
                        else
                        {
                            ele1 = BuildGrid.Layer[Layer].Spaces[E.OrigPosition.x, E.OrigPosition.y].Element;
                        }
                        #endregion

                        //Get element at Position
                        if (ele1 != null)
                        {
                            Vector3[] waypoint = new Vector3[Pos.Count];
                            int eleNumber =-1;

                            if (E.Emitted) //Emittion Element
                            {
                                #region Get Path
                                waypoint = new Vector3[E.Emitnum+ Pos.Count -1];
                                int onGridpos;
                                E.New_Element.transform.position = BuildGrid.OBJ_Layers[Layer].transform.TransformPoint(GridSetting.SpaceLocationPos(E.OrigPosition.x, E.OrigPosition.y) + new Vector3(0, GridSetting.SpaceSize.y * E.Emitnum, 0));
                                int count = 0;
                                for (int emit = E.Emitnum-1; emit >0; emit--)
                                {
                                   waypoint[E.Emitnum - 1- emit] = BuildGrid.OBJ_Layers[Layer].transform.TransformPoint(GridSetting.SpaceLocationPos(E.OrigPosition.x, E.OrigPosition.y) + new Vector3(0, GridSetting.SpaceSize.y * (emit+1), 0));
                                    count++;
                                  
                                }
                                onGridpos = count ;
                                foreach (Vector2Int Vpos in Pos)
                                {
                                    Vector3 pos = BuildGrid.OBJ_Layers[Layer].transform.TransformPoint(GridSetting.SpaceLocationPos(Vpos.x, Vpos.y));
                                    LastPos = new Vector2Int(Vpos.x, Vpos.y);
                                    waypoint[count] = pos;
                                    count++;
                                }
                                #endregion

                                #region Emittion Dotween

                                var dotw = ele1.transform.DOPath(waypoint, waypoint.Length * (1 / Speed));
                                ActiveFalling++;
                                DotweenPaths.Add(dotw);

                                dotw.OnWaypointChange((int i) =>
                                {
                                    if (onGridpos != 0 && i == onGridpos)
                                    {
                                        E.New_Element.transform.SetParent(BuildGrid.OBJ_Layers[Layer].transform);
                                        E.New_Element.transform.localScale = new Vector3(1, 1, 1);
                                    }
                                    if (onGridpos == 0 && i == onGridpos+1)
                                    {
                                        E.New_Element.transform.SetParent(BuildGrid.OBJ_Layers[Layer].transform);
                                        E.New_Element.transform.localScale = new Vector3(1, 1, 1);
                                    }

                                });

                                dotw.OnComplete(() => {

                                    ActiveFalling--;
                                   BuildGrid.Layer[Layer].Spaces[LastPos.x, LastPos.y].Element = E.New_Element;
                                   BuildGrid.Layer[Layer].Spaces[LastPos.x, LastPos.y].Elementnum = E.ElementNumber;
                                    E.New_Element.transform.SetParent(BuildGrid.OBJ_Layers[Layer].transform);
                                    E.New_Element.transform.localScale = new Vector3(1, 1, 1);

                                });

                                dotw.SetEase(Ease.Linear);
                                #endregion

                            }
                            else
                            {
                                #region Get Path
                                waypoint = new Vector3[Pos.Count];

                                for (int i = 0; i < Pos.Count; i++)
                                {
                                    Vector3 pos = GridSetting.SpaceLocationPos(Pos[i].x, Pos[i].y);
                                    waypoint[i] = new Vector3(pos.x, pos.y, 0);
                                    LastPos = new Vector2Int(Pos[i].x, Pos[i].y);

                                }

                                eleNumber = BuildGrid.Layer[Layer].Spaces[FirstPos.x, FirstPos.y].Elementnum;

                                BuildGrid.Layer[Layer].Spaces[FirstPos.x, FirstPos.y].Element = null;
                                BuildGrid.Layer[Layer].Spaces[FirstPos.x, FirstPos.y].Elementnum = -1;
                                #endregion

                                #region Dotween Normal
                                var dotw = ele1.transform.DOLocalPath(waypoint, waypoint.Length * (1 / Speed));
                                ActiveFalling++;
                                DotweenPaths.Add(dotw);

                                dotw.OnComplete(() =>
                                {
                                    ActiveFalling--;
                                    BuildGrid.Layer[Layer].Spaces[LastPos.x, LastPos.y].Element = ele1;
                                    BuildGrid.Layer[Layer].Spaces[LastPos.x, LastPos.y].Elementnum = eleNumber;

                                });

                                dotw.SetEase(Ease.Linear);
                                #endregion

                            }

                        }
                    }
                }
            }

            
            #endregion



        }

        /// <summary>Create emiision positions on the grid.</summary>
        public class Emit : BuildGrid 
        {

            /// <summary>Class of emission space.</summary>
            protected class EmitSpace
            {
                public GameObject EmitObj;
                public int[] EmitElementNum;
                public string FallDirection = "down";
                public int RandomElementNum()
                {
                    int num = Random.Range(0, EmitElementNum.Length);
                    return num;
                }
            }

            /// <summary>Class of emission layer.</summary>
            protected class _EmitLayer
            {
                public EmitSpace[,] Space = null;
            }

            /// <summary>Intialise emission layer.</summary>
            protected static _EmitLayer[] EmitLayer = new _EmitLayer[10];

            /// <summary>Emit new elements at position</summary>
            /// <param name="Layer">Layer to Emit Elements</param>
            /// <param name="Row">Row to Emit</param>
            /// <param name="Column">Column to Emit</param>
            /// <param name="ElementNumbers">Elements to emit at position randomly</param>
            public static void Create(int Layer, int Row, int Column, int [] ElementNumbers)
            {
                bool CanCreateEmittion = true;
                string Error = "";
                if(BuildGrid.OBJ_Layers[Layer] != null)
                {

                }
                else 
                {
                    Debug.LogError("Match3 Creator : BuildMove - Layer is not Found");
                    BuildGrid.CreateGridLayer(Layer);
                }

                

                //Emittion Can be created
                if (CanCreateEmittion)
                {
                    if (EmitLayer[Layer] == null)
                    {
                        EmitLayer[Layer] = new _EmitLayer();
                        EmitLayer[Layer].Space = new EmitSpace[GridSetting.Rows, GridSetting.Columns];
                    }


                    EmitLayer[Layer].Space[Row, Column] = new EmitSpace();
                    EmitLayer[Layer].Space[Row, Column].EmitElementNum = ElementNumbers;

                    EmitLayer[Layer].Space[Row, Column].EmitObj = new GameObject("Empty");

                    var obj = EmitLayer[Layer].Space[Row, Column].EmitObj;
                    obj.name = "Emit:" + Row + "," + Column;
                    obj.transform.SetParent(BuildGrid.OBJ_Layers[Layer].transform);
                    obj.AddComponent<RectTransform>();
                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(GridSetting.SpaceSize.x, GridSetting.SpaceSize.y);
                    obj.AddComponent<CanvasRenderer>();
                    obj.AddComponent<Image>();
                    obj.AddComponent<Mask>();

                    obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    obj.transform.localPosition = GridSetting.SpaceLocationPos(Row, Column);
                    obj.GetComponent<Mask>().showMaskGraphic = false;

                }
                else
                {
                    Debug.LogError(Error);
                }
            }

            /// <summary>Emit a single new element at position; Will erase element at position if exist.</summary>
            /// <param name="Layer">Layer to Emit Elements</param>
            /// <param name="Row">Row to Emit</param>
            /// <param name="Column">Column to Emit</param>
            /// <param name="ElementNumbers">Elements to Emit</param>
            public static void CreateAtPosition(int Layer, int Row, int Column, int ElementNumber)
            {
                if (BuildGrid.OBJ_Layers[Layer] != null)
                {

                }
                else
                {
                    Debug.LogError("Match3 Creator : BuildMove - Layer is not Found");
                    BuildGrid.CreateGridLayer(Layer);
                }

                BuildGrid.Layer[Layer].DeleteElementAtSpace(Row, Column, 0);

                GameObject p = Instantiate(BuildGrid.Elements[ElementNumber]);
                p.transform.SetParent(BuildGrid.OBJ_Layers[Layer].transform);
                p.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                p.transform.localPosition = SpaceLocationPos(Row, Column);

                BuildGrid.Layer[Layer].Spaces[Row, Column] = new Space();
                BuildGrid.Layer[Layer].Spaces[Row, Column].Element = p;
                BuildGrid.Layer[Layer].SetElementNumAtSpace(Row, Column, ElementNumber);
                BuildGrid.Layer[Layer].Spaces[Row, Column].posx = Row;
                BuildGrid.Layer[Layer].Spaces[Row, Column].posy = Column;
                BuildGrid.Layer[Layer].Spaces[Row, Column].OnLayer = Layer;

            }


        }


    }
}