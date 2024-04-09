using UnityEngine;

public static class Log
{

    public static void Info(string msg)
    {
        Debug.Log(msg);
    }
    public static void Info(string msg,object arg)
    {
        Debug.LogFormat(msg, arg);
    }


    public static void Error(string msg)
    {
        Debug.LogError(msg);
    }
    public static void Error(string msg, object arg)
    {
        Debug.LogErrorFormat(msg, arg);
    }
}
