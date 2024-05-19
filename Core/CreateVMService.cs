using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Core
{
    public class CreateVMService
    {
        private string vmDirectory;
        private readonly string blankVmPath = @"C:\Files\VM Files\HDD_BLANK";
        private readonly string configDirectory = @"C:\Files\VM Files\Configs";

        public void SetVmDirectory(string selectedDisk)
        {
            switch (selectedDisk)
            {
                case "C":
                    vmDirectory = @"C:\Disk C VMs";
                    break;
                case "D":
                    vmDirectory = @"D:\Disk D VMs";
                    break;
                default:
                    throw new ArgumentException("Invalid disk selected.");
            }
        }

        public async Task CreateVMAsync(string vmName, Action<double, string> updateProgress)
        {
            if (string.IsNullOrWhiteSpace(vmDirectory))
            {
                throw new InvalidOperationException("VM directory is not set. Please select a valid disk.");
            }

            if (string.IsNullOrWhiteSpace(vmName))
            {
                throw new ArgumentNullException(nameof(vmName), "VM name cannot be null or empty.");
            }

            try
            {
                await Task.Run(async () =>
                {
                    updateProgress(10, "Создание директории...");
                    string newVmPath = Path.Combine(vmDirectory, vmName);

                    lock (newVmPath)
                    {
                        if (Directory.Exists(newVmPath))
                            throw new Exception($"Директория VM уже существует: {newVmPath}");

                        Directory.CreateDirectory(newVmPath);
                    }

                    var filesToTrack = GetLargestFiles(blankVmPath);
                    double eachFileProgress = 70.0 / filesToTrack.Count;

                    foreach (var file in Directory.GetFiles(blankVmPath))
                    {
                        await CopyFileWithRetries(file, Path.Combine(newVmPath, Path.GetFileName(file)));
                        if (filesToTrack.Contains(file))
                        {
                            updateProgress(eachFileProgress, $"Копирование {Path.GetFileName(file)}...");
                        }
                    }

                    updateProgress(10, "Настройка VM...");
                    var configFolder = GetNextConfigFolder();
                    foreach (var file in Directory.GetFiles(configFolder))
                        File.Move(file, Path.Combine(newVmPath, Path.GetFileName(file)));

                    Directory.Delete(configFolder);
                    updateProgress(10, $"Создание завершено. Введите новое имя VM: {vmName}");
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка создания VM", ex);
            }
        }

        private async Task CopyFileWithRetries(string sourceFile, string destinationFile, int retryCount = 5, int delayMilliseconds = 1000)
        {
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    File.Copy(sourceFile, destinationFile, true);
                    return;
                }
                catch (IOException) when (i < retryCount - 1)
                {
                    await Task.Delay(delayMilliseconds);
                }
            }

            // Если все попытки не удались, выбросить исключение
            File.Copy(sourceFile, destinationFile, true);
        }

        private string GetNextConfigFolder()
        {
            var folders = Directory.GetDirectories(configDirectory);
            if (folders.Length == 0)
                throw new Exception("Конфигурационные папки не найдены.");

            Array.Sort(folders, (x, y) => int.Parse(Path.GetFileName(x)).CompareTo(int.Parse(Path.GetFileName(y))));
            return folders[0];
        }

        private List<string> GetLargestFiles(string directory, int count = 3)
        {
            return new DirectoryInfo(directory)
                .GetFiles()
                .OrderByDescending(f => f.Length)
                .Take(count)
                .Select(f => f.FullName)
                .ToList();
        }

        public (double CDiskUsagePercentage, string CDiskUsageInfo, double DDiskUsagePercentage, string DDiskUsageInfo) GetDiskUsage()
        {
            var driveC = new DriveInfo("C");
            double percentC = (double)driveC.TotalFreeSpace / driveC.TotalSize * 100;
            string cDiskUsageInfo = $"Диск C: {driveC.TotalFreeSpace / 1024 / 1024 / 1024} GB свободно из {driveC.TotalSize / 1024 / 1024 / 1024} GB";

            var driveD = new DriveInfo("D");
            double percentD = (double)driveD.TotalFreeSpace / driveD.TotalSize * 100;
            string dDiskUsageInfo = $"Диск D: {driveD.TotalFreeSpace / 1024 / 1024 / 1024} GB свободно из {driveD.TotalSize / 1024 / 1024 / 1024} GB";

            return (100 - percentC, cDiskUsageInfo, 100 - percentD, dDiskUsageInfo);
        }

        public int GetConfigCount()
        {
            var folders = Directory.GetDirectories(configDirectory);
            return folders.Length;
        }
    }
}
