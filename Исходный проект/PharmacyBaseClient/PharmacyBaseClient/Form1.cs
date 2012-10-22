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
    public partial class Form1 : Form
    {
        Form2 loginForm = new Form2();
        public Form1()
        {
            InitializeComponent();
        }

        DataSet sendRequest(string request)//Запрос к базе данных
        {
            DataSet buffer = new DataSet();
            if (loginForm.Text[0] != 'П')
            {
                SqlConnection connection = new SqlConnection(loginForm.Text);
                try
                {
                    if (request.Length != 0)
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(request, connection);
                        SqlCommandBuilder command = new SqlCommandBuilder(adapter);
                        adapter.Fill(buffer);
                    }
                    else
                    {
                        MessageBox.Show("Нет запроса");
                        buffer = null;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Плохие параметры подключения");
                buffer = null;
            }
            return buffer;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            loginForm.ShowDialog();//Вызов настроек соединения
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loginForm.ShowDialog();//Вызов настроек соединения
        }
        private void button3_Click(object sender, EventArgs e)
        {
            loginForm.ShowDialog();//Вызов настроек соединения
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //Запрос столбцов из таблицы по списку
            List<string> tablesList = new List<string>();
            List<string> columsList = new List<string>();
            List<string> conditionList = new List<string>();
            string colums = "";
            string tables = "";
            string condition = "";
            for (int i = 0; i < dataGridView3.Rows.Count; i++) 
            {
                // Разбор списка запрашиваемых столбцов и таблиц
                if (dataGridView3.Rows[i].Cells[0].Value != null)
                {
                    if (!tablesList.Contains((dataGridView3.Rows[i].Cells[0].Value.ToString().Split('.'))[0]))
                    {
                        tablesList.Add((dataGridView3.Rows[i].Cells[0].Value.ToString().Split('.'))[0]);
                    }
                    if (!columsList.Contains(dataGridView3.Rows[i].Cells[0].Value.ToString()))
                    {
                        columsList.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
                    }
                    if (dataGridView3.Rows[i].Cells[1].Value != null && dataGridView3.Rows[i].Cells[2].Value != null)
                    {
                        if (dataGridView3.Rows[i].Cells[1].Value.ToString() == "Найти")
                        {
                            conditionList.Add(dataGridView3.Rows[i].Cells[0].Value.ToString() + " LIKE '%" + dataGridView3.Rows[i].Cells[2].Value.ToString() + "%'");
                        }
                        else
                        {
                            int a;
                            if (int.TryParse(dataGridView3.Rows[i].Cells[2].Value.ToString(), out a))
                            {
                                conditionList.Add(dataGridView3.Rows[i].Cells[0].Value.ToString() + dataGridView3.Rows[i].Cells[1].Value.ToString() + dataGridView3.Rows[i].Cells[2].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("Значение не число");
                            }
                        }
                    }
                }
            }
            //Формирование требуемые условий для таблиц
            if (columsList.Contains("pharmgroups.name_of_group"))
            {
                if (!tablesList.Contains("medicaments"))
                {
                    tablesList.Add("medicaments");
                }
                conditionList.Add("medicaments.id_group = pharmgroups.id_group");
            }
            if (columsList.Contains("pharmkinds.name_of_kind"))
            {
                if (!tablesList.Contains("medicaments"))
                {
                    tablesList.Add("medicaments");
                }
                conditionList.Add("medicaments.id_kind = pharmkinds.id_kind");
            }
            if (tablesList.Contains("medicaments") && tablesList.Contains("pharmacies"))
            {
                tablesList.Add("indexes");
                conditionList.Add("medicaments.id_medicament=indexes.id_medicament AND indexes.id_pharmacy = pharmacies.id_pharmacy");
            }
            //Формирование текста запроса
            if (columsList.Count != 0)
            {
                colums = "SELECT";
                tables = " FROM";
                for (int i = 0; i < columsList.Count; i++)
                {
                    colums = colums + " " + columsList[i];
                    if (i != (columsList.Count - 1))
                    {
                        colums = colums + ", ";
                    }
                }
                for (int i = 0; i < tablesList.Count; i++)
                {
                    tables = tables + " " + tablesList[i];
                    if (i != (tablesList.Count - 1))
                    {
                        tables = tables + ", ";
                    }
                }
            }
            if (conditionList.Count != 0)
            {
                condition = " WHERE ";
                for (int i = 0; i < conditionList.Count; i++)
                {
                    condition = condition + conditionList[i];
                    if (i != (conditionList.Count - 1))
                    {
                        condition = condition + " AND ";
                    }
                }
            }
            string request = colums + tables + condition;
            //Вывод таблицы главное окно
            DataSet buffer = sendRequest(request);
            if (buffer != null)
            {
                dataGridView1.DataSource = buffer.Tables[0];
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            //Добавление столбца для запроса на отображение
            dataGridView3.Rows.Add(listBox1.SelectedItem);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            //Удаление столбца из запроса на отображение
            if (dataGridView3.CurrentCell != null)
            {
                dataGridView3.Rows.RemoveAt(dataGridView3.CurrentCell.RowIndex);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //Сохранение таблицы в файл
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (dataGridView1.DataSource != null)
                {
                    ((DataTable)dataGridView1.DataSource).WriteXml(saveFileDialog1.FileName);
                }
                else
                {
                    MessageBox.Show("Нет данных для сохранения");
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //Загрузка таблицы из файла
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataSet buffer = new DataSet();
                try
                {
                    buffer.ReadXml(openFileDialog1.FileName);
                    dataGridView1.DataSource = buffer.Tables[0];
                }
                catch
                {
                    MessageBox.Show("Ошибка чтения");
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //Запрос вывода редактируемой таблицы
            if (comboBox1.SelectedItem != null)
            {
                button6.Enabled = true;
                DataSet buffer = new DataSet();
                buffer = sendRequest("SELECT * FROM " + comboBox1.SelectedItem.ToString());
                dataGridView2.DataSource = buffer.Tables[0];
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //Сохранение изменений
            DataTable table = (DataTable)dataGridView2.DataSource;
            DataTable delta = table.GetChanges();
            if (delta!=null)
            {
                SqlConnection connection = new SqlConnection(loginForm.Text);
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM " + comboBox1.SelectedItem.ToString(), connection);
                SqlCommandBuilder command = new SqlCommandBuilder(adapter);
                adapter.Update(delta);
                DataSet buffer = new DataSet();
                //Обновление отображения таблицы
                buffer = sendRequest("SELECT * FROM " + comboBox1.SelectedItem.ToString());
                dataGridView2.DataSource = buffer.Tables[0];
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Блокировка изменения при изменении редактируемой таблицы
            button6.Enabled = false;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            //Изменение размеров отображений таблиц
            dataGridView1.Height = this.Size.Height - 286;
            dataGridView2.Width = this.Size.Width - 130;
        }
    }
}
