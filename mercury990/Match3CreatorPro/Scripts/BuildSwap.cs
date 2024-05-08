using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Match3Engine
{
    /// <summary>Enable Swapping on the grid.</summary>
    public class BuildSwap
    {

        /// <summary>Swapping options.</summary>
        public class Preset
        {
            /// <summary>Position pressed on.</summary>
            static Vector2Int ppos = new Vector2Int(-1, -1);

            /// <summary>Returns the position pressed on.</summary>
            public static Vector2Int PressedPosition_Vector2()
            {
                if (Input.GetMouseButtonUp(0))
                {
                    ppos = new Vector2Int(-1, -1);
                }
                if (GridSetting.MousePressedSpacePos().x != -1)
                {
                    ppos = GridSetting.MousePressedSpacePos();
                }
                return ppos;
            }

            /// <summary>Returns if position is pressed on.</summary>
            public static bool PressedPosition_Bool()
            {
                if (PressedPosition_Vector2().x == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            /// <summary>Returns an array of surrounding positions ([0] = Down position , [1] = Up position , [2] = Left position , [3] = Right position). </summary>
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Column position.</param>
            protected static Vector2Int[] PositionSurroundings(int Row, int Column)
            {
                Vector2Int Position = new Vector2Int(Row, Column);

                Vector2Int[] nxt = new Vector2Int[4];
                nxt[0] = GridSetting.SpaceNextToPos(Position.x, Position.y, 1, 0, 0, 0);
                nxt[1] = GridSetting.SpaceNextToPos(Position.x, Position.y, 0, 1, 0, 0);
                nxt[2] = GridSetting.SpaceNextToPos(Position.x, Position.y, 0, 0, 1, 0);
                nxt[3] = GridSetting.SpaceNextToPos(Position.x, Position.y, 0, 0, 0, 1);

                return nxt;
            }

            /// <summary>Checks if extra ordinary conditions that makes the Position unable to be interactable(Ex. Partition Grid). </summary>
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Column position.</param>
            protected static bool PositionIsAvailable(int Row, int Column)
            {
                Vector2Int Position = new Vector2Int(Row, Column);
                //Position is not Clickable
                if ((Position.x != -1 &&
                    Preset.SwapSpaces() != null &&
                    !Preset.SwapSpaces()[Position.x, Position.y].IsClickable)
                    ||
                    !Preset.CheckforElementNumbersPositionRestriction(Position) //Do Not Swap Elements that Share a position with these Element Numbers

                    )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>Returns a position based on mouse/touch direction. </summary>
            /// <param name="Row">Row position.</param>
            /// <param name="Column">Colunm position.</param>
            /// <param name="OutsideBounds">Must be outside the bounds of the given position.</param>
            protected static Vector2Int GetPositionSwipeTowardsPosition(int Row, int Column, bool OutsideBounds)
            {
                Vector2Int Position = new Vector2Int(Row, Column);
                string swipedir = GridSetting.MouseDirectionFromSpacePos(Position.x, Position.y);
                Vector2Int nxt = new Vector2Int(-1, -1);
                
                if (swipedir == "Down") { nxt = GridSetting.SpaceNextToPos(Position.x, Position.y, 1, 0, 0, 0); }//Debug.Log( "Down"); }
                else if (swipedir == "Up") { nxt = GridSetting.SpaceNextToPos(Position.x, Position.y, 0, 1, 0, 0); }//Debug.Log("Up"); }
                else if (swipedir == "Left") { nxt = GridSetting.SpaceNextToPos(Position.x, Position.y, 0, 0, 1, 0); }// Debug.Log("Left"); }
                else if (swipedir == "Right") { nxt = GridSetting.SpaceNextToPos(Position.x, Position.y, 0, 0, 0, 1); }//Debug.Log("Right"); }

                //Must be outside bounds of position bounds
                if (OutsideBounds && GridSetting.MouseOverSpacePos() != Position)
                {
                    return nxt;
                }
                else if (OutsideBounds)
                {
                    return new Vector2Int(-1, -1);
                }
                else
                {
                    return nxt;
                }

            }

            /// <summary>Returns if swap is occuring. </summary>
            public static bool AnySwapEngaged()
            {
                if (SingleSwap.SingleEngaged || SpaceSwap.SpaceSwapEngaged)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>Returns the swapping layer class based on LayerSwapping.</summary>
            public static BuildGrid.GridLayer SwapLayer()
            {
                return BuildGrid.Layer[LayerSwapping];
            }

            /// <summary>Returns the swapping spaces from the layer class.</summary>
            public static BuildGrid.Space[,] SwapSpaces()
            {
                return SwapLayer().Spaces;
            }
            /// <summary>Returns the pressed position space class.</summary>
            public static BuildGrid.Space PressedPositionSpace()
            {
                if (PressedPosition_Bool())
                {
                    return SwapSpaces()[PressedPosition_Vector2().x, PressedPosition_Vector2().y];
                }
                else { return null; }
            }

            /// <summary>Layer where the Swapping Occurs.</summary>
            public static int LayerSwapping = -1;
            /// <summary>Swap on positions only where given layer elements exist.</summary>
            public static int LayerBoundary = -1;
            /// <summary>Only Swap with given element numbers.</summary>
            public static int[] SwapElements = null;
            /// <summary>Only swap over given elements in LayerBoundary. </summary>
            public static int[] SwapElementsBoundary =null;
            /// <summary>Do not swap elements that share a position with these element numbers. This is regardless of layer. </summary>
            public static int[] ElementNumbersPositionRestriction = null;


            /// <summary>Gets all element numbers</summary>
            static int[] GetAllElementNums()
            {
                int count = 0;
                int[] temp = new int[100];
                for (int i = 0;  i < BuildGrid.Elements.Length; i++)
                {
                    if(BuildGrid.Elements[i] != null)
                    {
                        temp[count] = i;
                        count++;
                        
                    }
                }

                int[] temp2 = new int[count];

                for (int i = 0; i < count; i++)
                {
                    temp2[i] = temp[i];
                }

                return temp2;
            }

            /// <summary>Checks if element at position can swap. Checks Preset.SwapElements.</summary>
            protected static bool LayerSwappingConditionsMet(Vector2Int Position)
            {
                bool CanSwapElement = false;
                int[] SwappingE;
                if(SwapElements == null)
                {
                    SwappingE = GetAllElementNums();
                }
                else
                {
                    SwappingE = SwapElements;
                }

                //LayerSwapping
                //Checking each Element to find if element in layer can be swapped
                foreach (int LSE in SwappingE)
                {
                    if (Position.x < SwapLayer().Spaces.GetLength(0) && Position.y < SwapLayer().Spaces.GetLength(1))
                    {
                        int num = SwapLayer().Spaces[Position.x, Position.y].Elementnum;
                        if (num == LSE)
                        {
                            CanSwapElement = true;

                        }

                    }


                }


                return CanSwapElement;
            }

            /// <summary>Checks if element at position can swap over certain elements. Checks Preset.LayerBoundary and Preset.SwapElementsBoundary. </summary>
            protected static bool LayerSwapBoundConditionsMet(Vector2Int Position)
            {
                bool CanSwapOnBound = false;

       
                //Checking each Element to find if element in layer can be swapped
                if (LayerBoundary != -1 || SwapElementsBoundary != null)
                {
                    if (Position.x != -1)
                    {
                        if (SwapElementsBoundary != null)
                        {
                            foreach (int LSE in SwapElementsBoundary)
                            {
                                if (Position.x < BuildGrid.Layer[LayerBoundary].Spaces.GetLength(0) && Position.y < BuildGrid.Layer[LayerBoundary].Spaces.GetLength(1))
                                {
                                    int num = BuildGrid.Layer[LayerBoundary].Spaces[Position.x, Position.y].Elementnum;
                                    if (num == LSE)
                                    {
                                        CanSwapOnBound = true;

                                    }

                                }

                            }
                        }
                        else
                        {
                            if(BuildGrid.Layer[LayerBoundary].Spaces[Position.x, Position.y].Exist())
                            {
                                CanSwapOnBound = true;
                            }
                        }
                    }
                    return CanSwapOnBound;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>Checks if element can be swapped at position. Checks Preset.ElementNumbersPositionRestriction. </summary>
            protected static bool CheckforElementNumbersPositionRestriction(Vector2Int Position)
            {
                bool check = true;

                if (ElementNumbersPositionRestriction != null)
                {
                    foreach (BuildGrid.GridLayer L in BuildGrid.Layer)
                    {
                        foreach (int num in ElementNumbersPositionRestriction)
                        {
                            if (L != null && L.GetElementNumAtSpace(Position.x, Position.y) == num)
                            {
                                check = false;
                            }
                        }
                    }
                }
                else
                {
                    check = true;
                }

                return check;
            }

        }

        /// <summary>Swap one element with another element by a swipe. </summary>
        public class SingleSwap : Preset
        {
            /// <summary>Single Swap is engaged.</summary>
            static bool Engaged = false;

            /// <summary>Single Swap is active.</summary>
            static bool _SingleEngaged;

            /// <summary>Returns a whether swap is engaged. </summary>
            public static bool SingleEngaged
            {
                get { return _SingleEngaged; }
            }

            /// <summary>Sets swapping to disengaged. </summary>
            static void SwapComplete()
            {
                _SingleEngaged = false;
            }

            /// <summary>Can Swap with an Empty Spaces.</summary>
            public static bool CanSwapWithEmptySpaces = false;

            /// <summary>DG Moving tween Extension(Only when Move is Engaged)</summary>
            public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] SingleSwapTween;

            /// <summary>Amount of confirmed single swaps.</summary>
            static int ConfirmSwapCount = 0;

            /// <summary>Successful SingleSwap positions.</summary>
            static Vector2Int[,] ConfirmedSwaps = new Vector2Int[2, 999];

            /// <summary>Checks for extra ordinary conditions that makes the position unable to be interactable(Ex. Partition Grid). </summary>
            static bool PositionIsAvailable(Vector2Int Position)
            {
                
                //Position is not Clickable
                if ((Position.x != -1 &&
                    Preset.SwapSpaces() != null &&
                    !Preset.SwapSpaces()[Position.x, Position.y].IsClickable)
                    ||
                    !Preset.CheckforElementNumbersPositionRestriction(Position) //Do Not Swap Elements that Share a position with these Element Numbers
                    )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


            static bool ElementCanMove = false;
            static bool SpaceCanSwapWith = false;
            static bool EngageReset = false; //Reset Presets

            static Vector2Int PosPressedTemp = new Vector2Int(-1, -1);
            static Vector2Int PosPressedConfirm = new Vector2Int(-1, -1);
            static Vector2Int PosSwipeConfirm = new Vector2Int(-1, -1);
            static bool SwapStarted = false;

            /// <summary>Enable Single Swap.</summary>
            /// <param name="Layer">Layer to Engage Swapping on.</param>
            /// <param name="Speed">Duration of Swap.</param>
            public static void EngageSwap(int Layer, float Speed)
            {
                //Position is Released
                if (Input.GetMouseButtonUp(0) || EngageReset)
                {
                        if (EngageReset) { PosPressedTemp = new Vector2Int(-1, -1); }

                        Engaged = false;
                        ElementCanMove = false;
                        SpaceCanSwapWith = false;
                        EngageReset = false;
                        SwapStarted = false;

                

                    if (Input.GetMouseButtonUp(0) || Input.mousePosition.x == 0 || Input.mousePosition.y == 0) { PosPressedTemp = new Vector2Int(-1, -1); }
                        PosPressedConfirm = new Vector2Int(-1, -1);
                        PosSwipeConfirm = new Vector2Int(-1, -1);
                }

                //Gets pressed position
                if (Preset.PressedPosition_Bool())
                {
 
                        if (Preset.LayerSwapping == -1)
                        {
                            Preset.LayerSwapping = Layer;

                        }
                        else
                        {
                            Layer = Preset.LayerSwapping;
                        }

                      
                        if ( PosPressedTemp.x == -1)
                        {
                            PosPressedTemp = Preset.PressedPosition_Vector2();
                        }
                }

                //Position is pressed - Pressed Position not confirmed - Swap has not Occured
                if (PosPressedTemp.x != -1 && Engaged == false && !SwapStarted)
                {
                        Engaged = true;

                        //Check if Element Can Swap
                        if (PositionIsAvailable(PosPressedTemp) && Preset.LayerSwappingConditionsMet(PosPressedTemp) && Preset.LayerSwapBoundConditionsMet(PosPressedTemp))
                        {
                            ElementCanMove = true;
                            PosPressedConfirm = PosPressedTemp;
                        }
                        else
                        {
                            EngageReset = true;
                        }
                }

                //Wait For Swipe - Pressed Position Confirmed - Swap has not Occured
                if (Engaged && !SwapStarted)
                {
                        Vector2Int PosSwipe = Preset.GetPositionSwipeTowardsPosition(PosPressedTemp.x, PosPressedTemp.y, true);
                        if (PosSwipe.x != -1)
                        {
                            bool PosElementExist= BuildGrid.Layer[Preset.LayerSwapping].Spaces[PosSwipe.x, PosSwipe.y].Exist();

                            if (PositionIsAvailable(PosSwipe) && (Preset.LayerSwappingConditionsMet(PosSwipe) || (CanSwapWithEmptySpaces && !PosElementExist)) && Preset.LayerSwapBoundConditionsMet(PosSwipe))
                            {
                                SpaceCanSwapWith = true;
                                PosSwipeConfirm = PosSwipe;
                            }
                            else
                            {
                                EngageReset = true;
                            }
                        }
                }

                //Swipe Confirmed
                if (ElementCanMove && SpaceCanSwapWith && !SwapStarted)
                {
                        _SingleEngaged = true;
                        var p = BuildMove.SwitchElements(Preset.LayerSwapping, PosPressedConfirm, Preset.LayerSwapping, PosSwipeConfirm, Speed, false);

                        p[0].OnComplete(SwapComplete);


                        SingleSwapTween = p;
                        SwapStarted = true;

                        //All Swaps
                        ConfirmedSwaps[0, ConfirmSwapCount] = PosPressedConfirm;
                        ConfirmedSwaps[1, ConfirmSwapCount] = PosSwipeConfirm;

                        ConfirmSwapCount++;
                        if (ConfirmedSwaps.GetLength(1) <= ConfirmSwapCount)
                        {
                            ConfirmSwapCount = 0;
                            ConfirmedSwaps[0, ConfirmSwapCount] = PosPressedConfirm;
                            ConfirmedSwaps[1, ConfirmSwapCount] = PosSwipeConfirm;
                            ConfirmSwapCount++;
                        }

             
                }
           
            }
        }

        /// <summary>Swap one element with an empty space by tapping on that space. </summary>
        public class SpaceSwap : Preset
        {
            /// <summary>Space Swap is engaged.</summary>
            static bool Engaged = false;

            /// <summary>Space Swap is Active</summary>
            static bool _SpaceSwapEngaged;

            /// <summary>Returns a whether swap is engaged. </summary>
            public static bool SpaceSwapEngaged
            {
                get { return _SpaceSwapEngaged; }
            }

            /// <summary>Sets swapping to disengaged. </summary>
            static void SwapComplete()
            {
                _SpaceSwapEngaged = false;
            }

            /// <summary>DG Moving tween Extension(Only when Move is Engaged)</summary>
            public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] SpaceSwapTween;
            
            /// <summary>Swap the element with empty space.</summary>
            /// <param name="Layer">Layer to Engage Swapping on.</param>
            /// <param name="Speed">Duration of Swap.</param>
            public static void EngageSwap(int Layer, float Speed)
            {
                //Position is Released
                if (Input.GetMouseButtonUp(0))
                {
                    Engaged = false;
                }

                //Position is pressed
                if (Preset.PressedPosition_Bool() && Engaged == false)
                {
                    bool ElementCanMove = false;
                    bool SpaceCanMoveTo = false;
                    Vector2Int EmptyPos = new Vector2Int(-1, -1);

                    //Set Layer
                    if (Preset.LayerSwapping == -1)
                    {
                        Preset.LayerSwapping = Layer;

                    }
                    else
                    {
                        Layer = Preset.LayerSwapping;
                    }

                    //Check if Element Can Swap
                    if (Preset.PositionIsAvailable(Preset.PressedPosition_Vector2().x, Preset.PressedPosition_Vector2().y) && Preset.LayerSwappingConditionsMet(Preset.PressedPosition_Vector2()) && Preset.LayerSwapBoundConditionsMet(Preset.PressedPosition_Vector2()))
                    {
                        ElementCanMove = true;
                
                    }

                    //Check Surrounds for Empty Space && //If Empty Space Can be moved to
                    foreach (Vector2Int Psur in Preset.PositionSurroundings(Preset.PressedPosition_Vector2().x, Preset.PressedPosition_Vector2().y))
                    {
                        if (Psur.x != -1)
                        {
                            //Space Element Empty
                            if (Preset.PositionIsAvailable(Psur.x, Psur.y) && !BuildGrid.Layer[Preset.LayerSwapping].Spaces[Psur.x, Psur.y].Exist() && Preset.LayerSwapBoundConditionsMet(Psur))
                            {
                                EmptyPos = Psur;
                                SpaceCanMoveTo = true;
       
                            }

                        }
                    }

                    //If Swap has be Confirmed
                    if (ElementCanMove && SpaceCanMoveTo)
                    {
                        _SpaceSwapEngaged = true;
                        var p = BuildMove.SwitchElements(Preset.LayerSwapping, Preset.PressedPosition_Vector2(), Preset.LayerSwapping, EmptyPos, Speed, false);

                        p[0].OnComplete(SwapComplete);

                        SpaceSwapTween = p;
              
                    }

                    Engaged = true;
                }


            }
        }
    }
}

