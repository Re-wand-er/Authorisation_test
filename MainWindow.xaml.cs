using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        public static string connection = "Server = localhost; Database = master; Encrypt=false; Trusted_Connection = True;";
        public static string DataBase = "Users";
        public static string Table = "AutUsers";

        public MainWindow()
        {
            InitializeComponent();

           int a = 12;

        }
        

        public void Registration(object sender, RoutedEventArgs e)
        {
            Win_Registration registration = new Win_Registration
            {
                Owner = this
            };

            registration.ShowDialog();
        }

        async public void Enter_Button(object sender, RoutedEventArgs e)
        {
            
            //string connection = "Server = localhost; Database = master; Encrypt=false; Trusted_Connection = True;";
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                await sqlconnection.OpenAsync();

                SqlCommand command = new SqlCommand(
                    $"use {DataBase};" +
                    $"SELECT Login, Password FROM {Table} " +
                    $"where Login = '{Login.Text}' AND " +
                    $"Password = '{Password.Password.GetHashCode()}';"
                    ,sqlconnection);

               SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    MessageBox.Show("Зашел");

                    this.Close();// Запуск программы
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }

    }
    
}
