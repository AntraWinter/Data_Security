using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
	public partial class InputDialog : Window
	{
		public string UserInput { get; private set; } // Результат ввода

		public InputDialog(string prompt)
		{
			InitializeComponent();
			DataContext = this;
			PromptText = prompt;
		}

		public string PromptText { get; set; } // Свойство для привязки к TextBlock

		// Кнопка "Ок"
		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			UserInput = InputTextBox.Text;
			DialogResult = true; // Закрываем окно с результатом "Ок"
			Close();
		}

		// Кнопка "Отмена"
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false; // Закрываем окно с результатом "Отмена"
			Close();
		}
	}
}
