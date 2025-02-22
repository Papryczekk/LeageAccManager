using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace LeageAccManager
{
    public static class Lcu
    {
        public static async Task<Process> WaitForProcessByName(string processName)
        {
            DateTime startTime = DateTime.UtcNow;
            while ((DateTime.UtcNow - startTime).TotalMilliseconds < 30000) 
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    return processes[0]; 
                }
                await Task.Delay(250); 
            }

            return null;
        }
    }
}