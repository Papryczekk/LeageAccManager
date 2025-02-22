using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace LeageAccManager
{
    public static class AccLogin
    { 
       public static async Task Login(Account account)
        {
            try
            {
                var leaguePath = @"E:\Riot Games\Riot Client\RiotClientServices.exe";
                var arguments = "--launch-product=league_of_legends --launch-patchline=live";
                
                Logger.Log("Starting Riot Client...", LogType.Info);
                Process.Start(leaguePath, arguments);
                
                Process riotClientProcess = await Lcu.WaitForProcessByName(NativeImports.RiotClientProcessName);
                Logger.Log("Waiting for Riot Client window...", LogType.Info);
                
                if (riotClientProcess == null)
                {
                    MessageBox.Show("Could not find the Riot Client window within 30 seconds.");
                    return;
                }
                
                Color color = Color.FromArgb(Image.Alpha, Image.Red, Image.Green, Image.Blue);
                while (color != Image.GetPixelColor(400, 700))
                {
                    await Task.Delay(500);
                    Logger.Log("Waiting for Riot Client to become idle...", LogType.Info);
                }
                
                Logger.Log("Proceeding with login...", LogType.Info);
                
                IntPtr riotClientHandle = riotClientProcess.MainWindowHandle;
                if (riotClientHandle == IntPtr.Zero)
                {
                    MessageBox.Show("Could not get the Riot Client window handle.");
                    return;
                }
                
                if (NativeImports.IsIconic(riotClientHandle))
                {
                    Logger.Log("Restoring Riot Client window...", LogType.Warning);
                    NativeImports.ShowWindow(riotClientHandle, NativeImports.SwRestore);
                }
                
                Logger.Log("Setting Riot Client window to foreground...", LogType.Warning);
                NativeImports.SetForegroundWindow(riotClientHandle);

                
                Logger.Log("Entering username...", LogType.Warning);
                await Task.Delay(1000);
                InputSimulator sim = new InputSimulator();
                foreach (char c in account.Username)
                {
                    sim.Keyboard.TextEntry(c);
                    await Task.Delay(50);
                }
                
                Logger.Log("Switching to password field...", LogType.Warning);
                sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
                await Task.Delay(500);
                
                Logger.Log("Entering password...", LogType.Warning);
                foreach (char c in account.Password)
                {
                    sim.Keyboard.TextEntry(c);
                    await Task.Delay(50);
                }
                
                Logger.Log("Submitting login form...", LogType.Warning);
                sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting League of Legends: {ex.Message}");
            }
        }
    }
}