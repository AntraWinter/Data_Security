using System;
using System.IO;
using System.Windows;

namespace WpfApp1
{
	public partial class PassPhraseWindow : Window
	{
		private const string EncryptedFilePath = "users.enc";   // Зашифрованный файл
		private const string DecryptedFilePath = "users.txt";   // Расшифрованный файл

		public PassPhraseWindow()
		{
			InitializeComponent();
		}

		// Кнопка "Ок"
		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			string enteredPhrase = PasswordPhraseBox.Password;
			if (enteredPhrase != CryptoHelper.CorrectPhrase)
			{
				MessageBox.Show("Неверная парольная фраза!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				if (!File.Exists(EncryptedFilePath))
				{
					File.WriteAllText(DecryptedFilePath, "ADMIN,admin,False,False");
					CryptoHelper.EncryptFile(DecryptedFilePath, EncryptedFilePath, enteredPhrase);
					File.Delete(DecryptedFilePath);
				}
				else
				{
					CryptoHelper.DecryptFile(EncryptedFilePath, DecryptedFilePath, enteredPhrase);
					//File.Delete(EncryptedFilePath); //вернуть как было
				}

				// существует ли файл после дешифровки
				if (!File.Exists(DecryptedFilePath))
				{
					MessageBox.Show("Файл users.txt не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				App.LoadUsers();

				// загрузились ли пользователи
				if (App.Users.Count == 0)
				{
					MessageBox.Show("Пользователи не загрузились!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				var mainWindow = new MainWindow();
				mainWindow.Show();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Кнопка "Отмена"
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}


	}
}
