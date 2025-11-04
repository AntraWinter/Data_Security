using System;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
	public partial class MainWindow : Window
	{
		private int failedAttempts = 0; // Счётчик неудачных попыток ввода пароля
		
		public MainWindow()
		{
			InitializeComponent();
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameInput.Text;
			string password = PasswordInput.Password;

			// Ищем пользователя в коллекции
			var user = App.Users.FirstOrDefault(u => u.Username == username);

			if (user == null)
			{
				MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (user.IsBlocked)
			{
				MessageBox.Show("Пользователь заблокирован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (user.Password != password)
			{
				failedAttempts++;
				if (failedAttempts >= 3)
				{
					MessageBox.Show("Превышено количество попыток ввода пароля. Программа будет закрыта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					Application.Current.Shutdown();
					return;
				}
				MessageBox.Show($"Неверный пароль! Осталось попыток: {3 - failedAttempts}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Если пароль пустой (первый вход), просим его сменить
			if (string.IsNullOrEmpty(user.Password))
			{
				var changePasswordWindow = new ChangePass(user);
				changePasswordWindow.Show();
				this.Close();
				return;
			}

			// Если пользователь ADMIN, открываем окно администратора
			if (username == "ADMIN")
			{
				var adminWindow = new AdminWindow();
				adminWindow.Show();
				this.Close();
			}
			else
			{
				if (user.HasPasswordRestrictions && !(password.Any(char.IsLetter) && password.Any(char.IsDigit)))
				{
					MessageBox.Show("Пароль должен содержать буквы и цифры. Обновите пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
					var changePasswordWindow = new ChangePass(user);
					changePasswordWindow.ShowDialog();
					this.Close();
				}
				else
				{
					var userwindow = new UserWindow(user);
					userwindow.Show();
					this.Close();
				}

			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			WindowHelper.ReturnToPassPhraseWindow(this);
		}
	}
}

