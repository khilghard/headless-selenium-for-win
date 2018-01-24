using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace DriverPassThrough
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> argsList = new List<string>(args);
            string driverPath = "";

            if (argsList.Select(s => s.Contains("WinAppDriver.exe")).Count() > 0)
            {
                bool continueWhile = true;
                while (continueWhile)
                {
                    bool goAgain = false;
                    for (int i = 0; i < argsList.Count; i++)
                    {
                        if (argsList[i] == "-driver")
                        {
                            argsList.RemoveAt(i);
                            goAgain = true;
                            break;
                        }

                        if (argsList[i].ToLower().Contains("winappdriver.exe"))
                        {
                            driverPath = argsList[i];
                            argsList.RemoveAt(i);
                            goAgain = true;
                            break;
                        }

                        if (argsList[i].Contains("--port="))
                        {
                            argsList[i] = argsList[i].Split('=').Last();
                        }
                    }

                    if (goAgain == true)
                        continueWhile = true;
                    else
                        continueWhile = false;
                }
            }

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                Arguments = String.Join(' ', argsList),
                CreateNoWindow = false,
                FileName = driverPath,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
            };

            p.Start();

            StreamWriter myStreamWriter = p.StandardInput;

            p.WaitForExit();

            Console.WriteLine(">>> Process Finished <<<");
            Console.ReadLine();
        }
    }
}
