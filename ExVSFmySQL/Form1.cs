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

namespace Jdforsythe.MySQLConnection
{
    public partial class Form1 : Form
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.myarttable". При необходимости она может быть перемещена или удалена.
            this.myarttableTableAdapter.Fill(this.testDataSet.myarttable);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(textBox1.Text, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
                label1.Text = "Count: " + (this.Count()).ToString();

                // Refresh Grid.
                this.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM myarttable ORDER BY id DESC;", connection);
                DataTable dt = new DataTable();
                dt.Load(cmd1.ExecuteReader());
                this.CloseConnection();
                dataGridView1.DataSource = dt.DefaultView;

                MessageBox.Show("Query succeeded.");
            }
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "test";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM myarttable";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "CREATE DATABASE IF NOT EXISTS test;USE test;CREATE TABLE IF NOT EXISTS myArtTable (id int(11) NOT NULL auto_increment, text text NOT NULL, description text NOT NULL, keywords text NOT NULL, PRIMARY KEY (id)) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=cp1251;CREATE TABLE IF NOT EXISTS files (id_file int(11) NOT NULL auto_increment, id_my int(11) NOT NULL, description text NOT NULL, name_origin text NOT NULL, path text NOT NULL, date_upload text NOT NULL, PRIMARY KEY (id_file), FOREIGN KEY (id_my) REFERENCES myarttable(id)) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=cp1251;INSERT INTO myarttable (text, description, keywords) values ('at1', 'at2', 'at3');";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "DROP TABLE IF EXISTS files;DROP TABLE IF EXISTS myarttable;DROP DATABASE IF EXISTS test;";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "UPDATE myarttable SET keywords = 'Ivanov' WHERE id = 18;";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "INSERT INTO myarttable (text, description, keywords) values ('at1', 'at2', 'at3');";
        }
    }
}
