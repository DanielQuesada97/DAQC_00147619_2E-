using System;
using System.Windows.Forms;
using Parcial02.Modelo;

namespace Parcial02.Vista
{
    public partial class FrmChangePassword : Form
    {
        public FrmChangePassword()
        {
            InitializeComponent();
        }

        private void passwordButton_Click(object sender, EventArgs e)
        {
            if (userTextBox.Text.Equals("") ||
                passwordTextBox.Text.Equals("") ||
                newPasswordTxt.Text.Equals(""))
            {
                MessageBox.Show("Por favor llenar todos los campos");
            }
            else
            {
                try
                {
                    ConnectionDB.ExecuteNonQuery($"UPDATE public.appuser SET password = '{newPasswordTxt.Text}'" +
                                                 $"WHERE username = '{userTextBox.Text}' AND " +
                                                 $"password ='{passwordTextBox.Text}'");
                    MessageBox.Show("Se ha cambiado la contraseña exitosamente!");
                    Hide();
                    new FrmLogin().Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error!");
                }
            }
        }
    }
}