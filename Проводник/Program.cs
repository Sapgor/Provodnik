using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();
        int currentDriveIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите диск:");

            for (int i = 0; i < drives.Length; i++)
            {
                if (i == currentDriveIndex)
                    Console.Write("> ");
                else
                    Console.Write("  ");

                Console.WriteLine($"{drives[i].Name} ({drives[i].TotalFreeSpace / 1024 / 1024 / 1024} GB свободно)");
            }

            ConsoleKey key = Console.ReadKey().Key;

            if (key == ConsoleKey.Escape)
                break;
            else if (key == ConsoleKey.DownArrow)
                currentDriveIndex = (currentDriveIndex + 1) % drives.Length;
            else if (key == ConsoleKey.UpArrow)
                currentDriveIndex = (currentDriveIndex - 1 + drives.Length) % drives.Length;
            else if (key == ConsoleKey.Enter)
                Navigate(drives[currentDriveIndex].RootDirectory.FullName);
        }
    }

    static void Navigate(string path)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Текущая папка: {path}");
            Console.WriteLine();

            DirectoryInfo currentDirInfo = new DirectoryInfo(path);
            DirectoryInfo[] directories = currentDirInfo.GetDirectories();
            FileInfo[] files = currentDirInfo.GetFiles();

            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. [{directories[i].Name}] <Папка>");
            }

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + directories.Length + 1}. {files[i].Name}");
            }

            ConsoleKey key = Console.ReadKey().Key;
            int selectedIndex = -1;

            if (key == ConsoleKey.Escape)
                break;
            else if (key == ConsoleKey.DownArrow)
                selectedIndex = 0;
            else if (key == ConsoleKey.UpArrow)
                selectedIndex = directories.Length + files.Length - 1;
            else if (key == ConsoleKey.Enter)
                Process.Start("explorer.exe", path);

            if (selectedIndex != -1)
            {
                while (true)
                {
                    if (selectedIndex >= directories.Length + files.Length)
                        selectedIndex = directories.Length;

                    Console.Clear();
                    Console.WriteLine($"Текущая папка: {path}");
                    Console.WriteLine();

                    for (int i = 0; i < directories.Length; i++)
                    {
                        if (i == selectedIndex)
                            Console.Write("> ");
                        else
                            Console.Write("  ");

                        Console.WriteLine($"{i + 1}. [{directories[i].Name}] <Папка>");
                    }

                    for (int i = 0; i < files.Length; i++)
                    {
                        if (i + directories.Length == selectedIndex)
                            Console.Write("> ");
                        else
                            Console.Write("  ");

                        Console.WriteLine($"{i + directories.Length + 1}. {files[i].Name}");
                    }

                    ConsoleKey innerKey = Console.ReadKey().Key;

                    if (innerKey == ConsoleKey.Escape)
                        break;
                    else if (innerKey == ConsoleKey.DownArrow)
                        selectedIndex = (selectedIndex + 1) % (directories.Length + files.Length);
                    else if (innerKey == ConsoleKey.UpArrow)
                        selectedIndex = (selectedIndex - 1 + directories.Length + files.Length) % (directories.Length + files.Length);
                    else if (innerKey == ConsoleKey.Enter)
                    {
                        if (selectedIndex < directories.Length)
                            Navigate(directories[selectedIndex].FullName);
                        else
                            Process.Start("explorer.exe", $"{path}\\{files[selectedIndex - directories.Length].Name}");
                    }
                }
            }
        }
    }
}