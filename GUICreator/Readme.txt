GUI Creator


///Quick Tutorial/////
-Add the GUICreator namespace;
-Create the GUI Menu item using the template. The template code is below, where the "Script Name" is the name of your created script.


///Template/// 

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GUICreator;

public class "Script Name" : EditorWindow
{

const string Window_Title = "Example Title";                //<------------Title of window
    const string Dir = "Tools/ Example";                    //<------------Unity Menu Directory

    [MenuItem(Dir)]
    private static void ShowWindow()
    {
        GetWindow<"Script Name">(Window_Title);             //<------------Change to your Script name
        GetWindow<"Script Name">().Show();                  //<------------Change to your Script name
    }

    void OnGUI()
    {
        //GUI Items
    }

}
#endif

///Template end///

-Add the menu items using GUICreator.Items; Place "GUI_Module.END();" at the end of all items.//IMPORTANT!

void OnGUI()
{
        //GUI Items
        Items.Button Bt = new Items.Button(0, 0, "Click", 150, 50, null);
        Items.Label L = new Items.Label(0, 50, "HI", 50, 50, null);

        GUI_Module.END();                               //<------------//IMPORTANT!//////////////////////////
}


You can find tutorials in the Documentation folder.

If you have any problems contact this email: mercury990games@gmail.com