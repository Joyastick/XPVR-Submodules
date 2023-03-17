using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cognitive3D;

public class DynamicObjectOneButton : EditorWindow
{
    [MenuItem("Cognitive3D/XPVR One Button")]

    //private EditorWindow ew;

    public static void ShowWindow()
    {

        EditorWindow ew = EditorWindow.GetWindow(typeof(DynamicObjectOneButton));

        ew.minSize = new Vector2(450,400);
        ew.maxSize = new Vector2(450,400);

    }

    //Repaints the Inspector Window so that it is properly updated
    void OnInspectorUpdate() { Repaint(); }

    Vector2 scrollPosition = Vector2.zero;

    void OnGUI()
    {

        EditorWindow ew = EditorWindow.GetWindow(typeof(DynamicObjectOneButton));

        //GUI Title
        GUILayout.Label("Cognitive3D Dynamic Object Helper", EditorStyles.boldLabel);

        //Quick description on how the tool works
        GUILayout.Label("- First Select the Objects wanted to be Cognified", EditorStyles.label);
        GUILayout.Label("- Then click this button below", EditorStyles.label);
        GUILayout.Label("- It will give the objects the required components if possible", EditorStyles.label);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(450), GUILayout.Height(280));
        //Text area to display all the Selected Objects
        GUI.enabled = false;
        if (Selection.objects.Length != 0) EditorGUILayout.TextArea(UpdateSelectedObjects(Selection.objects));//, GUILayout.Height(300)); //This makes a string that has all the selected objects and displays their current information about the components that the object needs
        GUI.enabled = true;
        //We changes the enabled back and forth to make it so the textarea is not editable by the user
        GUILayout.EndScrollView();

        GUILayout.BeginArea(new Rect(0, 353, 450, 400));
        //Button for 'Cognifying the Selected Objects in the scene. This will add the required components of a Dynamic Object
        if (GUILayout.Button("Cognify Selected Objects", GUILayout.Height(50)))
        {

            //Going through each of the Selected Objects
            foreach (GameObject obj in Selection.objects)
            {

                //Checking if this object has a mesh filter on it, if not, displaying an error and moving onto the next object
                if (obj.GetComponent<MeshFilter>() == null)
                {
                    Debug.LogError("Object " + obj.name + " does not have a mesh");
                    continue;
                }

                //Adding Box Collider if there is no Collider at all on the object
                if (obj.GetComponent<Collider>() == null) obj.AddComponent<BoxCollider>();
                //Adding Dynamic Object Script if there is no Dynamic Object script
                if (obj.GetComponent<DynamicObject>() == null) obj.AddComponent<DynamicObject>();

            }

        }
        GUILayout.EndArea();

    }

    //This function is used to get the list of objects, and display their information (Their name, and if they have the required components or not
    private static string UpdateSelectedObjects(Object[] list)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //Goes through every gameobject in the array
        foreach (GameObject item in list)
        {
            //Display's the name
            sb.Append(item.name);
            sb.Append("\n");

            //Checks if it has a MeshFilter
            sb.Append("Has MeshFilter:");
            if (item.GetComponent<MeshFilter>() == null) sb.Append(" [FALSE] |");
            else sb.Append(" [TRUE] |");

            //Checks if it has a collider
            sb.Append(" Has Collider:");
            if (item.GetComponent<Collider>() == null) sb.Append(" [FALSE] |");
            else sb.Append(" [TRUE] |");

            //Checks if it has the DynamicObject Script
            sb.Append(" DynamicObject:");
            if (item.GetComponent<DynamicObject>() == null) sb.Append(" [FALSE]");
            else sb.Append(" [TRUE]");

            //Separating the objects
            if (!list[list.Length - 1].Equals(item))
            {
                sb.Append("\n");
                sb.Append("---------------------------------------------------------------");
                sb.Append("\n");
            }
        }

        return sb.ToString();
    }
}
