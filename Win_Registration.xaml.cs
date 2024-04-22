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
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace WpfApplication
{
    /// <summary>
    /// Логика взаимодействия для Win_Registration.xaml
    /// </summary>
    public partial class Win_Registration : Window
    {
        public Win_Registration()
        {
            InitializeComponent();


        }

        readonly Random rand = new Random();
        public int Id_marker;
        public async void Verif_button_Click(object sender, RoutedEventArgs e) // попробовать асинхронно заполнять базу данных и работать с окном чего-то
        {

            if (Login.Text == "" || Email_address.Text == "" || Password.Password == "" || Verification.Text == "")
            {
                MessageBox.Show("Пожалуйста заполните все поля");
            }
            else
            {
                /*Methods.Identification_number(Convert.ToString(Email_address));*/
                if(Verification.Text == Convert.ToString(Id_marker) || Verification.Text == "0") { // исправить код идентификации

                    MessageBox.Show("Вы стали полноценным участником этой игры!");

                    // string connection = "Server = localhost; Database = master; Encrypt=false; Trusted_Connection = True;";
                    using (SqlConnection sqlConnection = new SqlConnection(MainWindow.connection))
                    {
                        await sqlConnection.OpenAsync();

                        SqlCommand command = new SqlCommand
                        {
                            CommandText = "Use " + MainWindow.DataBase + "; insert into " + MainWindow.Table + "(Login,Email_address,Password) values('" + Login.Text + "','" + Email_address.Text + "','" + Password.Password.GetHashCode() + "')",
                            Connection = sqlConnection
                        };

                        await command.ExecuteNonQueryAsync();
                    }
                    this.Close(); 
                }

            }
            

        }

        public async void Send_Message(object sender, RoutedEventArgs e)
        {
            if(Email_address.Text == "")
            {
                MessageBox.Show("Пожалуйста правильно заполните поле с вашей почтой, чтобы мы могли прислать вам код идентификации!");
                return;
            }
            /* Methods.Identification_number(Convert.ToString(Email_address));*/
            Id_marker = rand.Next(1000,9999);

            MailAddress from = new MailAddress("tekirinka42@gmail.com", "TeKiRinKa Corp.");// адрес отправителя и его отображаемое имя
            
            MailAddress to = new MailAddress(Email_address.Text);

            MailMessage mail = new MailMessage(from, to)
            {
                Subject = "Код идентификации:",
                Body = "<h2>Пожалуйста введите ваш код в окне приложения.</h2>"
            };// Объект письма
            mail.Body += "<h3>Ваш идентификационный номер: </h3>";
            mail.Body += Convert.ToString(Id_marker);
            mail.IsBodyHtml = true;


            SmtpClient smtp_Client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("tekirinka42@gmail.com", "cwdwemmybjytsgij"),
                EnableSsl = true
            };

            MessageBox.Show("Письмо отправлено!");

            await smtp_Client.SendMailAsync(mail);
        }

        
    }

}
