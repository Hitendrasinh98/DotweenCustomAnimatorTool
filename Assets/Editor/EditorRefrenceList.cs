using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static AnimationData;

[CustomPropertyDrawer(typeof(RefrenceList))]
public class EditorRefrenceList : PropertyDrawer
{
    private SerializedProperty prop_BoolRT;
    private SerializedProperty prop_BoolImg;
    private SerializedProperty prop_BoolTxt;
    private SerializedProperty prop_BoolTMPUI;
    private SerializedProperty prop_BoolRAWImg;

    private SerializedProperty prop_ListRT;
    private SerializedProperty prop_ListIMG;
    private SerializedProperty prop_ListTXT;
    private SerializedProperty prop_ListTMPUI;
    private SerializedProperty prop_ListRAWIMG;

    private Rect currentPosition;
    private Color activeColor = Color.green; // Color when button is active
    private Color defaultColor = GUI.backgroundColor; // Default background color


    private void Set_Props(SerializedProperty property)
    {
        prop_BoolRT = property.FindPropertyRelative("canUseRT");
        prop_BoolImg = property.FindPropertyRelative("canUseImage");
        prop_BoolTxt = property.FindPropertyRelative("canUseText");
        prop_BoolTMPUI = property.FindPropertyRelative("canUseTMPUi");
        prop_BoolRAWImg = property.FindPropertyRelative("canUseRawImage");

        prop_ListRT = property.FindPropertyRelative("targetObject_RT");
        prop_ListIMG = property.FindPropertyRelative("targetObject_Image");
        prop_ListRAWIMG = property.FindPropertyRelative("targetObject_RawImage");
        prop_ListTXT = property.FindPropertyRelative("targetObject_Text");
        prop_ListTMPUI = property.FindPropertyRelative("targetObject_TMPUI");

    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        Set_Props(property);
        if (prop_BoolRT == null)
        {
            Debug.Log("animation Data is null");
            EditorGUI.HelpBox(position, "No AnimData Found", MessageType.Error);
            return;
        }

        currentPosition = position;
        currentPosition.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.BeginProperty(position, label, property);
        Set_Buttons();
        EditorGUI.EndProperty();
    }

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

    void Draw_Property(Rect _position, SerializedProperty _property)
    {
        EditorGUI.PropertyField(_position, _property, true);
        if (_property.propertyType == SerializedPropertyType.Enum)
            currentPosition.y += 10;
    }
    void Set_Buttons()
    {
        Rect butonRect = Get_Rect(-10);
        butonRect.width = currentPosition.width / 5;
        butonRect.height = EditorGUIUtility.singleLineHeight * 2;
        currentPosition.y += EditorGUIUtility.singleLineHeight;

        if (prop_BoolRT.boolValue)
        {
            Draw_Property(Get_Rect(), prop_ListRT);
            currentPosition.y += prop_ListRT.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListRT) : 0;
            GUI.backgroundColor = activeColor;
        }

        if (GUI.Button(butonRect, "Rect\nTransform"))
            prop_BoolRT.boolValue = !prop_BoolRT.boolValue;
        GUI.backgroundColor = defaultColor;

        butonRect.x += butonRect.width;
        if (prop_BoolImg.boolValue)
        {
            Draw_Property(Get_Rect(), prop_ListIMG);
            currentPosition.y += prop_ListIMG.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListIMG) : 0;
            GUI.backgroundColor = activeColor;
        }
        if (GUI.Button(butonRect, "Image"))
            prop_BoolImg.boolValue = !prop_BoolImg.boolValue;
        GUI.backgroundColor = defaultColor;

        

        butonRect.x += butonRect.width;
        if (prop_BoolTxt.boolValue)
        {
            Draw_Property(Get_Rect(), prop_ListTXT);
            currentPosition.y += prop_ListTXT.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListTXT) : 0;
            GUI.backgroundColor = activeColor;
        }
        if (GUI.Button(butonRect, "Text"))
            prop_BoolTxt.boolValue = !prop_BoolTxt.boolValue;
        GUI.backgroundColor = defaultColor;

        butonRect.x += butonRect.width;
        if (prop_BoolTMPUI.boolValue)
        {
            Draw_Property(Get_Rect(), prop_ListTMPUI);
            currentPosition.y += prop_ListTMPUI.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListTMPUI) : 0;
            GUI.backgroundColor = activeColor;
        }
        if (GUI.Button(butonRect, "TMPUI"))
            prop_BoolTMPUI.boolValue = !prop_BoolTMPUI.boolValue;
        GUI.backgroundColor = defaultColor;


        butonRect.x += butonRect.width;
        if (prop_BoolRAWImg.boolValue)
        {
            Draw_Property(Get_Rect(), prop_ListRAWIMG);
            currentPosition.y += prop_ListRAWIMG.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListRAWIMG) : 0;
            GUI.backgroundColor = activeColor;
        }
        if (GUI.Button(butonRect, "Raw\nImage"))
            prop_BoolRAWImg.boolValue = !prop_BoolRAWImg.boolValue;
        GUI.backgroundColor = defaultColor;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        
        Set_Props(property);


        int numProperties = 0;
        int buttonheight = 2;
        float extraManualHieght = 0;
        float fixSpace = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        if (prop_BoolRT.boolValue)
        {
            numProperties += 1 ;
            extraManualHieght += prop_ListRT.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListRT) : 0;
        }

        if (prop_BoolImg.boolValue)
        {
            numProperties += 1;
            extraManualHieght += prop_ListIMG.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListIMG) : 0;
        }

        if (prop_BoolRAWImg.boolValue)
        {
            numProperties += 1;
            extraManualHieght += prop_ListRAWIMG.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListRAWIMG) : 0;
        }

        if (prop_BoolTxt.boolValue)
        {
            numProperties += 1;
            extraManualHieght += prop_ListTXT.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListTXT) : 0;
        }

        if (prop_BoolTMPUI.boolValue)
        {
            numProperties += 1;
            extraManualHieght += prop_ListTMPUI.isExpanded ? EditorGUI.GetPropertyHeight(prop_ListTMPUI) : 0;
        }

        float height = fixSpace * ( numProperties + buttonheight);
        height += extraManualHieght;
        return height;
    }



}
