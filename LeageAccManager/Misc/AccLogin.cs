using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using WindowsInput;
using WindowsInput.Native;
using Condition = System.Windows.Automation.Condition;

namespace LeageAccManager
{
    public static class AccLogin
    {
       public static void Login(Account account)
        {
            try
            {
                var leaguePath = @"E:\Riot Games\Riot Client\RiotClientServices.exe";
                var arguments = "--launch-product=league_of_legends --launch-patchline=live";

                /*var app = FlaUI.Core.Application.Launch(leaguePath, arguments);
                IntPtr mainWindowHandle = NativeImports.FindWindow("Chrome_WidgetWin_1", "Riot Client");*/
                
                Process.Start(leaguePath, arguments);
                
                IntPtr riotClientHandle = IntPtr.Zero;
                DateTime startTime = DateTime.Now;

                // to kurwa ma czekac az liga sie odpali
                while ((DateTime.Now - startTime).TotalMilliseconds < NativeImports.WaitTimeout)
                {
                    riotClientHandle = NativeImports.FindWindow("Chrome_WidgetWin_1", "Riot Client");
                    if (riotClientHandle != IntPtr.Zero)
                    {
                        break;
                    }
                    Thread.Sleep(500);
                }
                
                if (riotClientHandle == IntPtr.Zero)
                {
                    MessageBox.Show("Could not find the League of Legends window.");
                    return;
                }

                NativeImports.SetForegroundWindow(riotClientHandle);
                AutomationElement usernameField = null;
                startTime = DateTime.Now;
                
                while ((DateTime.Now - startTime).TotalMilliseconds < NativeImports.WaitTimeout)
                {
                    AutomationElement rootElement = AutomationElement.RootElement;
                    
                    usernameField = FindFirstElement(rootElement, ControlType.Edit, "username");
                    
                    if (usernameField != null)
                    {
                        break;
                    }
                    Thread.Sleep(500);
                }
                
                if (usernameField == null)
                {
                    MessageBox.Show("Could not find the username field.");
                    return;
                }

                
                //Thread.Sleep(10000);

                InputSimulator sim = new InputSimulator();

                foreach (char c in account.Username)
                {
                    sim.Keyboard.TextEntry(c);
                    Thread.Sleep(100);
                }
                
                sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
                Thread.Sleep(100);

                foreach (char c in account.Password)
                {
                    Thread.Sleep(100);
                    sim.Keyboard.TextEntry(c);
                }

                sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting League of Legends: {ex.Message}");
            }
        }
        private static AutomationElement FindFirstElement(AutomationElement rootElement, ControlType controlType, string automationId)
        {
            Condition condition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, controlType),
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId)
            );

            return rootElement.FindFirst(TreeScope.Descendants, condition);
        }
       
    }
}