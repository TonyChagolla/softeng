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
        public int selectedIndex;

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
            selectedIndex = -1;
        }

        private void LibraryForm_Load(object sender, EventArgs e)
        {
            Text = "Library";
            lvBooks.View = View.Details;
            lvBooks.FullRowSelect = true;
            FillBooks(book_id);
        }

        

        public int FillBooks(int id_client)
        {
            lvBooks.Clear();
            this.lvBooks.Columns.Add("Cliente", 200, HorizontalAlignment.Left);
            this.lvBooks.Columns.Add("Libro", 180, HorizontalAlignment.Left);
            this.lvBooks.Columns.Add("Fehca Salida", 120, HorizontalAlignment.Left);
            this.lvBooks.Columns.Add("FEcha Retorno", 120, HorizontalAlignment.Left);
            string sqlcmd;
            if(id_client == -1)
            {
                sqlcmd = "SELECT * FROM client_books";
            }
            else
            {
                sqlcmd = "SELECT * FROM client_books WHERE c_id = '" + id_client + "'" ;
            }
            

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
            SelectForm dlgClient = new SelectForm(0);
            dlgClient.ShowDialog();
            if(dlgClient.getClient() != -1)
            {
                this.client_id = dlgClient.getClient();
                MessageBox.Show("" + client_id);
                getUserName();
            }
            FillBooks(this.client_id);
        }

        private void LibraryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnSelecBook_Click(object sender, EventArgs e)
        {
            if (client_id == -1)
            {
                MessageBox.Show("You need to select a client first.", "Library", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SelectForm dlgBook = new SelectForm(1);
            dlgBook.ShowDialog();
           if(dlgBook.getBook() != -1)
            {
                this.book_id = dlgBook.getBook();
                borrowBook();
                MessageBox.Show("" + book_id);
            }
            FillBooks(client_id);
        }

        private void borrowBook()
        {
            DateTime Hoy = DateTime.Now;
            string sqlcmd = "INSERT INTO book_borrowed (book_id, cliente_id, employee_id, date_b) VALUES (" + book_id + "," + client_id + "," + employee_id + ",'" + Hoy.ToString() + "')";
            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;

            try
            {
                lvBooks.Items.Clear();
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                if(cmd.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show("Error while inserting book","Error");
                }
                
            }
            catch (SqlException error)
            {
                MessageBox.Show(this, error.Message, "Error");
                this.Text = error.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        private void getUserName()
        {
            string sqlcmd;
            sqlcmd = "SELECT * FROM vw_client WHERE cliente_id = " + client_id;
            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                reader = cmd.ExecuteReader();
                reader.Read();
                Text = reader["name"].ToString();
                reader.Close();
            }
            catch (SqlException error)
            {
                MessageBox.Show(this, error.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }

        private void lvBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBooks.SelectedItems.Count > 0)
            {
                selectedIndex = (int)lvBooks.SelectedItems[0].Tag;
            }
            else { selectedIndex = -1; }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(selectedIndex <= 0)
            {
                MessageBox.Show("Seleccione un libro", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string sqlcmd = "DELETE FROM book_borrowed WHERE borrow_id = " + selectedIndex;
            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;

            try
            {
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show("Error while deleting book", "Error");
                }

            }
            catch (SqlException error)
            {
                MessageBox.Show(this, error.Message, "Error");
                this.Text = error.Message;
            }
            finally
            {
                connection.Close();
            }
            lvBooks.Items.Clear();
            selectedIndex = -1;
            FillBooks(client_id);

        }
    }
}
