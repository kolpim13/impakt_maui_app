using DeviceId;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace HW_QR_Scanner.Models
{
    public class AppSettings
    {
        public AppSettings() 
        { ;}

        public string SerialPortName { get; set; } = "COM3";
        public int BaudRate { get; set; } = 115200;
        public string LastScanningMode { get; set; } = "None";
    }

    public class AppCridentials
    {
        public AppCridentials() { ; }

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public static class DeviceService
    {
        private static readonly string _settingsPath;
        private static readonly string _cridentialsPath;

        public static AppSettings Settings { get; private set; } = new();
        public static AppCridentials Cridentials { get; private set; } = new();

        static DeviceService()
        {
            var configDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "HW_QR_Scanner");

            _settingsPath = Path.Combine(configDir, "settings.json");
            _cridentialsPath = Path.Combine(configDir, "cridentials.json");
        }

        [ModuleInitializer]
        public static void Initialize()
        {
            LoadSettings();
            LoadCridentials();
        }

        private static T LoadFromFile<T>(string path_to_file) where T : new()
        {
            if (!File.Exists(path_to_file))
                return new T();

            string json = File.ReadAllText(path_to_file);
            return JsonSerializer.Deserialize<T>(json) ?? new T();
        }

        private static void SaveToFile<T>(string path_to_file, T data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory(Path.GetDirectoryName(path_to_file)!);
            File.WriteAllText(path_to_file, json);
        }

        public static void LoadSettings()
        {
            Settings = LoadFromFile<AppSettings>(_settingsPath);
        }
        public static void SaveSettings()
        {
            SaveToFile(_settingsPath, Settings);
        }
        public static void SaveSettings(AppSettings settings)
        {
            SaveToFile(_settingsPath, settings);
            Settings = settings;
        }
        public static AppSettings GetSettingsDeepCopy()
        {
            return new AppSettings()
            {
                SerialPortName = Settings.SerialPortName,
                BaudRate = Settings.BaudRate,
                LastScanningMode = Settings.LastScanningMode,
            };
        }
        
        public static void LoadCridentials()
        {
            Cridentials = LoadFromFile<AppCridentials>(_cridentialsPath);
        }
        public static void SaveCridentials()
        {
            SaveToFile(_cridentialsPath, Cridentials);
        }
        public static void SaveCridentials(AppCridentials cridentials)
        { 
            SaveToFile(_cridentialsPath, cridentials);
            Cridentials = cridentials;
        }
    
        public static string GetDeviceFingerprint()
        {
            return new DeviceIdBuilder()
                .AddMachineName()
                .AddOsVersion()
                .OnWindows(windows => windows
                    .AddWindowsProductId()
                    .AddWindowsDeviceId())
                .OnLinux(linux => linux
                    .AddProductUuid()
                    .AddMachineId()
                    .AddCpuInfo())
                .UseFormatter(new DeviceId.Formatters.HashDeviceIdFormatter(() => System.Security.Cryptography.SHA256.Create(), new DeviceId.Encoders.Base64ByteArrayEncoder()))
                .ToString();
        }
        public static AppCridentials GetCridentialsDeepCopy()
        {
            return new AppCridentials()
            {
                Username = Cridentials.Username,
                Password = Cridentials.Password,
            };
        }
    }
}
