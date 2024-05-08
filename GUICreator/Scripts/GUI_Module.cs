#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUICreator
{

    public static class GUI_Module 
    {
        private static int BTT_Count = 0;   //Button
        private static int LBL_Count = 0;   //Label
        private static int TXF_Count = 0;   //Text Field
        private static int TXA_Count = 0;   //Text Area
        private static int FLO_Count = 0;   //Float Field
        private static int INT_Count = 0;   //Int Field
        private static int V2F_Count = 0;   //Vector2 Float
        private static int V2I_Count = 0;   //Vector2 Int
        private static int V3F_Count = 0;   //Vector3 Float
        private static int V3I_Count = 0;   //Vector3 Int
        private static int COF_Count = 0;   //Color Field 
        private static int OBF_Count = 0;   //Object Field
        private static int TGF_Count = 0;   //Toggle
        private static int SEG_Count = 0;   //Selection Grid
        private static int HOS_Count = 0;   //Horizontal Slider
        private static int VES_Count = 0;   //Vertical Slider
        private static int Scroll_Count = 0;   //Scroll Bar


        /// <summary>
        /// Track the Elements
        /// </summary>
        /// <param name="s"></param>
        private static int COUNT(string s)
        {

            if (s == "Button") 
            {
                BTT_Count++;
                if (GUI_Store.Bttn == null) { GUI_Store.Bttn = new List<GUI_Store.ButtonState>();}

                //Debug.Log("Blist: "+ GUI_Store.Bttn.Count+ " B" + BTT_Count);
                if (GUI_Store.Bttn.Count < BTT_Count)
                {
                   // Debug.Log("Button: " + BTT_Count);
                    GUI_Store.Bttn.Add(new GUI_Store.ButtonState(BTT_Count));
                }
                else
                {
                    // GUI_Store.Bttn[BTT_Count - 1] = new ButtonState(BTT_Count);
                }
                return BTT_Count;
            }
            else if (s == "Label")
            {
                LBL_Count++;
                if (GUI_Store.Label == null) { GUI_Store.Label = new List<GUI_Store.LabelState>(); }

                if (GUI_Store.Label.Count < BTT_Count)
                {
                   // Debug.Log("Label: " + LBL_Count);
                    GUI_Store.Label.Add(new GUI_Store.LabelState(LBL_Count));
                }
                else
                {
                    // GUI_Store.Bttn[BTT_Count - 1] = new ButtonState(BTT_Count);
                }
                return LBL_Count;
            }
            else if (s == "TextField")
            {
                TXF_Count++;
                if (GUI_Store.TxtF == null) { GUI_Store.TxtF = new List<GUI_Store.TextFieldState>(); }

                if (GUI_Store.TxtF.Count < TXF_Count)
                {
                    GUI_Store.TxtF.Add(new GUI_Store.TextFieldState(TXF_Count));
                }
                else
                {
                    // GUI_Store.Bttn[BTT_Count - 1] = new ButtonState(BTT_Count);
                }
                return TXF_Count;
            }
            else if (s == "TextAreaField")
            {
                TXA_Count++;
                if (GUI_Store.TxtFA == null) { GUI_Store.TxtFA = new List<GUI_Store.TextFieldAreaState>(); }

                if (GUI_Store.TxtFA.Count < TXA_Count)
                {
                    GUI_Store.TxtFA.Add(new GUI_Store.TextFieldAreaState(TXA_Count));
                }
                else
                {
                    // GUI_Store.Bttn[BTT_Count - 1] = new ButtonState(BTT_Count);
                }
                return TXA_Count;
            }
            else if (s == "FloatField")
            {
                FLO_Count++;
                if (GUI_Store.FloF == null) { GUI_Store.FloF = new List<GUI_Store.FloatFieldState>(); }

                if (GUI_Store.FloF.Count < FLO_Count)
                {
                    GUI_Store.FloF.Add(new GUI_Store.FloatFieldState(FLO_Count));
                }
                else
                {

                }
                return FLO_Count;
            }
            else if (s == "IntField")
            {
                INT_Count++;
                if (GUI_Store.IntF == null) { GUI_Store.IntF = new List<GUI_Store.IntFieldState>(); }

                if (GUI_Store.IntF.Count < INT_Count)
                {
                    GUI_Store.IntF.Add(new GUI_Store.IntFieldState(INT_Count));
                }
                else
                {

                }
                return INT_Count;
            }
            else if (s == "Vector2FloatField")
            {
                V2F_Count++;
      
                if (GUI_Store.Vec2FloF == null) { GUI_Store.Vec2FloF = new List<GUI_Store.Vector2FloatState>(); }

                if (GUI_Store.Vec2FloF.Count < V2F_Count)
                {
                    GUI_Store.Vec2FloF.Add(new GUI_Store.Vector2FloatState(V2F_Count));
                }
                else
                {

                }
                return V2F_Count;
            }
            else if (s == "Vector2IntField")
            {
                V2I_Count++;
                
                if (GUI_Store.Vec2IntF == null) { GUI_Store.Vec2IntF = new List<GUI_Store.Vector2IntState>(); }

                if (GUI_Store.Vec2IntF.Count < V2I_Count)
                {
                    GUI_Store.Vec2IntF.Add(new GUI_Store.Vector2IntState(V2I_Count));
                }
                else
                {

                }
                return V2I_Count;
            }
            else if (s == "Vector3FloatField")
            {
                V3F_Count++;

                if (GUI_Store.Vec3FloF == null) { GUI_Store.Vec3FloF = new List<GUI_Store.Vector3FloatState>(); }

                if (GUI_Store.Vec3FloF.Count < V3F_Count)
                {
                    GUI_Store.Vec3FloF.Add(new GUI_Store.Vector3FloatState(V3F_Count));
                }
                else
                {

                }
                return V3F_Count;
            }
            else if (s == "Vector3IntField")
            {
                V3I_Count++;

                if (GUI_Store.Vec3IntF == null) { GUI_Store.Vec3IntF = new List<GUI_Store.Vector3IntState>(); }

                if (GUI_Store.Vec3IntF.Count < V3I_Count)
                {
                    GUI_Store.Vec3IntF.Add(new GUI_Store.Vector3IntState(V3I_Count));
                }
                else
                {

                }
                return V3I_Count;
            }
            else if (s == "ColorField")
            {
                COF_Count++;

                if (GUI_Store.ColF == null) { GUI_Store.ColF = new List<GUI_Store.ColorFieldState>(); }

                if (GUI_Store.ColF.Count < COF_Count)
                {
                    GUI_Store.ColF.Add(new GUI_Store.ColorFieldState(COF_Count));
                }
                else
                {

                }
                return COF_Count;
            }
            else if (s == "ObjectField")
            {
                OBF_Count++;

                if (GUI_Store.ObjF == null) { GUI_Store.ObjF = new List<GUI_Store.ObjFieldState>(); }

                if (GUI_Store.ObjF.Count < OBF_Count)
                {
                    GUI_Store.ObjF.Add(new GUI_Store.ObjFieldState(OBF_Count));
                }
                else
                {

                }
                return OBF_Count;
            }
            else if (s == "Toggle")
            {
                TGF_Count++;

                if (GUI_Store.Togg == null) { GUI_Store.Togg = new List<GUI_Store.ToggleState>(); }

                if (GUI_Store.Togg.Count < TGF_Count)
                {
                    GUI_Store.Togg.Add(new GUI_Store.ToggleState(TGF_Count));
                }
                else
                {

                }
                return TGF_Count;
            }
            else if (s == "SelectionGrid")
            {
                SEG_Count++;

                if (GUI_Store.SelG == null) { GUI_Store.SelG = new List<GUI_Store.SelectionGridState>(); }

                if (GUI_Store.SelG.Count < SEG_Count)
                {
                    GUI_Store.SelG.Add(new GUI_Store.SelectionGridState(SEG_Count));
                }
                else
                {

                }
                return SEG_Count;
            }
            else if (s == "HorizontalSlider")
            {
                HOS_Count++;

                if (GUI_Store.HorS == null) { GUI_Store.HorS = new List<GUI_Store.HorizontalSliderState>(); }

                if (GUI_Store.HorS.Count < HOS_Count)
                {
                    GUI_Store.HorS.Add(new GUI_Store.HorizontalSliderState(HOS_Count));
                }
                else
                {

                }
                return HOS_Count;
            }
            else if (s == "VerticalSlider")
            {
                VES_Count++;

                if (GUI_Store.VerS == null) { GUI_Store.VerS = new List<GUI_Store.VerticalSliderState>(); }

                if (GUI_Store.VerS.Count < VES_Count)
                {
                    GUI_Store.VerS.Add(new GUI_Store.VerticalSliderState(VES_Count));
                }
                else
                {

                }
                return VES_Count;
            }
            else if (s == "Scroll")
            {
                Scroll_Count++;

                if (GUI_Store.Scroll == null) { GUI_Store.Scroll = new List<GUI_Store.ScrollState>(); }

                if (GUI_Store.Scroll.Count < Scroll_Count)
                {
                    GUI_Store.Scroll.Add(new GUI_Store.ScrollState(Scroll_Count));
                }
                else
                {

                }
                return Scroll_Count;
            }
            else
            {
                return -1;
            }


        }

        private static void RESET()
        {
            BTT_Count = 0;
            LBL_Count = 0;   //Label
            TXF_Count = 0;   //Text Field
            TXA_Count = 0;   //Text Area
            FLO_Count = 0;   //Float Field
            INT_Count = 0;   //Int Field
            V2F_Count = 0;   //Vector2 Float
            V2I_Count = 0;   //Vector2 Int
            V3F_Count = 0;   //Vector3 Float
            V3I_Count = 0;   //Vector3 Int
            COF_Count = 0;   //Color Field 
            OBF_Count = 0;   //Object Field
            TGF_Count = 0;   //Toggle
            SEG_Count = 0;   //Selection Grid
            HOS_Count = 0;   //Horizontal Slider
            VES_Count = 0;   //Vertical Slider
            Scroll_Count = 0;   //Scroll Bar
        }

        /// <summary>
        /// Reached the end of the GUI Creation
        /// </summary>
        public static void END()
        {
            GUI_BeginCycle = false;
            GUI_EndCycle = true;
           // Debug.Log("Cycle End");
            RESET();
        }

        /// <summary>
        /// GUI Has Started **Very first startup
        /// </summary>
        private static bool GUI_Start = false;

        /// <summary>
        /// GUI Has Started
        /// </summary>
        private static bool GUI_BeginCycle = false;

        /// <summary>
        /// GUI Has Started
        /// </summary>
        private static bool GUI_EndCycle = true;

        internal static void OnGUI(string type, out int ID)
        {
          
            //Has Been Started
            if (!GUI_Start) { GUI_Start = true; }

            //Cycle has started
            if (GUI_EndCycle) { GUI_BeginCycle = true; GUI_EndCycle = false; }
            //Debug.Log("Cycle Start"); }

            ID = COUNT(type);


        }

        /// <summary>
        /// Convert a Sprite to a texture ////Caution Some Sprites cannot be converted
        /// </summary>
        /// <param name="sprite">Sprite</param>
        /// <returns>Texture 2D</returns>
        public static Texture2D SpriteToTexture(Sprite sprite)
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                try
                {
                    
                    Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                                 (int)sprite.textureRect.y,
                                                                 (int)sprite.textureRect.width,
                                                                 (int)sprite.textureRect.height);
                    newText.SetPixels(newColors);
                    newText.Apply();

                    return newText;
                }
                catch
                {
                   // Debug.LogWarning("GUICreator --- Could NOT Convert Sprite Texture// May not need Conversion");
                    return sprite.texture;
                }

                
            }
            else
                return sprite.texture;
        }

        /// <summary>
        /// Draws a Color on the GUI 
        /// </summary>
        /// <param name="PositionX">x Position</param>
        /// <param name="PositionY">y Position</param>
        /// <param name="Width">Width of Texture</param>
        /// <param name="Height">Height of Texture</param>
        /// <param name="color">Color of the texture</param>
        public static void DrawTexture(int PositionX, int PositionY, float Width, float Height, Color color)
        {
            Rect position = new Rect(PositionX, PositionY, Width, Height);
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(position, GUIContent.none);
        }

        /// <summary>
        /// Draws a Texture on the GUI 
        /// </summary>
        /// <param name="PositionX">x Position</param>
        /// <param name="PositionY">y Position</param>
        /// <param name="Width">Width of Texture</param>
        /// <param name="Height">Height of Texture</param>
        /// <param name="texture">Texture to draw</param>
        public static void DrawTexture(int PositionX, int PositionY, float Width, float Height, Texture2D texture)
        {
            Rect position = new Rect(PositionX, PositionY, Width, Height);
            GUI.skin.box.normal.background = texture;
            GUI.Box(position, GUIContent.none);
        }

        

    }


    internal static class GUI_Store 
    {
        #region Type Class
        public class ButtonState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            /// <summary>
            /// Amount of times clicked
            /// </summary>
            public int TimesClicked;

            public ButtonState(int _ID)
            {
                ID = _ID;

            }
        }

        public class LabelState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public LabelState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Label Text
            /// </summary>
            public string Text { get; set; }
        }

        public class TextFieldState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public TextFieldState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get TextField Text
            /// </summary>
            public string Text { get; set; }
        }

        public class TextFieldAreaState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public TextFieldAreaState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get TextField Text
            /// </summary>
            public string Text { get; set; }
        }

        public class FloatFieldState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public FloatFieldState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Float Value
            /// </summary>
            public float Value { get; set; }
        }

        public class IntFieldState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public IntFieldState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Int Value
            /// </summary>
            public int Value { get; set; }
        }

        public class Vector2FloatState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public Vector2FloatState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Vector Value
            /// </summary>
            public Vector2 Value { get; set; }
        }

        public class Vector2IntState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public Vector2IntState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Vector Value
            /// </summary>
            public Vector2Int Value { get; set; }
        }

        public class Vector3FloatState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public Vector3FloatState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Vector Value
            /// </summary>
            public Vector3 Value { get; set; }
        }

        public class Vector3IntState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public Vector3IntState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Vector Value
            /// </summary>
            public Vector3Int Value { get; set; }
        }

        public class ColorFieldState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public ColorFieldState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Color Value
            /// </summary>
            public Color Value { get; set; }
        }

        public class ObjFieldState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public ObjFieldState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Object
            /// </summary>
            public UnityEngine.Object Obj { get; set; }
        }

        public class ToggleState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public ToggleState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get state Value
            /// </summary>
            public bool Value { get; set; }
        }

        public class SelectionGridState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public SelectionGridState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Selection Value
            /// </summary>
            public int Value { get; set; }
        }

        public class HorizontalSliderState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public HorizontalSliderState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Float Value
            /// </summary>
            public float Value { get; set; }
        }

        public class VerticalSliderState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public VerticalSliderState(int _ID)
            {
                ID = _ID;

            }

            /// <summary>
            /// Get Float Value
            /// </summary>
            public float Value { get; set; }
        }

        public class ScrollState
        {
            /// <summary>
            /// Unique number id
            /// </summary>
            public int ID;

            public Vector2 scrollPosition = Vector2.zero;

            public ScrollState(int _ID)
            {
                ID = _ID;

            }
        }

        #endregion


        public static List<ButtonState> Bttn;// = new List<ButtonState>();
        public static List<LabelState> Label;
        public static List<TextFieldState> TxtF;
        public static List<TextFieldAreaState> TxtFA;
        public static List<FloatFieldState> FloF;
        public static List<IntFieldState> IntF;
        public static List<Vector2FloatState> Vec2FloF;
        public static List<Vector2IntState> Vec2IntF;
        public static List<Vector3FloatState> Vec3FloF;
        public static List<Vector3IntState> Vec3IntF;
        public static List<ColorFieldState> ColF;
        public static List<ObjFieldState> ObjF;
        public static List<ToggleState> Togg;
        public static List<SelectionGridState> SelG;
        public static List<HorizontalSliderState> HorS;
        public static List<VerticalSliderState> VerS;
        public static List<ScrollState> Scroll;

    }



    

    



}
#endif




