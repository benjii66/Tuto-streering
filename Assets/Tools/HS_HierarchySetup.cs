using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public class HS_DefaultSetup
{
    [MenuItem("Project setup/Hierarchy")]
    static void DefaultSetup()
    {
        SetObjectInHierarchy<Camera>("CAMERAS");
        SetObjectInHierarchy<Light>("LIGHTS");
        SetObjectInHierarchy<AudioSource>("AUDIO SOURCE");
        SetObjectInHierarchy<ParticleSystem>("PARTICLE SYSTEM");
        CreateHierarchySection("PROPS");
        CreateHierarchySection("LOGIC");
        CreateHierarchySection("CHARACTERS");
        CreateHierarchySection("AI");
    }
    [MenuItem("Project setup/Setup")]
    static void DefaultFolderSetup()
    {
        CreateFolder("Scripts");
        CreateFolder("Meshes");
        CreateFolder("Prefab");
        CreateFolder("AI");
    }
    //
    static void SetObjectInHierarchy<T>(string _sectionName) where T : Component
    {
        int _sectionIndex = FindSection(_sectionName);
        Type _type = typeof(T);
        Object[] _m = GameObject.FindObjectsOfType(_type);
        for (int i = 0; i < _m.Length; i++)
        {
            T _object = (T)Convert.ChangeType(_m[i], _type);
            _object.transform.SetSiblingIndex(_sectionIndex);
        }
    }
    static void CreateHierarchySection(string _sectionName)
    {
        GameObject _section = GameObject.Find($"------[{_sectionName}]------");
        if (_section) return;
        _section = new GameObject($"------[{_sectionName}]------");
        _section = new GameObject($"------------------");
    }
    static int FindSection(string _sectionName)
    {
        GameObject _section = null;
        if (!(_section = GameObject.Find($"------[{_sectionName}]------")))
        {
            _section = new GameObject($"------[{_sectionName}]------");
            _section = new GameObject($"------------------");
            return _section.transform.GetSiblingIndex() - 1;
        }
        return _section.transform.GetSiblingIndex() + 1;
        //return _section;
    }   
  

    static void CreateFolder(string _folderName)
    {
        string _guid = AssetDatabase.CreateFolder("Assets", _folderName);
        string newFolderPath = AssetDatabase.GUIDToAssetPath(_guid);
    }

 
}
