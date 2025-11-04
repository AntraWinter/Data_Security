using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace WpfApp1
{
	public partial class App : Application
	{
		public static ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
		private const string DecryptedFilePath = "users.txt"; // Расшифрованный файл
		public static bool isUsersChanged = false;
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// Запускаем окно для ввода парольной фразы
			var passwordWindow = new PassPhraseWindow();
			passwordWindow.Show();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			// При закрытии программы шифруем users.txt обратно в users.enc
			var passwordWindow = new PassPhraseWindow();

			// Сохраняем данные
			App.SaveUsers();

			// Шифруем файл
			if (File.Exists("users.txt") && isUsersChanged)
			{
				CryptoHelper.EncryptFile(DecryptedFilePath, "users.enc", CryptoHelper.CorrectPhrase);
				File.Delete("users.txt");
			}
			else
			{
				File.Delete("users.txt");
			}

			base.OnExit(e);
		}

		// Загрузка пользователей из расшифрованного файла
		public static void LoadUsers()
		{
			Users.Clear();
			if (File.Exists(DecryptedFilePath))
			{
				string[] lines = File.ReadAllLines(DecryptedFilePath);
				foreach (string line in lines)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						string[] parts = line.Split(',');
						if (parts.Length == 4)
						{
							Users.Add(new User
							{
								Username = parts[0],
								Password = parts[1],
								IsBlocked = bool.Parse(parts[2]),
								HasPasswordRestrictions = bool.Parse(parts[3])
							});
						}
					}
				}

				//// ДОБАВЬТЕ ДЛЯ ОТЛАДКИ:
				//MessageBox.Show($"Загружено пользователей: {Users.Count}");
				//foreach (var user in Users)
				//{
				//	MessageBox.Show($"User: {user.Username}, Pass: {user.Password}");
				//}
			}
			//else
			//{
			//	MessageBox.Show($"Файл {DecryptedFilePath} не существует!");
			//}
		}

		// Сохранение пользователей в расшифрованный файл
		public static void SaveUsers()
		{
			List<string> lines = new List<string>();
			foreach (var user in Users)
			{
				lines.Add($"{user.Username},{user.Password},{user.IsBlocked},{user.HasPasswordRestrictions}");
			}
			File.WriteAllLines(DecryptedFilePath, lines);
		}


	}
}
