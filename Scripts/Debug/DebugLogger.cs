using System;
using UnityEngine;

namespace EFK2.Debugs
{
    public class DebugLogger : MonoBehaviour
    {
        private FileDebugWriter _fileDebugWriter;

        public bool CanSaveResult
        {
            get => _fileDebugWriter.CanSaveResult;
            set => _fileDebugWriter.CanSaveResult = value;
        }

        private void Awake()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessageReceived;

            _fileDebugWriter.Dispose();
        }

        public void Construct()
        {
            _fileDebugWriter = new(Application.dataPath);
        }

        private void OnLogMessageReceived(string message, string stackTrace, LogType type)
        {
            _fileDebugWriter.WriteToFile($"[{DateTime.Now}, {type}] {message}\nStackTrace: {stackTrace}");
        }
    }
}