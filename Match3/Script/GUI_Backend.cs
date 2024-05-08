using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace GUICreator
{
    public class GUI

    {

        public class windowInit
        {
            public EditorWindow window;
            public windowInit(EditorWindow window)
            {
                this.window = window;
            }
        }

        private class windowProps
        {
            EditorWindow window;
            public windowProps(EditorWindow window)
            {
                this.window = window;
            }
        }

    }






    public class GUI_Backend
    {

        EditorWindow window;

        public GUI_Backend(EditorWindow window)
        {
            this.window = window;
        
        
        
        
        }
        /*
        BindingFlags fullBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private MainGUI mainGUI;

        public bool IsDocked(MainGUI mainGUI)
        {

                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo method = GetType().GetProperty("docked", flags).GetGetMethod(true);
                return (bool)method.Invoke(mainGUI, null);
        }
        */
        /// <summary>
        /// Window Properties and conditions.
        /// </summary>
        /// <param name="editorWindowPosition"></param>
        /// <returns></returns>
        public Rect windowProperties(Rect editorWindowPosition)
        {
            
            if (editorWindowPosition.width > 1000) { editorWindowPosition.width = 1000; }
            if (editorWindowPosition.width < 800) { editorWindowPosition.width = 800; }

            if (editorWindowPosition.height != 600) { editorWindowPosition.height = 600; }
            //if (editorWindowPosition.height < 700) { editorWindowPosition.height = 700; }

            return editorWindowPosition;
        }

        /// <summary>
        /// Check if Properties has been are correct.
        /// </summary>
        /// <param name="editorWindowPosition"></param>
        /// <returns></returns>
        public Rect windowProperties_Fit(Rect editorWindowPosition)
        {
            if (editorWindowPosition != windowProperties(editorWindowPosition)) { return windowProperties(editorWindowPosition); }
            else { return new Rect(); }


        }

    }
}