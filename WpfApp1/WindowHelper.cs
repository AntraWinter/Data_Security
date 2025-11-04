using System;
using System.IO;
using System.Windows;

namespace WpfApp1
{
	public static class WindowHelper
	{
		/// <summary>
		/// Универсальный метод для возврата к окну ввода парольной фразы
		/// </summary>
		public static void ReturnToPassPhraseWindow(Window currentWindow)
		{
			try
			{
				// Сохраняем данные
				App.SaveUsers();

				// Шифруем файл
				if (File.Exists("users.txt") && App.isUsersChanged)
				{
					CryptoHelper.EncryptFile("users.txt", "users.enc", CryptoHelper.CorrectPhrase);
					File.Delete("users.txt");
				}
				else
				{
					File.Delete("users.txt");
				}

				// Открываем окно ввода парольной фразы
				var passPhraseWindow = new PassPhraseWindow();
				passPhraseWindow.Show();

				currentWindow.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при возврате: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}