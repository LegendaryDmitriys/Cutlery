using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private int countAttempts;
        private connection con = new connection();
        private string UserId, UserLogin, UserPassword, UserRole, UserName, UserSurname, UserPatronymic;
        
        public Form1()
        {
            InitializeComponent();
            ApplyColorScheme();
            countAttempts = 0;
        }
        
        // Применение цветовой схемы
        private void ApplyColorScheme()
        {
            this.BackColor = Color.White;
            button1.BackColor = Color.FromArgb(73, 140, 81); 
            button1.ForeColor = Color.White;
            button2.BackColor = Color.FromArgb(73, 140, 81); 
            button2.ForeColor = Color.White;
        }

        // Обработчик нажатия на кнопку "Войти"
        private async void button1_Click(object sender, EventArgs e)
        {
            string Login = userLogin.Text.Trim();
            string Password = userPassword.Text.Trim();

            try
            {
                if (Login != "" && Password != "")
                {
                    
                    con.Open();

                    
                    // SQL-запрос
                    string query = "SELECT UserId, UserLogin, UserPassword,UserRole, UserName , UserSurname, UserPatronymic FROM user WHERE UserLogin ='" + Login + "' AND UserPassword ='" + Password + "'";

                    
                    // Выполнение запроса и обработка результатов
                    using (MySqlDataReader reader = con.ExecuteQuery(query))
                    {
                        if (reader != null && reader.Read())
                        {
                            UserId = reader["UserId"].ToString();
                            UserLogin = reader["UserLogin"].ToString();
                            UserPassword = reader["UserPassword"].ToString();
                            UserRole = reader["UserRole"].ToString();
                            UserName = reader["UserName"].ToString();
                            UserSurname = reader["UserSurname"].ToString();
                            UserPatronymic = reader["UserPatronymic"].ToString();
                            
                            // Отображение формы
                            ShowFormUsers(UserSurname + " " + UserName + " " + UserPatronymic, UserRole); 
                            Console.WriteLine("Вход выполнен. UserId: " + UserId + ", UserLogin: " + UserLogin + ", UserPassword: " + UserPassword + ", UserRole : " + UserRole);
                            
                        }
                        else
                        {
                            // Показ капчи и блокировка кнопки "Войти"
                            MessageBox.Show("Пользователь не найден. Пожалуйста, проверьте логин и пароль.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            countAttempts++;
                            if (countAttempts >= 2)
                            {
                                ShowCaptcha();
                                button1.Enabled = false;
                                await Task.Delay(10 * 1000);
                                button1.Enabled = true;
                                countAttempts = 0;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Логин и пароль не могут быть пустыми.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            
        }

        
        // Показ формы капчи
        private void ShowCaptcha()
        {
            Captcha captcha = new Captcha();
            captcha.ShowDialog();
            
        }

        
        // Показ основной формы 
        private void ShowFormUsers(string userFullName,string UserRole)
        {
            DataBank.Text = userFullName;
            DataBank.Text2 = UserRole;
            FormUsers user = new FormUsers();
            user.ShowDialog();
        }
        

        // Обработчик нажатия на кнопку "Войти как гость"
        private void button2_Click(object sender, EventArgs e)
        {
            ShowFormUsers("", UserRole);
        }
    }
}