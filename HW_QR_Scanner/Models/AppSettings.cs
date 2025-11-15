using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HW_QR_Scanner.Models
{
    public class AppSettings
    {
        public string SerialPortName { get; set; } = "COM3";
        public int BaudRate { get; set; } = 115200;
        public string LastScanningMode { get; set; } = "None";
    }

    public class SettingsService
    {
        private readonly string _filePath;

        public AppSettings Settings { get; private set; } = new();

        public SettingsService(string filePath)
        {
            _filePath = filePath;
        }

        public void Load()
        {
            if (!File.Exists(_filePath))
            {
                Settings = new AppSettings(); // defaults
                return;
            }

            var json = File.ReadAllText(_filePath);
            Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            File.WriteAllText(_filePath, json);
        }
    }
}
