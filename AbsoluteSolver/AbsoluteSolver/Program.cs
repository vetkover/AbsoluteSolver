using AbsoluteSolver;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceProcess;

namespace AbsoluteSolver
{
    class Program
    {
        private static inWork worker = new inWork();
        private static Interface Interface = new Interface();

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                
                static string IsClientAdmin()
                {
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    if (principal.IsInRole(WindowsBuiltInRole.Administrator)) 
                    {
                        return "Administrator";
                    } else
                    {
                        return "User";
                    }
                }

                string isClientAdmin = IsClientAdmin();
                string serviceName = "AbsoluteSolver";

                ServiceController service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);
                if (IsClientAdmin() != "User")
                {
                    if (service != null)
                    {
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            Interface.basicInfo("work", "Administrator");
                            onStart();
                        }
                        else if (service.Status == ServiceControllerStatus.Stopped)
                        {
                            Interface.basicInfo("launch", "Administrator");
                            service.Start();
                            service.WaitForStatus(ServiceControllerStatus.Running);
                            Interface.basicInfo("work", "Administrator");
                            onStart();
                        }
                    }
                    else
                    {
                        //Console.WriteLine("AbsoluteSolver started installation");
                        Interface.basicInfo("installation", "Administrator");
                        Installation.onInstallation();
                    }
                }
                else
                {
                    Interface.basicInfo("idle", "User");
                }
            }

            void onStart()
            {
                Thread executorThread = new Thread(worker.executor);
                executorThread.Start();
                executorThread.Join();
            }

            while (true)
            {
                Task.Delay(1000);
            }
        }

    }

    class Installation
    {
        public static void onInstallation()
        {
            createBatFile();
            Thread.Sleep(400);
            createSheduleTask();
            Thread.Sleep(400);
            Process.Start("shutdown", "/r /t 0");
        }
        static private void createSheduleTask()
        {
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\firstTask.bat";
            string arguments = $@"/c schtasks /create /tn ""AbsoluteSolver"" /tr ""{currentDirectory}"" /ru ""NT AUTHORITY\SYSTEM"" /sc onstart";

            Process process = new Process();
            process.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            process.WaitForExit();
        }


        static private void createBatFile()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string batFilePath = Path.Combine(currentDirectory, "firstTask.bat");
            string serviceFilePath = Path.Combine(currentDirectory, "AbsoluteSolverService.exe");

            string batFileContent =
                $@"sc stop AbsoluteSolver
sc delete AbsoluteSolver
sc create AbsoluteSolver binPath= ""{serviceFilePath}"" obj= ""NT AUTHORITY\SYSTEM""";
            //I forbid the use of tabs and spaces in the lines above, this entity is above me and you 
            try
            {
                File.WriteAllText(batFilePath, batFileContent);
            }
            catch (Exception e)
            {
                Console.WriteLine("firstTask.bat cannot be created: " + e.Message);
            }
        }
    }
}

class inWork
{
    public void executor()
    {
        Thread logerThread = new Thread(logListener);
        logerThread.Start();

        Thread cmdThread = new Thread(userInputListener);
        cmdThread.Start();

        logerThread.Join();
        cmdThread.Join();
    }

    public void logListener()
    {

        string keyPath = @"SYSTEM\AbsoluteSolver";
        string valueName = "AbsoluteSolverLog";

        while (true)
        {
             Task.Delay(400).Wait();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true);

            if (key != null)
            {
                object value = key.GetValue(valueName);

                if (value != null && value.ToString() != " ")
                {
                    Console.WriteLine($@"{value}");

                        key.SetValue(valueName, " ");
                }
            }
            else
            {
                Console.WriteLine("AbsoluteSolver key does not exist");
                break;
            }
        }
    }

    static void userInputListener()
    {
        while (true)
        {
            string keyPath = @"SYSTEM\AbsoluteSolver";
            string valueName = "AbsoluteSolverCMD";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true);
            //Console.Write("input: \n");
            string input = Console.ReadLine();
            key.SetValue(valueName, input);
        }
    }
}