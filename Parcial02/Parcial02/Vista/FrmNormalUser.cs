using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Parcial02.Modelo;

namespace Parcial02.Vista
{
    public partial class FrmNormalUser : Form
    {
        private string username;
        public FrmNormalUser(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void addAddress_button_Click(object sender, EventArgs e)
        {
            if (addressTxt.Text.Equals(""))
            {
                MessageBox.Show("Por favor llenar todos los campos");
            }
            else
            {
                try
                {
                    var dt = ConnectionDB.ExecuteQuery(
                        $"SELECT iduser FROM public.appuser WHERE username = '{username}'");
                    int id = int.Parse(dt.Rows[0][0].ToString());
                    ConnectionDB.ExecuteNonQuery("INSERT INTO public.address(iduser, address) VALUES(" +
                                                 $"{id}," +
                                                 $"'{addressTxt.Text}')");
                    MessageBox.Show("Se ha agregado la direccion!");
                    UpdateData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error!");
                }
            }
        }
        

        private void deleteAddress_button_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionDB.ExecuteNonQuery("DELETE FROM public.address WHERE" +
                                             $" address = '{addressCmb.GetItemText(addressCmb.SelectedItem)}'");

                MessageBox.Show("Se ha eliminado la direccion");
                UpdateData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!");
            }
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            //Combo box de direccion
            var dt2 = ConnectionDB.ExecuteQuery(
                $"SELECT iduser FROM public.appuser WHERE username = '{username}'");
            int id = int.Parse(dt2.Rows[0][0].ToString());
            var address = ConnectionDB.ExecuteQuery($"SELECT address FROM public.address WHERE iduser = {id}");
            var adressCombo = new List<string>();

            foreach (DataRow dr in address.Rows)
            {
                adressCombo.Add(dr[0].ToString());
            }

            addressCmb.DataSource = adressCombo;
            addressCmb2.DataSource = adressCombo;
            
            //Combo box de Producto
            var product = ConnectionDB.ExecuteQuery("SELECT name FROM public.product");
            var productCombo = new List<string>();

            foreach (DataRow dr in product.Rows)
            {
                productCombo.Add(dr[0].ToString());
            }
            
            productsCmb.DataSource = productCombo;
            
            //DataGridView
            var dt = ConnectionDB.ExecuteQuery($"SELECT * FROM public.address WHERE iduser = {id}");
            dataGridView1.DataSource = dt;
            
            var dt3 = ConnectionDB.ExecuteQuery($"SELECT ao.idOrder, ao.createDate, pr.name, au.fullname, ad.address " +
                                                $"FROM APPORDER ao, ADDRESS ad, PRODUCT pr, APPUSER au " +
                                                $"WHERE ao.idProduct = pr.idProduct " +
                                                $"AND ao.idAddress = ad.idAddress " +
                                                $"AND ad.idUser = au.idUser " +
                                                $"AND au.idUser = {id}");
            dataGridView2.DataSource = dt3;
        }

        private void FrmNormalUser_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void Order_button_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            String year = date.Year.ToString();
            String month = date.Month.ToString();
            String day = date.Day.ToString();
            var dt = ConnectionDB.ExecuteQuery(
                $"SELECT idaddress FROM public.address WHERE address = '{addressCmb2.GetItemText(addressCmb2.SelectedItem)}'");
            int idaddress = int.Parse(dt.Rows[0][0].ToString());
            dt = ConnectionDB.ExecuteQuery(
                $"SELECT idproduct FROM public.product " +
                $"WHERE name = '{productsCmb.GetItemText(productsCmb.SelectedItem)}'");
            int idproduct = int.Parse(dt.Rows[0][0].ToString());
            
            //Orden
            ConnectionDB.ExecuteNonQuery("INSERT INTO public.apporder(createdate, idproduct, idaddress) VALUES(" +
                                         $"'{day}-{month}-{year}'," +
                                         $"{idproduct},{idaddress})");
            MessageBox.Show("Se ha agregado la orden!");
            UpdateData();
            
            
        }
    }
}