using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace IngameDebugConsole
{
    public static class DefaultCommands
    {
        private static Dictionary<string, ConsoleMethodInfo> methods;

        public static void Load(Dictionary<string, ConsoleMethodInfo> methods)
        {
            DefaultCommands.methods = methods;
            DebugLogConsole.AddCommandStatic("help", "Prints all commands", "LogAllCommands", typeof(DefaultCommands));
            DebugLogConsole.AddCommandStatic("sysinfo", "Prints system information", "LogSystemInfo", typeof(DefaultCommands));
            DebugLogConsole.AddCommandStatic("save_logs", "Saves logs to a file", "SaveLogsToFile", typeof(DefaultCommands));
        }

        // Logs the list of available commands
        public static void LogAllCommands()
        {
            int length = 25;
            foreach (var entry in methods)
            {
                if (entry.Value.IsValid())
                    length += 3 + entry.Value.signature.Length;
            }

            StringBuilder stringBuilder = new StringBuilder(length);
            stringBuilder.Append("Available commands:");

            foreach (var entry in methods)
            {
                if (entry.Value.IsValid())
                    stringBuilder.Append("\n- ").Append(entry.Value.signature);
            }

            Debug.Log(stringBuilder.Append("\n").ToString());
        }

        // Logs system information
        public static void LogSystemInfo()
        {
            StringBuilder stringBuilder = new StringBuilder(1024);

            stringBuilder.Append("Rig: ");
            AppendSysInfoIfPresent(stringBuilder, SystemInfo.deviceModel, " ");
            AppendSysInfoIfPresent(stringBuilder, SystemInfo.processorType, " ");
            AppendSysInfoIfPresent(stringBuilder, SystemInfo.processorCount, " cores ");
            AppendSysInfoIfPresent(stringBuilder, SystemInfo.systemMemorySize, "MB RAM\n");

            stringBuilder.Append("OS: ").Append(SystemInfo.operatingSystem).Append("\n");

            stringBuilder.Append("GPU: ")
                         .Append(SystemInfo.graphicsDeviceName).Append(" ")
                         .Append(SystemInfo.graphicsMemorySize).Append("MB ")
                         .Append(SystemInfo.graphicsDeviceVersion)
                         .Append(SystemInfo.graphicsMultiThreaded ? " multi-threaded\n" : "\n");

            stringBuilder.Append("Data Path: ").Append(Application.dataPath).Append("\n");
            stringBuilder.Append("Persistent Data Path: ").Append(Application.persistentDataPath).Append("\n");
            stringBuilder.Append("StreamingAssets Path: ").Append(Application.streamingAssetsPath).Append("\n");
            stringBuilder.Append("Temporary Cache Path: ").Append(Application.temporaryCachePath).Append("\n");
            stringBuilder.Append("Device ID: ").Append(SystemInfo.deviceUniqueIdentifier).Append("\n");
            stringBuilder.Append("Max Texture Size: ").Append(SystemInfo.maxTextureSize).Append("\n");
            stringBuilder.Append("Max Cubemap Size: ").Append(SystemInfo.maxCubemapSize).Append("\n");
            stringBuilder.Append("Accelerometer: ").Append(SystemInfo.supportsAccelerometer ? "supported\n" : "not supported\n");
            stringBuilder.Append("Gyro: ").Append(SystemInfo.supportsGyroscope ? "supported\n" : "not supported\n");
            stringBuilder.Append("Location Service: ").Append(SystemInfo.supportsLocationService ? "supported\n" : "not supported\n");

#if !UNITY_2019_1_OR_NEWER
            stringBuilder.Append( "Image Effects: " ).Append( SystemInfo.supportsImageEffects ? "supported\n" : "not supported\n" );
            stringBuilder.Append( "RenderToCubemap: " ).Append( SystemInfo.supportsRenderToCubemap ? "supported\n" : "not supported\n" );
#endif

            stringBuilder.Append("Compute Shaders: ").Append(SystemInfo.supportsComputeShaders ? "supported\n" : "not supported\n");
            stringBuilder.Append("Shadows: ").Append(SystemInfo.supportsShadows ? "supported\n" : "not supported\n");
            stringBuilder.Append("Instancing: ").Append(SystemInfo.supportsInstancing ? "supported\n" : "not supported\n");
            stringBuilder.Append("Motion Vectors: ").Append(SystemInfo.supportsMotionVectors ? "supported\n" : "not supported\n");
            stringBuilder.Append("3D Textures: ").Append(SystemInfo.supports3DTextures ? "supported\n" : "not supported\n");
            stringBuilder.Append("3D Render Textures: ").Append(SystemInfo.supports3DRenderTextures ? "supported\n" : "not supported\n");
            stringBuilder.Append("2D Array Textures: ").Append(SystemInfo.supports2DArrayTextures ? "supported\n" : "not supported\n");
            stringBuilder.Append("Cubemap Array Textures: ").Append(SystemInfo.supportsCubemapArrayTextures ? "supported" : "not supported");

            Debug.Log(stringBuilder.Append("\n").ToString());
        }

        private static StringBuilder AppendSysInfoIfPresent(StringBuilder stringBuilder, object sysInfo, params object[] str)
        {
            string info = sysInfo.ToString();

            if (info != SystemInfo.unsupportedIdentifier && info != "0")
            {
                stringBuilder.Append(info);

                for (int i = 0; i < str.Length; i++)
                {
                    if (!string.IsNullOrEmpty(str[i].ToString()))
                        stringBuilder.Append(str[i]);
                }
            }

            return stringBuilder;
        }

        public static void SaveLogsToFile()
        {
            string path = string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 6)
                                        , "Log_"
                                        , System.DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss")
                                        , ".txt"
                                        );

            try
            {
                File.WriteAllText(path, DebugLogManager.GetAllLogs());
                Debug.Log("Logs saved to: " + path);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error saving logs\n" + e.ToString());
            }
        }
    }
}
