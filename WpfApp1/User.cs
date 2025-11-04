using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
	public class User
	{
		public string Username { get; set; }      // Имя пользователя
		public string Password { get; set; }      // Пароль
		public bool IsBlocked { get; set; }       // Заблокирован ли пользователь
		public bool HasPasswordRestrictions { get; set; } // Ограничения на пароль (вариант №3)
	}
}
