using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyManager
{
    public partial class Form1 : Form
    {
        List<Transaction> transacs = new List<Transaction>();
        Bank bank = new Bank();
        Sql sql = new Sql("Server=192.168.30.167;Database=master;User ID=Adddriga;Password=123456;Encrypt=false;MultipleActiveResultSets=true", "Server=192.168.30.167;Database=transacsdb;User ID=Adddriga;Password=123456;Encrypt=false;MultipleActiveResultSets=true");
        public Form1()
        {
            InitializeComponent();
        }

        public async void updateButtons()
        {
            try
            {
                int minidingrid = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value);
                int maxidingrid = Convert.ToInt32(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value);
                if (minidingrid != await sql.getMinId())
                {
                    button2.Visible = true;
                }
                else
                {
                    button2.Visible = false;
                }
                if (maxidingrid == await sql.getMaxId())
                {
                    button3.Visible = false;
                }
                else
                {
                    button3.Visible = true;
                }
            }
            catch (Exception ex) 
            {
                button2.Visible = false;
                button3.Visible = false;
                Console.WriteLine(ex.Message);
            }
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            button3.Visible = false;
            //await sql.Insert("Income", "name2", "description", 100, DateTime.Now, 0, 100, 0);
            //Console.WriteLine(sql.CheckSqlConnection());
            sql.CreateDB(richTextBox2);
            sql.CreateTable(richTextBox2);
            sql.CreateInsertProcedure(richTextBox2);
            sql.CreateGetProcedure(richTextBox2);
            //bank.CreateManyTransactions(200);
            transacs = await sql.GetTenTransacs(await sql.getMaxId(), true);
            double money = await sql.getLatestMoneyValue();
            Console.WriteLine(money);
            //money = Math.Round(money, 2);
            bank.setMoney(money);
            label1.Text = bank.getMoney().ToString();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Type", "Type");
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("Amount", "Amount");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("Date", "Date");
            for (int i = 0; i < transacs.Count; i++)
            {
                dataGridView1.Rows.Add(transacs[i].getId(), transacs[i].getType(), transacs[i].getName(), transacs[i].getAmount(), transacs[i].getDescription(), transacs[i].getDate().ToString());
            }
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            updateButtons();
        }
        
        private void fillDataGrid(List<Transaction> transacs)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < transacs.Count; i++)
            {
                dataGridView1.Rows.Add(transacs[i].getId(), transacs[i].getType(), transacs[i].getName(), transacs[i].getAmount(), transacs[i].getDescription(), transacs[i].getDate().ToString());
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == -1) 
            {
                errorProvider1.SetError(comboBox1, "Type is NULL");
            }
            else
            {
                errorProvider1.Clear();
            }
            if(textBox1 == null) 
            {
                errorProvider1.SetError(textBox1, "Name is NULL");
            }
            else
            {
                errorProvider1.Clear();
            }
            if(textBox2 == null)
            {
                errorProvider1.SetError(textBox2, "Amount is NULL");
            }
            else
            {
                double tmp;
                if(double.TryParse(textBox2.Text, out tmp))
                {
                    errorProvider1.Clear();
                }
                else
                {
                    errorProvider1.SetError(textBox2, "Amount isn't DOUBLE");
                }
            }
            if(richTextBox1 == null)
            {

            }
            else
            {

            }
            if(dateTimePicker1 == null)
            {
                errorProvider1.SetError(dateTimePicker1, "Date is NULL");
            }
            else
            {
                errorProvider1.Clear();
            }
            try
            {
                //await sql.Insert("Income", "name2", "description", 100, DateTime.Now, 0, 100, 0);
                Transaction transtmp = await bank.HandleNewTransaction(comboBox1.Text, textBox1.Text, richTextBox1.Text, double.Parse(textBox2.Text), dateTimePicker1.Value);
                int id = transtmp.getId();
                transacs = await sql.GetTenTransacs(id);
                fillDataGrid(transacs);
                updateButtons();
                //transacs.Add(transtmp);
                label1.Text = bank.getMoney().ToString();
                
            }
            catch(ArgumentException ex)
            {
                richTextBox2.Text += ex.Message + "\n";
            }
        }
        
        private async void button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value);
            transacs = await sql.GetTenTransacs(id);
            if (transacs.Count < 10)
            {
                id = transacs[transacs.Count - 1].getId();
                while (true)
                {
                    if(id != await sql.getMaxId())
                    {
                        id += 1;
                    }
                    Console.WriteLine("!!!!!!ID: " + id);
                    transacs = await sql.GetTenTransacs(id);
                    if (transacs.Count >= 10)
                    {
                        break;
                    }
                }
            }
            fillDataGrid(transacs);
            updateButtons();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value);
            Console.WriteLine("!!!!!!ID: " + id);
            transacs = await sql.GetTenTransacs(id, false);
            if(transacs.Count < 10)
            {
                id = transacs[0].getId();
                while(true)
                {
                    if(id != await sql.getMinId())
                    {
                        id -= 1;
                    }
                    Console.WriteLine("!!!!!!ID: " + id);
                    transacs = await sql.GetTenTransacs(id, false);
                    if (transacs.Count >= 10)
                    {
                        break;
                    }
                }
            }
            fillDataGrid(transacs);
            updateButtons();
        }

        private async void button4_Click(object sender, EventArgs e)
        { 
            List<int> ids = new List<int>();
            List<string> type = new List<string>();
            for(int i = 0; i < dataGridView1.SelectedRows.Count; i++) 
            {
                ids.Add(Convert.ToInt32(dataGridView1.SelectedRows[i].Cells[0].Value));
                type.Add(dataGridView1.SelectedRows[i].Cells[1].Value.ToString());
                Console.WriteLine(ids[i] + " " + type[i]);
            }
            DialogResult dr = MessageBox.Show("Are you sure?", "Delete Dialog", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes) 
            {
                for(int i = 0; i < ids.Count; i++)
                {
                    if (type[i] == "Income")
                    {
                        bool flag = await sql.UpdateActive(ids[i], "Income", 0);
                    }
                    else if (type[i] == "Spending")
                    {
                        bool flag = await sql.UpdateActive(ids[i], "Spending", 0);
                    }
                    //bool flag = await sql.UpdateActive(ids[i], 0);
                    double money = await sql.getLatestMoneyValue();
                    bank.setMoney(money);
                    label1.Text = bank.getMoney().ToString();
                }
            }
            else if(dr == DialogResult.No) 
            {

            }
        }
    }
}
