using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Protocol;

public class MyConsole
{
    private static MyConsole _instance;
    private static readonly object syn = new object();
    private static ConsoleMode consoleMode = ConsoleMode.Debug;
    private MyConsole() //构造函数设置private，不能被new，单例模式
    {

    }
    public static MyConsole CreateInstance()
    {
        if (_instance == null)
        {
            lock (syn)  //加锁防止多线程
            {
                if (_instance == null)
                {
                    _instance = new MyConsole();
                }
            }
        }
        return _instance;
    }

    public static void WriteLine(int msg)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg);
        }
    }

    public static void WriteLine(int msg, MessageType type)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg);
        }
        else
        {
            if (type != MessageType.General)
            {
                Console.WriteLine(msg);
            }
        }
    }

    public static void WriteLine(string msg)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg);
        }
    }

    public static void WriteLine(string msg,MessageType type)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg);
        }
        else
        {
            if (type != MessageType.General)
            {
                Console.WriteLine(msg);
            }
        }
    }

    public static void WriteLine(string msg,object arg)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg, arg);
        }
    }

    public static void WriteLine(string msg, object arg, MessageType type)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg, arg);
        }
        else
        {
            if (type != MessageType.General)
            {
                Console.WriteLine(msg, arg);
            }
        }
    }

    public static void WriteLine(string msg, params object[] arg)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg, arg);
        }
    }

    public static void WriteLine(string msg, MessageType type, params object[] arg)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.WriteLine(msg, arg);
        }
        else
        {
            if (type != MessageType.General)
            {
                Console.WriteLine(msg, arg);
            }
        }
    }

    public static void Write(string msg)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.Write(msg);
        }
    }

    public static void Write(string msg, MessageType type)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.Write(msg);
        }
        else
        {
            if (type != MessageType.General)
            {
                Console.Write(msg);
            }
        }
    }

    public static void Write(string msg, object arg)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.Write(msg, arg);
        }
    }

    public static void Write(string msg, object arg, MessageType type)
    {
        if (consoleMode == ConsoleMode.Debug)
        {
            Console.Write(msg, arg);
        }
        else
        {
            if (type != MessageType.General)
            {
                Console.Write(msg, arg);
            }
        }
    }

    public static void ResetColor()
    {
        Console.ResetColor();
    }
}

