#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class MainGUI : EditorWindow
{

    const string Window_Title = "Example Title";    //<------------Title of window
    const string Dir = "Match3/ Create_New_Level";            //<------------Unity Menu Directory

    [MenuItem(Dir)]
    private static void ShowWindow()
    {
        GetWindow <MainGUI> (Window_Title);     //<------------Change to your Script name
        GetWindow <MainGUI> ().Show();          //<------------Change to your Script name

        MainGUI window = GetWindow<MainGUI>();

        window.minSize = new Vector2(400, 400); //	The minimum size of this window when it is floating or modal. The minimum size is not used when the window is docked.
        window.maxSize = new Vector2(500, 500); //	The maximum size of this window when it is floating or modal. The maximum size is not used when the window is docked.

    }



    void OnGUI()
    {

   
    }

}
#endif