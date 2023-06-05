using System.Text;


namespace AbsoluteSolver
{
    internal class stuff
    {
       
    }
    internal class Interface
    {
        string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public void basicInfo(string status, string clientLevel )
        {

            string repeatPathSpace = new string(' ', 80 - ( currentDirectory.Length + 19 ));
            string repeatClientSpace = new string(' ', (clientLevel.Length + 21) - (101 - 58 - 20));
            string repeatStatusSpace = new string(' ', (status.Length + 62) - (101 - 58 - 8));
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            Console.WriteLine(@$"
|---------------------------------------------------------------------------------------------------|
|{currentDirectory + "\\AbsoluteSolver.exe" + repeatPathSpace + " |  _  |  □  |  x  |"} 
|---------------------------------------------------------------------------------------------------|
|                        ║╢                                                                         |
|                       ║▒▒╢                                                                        |
|                      ╓▒▒▒▒╖                                                                       |
|                     ╓▒▒▒▒▒▒╖                                                                      |
|                       ╙▒▒╜                                                                        |
|                        ▒▒                                                                         |
|                        ▒▒                                                                         |
|                        ▒▒                                                                         |
|                     ,╓@╜╙%╖,                                                                      |
|                  ╓@╜,╥╢▒▒╢╥,╙%╖                                                                   |
|                  ▒ ▒▒▒▒▒▒▒▒▒▒ ▒                                                                   |
|                  ▒ ▒▒▒▒▒▒▒▒▒▒ ▒                                                                   |
|                  ▒ ▒▒▒▒▒▒▒▒▒▒ ▒                                                                   |
|               ╓╢╢╜╙╥,╙╢▒▒╢╜,╥╜╙╢╢╖                     Client permission: {clientLevel + repeatClientSpace + '|'}     
|      ╓H╖  ,╥╨╜`      ╙╨╥╥╨╜       ╙╨╥,   ╓H╖           Status: {status + repeatStatusSpace + '|'}
|    ╓╢▒▒▒▒╢                            ║▒▒▒▒▒╖                                                     |
|   ╢▒▒▒▒▒▒╢                            ║▒▒▒▒▒▒╢                                                    |
| d╨╨╨╜╜  ``                               `  ╙╜╨╨╨h                                                |                            
|___________________________________________________________________________________________________|
            ");
            if (clientLevel == "User")
            {
                Console.WriteLine("Restart me with admin rights or I'm about to break something -_-");
            }
        }
    }
}
