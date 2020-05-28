using System.Drawing;
using System.Windows.Forms;
using Parcial02.Modelo;
using System;
using Parcial02.Properties;

namespace Parcial02.Vista
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            passwordTextBox.PasswordChar = '*';
        }
        
        public void login()
        {
            try
            {
                string query =
                    $"SELECT usertype FROM public.appuser WHERE username='{userTextBox.Text}' AND password='{passwordTextBox.Text}'";

                var dt = ConnectionDB.ExecuteQuery(query);
                bool admin = Convert.ToBoolean(dt.Rows[0][0].ToString());

                if (dt.Rows.Count == 1)
                {
                    Hide();
                    if (admin)
                    {
                        new FrmAdmin().Show();
                    }
                    else
                    {
                        new FrmNormalUser(userTextBox.Text).Show();
                    }
                }
                else
                {
                    MessageBox.Show("Usuario y/o Contraseña Incorrecta");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            login();
        }

        private void passwordButton_Click(object sender, EventArgs e)
        {
            Hide();
            new FrmChangePassword().Show();
            
        }
    }
}