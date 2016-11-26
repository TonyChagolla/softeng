using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Library
{
    public partial class SelectForm : Form
    {
        public int client_id;
        public int book_id;
        public SelectForm()
        {
            InitializeComponent();
            book_id = -1;
            client_id = -1;
        }
        public SelectForm(int client_id, int book_id)
        {
            InitializeComponent();
            this.book_id = book_id;
            this.client_id = client_id;
            FillClient();
        }



        private void SelectForm_Load(object sender, EventArgs e)
        {
            lvMain.View = View.Details;
            lvMain.FullRowSelect = true;
        }

        private int FillBook()
        {
            lvMain.Clear();
            this.lvMain.Columns.Add("Libro", 200, HorizontalAlignment.Left);
            this.lvMain.Columns.Add("Autor", 200, HorizontalAlignment.Left);
            this.lvMain.Columns.Add("Paginas", 100, HorizontalAlignment.Left);
            this.lvMain.Columns.Add("Stock", 100, HorizontalAlignment.Left);

            string sqlcmd = "SELECT book_id, name, author, pages, stock FROM books";

            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            ListViewItem lvi = null;
            string[] colText = new string[4];
            int item_id;

            try
            {
                lvMain.Items.Clear();
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item_id = (int)reader["book_id"];
                    colText[0] = reader["name"].ToString();
                    colText[1] = reader["author"].ToString();
                    colText[2] = reader["pages"].ToString();
                    colText[3] = reader["stock"].ToString();
                    lvi = new ListViewItem(colText, 0);
                    lvi.Tag = item_id;
                    lvMain.Items.Add(lvi);
                }
                reader.Close();
            }
            catch (SqlException error)
            {
                MessageBox.Show(this, error.Message, "Error");
                this.Text = error.Message;
                return 1;
            }
            finally
            {
                connection.Close();
            }
            return 0;
        }


        private int FillClient()
        {

            lvMain.Clear();
            lvMain.Columns.Add("Nombre", 100, HorizontalAlignment.Left);
            lvMain.Columns.Add("Apellido", 100, HorizontalAlignment.Left);
            lvMain.Columns.Add("Direccion", 100, HorizontalAlignment.Left);

            string sqlcmd = "SELECT cliente_id, first_name, last_name, client_address FROM cliente";

            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            ListViewItem lvi = null;
            string[] colText = new string[3];
            int item_id;

            try
            {
                lvMain.Items.Clear();
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item_id = (int)reader["cliente_id"];
                    colText[0] = reader["first_name"].ToString();
                    colText[1] = reader["last_name"].ToString();
                    colText[2] = reader["client_address"].ToString();
                    lvi = new ListViewItem(colText, 0);
                    lvi.Tag = item_id;
                    lvMain.Items.Add(lvi);
                }
                reader.Close();
            }
            catch (SqlException error)
            {
                MessageBox.Show(this, error.Message, "Error");
                this.Text = error.Message;
                return 1;
            }
            finally
            {
                connection.Close();
            }
            return 0;
        }

        private void lvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tbxAccept_Click(object sender, EventArgs e)
        {
           
        }
    }
}
