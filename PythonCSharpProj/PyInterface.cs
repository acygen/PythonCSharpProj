using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PythonCSharpProj
{
    internal class PyInterface
    {
        private static bool isInited = false;
        public static void SetPyEnvironment(string pyDllPath, string pyModelFolderPath)
        {
            if (!isInited)
            {
                string pathToVirtualEnv = Path.GetDirectoryName(pyDllPath);
                // string pathToVirtualEnv = @"C:\Users\28602\AppData\Local\Programs\Python\Python310\python310.dll";
                //Environment.SetEnvironmentVariable("PATH", pathToVirtualEnv, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
                //Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib;{pathToVirtualEnv}\\DLLs;{pathToVirtualEnv}\\tcl;{pythonFilePath}", EnvironmentVariableTarget.Process);
                string pytonPath = $"{pathToVirtualEnv}\\Lib\\site-packages;" +
                    $"{pathToVirtualEnv}\\Lib;" +
                    $"{pathToVirtualEnv}\\DLLs;" +
                    $"{pathToVirtualEnv}\\tcl;" +
                    $"{pyModelFolderPath};";
                Runtime.PythonDLL = pyDllPath;
                PythonEngine.PythonHome = Path.Combine(pathToVirtualEnv, "python.exe");
                //PythonEngine.PythonPath = Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
                PythonEngine.PythonPath = pytonPath;
                PythonEngine.Initialize();
                isInited = true;
            }
        }

        public static T? CallPythonMethod<T>(string modelName, string funcName, Object[] args, Action<object> log)
        {
            if (!isInited)
            {
                log("python还未初始化！");
                return default;
            }
            PythonEngine.Initialize();

            using (Py.GIL())
            {
                PyObject pyModel = Py.Import(modelName);

                List<PyObject> pyArgs = new();
                foreach (var obj in args)
                {
                    pyArgs.Add(obj.ToPython());
                }
                pyArgs.Add(log.ToPython());
                dynamic result = pyModel.InvokeMethod(funcName, pyArgs.ToArray());
                return (T)result;
            }
        }
    }
}
