using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationData))]
public class AnimationDataDrawer : PropertyDrawer
{
    #region Declaration

    private SerializedProperty Prop_animType;
    private SerializedProperty prop_canLoop;
    private SerializedProperty prop_LoopCycle;
    private SerializedProperty prop_LoopType;

    private SerializedProperty prop_RefrenceList;

    private SerializedProperty prop_Ease;
    private SerializedProperty prop_StartAt;
    private SerializedProperty prop_Duration;
    private SerializedProperty prop_Delay;

    private SerializedProperty prop_Animcalllback;
    private SerializedProperty prop_FadeData;
    private SerializedProperty prop_ScaleData;
    private SerializedProperty prop_MoveData;
    private SerializedProperty prop_FillAmount;
    private SerializedProperty prop_RotateData;
    private SerializedProperty prop_ShakeData;

    

    private SerializedProperty prop_UnityEvent_OnStart;
    private SerializedProperty prop_UnityEvent_OnUpdate;
    private SerializedProperty prop_UnityEvent_OnComplete;

    private SerializedProperty prop_IsActive_Start;
    private SerializedProperty prop_IsActive_Update;
    private SerializedProperty prop_IsActive_Complete;

    
    private Rect currentPosition; //to draw layout manually

    private Color activeColor = Color.green; // Color when button is active
    private Color defaultColor = GUI.backgroundColor; // Default background color

    #endregion

    // Setting property before starting layout 
    private void Set_Props(SerializedProperty property)
    {
        Prop_animType = property.FindPropertyRelative("animType");
        prop_canLoop = property.FindPropertyRelative("canLoop");
        prop_LoopCycle = property.FindPropertyRelative("loopCycle");
        prop_LoopType= property.FindPropertyRelative("loopType");

        prop_RefrenceList = property.FindPropertyRelative("refrenceList");
        prop_Ease = property.FindPropertyRelative("ease");
        prop_StartAt = property.FindPropertyRelative("startAt");
        prop_Duration = property.FindPropertyRelative("duration");
        prop_Delay = property.FindPropertyRelative("delay");
        
        prop_Animcalllback = property.FindPropertyRelative("event_AnimType");
        prop_FadeData = property.FindPropertyRelative("fadeData");
        prop_ScaleData = property.FindPropertyRelative("scaleData");
        prop_MoveData = property.FindPropertyRelative("moveData");
        prop_RotateData = property.FindPropertyRelative("rotateData");
        prop_FillAmount = property.FindPropertyRelative("fillAmountData");
        prop_ShakeData = property.FindPropertyRelative("shakeData");


        prop_UnityEvent_OnStart    = property.FindPropertyRelative("event_OnStart");
        prop_UnityEvent_OnUpdate   = property.FindPropertyRelative("event_OnUpdate");
        prop_UnityEvent_OnComplete = property.FindPropertyRelative("event_OnComplete");

        prop_IsActive_Start = property.FindPropertyRelative("isActive_Start");
        prop_IsActive_Update = property.FindPropertyRelative("isActive_Update");
        prop_IsActive_Complete =property.FindPropertyRelative("isActive_Complete");        
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Caching all the Proprety first
        Set_Props(property);
        if (Prop_animType == null) //failed to cache property then no need to go further
        {
            Debug.Log("animation Data is null");
            EditorGUI.HelpBox(position, "No AnimData Found", MessageType.Error);
            return;
        }
        //Saving property root node Rect position 
        currentPosition = position;
        currentPosition.height = EditorGUIUtility.singleLineHeight; 
        //making root node foldable
        property.isExpanded = EditorGUI.Foldout(currentPosition, property.isExpanded, property.displayName, true); 
        
        if (property.isExpanded)
        {
            EditorGUI.BeginProperty(position, label, property);
            //adding nested child count so unity automatic add horizontal space for nested elements
            EditorGUI.indentLevel++; 
            currentPosition.y += EditorGUIUtility.singleLineHeight;

            Draw_Property(currentPosition, property.FindPropertyRelative("title"));
            Draw_Property(Get_Rect(0, true), prop_Ease);
            Draw_Property(Get_Rect(), prop_StartAt);
            Draw_Property(Get_Rect(), prop_Duration);
            Draw_Property(Get_Rect(), prop_Delay);
                        
            Set_Loop();     // Can Loop              
            Set_RefrenceList(); //Object Refrence list            
            Set_AnimType((AnimationData.AnimType)Prop_animType.enumValueIndex); //Anim type Enum

            if ((AnimationData.AnimType)Prop_animType.enumValueIndex != AnimationData.AnimType.Callback)
                Set_Buttons();  //Set UnityEvents


            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
    }

    void Set_RefrenceList()
    {
        //currentPosition.y += 10;

        if (prop_RefrenceList == null  )
        {
            Debug.Log("Property Refrence List is null ");
            return ;
        }
        else if((AnimationData.AnimType)Prop_animType.enumValueIndex == AnimationData.AnimType.Callback)
        {
            Debug.Log("callback animtype ,so no refrence list");
            return ;
        }

        Draw_Property(Get_Rect(-10), prop_RefrenceList);
        currentPosition.y += EditorGUI.GetPropertyHeight(prop_RefrenceList);
    }

    

    void Set_AnimType(AnimationData.AnimType _type)
    {
        Draw_Property(Get_Rect(0, true), Prop_animType);
        currentPosition.y -= 10;
        switch (_type)
        {
            case AnimationData.AnimType.Callback:
                Draw_Property(Get_Rect(), prop_Animcalllback);
                currentPosition.y += EditorGUI.GetPropertyHeight(prop_Animcalllback);                
                break;
            
            case AnimationData.AnimType.Fade:
                
                Draw_Property(Get_Rect(), prop_FadeData);
                currentPosition.y += prop_FadeData.isExpanded
                    ? EditorGUI.GetPropertyHeight(prop_FadeData)
                    : EditorGUIUtility.singleLineHeight;

                break;
            case AnimationData.AnimType.Scale:
                
                Draw_Property(Get_Rect(), prop_ScaleData);
                currentPosition.y += prop_ScaleData.isExpanded
                    ? EditorGUI.GetPropertyHeight(prop_ScaleData)
                    : EditorGUIUtility.singleLineHeight;

                break;
            case AnimationData.AnimType.Move:                                
                Draw_Property(Get_Rect(), prop_MoveData);
                currentPosition.y += prop_MoveData.isExpanded
                    ? EditorGUI.GetPropertyHeight(prop_MoveData)
                    : EditorGUIUtility.singleLineHeight;

                break;
            case AnimationData.AnimType.FillIn:
                
                Draw_Property(Get_Rect(), prop_FillAmount);
                currentPosition.y += prop_FillAmount.isExpanded
                    ? EditorGUI.GetPropertyHeight(prop_FillAmount)
                    : EditorGUIUtility.singleLineHeight;

                break;
            
            case AnimationData.AnimType.Rotate:
                
                Draw_Property(Get_Rect(), prop_RotateData);
                currentPosition.y += prop_RotateData.isExpanded
                    ? EditorGUI.GetPropertyHeight(prop_RotateData)
                    : EditorGUIUtility.singleLineHeight;
                break;
            case AnimationData.AnimType.Shake:
                Draw_Property(Get_Rect(), prop_ShakeData);
                currentPosition.y += prop_ShakeData.isExpanded
                    ? EditorGUI.GetPropertyHeight(prop_ShakeData)
                    : EditorGUIUtility.singleLineHeight;

                break;
        }
    }


    void Set_Loop()
    {
        Draw_Property(Get_Rect(), prop_canLoop);
        if (prop_canLoop.boolValue)
        {
            Draw_Property(Get_Rect(), prop_LoopCycle);
            Draw_Property(Get_Rect(0,true), prop_LoopType);
        }
    }

    void Set_Buttons()
    {
        Rect butonRect = Get_Rect(-10);
        butonRect.width = currentPosition.width/ 3;
        butonRect.height = EditorGUIUtility.singleLineHeight *2;
        currentPosition.y += EditorGUIUtility.singleLineHeight;
        
        if (prop_IsActive_Start.boolValue)
        {
            Draw_Property(Get_Rect(),prop_UnityEvent_OnStart);
            currentPosition.y += EditorGUI.GetPropertyHeight(prop_UnityEvent_OnStart);
            GUI.backgroundColor = activeColor;
        }
        
        if (GUI.Button(butonRect, "OnStart"))
            prop_IsActive_Start.boolValue = !prop_IsActive_Start.boolValue;
        GUI.backgroundColor = defaultColor;
        
        butonRect.x += butonRect.width;
        if (prop_IsActive_Update.boolValue)
        {
            Draw_Property(Get_Rect(),prop_UnityEvent_OnUpdate);
            currentPosition.y += EditorGUI.GetPropertyHeight(prop_UnityEvent_OnUpdate);
            GUI.backgroundColor = activeColor;
        }
        if (GUI.Button(butonRect, "OnUpdate"))
            prop_IsActive_Update.boolValue= !prop_IsActive_Update.boolValue;
        GUI.backgroundColor = defaultColor;
        
        butonRect.x += butonRect.width;
        if (prop_IsActive_Complete.boolValue)
        {
            Draw_Property(Get_Rect(),prop_UnityEvent_OnComplete);
            currentPosition.y += EditorGUI.GetPropertyHeight(prop_UnityEvent_OnComplete);
            GUI.backgroundColor = activeColor;
        }
        if (GUI.Button(butonRect, "OnComplete"))
            prop_IsActive_Complete.boolValue= !prop_IsActive_Complete.boolValue;
        GUI.backgroundColor = defaultColor;
        
        

    }
    
    /// <summary>
    /// Getting letest posistion in custom layout
    /// </summary>
    /// <param name="extraSpace">if we want little bit up or down</param>
    /// <param name="_isThisEnum">Enum need little extra space</param>
    /// <returns></returns>
    Rect Get_Rect(float extraSpace = 0, bool _isThisEnum = false)
    {
        currentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + extraSpace;

        if (_isThisEnum)
        {
            Rect enumRect = currentPosition;
            enumRect.height = EditorGUIUtility.singleLineHeight * 1.5f;
            return enumRect;
        }

        return currentPosition;
    }

    /// <summary>
    /// Will draw porperty 
    /// if enum then we will add 10 height after
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_property"></param>
    void Draw_Property(Rect _position, SerializedProperty _property )
    {
        EditorGUI.PropertyField(_position, _property,true);
        if (_property.propertyType == SerializedPropertyType.Enum)
            currentPosition.y += 10;
    }

   
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
 
        //caching all property first
        Set_Props(property);
        
        //num of basic elements in our custom editor
        int numProperties = 7;
        int loopData = prop_canLoop.boolValue ? 2 : 0;
        int buttonheight = 0; 
        float extraManualHieght =0;
        //fix height of each element
        float fixSpace = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        //featching all heights that drawen by properties
        switch ((AnimationData.AnimType)Prop_animType.enumValueIndex)
        {
            case AnimationData.AnimType.Callback:
                extraManualHieght += EditorGUI.GetPropertyHeight(prop_Animcalllback) + fixSpace *2;
                break;
            case AnimationData.AnimType.Fade:
                extraManualHieght += EditorGUI.GetPropertyHeight(prop_FadeData);
                break;
            case AnimationData.AnimType.Scale:
                extraManualHieght += EditorGUI.GetPropertyHeight(prop_ScaleData);
                break;
            case AnimationData.AnimType.Move:
                extraManualHieght += EditorGUI.GetPropertyHeight(prop_MoveData);
                break;
            case AnimationData.AnimType.Rotate:
                extraManualHieght += EditorGUI.GetPropertyHeight(prop_RotateData);
                break;
            case AnimationData.AnimType.FillIn:
                extraManualHieght += EditorGUI.GetPropertyHeight(prop_FillAmount);
                break;
            case AnimationData.AnimType.Shake:                
                extraManualHieght += EditorGUI.GetPropertyHeight(prop_ShakeData);
                break;
        }
        if ((AnimationData.AnimType)Prop_animType.enumValueIndex != AnimationData.AnimType.Callback)
        {
            buttonheight = 2;
            numProperties += 3;
            extraManualHieght += EditorGUI.GetPropertyHeight(prop_RefrenceList) ;

            extraManualHieght += prop_IsActive_Start.boolValue ? EditorGUI.GetPropertyHeight(prop_UnityEvent_OnStart) : 0;
            extraManualHieght += prop_IsActive_Update.boolValue ? EditorGUI.GetPropertyHeight(prop_UnityEvent_OnUpdate) : 0;
            extraManualHieght += prop_IsActive_Complete.boolValue ? EditorGUI.GetPropertyHeight(prop_UnityEvent_OnComplete) : 0;
        
            int activeEventCount = Convert.ToInt32(prop_IsActive_Start.boolValue) + Convert.ToInt32(prop_IsActive_Update.boolValue) + Convert.ToInt32(prop_IsActive_Complete.boolValue);
            extraManualHieght += activeEventCount * EditorGUIUtility.singleLineHeight * .5f; // for the end of this element        
        }

        //Returning final height that this porrpety took
        float height = fixSpace * ( loopData + numProperties + buttonheight);
        height += extraManualHieght;
        return height;
    }


}


