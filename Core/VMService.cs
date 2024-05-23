using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using Manager.Core.Models;

namespace Manager.Core
{
    public class VMService
    {
        private readonly SchedulerService _schedulerService;
        private readonly string _vmConfigFilePath = @"C:\Files\Manager\Configs\configVM.json";
        private readonly string _generalConfigFilePath = @"C:\Files\Manager\Configs\config.json";

        public VMService(SchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        public IEnumerable<VM> GetVMs()
        {
            if (!File.Exists(_vmConfigFilePath))
            {
                return new List<VM>();
            }

            var jsonData = File.ReadAllText(_vmConfigFilePath);
            var configData = JsonConvert.DeserializeObject<VMConfig>(jsonData);

            if (configData != null && configData.VMs != null)
            {
                return configData.VMs;
            }
            return new List<VM>();
        }

        public void StartVM(string vmName)
        {
            var config = LoadVMConfig(_vmConfigFilePath);
            if (config == null)
            {
                throw new InvalidOperationException("VM config not loaded.");
            }

            var vmPath = GetVMPath(vmName, config);
            if (string.IsNullOrEmpty(vmPath))
            {
                throw new InvalidOperationException($"VM path for {vmName} not found.");
            }

            if (string.IsNullOrEmpty(config.General.VmrunPath))
            {
                throw new InvalidOperationException("VMrun path not found in general configuration.");
            }

            Process.Start(new ProcessStartInfo(config.General.VmrunPath, $"start \"{vmPath}\"")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });
        }

        public void StopVM(string vmName)
        {
            var config = LoadVMConfig(_vmConfigFilePath);
            if (config == null)
            {
                throw new InvalidOperationException("VM config not loaded.");
            }

            var vmPath = GetVMPath(vmName, config);
            if (string.IsNullOrEmpty(vmPath))
            {
                throw new InvalidOperationException($"VM path for {vmName} not found.");
            }

            if (string.IsNullOrEmpty(config.General.VmrunPath))
            {
                throw new InvalidOperationException("VMrun path not found in general configuration.");
            }

            Process.Start(new ProcessStartInfo(config.General.VmrunPath, $"stop \"{vmPath}\"")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });
        }

        public void StopAllVMs()
        {
            var vms = GetVMs();
            foreach (var vm in vms)
            {
                StopVM(vm.Name);
            }
        }

        public VMConfig LoadVMConfig(string filePath)
        {
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<VMConfig>(jsonData);
            }
            return null;
        }

        public Config LoadGeneralConfig()
        {
            if (File.Exists(_generalConfigFilePath))
            {
                var jsonData = File.ReadAllText(_generalConfigFilePath);
                return JsonConvert.DeserializeObject<Config>(jsonData);
            }
            return null;
        }

        private string GetVMPath(string vmName, VMConfig config)
        {
            var vm = config.VMs.FirstOrDefault(v => v.Name == vmName);
            if (vm != null)
            {
                return $@"{vm.Disk}:\Disk {vm.Disk} VMs\{vm.Name}\Windows 10 x64.vmx";
            }
            return null;
        }

        public void ArrangeWindows()
        {
            var config = LoadVMConfig(_vmConfigFilePath);
            var generalConfig = LoadGeneralConfig();

            if (config == null || generalConfig == null)
            {
                throw new InvalidOperationException("Configuration not loaded.");
            }

            if (generalConfig.Grid == null)
            {
                throw new ArgumentNullException(nameof(generalConfig.Grid), "Grid configuration cannot be null.");
            }

            foreach (var vm in config.VMs)
            {
                var position = CalculateVMPosition(vm.Cell, generalConfig.Grid);
                MoveVMWindow(vm.Name, position.x, position.y, generalConfig.Grid.CellWidth, generalConfig.Grid.CellHeight);
            }
        }

        private (int x, int y) CalculateVMPosition(int cellNumber, GridConfig gridConfig)
        {
            if (gridConfig == null)
            {
                throw new ArgumentNullException(nameof(gridConfig), "Grid configuration cannot be null.");
            }

            int columns = gridConfig.Columns;
            int width = gridConfig.CellWidth;
            int height = gridConfig.CellHeight;

            int row = (cellNumber - 1) / columns;
            int column = (cellNumber - 1) % columns;
            int x = column * width;
            int y = row * height + 35; // Added 35 pixels offset for the top

            return (x, y);
        }

        private void MoveVMWindow(string vmName, int x, int y, int width, int height)
        {
            var windowTitle = $"{vmName} - VMware Workstation";

            IntPtr windowHandle = FindWindowByTitle(windowTitle);
            if (windowHandle == IntPtr.Zero)
            {
                // Debug message: window not found
            }
            else
            {
                // Debug message: found window
                bool result = SetWindowPos(windowHandle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
                // Debug message: SetWindowPos result
            }
        }

        private IntPtr FindWindowByTitle(string title)
        {
            IntPtr foundWindow = IntPtr.Zero;
            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                int length = GetWindowTextLength(wnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(wnd, builder, length + 1);

                if (builder.ToString().Contains(title))
                {
                    foundWindow = wnd;
                    return false; // Stop enumeration
                }

                return true;
            }, IntPtr.Zero);

            return foundWindow;
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_NOACTIVATE = 0x0010;

        public string GetCurrentShift()
        {
            return _schedulerService.GetCurrentShift();
        }

        public void ManageVMs(string currentShift)
        {
            var vms = GetVMs();
            foreach (var vm in vms)
            {
                if (vm.Shift == currentShift && vm.Status == "Active")
                {
                    StartVM(vm.Name);
                }
                else
                {
                    StopVM(vm.Name);
                }
            }
        }
    }

    public class VMConfig
    {
        [JsonProperty("general")]
        public GeneralConfig General { get; set; }

        [JsonProperty("vms")]
        public List<VM> VMs { get; set; }
    }

    public class GeneralConfig
    {
        [JsonProperty("vmrunPath")]
        public string VmrunPath { get; set; }
    }
}
