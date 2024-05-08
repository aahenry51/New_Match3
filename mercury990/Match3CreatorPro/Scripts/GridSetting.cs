using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3Engine
{
    /// <summary>General Settings of the Grid.</summary>
    public class GridSetting : MonoBehaviour
    {
        /// <summary>Rows of the Grid.</summary>
        private static int _BaseCellPosX = 0;
        /// <summary>Columns of the Grid. </summary>
        private static int _BaseCellPosY = 0;
        /// <summary>Parent Object of the Grid. </summary>
        private static GameObject _GridContainer;
        /// <summary>Canvas that contains grid. </summary>
        private static Canvas _GridCanvas;
        /// <summary>Size of each space on the grid. </summary>
        private static Vector2 _CellSize;
        /// <summary>Local position of the parent object of the grid. </summary>
        public static Vector3 GridContainer_LocalPosition = new Vector3(0, 0, 0);

        /// <summary>Sets the basic setting of the grid.</summary>
        protected static void BasicSettings(int Rows, int Columns, Vector2 SpaceSize, Transform Parent_Base, Canvas C)
        {
            _BaseCellPosX = Rows;
            _BaseCellPosY = Columns;
            _CellSize = SpaceSize;
            _GridContainer = new GameObject("Empty");
            _GridContainer.AddComponent(typeof(RectTransform));
            _GridContainer.name = "Grid_Container";
            _GridContainer.transform.SetParent(Parent_Base);
            _GridContainer.transform.localPosition = new Vector3(0, 0, 0);
            _GridContainer.transform.localScale = new Vector3(1, 1, 1);
            _GridCanvas = C;
            RectTransform rect = _GridContainer.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(Columns * SpaceSize.x, Rows * SpaceSize.y);

            //Setup Fall
            BuildMove.Fall.CustomFalldir = new BuildMove.Fall.FallPattern[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BuildMove.Fall.CustomFalldir[i, j] = new BuildMove.Fall.FallPattern();
                    BuildMove.Fall.CustomFalldir[i, j].Directions = new Vector2Int[] { new Vector2Int(1, 0) };
                }
            }

        }

        //Read only
        /// <summary>Returns the Total Amount of Rows (Read Only) To Set Use [BuildGrid.Create]. </summary>
        public static int Rows
        {
            get { return _BaseCellPosX; }
        }

        /// <summary>Returns the Total Amount of Columns (Read Only) To Set Use [BuildGrid.Create]. </summary>
        public static int Columns
        {
            get { return _BaseCellPosY; }
        }

        /// <summary>Returns the Parent Object of Grid Elements(Read Only) To Set Use [BuildGrid.Create]. </summary>
        public static GameObject GridContainer
        {
            get { return _GridContainer; }
        }

        /// <summary>Returns the Canvas Parent Object(Read Only) To Set Use [BuildGrid.Create]. </summary>
        public static Canvas GridCanvas
        {
            get { return _GridCanvas; }
        }

        /// <summary>Returns size of the Spaces(Read Only) To Set Use [BuildGrid.Create]. </summary>
        public static Vector2 SpaceSize
        {
            get { return _CellSize; }
        }

        /// <summary>Returns the Local Position of Space(Read Only)[Parent: GridContainer] To Set Use [BuildGrid.Create]. </summary>
        public static Vector3 SpaceLocationPos(int Row, int Column)
        {
            int i = Row;
            int j = Column;
            Vector3 Cellp = new Vector3();

            RectTransform rect = _GridContainer.GetComponent<RectTransform>();
            Cellp = new Vector3((j * _CellSize.x) - (rect.sizeDelta.x / 2) + (_CellSize.x / 2), (-i * _CellSize.y) + (rect.sizeDelta.y / 2) - (_CellSize.y / 2), 0);
            return Cellp;
        }

        /// <summary>Returns the Local Position of Space TopLeft[0] and BottomRight[1](Read Only)[Parent: GridContainer].  </summary>
        public static Vector3[] SpaceBounds(int Row, int Column)
        {
            int i = Row;
            int j = Column;
            Vector3[] pos = new Vector3[2];
            //Top Left
            pos[0] = new Vector3(SpaceLocationPos(i, j).x - (SpaceSize.x / 2), SpaceLocationPos(i, j).y + (SpaceSize.y / 2), 0);
            //Bottom Right
            pos[1] = new Vector3(SpaceLocationPos(i, j).x + (SpaceSize.x / 2), SpaceLocationPos(i, j).y - (SpaceSize.y / 2), 0);

            return pos;
        }

        /// <summary>Returns the grid space position the mouse is over. </summary>
        public static Vector2Int MouseOverSpacePos()
        {
            Vector3[] Cellbound;
            Vector3 m;
            if (GridCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                m = Input.mousePosition;
            }
            else
            {
                m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            Vector2Int MouseOver = new Vector2Int(-1, -1);

            for (int i = 0; i < _BaseCellPosX; i++)
            {
                for (int j = 0; j < _BaseCellPosY; j++)
                {
                    Cellbound = SpaceBounds(i, j);
                    Vector3 TopLeft = _GridContainer.transform.TransformPoint(Cellbound[0]);
                    Vector3 BottomRight = _GridContainer.transform.TransformPoint(Cellbound[1]);
                    if (m.x > TopLeft.x && m.x < BottomRight.x && m.y < TopLeft.y && m.y > BottomRight.y)
                    {
                        MouseOver = new Vector2Int(i, j);//Touch within bounds of x and y
                    }

                }
            }

            return MouseOver;
        }

        /// <summary>Returns the Space Grid position Mouse Pressed down on (Single Frame). </summary>
        public static Vector2Int MousePressedSpacePos()
        {
            Vector2Int MousePressed = new Vector2Int(-1, -1);

            if (Input.GetMouseButtonDown(0))
            {
                MousePressed = MouseOverSpacePos();

            }
            return MousePressed;
        }

        /// <summary>Returns the Space Grid position Mouse Released on (Single Frame). </summary>
        public static Vector2Int MouseReleasedSpacePos()
        {
            Vector2Int MousePressed = new Vector2Int(-1, -1);

            if (Input.GetMouseButtonUp(0))
            {
                MousePressed = MouseOverSpacePos();

            }
            return MousePressed;
        }

        /// <summary>Returns the Direction("Up","Down","Left","Right") based on Space Position to Mouse Position Point Vector. </summary>
        /// <param name="Row">The Space Row Location</param>
        /// <param name="Column">The Space Column Location</param>
        public static string MouseDirectionFromSpacePos(int Row, int Column)
        {
            int i = Row;
            int j = Column;

            //Debug.Log();

            _GridContainer.transform.TransformPoint(SpaceLocationPos(i, j));
            Vector3 temptile = _GridContainer.transform.TransformPoint(SpaceLocationPos(i, j));
            // Vector3 m = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
            //Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            Vector3 m;
            if (GridCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                m = Input.mousePosition;
            }
            else
            {
                m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            //Current position minus initial position
            Vector3 change = m - ((temptile));
            //float chy = tctrl.curpy - (temptile.GetComponent<obj_grid>().gridy new Vector2((tile_width / 2), -tile_height - 1););
            float deg = (float)((180 / 3.14) * Mathf.Atan2(change.y, change.x));

            if (deg < -45 && deg >= -135) { return "Down"; }//Down
            else if (deg < 135 && deg >= 45) { return "Up"; }//up
            else if (deg < -135 || deg >= 135) { return "Left"; }//Left
            else if (deg < 45 && deg >= 0 || deg < 0 && deg >= -45) { return "Right"; } //right
            else { return "null"; }


        }

        /// <summary>Returns a grid position given the amount of spaces to move. </summary>
        /// <param name="Row">The Row postiion. </param>
        /// <param name="Column">The Column position. </param>
        /// <param name="MoveDown">Amount of space to move down.</param>
        /// <param name="MoveUp">Amount of space to move up.</param>
        /// <param name="MoveLeft">Amount of space to move left.</param>
        /// <param name="MoveRight">Amount of space to move right./param>
        public static Vector2Int SpaceNextToPos(int Row, int Column, int MoveDown, int MoveUp, int MoveLeft, int MoveRight)
        {
            int i = Row;
            int j = Column;

            int AmountToMoveDown = MoveDown - MoveUp;
            int AmountToMoveRight = MoveRight - MoveLeft;

            int newCellipos = i + AmountToMoveDown;
            int newCelljpos = j + AmountToMoveRight;

            if ((newCellipos >= 0 && newCellipos < Rows) && (newCelljpos >= 0 && newCelljpos < Columns))
            {
                return new Vector2Int(newCellipos, newCelljpos);
            }
            else
            {
                return new Vector2Int(-1, -1);
            }
        }

        /// <summary>Returns if Mouse is Off Screen. </summary>
        public static bool MouseOffScreen()
        {
            if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}