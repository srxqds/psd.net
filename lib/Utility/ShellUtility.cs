/*
 * File: Assets/Scripts/Game/Utility/ShellUtility.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/23/2015 1:08:31 PM
 */
using UnityEngine;
using System.Collections;
using Utility;

public class ShellUtility
{
    /// <summary>
    /// 注意传参中路径包含空格的问题，使用 "\"" + path + "\"" 来转义
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="arguments"></param>
    public static void StartProcess(string fileName, string arguments)
    {
        System.Diagnostics.ProcessStartInfo pi = new System.Diagnostics.ProcessStartInfo();
        pi.FileName = fileName;

        pi.Arguments = arguments;
        pi.RedirectStandardError = true;
        pi.RedirectStandardOutput = true;
        pi.UseShellExecute = false;
        pi.CreateNoWindow = true;
        System.Diagnostics.Process p = System.Diagnostics.Process.Start(pi);
        p.WaitForExit();
        if (p.StandardOutput != null)
        {
            while (true)
            {
                string output = p.StandardOutput.ReadLine();
                if (output != null)
                    LogUtility.LogInfo(p.StandardOutput.ReadLine());
                else
                    break;
            }
        }
        if (p.StandardError != null)
        {
            while (true)
            {
                string output = p.StandardError.ReadLine();
                if (output != null)
                    LogUtility.LogInfo(p.StandardError.ReadLine());
                else
                    break;
            }
        }
    }
        

}

