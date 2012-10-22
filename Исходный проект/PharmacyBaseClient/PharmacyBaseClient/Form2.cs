using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Collections;

namespace PharmacyBaseClient
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String connectString = @"Server=" + textBox1.Text + ";Database=" + textBox2.Text + ";uid=" + textBox3.Text + ";pwd=" + textBox4.Text;
            SqlConnection connection = new SqlConnection(connectString);
            try
            {
                connection.Open();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (connection.State == ConnectionState.Open)
            {
                DialogResult = DialogResult.OK;
                this.Text = connectString;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Введите параметры подключения");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                String connectString = @"Server=localhost\SQLEXPRESS;Database=PharmacyBase;uid=sa;Trusted_Connection=True";
                SqlConnection connection = new SqlConnection(connectString);
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                if (connection.State == ConnectionState.Open)
                {
                    DialogResult = DialogResult.OK;
                    this.Text = connectString;
                }
            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }
    }
}
