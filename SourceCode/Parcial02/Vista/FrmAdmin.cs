using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Parcial02.Modelo;

namespace Parcial02.Vista
{
    public partial class FrmAdmin : Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
            
        }

        private void addUser_button_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Equals("") ||
                usernameTextBox.Text.Equals(""))
            {
                MessageBox.Show("Por favor llenar todos los campos");
            }
            else
            {
                try
                {
                    var qr = ConnectionDB.ExecuteQuery(
                        $"SELECT username FROM public.appuser WHERE username = '{usernameTextBox.Text}'");
                    if (checkBox1.Checked && qr != null)
                    {
                        ConnectionDB.ExecuteNonQuery("INSERT INTO public.appuser(fullname, username, password, userType) VALUES(" +
                                                     $"'{nameTextBox.Text}'," +
                                                     $"'{usernameTextBox.Text}'," + 
                                                     $"'{usernameTextBox.Text}'," +
                                                     $"{true})");
                        MessageBox.Show($"Se ha creado el usuario {usernameTextBox.Text}!");
                        Update();
                    }
                    else if(qr != null)
                    {
                        ConnectionDB.ExecuteNonQuery("INSERT INTO public.appuser(fullname, username, password, userType) VALUES(" +
                                                     $"'{nameTextBox.Text}'," +
                                                     $"'{usernameTextBox.Text}'," + 
                                                     $"'{usernameTextBox.Text}'," +
                                                     $"{false})");
                        MessageBox.Show($"Se ha creado el usuario {usernameTextBox.Text}!");
                        Update();
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error! El usuario ya exite!");
                }
            }
        }
        private void deleteUser_button_Click(object sender, EventArgs e) 
        {
            if (usernameTextBox2.Text.Equals(""))
            {
                MessageBox.Show("Llene todos los campos");
            }
            else
            {
                try
                {
                    ConnectionDB.ExecuteNonQuery("DELETE FROM public.appuser WHERE" +
                                                 $" username = '{usernameTextBox2.Text}'");

                    MessageBox.Show("Se ha eliminado el usuario");
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error!");
                }
            }
        }
        

        private void addBussines_button_Click(object sender, EventArgs e)
        {
            if (bussinessNameTxt.Text.Equals("") ||
                descriptionTxt.Text.Equals(""))
            {
                MessageBox.Show("Por favor llenar todos los campos");
            }
            else
            {
                try
                {
                    ConnectionDB.ExecuteNonQuery($"INSERT INTO public.business(name, description) VALUES(" +
                                                     $"'{bussinessNameTxt.Text}'," +
                                                     $"'{descriptionTxt.Text}')");
                    MessageBox.Show($"Se ha creado el negocio {bussinessNameTxt.Text}!");
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error!");
                }
            }
        }

        private void deleteBussiness_button_Click(object sender, EventArgs e)
        {
            if (bussinesNameTxt2.Text.Equals(""))
            {
                MessageBox.Show("Llene todos los campos");
            }
            else
            {
                try
                {
                    ConnectionDB.ExecuteNonQuery("DELETE FROM public.business WHERE" +
                                                 $" name = '{bussinesNameTxt2.Text}'");

                    MessageBox.Show("Se ha eliminado el Negocio");
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error!");
                }
            }
        }

        private void addProduct_button_Click(object sender, EventArgs e)
        {
            if (productNameTxt.Text.Equals(""))
            {
                MessageBox.Show("Por favor llenar todos los campos");
            }
            else
            {
                try
                {
                    var dt = ConnectionDB.ExecuteQuery("SELECT idbusiness FROM public.business WHERE " +
                                                       $"name = '{businessCmb.GetItemText(businessCmb.SelectedItem)}'");
                    int bId = int.Parse(dt.Rows[0][0].ToString());
                    ConnectionDB.ExecuteNonQuery("INSERT INTO public.product(idbusiness, name) VALUES(" +
                                                 $"{bId}," + 
                                                 $"'{productNameTxt.Text}')");
                    MessageBox.Show($"Se ha añadido el producto");
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error!");
                }
            }
        }

        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            Update();
        }

        private void deleteProduct_button_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionDB.ExecuteNonQuery("DELETE FROM public.product WHERE" +
                                             $" name = '{productCmb.GetItemText(productCmb.SelectedItem)}'");

                MessageBox.Show("Se ha eliminado el producto");
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!");
            }
            
        }

        private void Update()
        {
            //Combo box de negocio
            var business = ConnectionDB.ExecuteQuery("SELECT name FROM public.business");
            var businessCombo = new List<string>();

            foreach (DataRow dr in business.Rows)
            {
                businessCombo.Add(dr[0].ToString());
            }
            
            businessCmb.DataSource = businessCombo;
            
            //Combo box de Producto
            var product = ConnectionDB.ExecuteQuery("SELECT name FROM public.product");
            var productCombo = new List<string>();

            foreach (DataRow dr in product.Rows)
            {
                productCombo.Add(dr[0].ToString());
            }
            
            
            //DataGridView
            productCmb.DataSource = productCombo;
            var dt = ConnectionDB.ExecuteQuery($"SELECT ao.idOrder, ao.createDate, pr.name, au.fullname, ad.address " +
                                               $"FROM APPORDER ao, ADDRESS ad, PRODUCT pr, APPUSER au " +
                                               $"WHERE ao.idProduct = pr.idProduct " +
                                               $"AND ao.idAddress = ad.idAddress " +
                                               $"AND ad.idUser = au.idUser");
            dataGridView2.DataSource = dt;
            
            var dt2 = ConnectionDB.ExecuteQuery("SELECT * FROM public.appuser");
            dataGridView1.DataSource = dt2;
        }
    }
}