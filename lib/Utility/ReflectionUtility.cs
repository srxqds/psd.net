/*
 * File: Assets/Editor/BuildTools/ReflectionUtility.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/28/2015 9:52:18 PM
 */
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using Utility;

public class ReflectionUtility
{
    //区分修饰符
    public static string DecompileAssmebly(Assembly assembly)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Type type in assembly.GetTypes())
        {
            sb.AppendLine(type.ToString());

            string memberText = GetMemebersOutput(type, BindingFlags.Static | BindingFlags.Public);
            if (!string.IsNullOrEmpty(memberText))
            {
                sb.AppendLine("\tpublic static:");
                sb.AppendLine(memberText);
            }

            memberText = GetMemebersOutput(type, BindingFlags.Static | BindingFlags.NonPublic);
            if (!string.IsNullOrEmpty(memberText))
            {
                sb.AppendLine("\tprivate static:");
                sb.AppendLine(memberText);
            }

            memberText = GetMemebersOutput(type, BindingFlags.Instance | BindingFlags.Public);
            if (!string.IsNullOrEmpty(memberText))
            {
                sb.AppendLine("\tpublic:");
                sb.AppendLine(memberText);
            }

            memberText = GetMemebersOutput(type, BindingFlags.Instance | BindingFlags.NonPublic);
            if (!string.IsNullOrEmpty(memberText))
            {
                sb.AppendLine("\tprivate:");
                sb.AppendLine(memberText);
            }

        }
        return sb.ToString();
    }

    private static string GetMemebersOutput(Type type,BindingFlags bindingFlags)
    {
        StringBuilder sb = new StringBuilder();
        var members = type.GetMembers(bindingFlags);
        bool haveMemeber = false;
        foreach (MemberInfo member in members)
        {
            string name = member.Name;
            if (name == ".ctor") continue;
            if (name == "ToString") continue;
            if (name == "CompareTo") continue;
            if (name == "GetType") continue;
            if (name == "GetTypeCode") continue;
            if (name == "HasFlag") continue;
            if (name == "Equals") continue;
            if (name == "GetHashCode") continue;
            if (name == "Finalize") continue;
            if (name == "MemberwiseClone") continue;
            if (name == "obj_address") continue;
            if (name == "value__") continue;
            haveMemeber = true;
            sb.AppendLine("\t\t" + member.ToString());
        }
        if (!haveMemeber)
            return string.Empty;
        return sb.ToString();
    }

    //如果是static 直接传 类型Type
    public static object[] GetValuesByType(object obj, Type filedType, BindingFlags bindingFlags)
    {
        if (obj == null)
            return null;
        Type type = null;
        bool isStatic = false;
        if (obj is Type)
        {
            isStatic = true;
            type = obj as Type;
        }
        else
        {
            type = obj.GetType();
        }
        FieldInfo[] fields = type.GetFields(bindingFlags);
        List<object> values = new List<object>();
        foreach (var field in fields)
        {
            Debug.Log("Field Name:" + field.Name);
            if (field.FieldType.Equals(filedType))
            {
                if (isStatic)
                    values.Add(field.GetValue(null));
                else
                    values.Add(field.GetValue(obj));
            }
        }

        PropertyInfo[] proertys = type.GetProperties(bindingFlags);
        foreach (var proerty in proertys)
        {
            Debug.Log("Property　Name:" + proerty.Name);
            if (proerty.PropertyType.Equals(filedType))
            {
                if (isStatic)
                    values.Add(proerty.GetValue(null,null));
                else
                    values.Add(proerty.GetValue(obj,null));
            }

        }
        return values.ToArray();
    }

    /*public static object[] GetFieldValuesByType(Type type, Type filedType, BindingFlags bindingFlags)
    {
        FieldInfo[] fields = type.GetFields(bindingFlags);
        List<object> values = new List<object>();
        foreach (var field in fields)
        {
            if (field.FieldType.Equals(filedType))
            {
                values.Add(field.GetValue(null));
            }
        }
        return values.ToArray();
    }*/
    //深度复制
    public static object DeepCopy(object obj)
    {
        if (obj == null)
            return null;
        Type type = obj.GetType();
        if (type.IsValueType || type == typeof(string))
        {
            return obj;
        }
        else if (type.IsArray)
        {
            Type elementType = Type.GetType(
                 type.FullName.Replace("[]", string.Empty));

            var array = obj as Array;
            Array copied = Array.CreateInstance(elementType, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                copied.SetValue(DeepCopy(array.GetValue(i)), i);
            }
            return Convert.ChangeType(copied, obj.GetType());
        }
        else if (type.IsClass)
        {
            object toret = Activator.CreateInstance(obj.GetType());

            FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(obj);
                if (fieldValue == null)
                    continue;
                field.SetValue(toret, DeepCopy(fieldValue));
            }
            return toret;
        }
        else
            throw new ArgumentException("Unknown type");
    }

    [MenuItem("Editor/Utility/DecompileUnity")]
    public static void DecompileUnity()
    {
        Type unitEngineType = typeof (MonoBehaviour);
        string unityEngineDecompile = DecompileAssmebly(unitEngineType.Assembly);
        File.WriteAllText(EditorUtils.ProjectPath + @"Assets/Editor/Document/UnityEngine.txt", unityEngineDecompile);

        Type unitEditorType = typeof(EditorWindow);
        string unityEditorDecompile = DecompileAssmebly(unitEditorType.Assembly);
        File.WriteAllText(EditorUtils.ProjectPath + @"Assets/Editor/Document/UnityEditor.txt", unityEditorDecompile);
    }

}
