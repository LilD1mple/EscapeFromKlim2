namespace EFK2.Game.Save
{
	public static class SaveUtility
	{
		private const string _settingsDataFile = "Settings.es3";

		public static bool KeyExists(string key) => ES3.KeyExists(key, _settingsDataFile);

		public static bool KeyExists(string key, string fileName) => ES3.KeyExists(key, fileName);

		public static void SaveData<T>(string key, T value) => ES3.Save(key, value, _settingsDataFile);

		public static void SaveData<T>(string key, T value, string fileName) => ES3.Save(key, value, fileName);

		public static T LoadData<T>(string key, T defaultValue) => KeyExists(key) ? ES3.Load<T>(key, _settingsDataFile) : defaultValue;

		public static T LoadData<T>(string key, T defaultValue, string fileName) => KeyExists(key, fileName) ? ES3.Load<T>(key, fileName) : defaultValue;

		public static void DeleteKey(string key) => ES3.DeleteKey(key, _settingsDataFile);
	}
}