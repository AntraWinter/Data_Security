using System.Windows;

namespace WpfApp1
{
	public partial class UserWindow : Window
	{
		private User currentUser;

		public UserWindow(User user)
		{
			InitializeComponent();
			currentUser = user;
			DataContext = this; // Для привязки WelcomeMessage
			WelcomeMessage = $"Добро пожаловать, {user.Username}!";
		}

		public string WelcomeMessage { get; set; } // Приветствие (для привязки в XAML)

		// Кнопка "Сменить пароль"
		private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
		{
			var changePasswordWindow = new ChangePass(currentUser, isFirstLogin: false);
			changePasswordWindow.Show();
			this.Close();
		}

		// Кнопка "Выйти"
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
		}
	}
}
