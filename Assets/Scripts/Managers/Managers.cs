using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    public static InputManager Input
    {
        get
        {
            return Instance._input;
        }
    }
    public static ResourceManager Resource
    {
        get
        {
            return Instance._resource;
        }
    }
    void Start()
    {
        Init();
    }
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }
        DontDestroyOnLoad(go);
        s_instance = go.GetComponent<Managers>();
    }
}
