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
        private int client_id;
        private int book_id;
        private int selectMode;
        private int employee_id;
        private int selectedIndex;
        public SelectForm(int selectMode)
        {
            InitializeComponent();
            client_id = -1;
            book_id = -1;
            this.selectMode = selectMode;
        }

        private void SelectForm_Load(object sender, EventArgs e)
        {
            lvMain.View = View.Details;
            lvMain.FullRowSelect = true;
            if (selectMode == 0)
            {
                FillClient();
            }
            else if (selectMode == 1)
            {
                FillBook();
            }
            else
            {
                this.Close();
            }
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
            if (lvMain.SelectedItems.Count > 0)
            {
                selectedIndex = (int)lvMain.SelectedItems[0].Tag;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (lvMain.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Select an element");
                return;
            }
            if (selectMode == 0)
            {
                client_id = selectedIndex;
            }
            else if (selectMode == 1)
            {
                    book_id = selectedIndex;
            }
            this.Close();
        }

        public int getClient()
        {
            return client_id;
        }

        public int getBook()
        {
            return book_id;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            client_id = -1;
            book_id = -1;
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedIndex <= 0)
            {
                MessageBox.Show("Seleccione un registro", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            delete(selectedIndex);

            selectedIndex = -1;

            if (selectMode == 0)
            {
                FillClient();
            }
            else if (selectMode == 1)
            {
                FillBook();
            }
            else
            {
                this.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddForm dlgAdd = new AddForm(selectMode);
            dlgAdd.ShowDialog();
            if (selectMode == 0)
            {
                FillClient();
            }
            else if (selectMode == 1)
            {
                FillBook();
            }
            else
            {
                this.Close();
            }

        }

        private void Testdelete()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../testDelete.txt", true))
            {
                int count = lvMain.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    this.lvMain.Items[i].Focused = true;
                    this.lvMain.Items[i].Selected = true;
                    selectedIndex = (int)lvMain.SelectedItems[i].Tag;
                    if (delete(selectedIndex) == 0)
                    {
                        file.WriteLine("Delete Success: " + i + "; Delete Id: " + selectedIndex);
                    }

                }
            }
        }

        private int delete(int selectedIndex)
        {
            
            string sqlcmd = null;
            if (selectMode == 0)
            {
                sqlcmd = "DELETE FROM cliente WHERE cliente_id = " + selectedIndex;
            }
            else if (selectMode == 1)
            {
                sqlcmd = "DELETE FROM books WHERE book_id = " + selectedIndex;
            }

            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;

            try
            {
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show("Error while deleting", "Error");
                    return 1;
                }

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
    }
}
