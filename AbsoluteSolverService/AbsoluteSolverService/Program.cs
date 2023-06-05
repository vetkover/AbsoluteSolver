using System;
using System.ServiceProcess;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.IO;
using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace AppLauncherService
{
    public partial class AppLauncherService : ServiceBase
    {
        private Process appProcess;
        private bool isRunning;

        protected override void OnStart(string[] args)
        {
            StartApp();
        }

        protected override void OnStop()
        {
            StopApp();
        }

        private void StartApp()
        {
            CreateRegistryKeyWithPermissions();
            isRunning = true;

            Thread cmdThreader = new Thread(cmdThread);
            cmdThreader.Start();

        }

        private async void cmdExecutor(string command)
        {
            string keyPath = @"SYSTEM\AbsoluteSolver";
            string valueName = "AbsoluteSolverLog";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true);
            try
            {

                Process processInfo = new Process();
                processInfo.StartInfo.FileName = "cmd.exe";
                processInfo.StartInfo.Arguments = $@"/c ""{command}""";
                processInfo.StartInfo.RedirectStandardInput = true;
                processInfo.StartInfo.RedirectStandardOutput = true;
                processInfo.StartInfo.UseShellExecute = false;

                processInfo.Start();

                StreamWriter myStreamWriter = processInfo.StandardInput;
                myStreamWriter.WriteLine(command);

                    string output = processInfo.StandardOutput.ReadToEnd();
                    key.SetValue(valueName, output);

                processInfo.WaitForExit();
                processInfo.Close();  

            }
            catch (Exception e)
            {
                key.SetValue(valueName, e);

            }
        }

        private void cmdThread()
        {
            while (isRunning)
            {
                Task.Delay(300).Wait();

                string keyPath = @"SYSTEM\AbsoluteSolver";
                string valueName = "AbsoluteSolverCMD";
                string valueName2 = "AbsoluteSolverLog";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true);

                string input;

                    try
                    {
                        object value = key.GetValue(valueName);
                        if (value != null && value.ToString() != " ")
                        {
                            input = value.ToString();
                        key.SetValue(valueName, " ");
                        cmdExecutor(input);
                            
                        }
                    }
                    catch (Exception e)
                    {
                        key.SetValue(valueName2, e);
                    }
            }
        }

        private void StopApp()
        {
            try
            {
                isRunning = false; 

                if (appProcess != null && !appProcess.HasExited)
                {
                    appProcess.WaitForExit();
                    appProcess.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void CreateRegistryKeyWithPermissions()
        {
            string keyPath = @"SYSTEM\AbsoluteSolver";

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath))
            {
                RegistrySecurity registrySecurity = new RegistrySecurity();

                key.SetValue("AbsoluteSolverCMD", " ", RegistryValueKind.String);
                key.SetValue("AbsoluteSolverLog", " ", RegistryValueKind.String);
            }
        }

        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AppLauncherService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
