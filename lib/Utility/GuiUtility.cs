/*
 * File: Assets/Editor/Utility/GuiUtility.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 8/3/2015 6:07:45 PM
 */
using UnityEngine;
using System.Collections;
using UnityEditor;

public class GuiUtility 
{

    public enum DragDirection
    {
        Horizontal,
    }

    // Size is the offset into the rect to draw the DragableHandle
    public const float resizeBarHotSpotSize = 2.0f;
    public static float DragableHandle(int id, Rect windowRect, float offset, DragDirection direction)
    {
        int controlID = GUIUtility.GetControlID(id, FocusType.Passive);
        Vector2 positionFilter = Vector2.zero;
        Rect controlRect = windowRect;
        switch (direction)
        {
            case DragDirection.Horizontal:
                controlRect = new Rect(controlRect.x + offset - resizeBarHotSpotSize,
                                       controlRect.y,
                                       resizeBarHotSpotSize * 2 + 1.0f,
                                       controlRect.height);
                positionFilter.x = 1.0f;
                break;
        }
        EditorGUIUtility.AddCursorRect(controlRect, MouseCursor.ResizeHorizontal);

        if (GUIUtility.hotControl == 0)
        {
            if (Event.current.type == EventType.MouseDown && controlRect.Contains(Event.current.mousePosition))
            {
                GUIUtility.hotControl = controlID;
                Event.current.Use();
            }
        }
        else if (GUIUtility.hotControl == controlID)
        {
            if (Event.current.type == EventType.MouseDrag)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                Vector2 handleOffset = new Vector2((mousePosition.x - windowRect.x) * positionFilter.x,
                                                   (mousePosition.y - windowRect.y) * positionFilter.y);
                offset = handleOffset.x + handleOffset.y;
                HandleUtility.Repaint();
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                GUIUtility.hotControl = 0;
            }
        }

        // Debug draw
        // GUI.Box(controlRect, "");

        return offset;
    }

    static int activePositionHandleId = 0;
    static Vector2 activePositionHandlePosition = Vector2.zero;
    static Vector2 positionHandleOffset = Vector2.zero;

    public static Vector2 Handle(GUIStyle style, int id, Vector2 position, bool allowKeyboardFocus)
    {
        int handleSize = (int)style.fixedWidth;
        Rect rect = new Rect(position.x - handleSize / 2, position.y - handleSize / 2, handleSize, handleSize);
        int controlID = id;

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                {
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        activePositionHandleId = id;
                        if (allowKeyboardFocus)
                        {
                            GUIUtility.keyboardControl = controlID;
                        }
                        positionHandleOffset = Event.current.mousePosition - position;
                        GUIUtility.hotControl = controlID;
                        Event.current.Use();
                    }
                    break;
                }

            case EventType.MouseDrag:
                {
                    if (GUIUtility.hotControl == controlID)
                    {
                        position = Event.current.mousePosition - positionHandleOffset;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                    break;
                }

            case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == controlID)
                    {
                        activePositionHandleId = 0;
                        position = Event.current.mousePosition - positionHandleOffset;
                        GUIUtility.hotControl = 0;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                    break;
                }

            case EventType.Repaint:
                {
                    bool selected = (GUIUtility.keyboardControl == controlID ||
                                     GUIUtility.hotControl == controlID);
                    style.Draw(rect, selected, false, false, false);
                    break;
                }
        }

        return position;
    }

    private static bool backupGuiChangedValue = false;
    public static void BeginChangeCheck()
    {
        backupGuiChangedValue = GUI.changed;
        GUI.changed = false;
    }

    public static bool EndChangeCheck()
    {
        bool hasChanged = GUI.changed;
        GUI.changed |= backupGuiChangedValue;
        return hasChanged;
    }

    public static void CreateSubtitle(string title)
    {
        GUIContent label = new GUIContent(title);
        GUIStyle style = new GUIStyle();
        style.padding = new RectOffset(6, 0, 0, 0);
        style.fontStyle = FontStyle.Bold;
        GUILayout.Label(label, style);
    }
}
