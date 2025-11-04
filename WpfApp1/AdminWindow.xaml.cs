using System.Linq;
using System.Windows;

namespace WpfApp1
{
	public partial class AdminWindow : Window
	{
		public AdminWindow()
		{
			InitializeComponent();
			UsersDataGrid.ItemsSource = App.Users; // Привязываем данные к DataGrid
		}

		// Кнопка "Добавить пользователя"
		private void AddUserButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new InputDialog("Введите имя нового пользователя:");
			if (dialog.ShowDialog() == true)
			{
				string newUsername = dialog.UserInput;
				if (!string.IsNullOrEmpty(newUsername))
				{
					if (App.Users.Any(u => u.Username == newUsername))
					{
						MessageBox.Show("Пользователь с таким именем уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

					App.Users.Add(new User
					{
						Username = newUsername,
						Password = "",
						IsBlocked = false,
						HasPasswordRestrictions = false
					});

					App.isUsersChanged = true;
				}
			}
		}

		// Кнопка "Сменить пароль админа"
		private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
		{
			// Получаем выбранного пользователя из таблицы
			User selectedUser = UsersDataGrid.SelectedItem as User;

			if (selectedUser == null)
			{
				MessageBox.Show("Выберите пользователя в таблице!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			// Открываем окно смены пароля для выбранного пользователя
			var changePasswordWindow = new ChangePass(selectedUser);
			changePasswordWindow.Show();
			this.Close();
		}

		private void About_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(
				"Лабораторная работа 1 и 3\n" +
				"Вариант задания: 3 (наличие букв и цифр в пароле)\n" +
				"Автор: Анастасия Белоусова ИДБ-22-03\n" +
				"Тип симм. шифрования: блочный\n" +
				"Режим шифрования: сцепление блоков шифра\n" +
				"Добавление к ключу случайного значения: да\n" +
				"Используемый алгоритм хеширования: MD5",
				"О программе",
				MessageBoxButton.OK,
				MessageBoxImage.Information);
		}

		// Кнопка "Отмена"
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
			
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			App.SaveUsers();
			MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
