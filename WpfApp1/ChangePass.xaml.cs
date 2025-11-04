using System.Linq;
using System.Windows;

namespace WpfApp1
{
	public partial class ChangePass : Window
	{
		private User currentUser;
		private bool isFirstLogin; // Флаг: первый вход пользователя или нет

		public ChangePass(User user, bool isFirstLogin = false)
		{
			InitializeComponent();
			currentUser = user;
			this.isFirstLogin = isFirstLogin; // Если true, то это первый вход
		}

		// Кнопка "Ок" (смена пароля)
		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			string oldPassword = OldPasswordInput.Password;
			string newPassword = NewPasswordInput.Password;
			string confirmPassword = ConfirmPasswordInput.Password;
			if (oldPassword == currentUser.Password)
			{
				// Проверяем, что пароли совпадают
				if (newPassword != confirmPassword)
				{
					MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Проверяем ограничения на пароль (если включены)
				if (currentUser.HasPasswordRestrictions && !IsPasswordValid(newPassword))
				{
					// Пароль не соответствует ограничениям
					MessageBoxResult result = MessageBox.Show(
						"Пароль не соответствует требованиям! Он должен содержать буквы и цифры.\n\n" +
						"Хотите ввести другой пароль?",
						"Ошибка",
						MessageBoxButton.YesNo,
						MessageBoxImage.Warning);

					if (result == MessageBoxResult.Yes)
					{
						// Очищаем поля для ввода нового пароля
						NewPasswordInput.Clear();
						ConfirmPasswordInput.Clear();
						return; // Пользователь может ввести пароль заново
					}
					else
					{
						// Пользователь отказался вводить пароль заново
						if (isFirstLogin)
						{
							// Если это первый вход, завершаем работу программы
							MessageBox.Show("Работа программы завершена.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
							Application.Current.Shutdown();
						}
						else
						{
							// Если это не первый вход, просто закрываем окно смены пароля
							this.Close();
						}
					}
					return; // Прерываем выполнение, так как пароль неверный
				}

				if (newPassword == "" || confirmPassword == "")
				{
					MessageBox.Show("Пароли не могут быть пустыми!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Если всё хорошо, обновляем пароль
				currentUser.Password = newPassword;
				App.SaveUsers(); // Сохраняем изменения
				App.isUsersChanged = true;
				MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

				// Возвращаемся к окну входа или администратора
				if (currentUser.Username == "ADMIN")
				{
					var adminWindow = new AdminWindow();
					adminWindow.Show();
				}
				else
				{
					var mainWindow = new MainWindow();
					mainWindow.Show();
				}
				this.Close();
			}
			else
			{
				MessageBox.Show("Неверный старый пароль", "Успех", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Проверка пароля на соответствие варианту №3 (буквы + цифры)
		private bool IsPasswordValid(string password)
		{
			return password.Any(char.IsLetter) && password.Any(char.IsDigit);
		}

		// Кнопка "Отмена"
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			if (isFirstLogin)
			{
				// Если это первый вход, завершаем программу
				MessageBox.Show("Работа программы завершена.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
				Application.Current.Shutdown();
			}
			else
			{
				// Если это не первый вход, просто закрываем окно

				this.Close();
			}
		}
	}
}
