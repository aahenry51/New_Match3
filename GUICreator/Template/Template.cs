/*
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GUICreator;

public class "Script Name" : EditorWindow
{

    const string Window_Title = "Example Title";    //<------------Title of window
    const string Dir = "Tools/ Example";            //<------------Unity Menu Directory

    [MenuItem(Dir)]
    private static void ShowWindow()
    {
        GetWindow<"Script Name">(Window_Title);     //<------------Change to your Script name
        GetWindow<"Script Name">().Show();          //<------------Change to your Script name
    }   

    void OnGUI()
    {
        //GUI Items
        GUI_Module.END();                           //<------------//IMPORTANT!//////////////////////////
    }
 }
#endif
*/
