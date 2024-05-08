#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using GUICreator;

public class Example : EditorWindow
{

    const string Window_Title = "Example Title";
    const string Dir = "Tools/ Example";

    [MenuItem(Dir)]
    private static void ShowWindow()
    {
        GetWindow<Example>(Window_Title);
        GetWindow<Example>().Show();
    }


    void OnGUI()
    {
        
        Items.Button Bt = new Items.Button(0, 0, "Click", 150, 50, null);

        Items.Label L = new Items.Label(0, 50, "HI", 50, 50, null);
        
        Items.TextField T = new Items.TextField(0, 100, 50, 30, null);

        Items.TextAreaField T2 = new Items.TextAreaField(0, 150, 50, 30, null);

        Items.FloatField F = new Items.FloatField(0, 200, false, 50, 20, null);

        Items.IntField I = new Items.IntField(0, 250, false, 50, 20, null);

        Items.Vector2FloatField V2F = new Items.Vector2FloatField(0, 300, 100, 50, null);

        Items.Vector2IntField V2I = new Items.Vector2IntField(0, 350, 100, 50, null);

        Items.Vector3FloatField V3F = new Items.Vector3FloatField(0, 400, 150, 50, null);

        Items.Vector3IntField V3I = new Items.Vector3IntField(0, 450, 150, 50, null);

        Items.ColorField Col = new Items.ColorField(0, 500, 100, 20, null);

        Items.ObjectField Obj = new Items.ObjectField(0, 550, "Image", 100, 20, null);

        Items.Toggle Tog = new Items.Toggle(200, 0, "Toggle", 100, 30, null);

        Items.SelectionGrid SGrid = new Items.SelectionGrid(200, 50, new string[] { "A", "B", "C" }, 100, 100, null);

        Items.HorizontalSlider Hors = new Items.HorizontalSlider(200, 200, 0, 50, 100, 20,null);

        Items.VerticalSlider VerS = new Items.VerticalSlider(200, 240, 0, 50, 50, 100,null);


        Items.ScrollPosition Scr = new Items.ScrollPosition(400, 0, 300, 300, 400, 0, 500, 500);
     
        GUICreator.GUI_Module.DrawTexture(400, 0, 500, 500, Col.value); //Create a background Based on color
        
        

        if (Obj.Object != null)
        {
            Image Img = (Image)Obj.Object;

            Texture2D Tex = GUI_Module.SpriteToTexture(Img.sprite);
            GUICreator.GUI_Module.DrawTexture(400, 0, 200, 200, Tex); //Create a background Based on color
        }

        Scr.EndScrollView();


        GUI_Module.END();

    }
 




}
#endif