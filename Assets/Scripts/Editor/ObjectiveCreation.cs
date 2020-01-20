﻿using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;

public class Objectives : EditorWindow
{    
    // Variables needed Triggers.
    int chosenTrigger = 0;
    Button button;

    // enum TriggerOptions is used to refrence string[] triggerOptions.
    // As such, the order of the options in each needs to be the same.
    enum TriggerOptions
    {
        Button,
        Bool,
        Areas
    }
    string[] triggerOptions = 
    {
        "On Button Press",
        "On Bool is True",
        "On Area Entered"
    };

    // Variables needed for effects.
    bool ambientLighting = false;
    Color ambientLightingColor;

    [MenuItem("Window/Dev Tools/Objective Creation")]
    public static void ShowWindow()
    {
        GetWindow<Objectives>("Objective Creation (WIP)");
    }

    void OnGUI()
    {
        Triggers();
        EditorGUILayout.Separator();
        Effects();

        if(GUILayout.Button("Create Objective"))
        {
            if(ambientLighting)
            {
                AddAmbientLightingToScript();
            }
        }
    }

    // Renders menu items handling the selection of triggers.
    void Triggers()
    {
        GUILayout.Label("Select Trigger Condition");

        if(EditorGUILayout.DropdownButton(new GUIContent(triggerOptions[chosenTrigger]), new FocusType()))
        {
            GenericMenu menu = new GenericMenu();

            // Loops through every element in TriggerOptions.
            foreach (TriggerOptions element in (TriggerOptions[]) Enum.GetValues(typeof(TriggerOptions)))
            {
                AddTriggerOptionToMenu(menu, element);
            }

            menu.ShowAsContext();
        }

        switch (chosenTrigger)
        {
            case 0:
                button = (Button)EditorGUILayout.ObjectField("Trigger Button", button, typeof(Button), true);
                return;

            default:
                return;
        }
    }

    // Function to make adding items cleaner.
    // Uses TriggerOptions value to select the right string from triggerOptions.
    void AddTriggerOptionToMenu(GenericMenu menu, TriggerOptions trigger)
    {
        menu.AddItem(new GUIContent(triggerOptions[(int)trigger]), chosenTrigger.Equals(trigger), setTriggerOptions, trigger);
    }

    void setTriggerOptions(object choice)
    {
        chosenTrigger = (int)choice;
    }

    // Renders menu items handling the effect of the trigger.
    void Effects()
    {
        GUILayout.Label("Select Effects");

        ambientLighting = EditorGUILayout.Toggle("Change Ambient Lighting?", ambientLighting);

        if(ambientLighting)
        {
            ambientLightingColor = RenderSettings.ambientLight;
            ambientLightingColor = EditorGUILayout.ColorField("Color", ambientLightingColor);
        }
    }

    void AddAmbientLightingToScript()
    {
        
    }
}