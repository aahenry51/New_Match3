#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GUICreator
{

   

    public class Items
    {


       // public int TextField_Count = -1;
       // public string[] TextField_Text = new string[1000];

        public float GUIoffset_X = 0;
        public float GUIoffset_Y = 0;

        public class ScrollPosition
        {
            private int IDNumber = -1;

            public Vector2 scrollPosition
            {
                get { return GUI_Store.Scroll[IDNumber - 1].scrollPosition; }
                set { GUI_Store.Scroll[IDNumber - 1].scrollPosition = value; }
            }

            /// <summary>
            /// Create Scrollable Contain for GUI Items// Must have a EndScrollView()
            /// </summary>
            /// <param name="WindowX">X Position Window to view the container</param>
            /// <param name="WindowY">Y Position Window to view the container</param>
            /// <param name="Window_Width">Width Window to view the container</param>
            /// <param name="Window_Height">Height Window to view the container</param>
            /// <param name="InsideX">X Position of inside the container</param>
            /// <param name="InsideY">Y Position of inside the container</param>
            /// <param name="Inside_Width">Width of inside the container</param>
            /// <param name="Inside_Height">Height of inside the container</param>
            public ScrollPosition(int WindowX, int WindowY, float Window_Width, float Window_Height, int InsideX, int InsideY, float Inside_Width, float Inside_Height)
            {
                
                GUI_Module.OnGUI("Scroll", out IDNumber);
                scrollPosition = GUI.BeginScrollView(new Rect(WindowX, WindowY, Window_Width, Window_Height), scrollPosition, new Rect(InsideX, InsideY, Inside_Width, Inside_Height));
               
            }

            /// <summary>
            /// End of Reading Items into the scroll View
            /// </summary>
            public void EndScrollView()
            {
                GUI.EndScrollView();
            }
        }

        /// <summary>
        /// Set to default if custom is not created
        /// </summary>
        /// <param name="GStyle"></param>
        /// <param name="EntryType"></param>
        /// <returns></returns>
        private GUIStyle SetStyle(GUIStyle GStyle, string EntryType)
        {
            if (GStyle == default(GUIStyle) || GStyle == null)
            {
                return new GUIStyle(EntryType);
            }
            else
            {
                return GStyle;
            }

        }

        public class Button : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Button to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Title">Sets the Text of the Button</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public Button(int PositionX, int PositionY, string Title = "", float Width = 50, float Height = 50, GUIStyle GStyle = default(GUIStyle))
            {

                //Creating GUI
                
                GUI_Module.OnGUI("Button", out IDNumber);

                OnClick = GUI.Button(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), Title, SetStyle(GStyle, "Button"));
                if (OnClick == true) { GUI_Store.Bttn[IDNumber - 1].TimesClicked++; }// Debug.Log((IDNumber - 1) + "Times Click: "+ GUI_Store.Bttn[IDNumber-1].TimesClicked); };
            }

            /// <summary>
            /// Returns if Button is Clicked on
            /// </summary>
            public bool OnClick;

            /// <summary>
            /// Return the Amount of Times Clicked
            /// </summary>
            /// <returns></returns>
            public int TimesClicked()
            {
                return GUI_Store.Bttn[IDNumber].TimesClicked;
            }


        }

        public class Label : Items
        {
            /// <summary>
            /// Unique ID Number
            /// </summary>
            private int IDNumber = -1;

            /// <summary>Adds a Label to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Text">Sets the Text of the Label</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public Label(int PositionX, int PositionY, string text, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {
                GUI_Module.OnGUI("Label", out IDNumber);

                
                //Creating GUI

                GUI.Label(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.Label[IDNumber - 1].Text, SetStyle(GStyle, "Label"));

                GUI_Store.Label[IDNumber - 1].Text = text;
            }

            /// <summary>
            /// Get/Set Label Text
            /// </summary>
            public string text
            {
                get { return GUI_Store.Label[IDNumber - 1].Text; }
                set { GUI_Store.Label[IDNumber - 1].Text = value; }
            }

        }

        public class TextField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Text Box to the GUI</summary>
            /// <param name="UniqueID">A Reference identifier to the field; Must be positive and less than 1000</param>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public TextField(int PositionX, int PositionY, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("TextField", out IDNumber);

                GUI_Store.TxtF[IDNumber-1].Text = GUI.TextField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.TxtF[IDNumber - 1].Text, SetStyle(GStyle, "TextField"));
                text = GUI_Store.TxtF[IDNumber - 1].Text;


            }

            /// <summary>
            /// Get/Set TextField Text
            /// </summary>
            public string text
            {
                get { return GUI_Store.TxtF[IDNumber - 1].Text; }
                set { GUI_Store.TxtF[IDNumber - 1].Text = value; }
            }

            /// <summary>
            /// Clears the Field
            /// </summary>
            public void Clear()
            {
                GUI_Store.TxtF[IDNumber - 1].Text = "";
                text = "";
            }
        }

        public class TextAreaField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Text Area to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public TextAreaField(int PositionX, int PositionY, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("TextAreaField", out IDNumber);
                GUI_Store.TxtFA[IDNumber - 1].Text = GUI.TextArea(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y,  Width, Height), GUI_Store.TxtFA[IDNumber - 1].Text, SetStyle(GStyle, "TextField"));
                text = GUI_Store.TxtFA[IDNumber - 1].Text;
            }

            /// <summary>
            /// Get/Set TextField Text
            /// </summary>
            public string text
            {
                get { return GUI_Store.TxtFA[IDNumber - 1].Text; }
                set { GUI_Store.TxtFA[IDNumber - 1].Text = value; }
            }

            /// <summary>
            /// Clears the Field
            /// </summary>
            public void Clear()
            {
                GUI_Store.TxtFA[IDNumber - 1].Text = "";
                text = "";
            }

        }

        public class FloatField : Items
        {

            private int IDNumber = -1;

            /// <summary>Adds a Float Entry to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Delayed">If true, will not return the new value until the user has pressed enter or focus is moved away from the float field</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public FloatField(int PositionX, int PositionY, bool Delayed, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("FloatField", out IDNumber);
                if (Delayed)
                {
                    GUI_Store.FloF[IDNumber - 1].Value = EditorGUI.DelayedFloatField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.FloF[IDNumber - 1].Value);
                }
                else
                {
                    GUI_Store.FloF[IDNumber - 1].Value = EditorGUI.FloatField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.FloF[IDNumber - 1].Value);

                }

                value = GUI_Store.FloF[IDNumber - 1].Value;


            }

            /// <summary>
            /// Get/Set Float Value
            /// </summary>
            public float value
            {
                get { return GUI_Store.FloF[IDNumber - 1].Value; }
                set { GUI_Store.FloF[IDNumber - 1].Value = value; }
            }


            /// <summary>
            /// Clears the Field
            /// </summary>
            public void Clear()
            {
                GUI_Store.FloF[IDNumber - 1].Value = 0;
                value = 0;
            }

        }

        public class IntField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Float Entry to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Delayed">If true, will not return the new value until the user has pressed enter or focus is moved away from the float field</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public IntField(int PositionX, int PositionY, bool Delayed, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("IntField", out IDNumber);
                if (Delayed)
                {
                    GUI_Store.IntF[IDNumber - 1].Value = EditorGUI.DelayedIntField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.IntF[IDNumber - 1].Value);
                }
                else
                {
                    GUI_Store.IntF[IDNumber - 1].Value = EditorGUI.IntField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.IntF[IDNumber - 1].Value);
                    value = GUI_Store.IntF[IDNumber - 1].Value;
                }

            }

                /// <summary>
                /// Get/Set Int Value
                /// </summary>
            public int value
            {
                get { return GUI_Store.IntF[IDNumber - 1].Value; }
                set { GUI_Store.IntF[IDNumber - 1].Value = value; }
            }


                /// <summary>
                /// Clears the Field
                /// </summary>
                public void Clear()
                {
                    GUI_Store.IntF[IDNumber - 1].Value = 0;
                    value = 0;
                }


        }

        public class Vector2FloatField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Vector2 Float Field to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Title">Sets the Text of the Label</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public Vector2FloatField(int PositionX, int PositionY, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("Vector2FloatField", out IDNumber);

                GUI_Store.Vec2FloF[IDNumber - 1].Value = EditorGUI.Vector2Field(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), "", GUI_Store.Vec2FloF[IDNumber - 1].Value);

            }

            /// <summary>
            /// Get/Set Vector2 Float Value
            /// </summary>
            public Vector2 value
            {
                get { return GUI_Store.Vec2FloF[IDNumber - 1].Value; }
                set { GUI_Store.Vec2FloF[IDNumber - 1].Value = value; }
            }


            /// <summary>
            /// Clears the Field
            /// </summary>
            public void Clear()
            {
                GUI_Store.Vec2FloF[IDNumber - 1].Value = new Vector2(0, 0);
                value = new Vector2(0,0);
            }
        }

        public class Vector2IntField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Vector2 Float Field to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Title">Sets the Text of the Label</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public Vector2IntField(int PositionX, int PositionY, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("Vector2IntField", out IDNumber);
                GUI_Store.Vec2IntF[IDNumber - 1].Value = EditorGUI.Vector2IntField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), "", GUI_Store.Vec2IntF[IDNumber - 1].Value);
                value = GUI_Store.Vec2IntF[IDNumber - 1].Value;
            }

            /// <summary>
            /// Get/Set Vector2 Int Value
            /// </summary>
            public Vector2Int value
            {
                get { return GUI_Store.Vec2IntF[IDNumber - 1].Value; }
                set { GUI_Store.Vec2IntF[IDNumber - 1].Value = value; }
            }


            /// <summary>
            /// Clears the Field
            /// </summary>
            public void Clear()
            {
                GUI_Store.Vec2IntF[IDNumber - 1].Value = new Vector2Int(0, 0);
                value = new Vector2Int(0, 0);
            }
        }

        public class Vector3FloatField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Vector2 Float Field to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Title">Sets the Text of the Label</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public Vector3FloatField(int PositionX, int PositionY, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("Vector3FloatField", out IDNumber);
                GUI_Store.Vec3FloF[IDNumber - 1].Value = EditorGUI.Vector3Field(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), "", GUI_Store.Vec3FloF[IDNumber - 1].Value);
                value = GUI_Store.Vec3FloF[IDNumber - 1].Value;
            }

            /// <summary>
            /// Get/Set Vector2 Float Value
            /// </summary>
            public Vector3 value
            {
                get { return GUI_Store.Vec3FloF[IDNumber - 1].Value; }
                set { GUI_Store.Vec3FloF[IDNumber - 1].Value = value; }
            }


            /// <summary>
            /// Clears the Field
            /// </summary>
            public void Clear()
            {
                GUI_Store.Vec3FloF[IDNumber - 1].Value = new Vector3(0, 0);
                value = new Vector3(0, 0);
            }
        }

        public class Vector3IntField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Vector2 Float Field to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Title">Sets the Text of the Label</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public Vector3IntField(int PositionX, int PositionY, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("Vector3IntField", out IDNumber);
                GUI_Store.Vec3IntF[IDNumber - 1].Value = EditorGUI.Vector3IntField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), "", GUI_Store.Vec3IntF[IDNumber - 1].Value);
                value = GUI_Store.Vec3IntF[IDNumber - 1].Value;
            }

            /// <summary>
            /// Get/Set Vector2 Int Value
            /// </summary>
            public Vector3Int value
            {
                get { return GUI_Store.Vec3IntF[IDNumber - 1].Value; }
                set { GUI_Store.Vec3IntF[IDNumber - 1].Value = value; }
            }


            /// <summary>
            /// Clears the Field
            /// </summary>
            public void Clear()
            {
                GUI_Store.Vec3IntF[IDNumber - 1].Value = new Vector3Int(0, 0);
                value = new Vector3Int(0, 0);
            }
        }

        public class ColorField : Items
        {

            private int IDNumber = -1;

            /// <summary>Adds a Color Field to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Title">Sets the Text of the Label</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public ColorField(int PositionX, int PositionY, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {
                GUI_Module.OnGUI("ColorField", out IDNumber);

                //Debug.Log("d" + IDNumber);
                value= EditorGUI.ColorField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), "", value);
                GUI_Store.ColF[IDNumber - 1].Value = value;
                //Value = GUI_Store.ColF[IDNumber - 1].Value;
            }


            /// <summary>
            /// Get/Set Color Value
            /// </summary>
            public Color value
            {
                get { return GUI_Store.ColF[IDNumber - 1].Value; }
                set { GUI_Store.ColF[IDNumber - 1].Value = value; }
            }


        


        }

        public class ObjectField : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Object Field to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="ObjectType">Sets the type of Unity Object to be entered ex.("GameObject","Texture", "Sprite", "Image")</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            public ObjectField(int PositionX, int PositionY, string ObjectType, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {


                GUI_Module.OnGUI("ObjectField", out IDNumber);

                string fullassembly = "UnityEngine." + ObjectType + ", UnityEngine";
                if (Type.GetType(fullassembly) == null)
                {
                    //Try UnityEngine.UI
                    fullassembly = "UnityEngine.UI." + ObjectType + ", UnityEngine.UI";
                }


                GUI_Store.ObjF[IDNumber - 1].Obj = (UnityEngine.Object)EditorGUI.ObjectField(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), "", GUI_Store.ObjF[IDNumber - 1].Obj, Type.GetType(fullassembly), true);

                Object = GUI_Store.ObjF[IDNumber - 1].Obj;
            }

            /// <summary>
            /// Get/Set Object
            /// </summary>
            public UnityEngine.Object Object
            {
                get { return GUI_Store.ObjF[IDNumber - 1].Obj; }
                set { GUI_Store.ObjF[IDNumber - 1].Obj = value; }
            }

        }

        public class Toggle : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Toggle to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="Title">Sets the Text of the Label</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public Toggle(int PositionX, int PositionY, string Title, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {

                GUI_Module.OnGUI("Toggle", out IDNumber);

                GUI_Store.Togg[IDNumber - 1].Value = GUI.Toggle(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.Togg[IDNumber - 1].Value, Title);
                value = GUI_Store.Togg[IDNumber - 1].Value;
            }


            /// <summary>
            /// Get/Set State
            /// </summary>
            public bool value
            {
                get { return GUI_Store.Togg[IDNumber - 1].Value; }
                set { GUI_Store.Togg[IDNumber - 1].Value = value; }
            }


        }

        public class SelectionGrid : Items
        {

            private int IDNumber = -1;

            /// <summary>Adds a SelectionGrid to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="selectionStrings">An array of strings to show on the buttons</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public SelectionGrid(int PositionX, int PositionY, string[] selectionStrings, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {
                GUI_Module.OnGUI("SelectionGrid", out IDNumber);
                GUI_Store.SelG[IDNumber - 1].Value = GUI.SelectionGrid(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.SelG[IDNumber - 1].Value, selectionStrings, 1);
                value = GUI_Store.SelG[IDNumber - 1].Value;

            }

            /// <summary>
            /// Get/Set value
            /// </summary>
            public int value
            {
                get { return GUI_Store.SelG[IDNumber - 1].Value; }
                set { GUI_Store.SelG[IDNumber - 1].Value = value; }
            }

        }

        public class HorizontalSlider :Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Horizontal Slider to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="min">Sets the minimum slider value</param>
            /// <param name="max">Sets the maximum slider value</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public HorizontalSlider(int PositionX, int PositionY, float min, float max, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {
                GUI_Module.OnGUI("HorizontalSlider", out IDNumber);
                GUI_Store.HorS[IDNumber - 1].Value = GUI.HorizontalSlider(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.HorS[IDNumber - 1].Value, min, max);
                value = GUI_Store.HorS[IDNumber - 1].Value;
            }

            /// <summary>
            /// Get Silder Value
            /// </summary>
            public float value
            {
                get { return GUI_Store.HorS[IDNumber - 1].Value; }
                set { GUI_Store.HorS[IDNumber - 1].Value = value; }
            }

        }

        public class VerticalSlider : Items
        {
            private int IDNumber = -1;

            /// <summary>Adds a Vertical Slider to the GUI</summary>
            /// <param name="PositionX">Sets the x Position on Layout</param>
            /// <param name="PositionY">Sets the y Position on Layout</param>
            /// <param name="min">Sets the minimum slider value</param>
            /// <param name="max">Sets the maximum slider value</param>
            /// <param name="Width">Sets the Width of the GUI</param>
            /// <param name="Height">Sets the Height of the GUI</param>
            /// <param name="GStyle">Set GUIStyle</param>
            public VerticalSlider(int PositionX, int PositionY, float min, float max, float Width, float Height, GUIStyle GStyle = default(GUIStyle))
            {
                GUI_Module.OnGUI("VerticalSlider", out IDNumber);
                GUI_Store.VerS[IDNumber - 1].Value = GUI.VerticalSlider(new Rect(PositionX + GUIoffset_X, PositionY + GUIoffset_Y, Width, Height), GUI_Store.VerS[IDNumber - 1].Value, min, max);
                value = GUI_Store.VerS[IDNumber - 1].Value;
            }

            /// <summary>
            /// Get/Set Silder Value
            /// </summary>
            public float value
            {
                get { return GUI_Store.VerS[IDNumber - 1].Value; }
                set { GUI_Store.VerS[IDNumber - 1].Value = value; }
            }
        }



    }

}
#endif
