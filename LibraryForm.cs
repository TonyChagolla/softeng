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
    public partial class LibraryForm : Form
    {
        public int employee_id;
        public int book_id;
        public int client_id;

        public LibraryForm()
        {
            InitializeComponent();
        }

        public LibraryForm(int employee_id)
        {
            InitializeComponent();
            this.employee_id = employee_id;
            book_id = -1;
            client_id = -1;
        }

        private void LibraryForm_Load(object sender, EventArgs e)
        {
            Text = "Library";
            lvBooks.View = View.Details;
            lvBooks.FullRowSelect = true;
            FillBooks();
        }

        

        public int FillBooks()
        {
            lvBooks.Clear();
            this.lvBooks.Columns.Add("Cliente", 200, HorizontalAlignment.Left);
            this.lvBooks.Columns.Add("Libro", 180, HorizontalAlignment.Left);
            this.lvBooks.Columns.Add("Fehca Salida", 120, HorizontalAlignment.Left);
            this.lvBooks.Columns.Add("FEcha Retorno", 120, HorizontalAlignment.Left);

            string sqlcmd = "SELECT * FROM client_books";

            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            ListViewItem lvi = null;
            string[] colText = new string[4];
            int item_id;

            try
            {
                lvBooks.Items.Clear();
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item_id = (int)reader["b_id"];
                    colText[0] = reader["c_name"].ToString();
                    colText[1] = reader["b_name"].ToString();
                    colText[2] = reader["b_date"].ToString();
                    colText[3] = reader["r_Date"].ToString();
                    lvi = new ListViewItem(colText, 0);
                    lvi.Tag = item_id;
                    lvBooks.Items.Add(lvi);
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

        private void lvClients_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSelectClient_Click(object sender, EventArgs e)
        {
            SelectForm dlgClient = new SelectForm(0, 0);
            dlgClient.ShowDialog();
        }

        private void LibraryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
