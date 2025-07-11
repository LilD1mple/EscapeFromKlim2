using System;
using System.IO;

namespace EFK2.Debugs
{
	public sealed class FileDebugWriter : IDisposable
	{
		private readonly string _executablePath;

		private readonly TextWriter _logWriter;

		private const string _delimiter = "-------------------------------------";

		public FileDebugWriter(string dataPath)
		{
			_executablePath = CreateLogFilePath(dataPath);

			_logWriter = File.CreateText(_executablePath);
		}

		public bool CanSaveResult { get; set; } = false;

		public void WriteToFile(string value)
		{
			_logWriter.Write(value);

			_logWriter.WriteLine(_delimiter);
		}

		public void Dispose()
		{
			_logWriter.Close();

			if (CanSaveResult == false)
				File.Delete(_executablePath);
		}

		private static string CreateLogFilePath(string dataPath)
		{
			string workingDirectory = Path.Combine(dataPath, "Reports");

			if (Directory.Exists(workingDirectory) == false)
				Directory.CreateDirectory(workingDirectory);

			string fileName = $"report-{DateTime.Now:dd-MM-yyyy-HH-mm-ss}.txt";

			return Path.Combine(workingDirectory, fileName);
		}
	}
}
