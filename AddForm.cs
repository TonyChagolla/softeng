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
    public partial class AddForm : Form
    {
        private int cliente_id;
        private int book_id;
        private int mode;
        public AddForm(int mode)
        {
            InitializeComponent();
            cliente_id = -1;
            book_id = -1;
            this.mode = mode;
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            if(mode == 0)
            {
                lbl1.Text = "Nombre";
                lbl2.Text = "Apellido";
                lbl3.Text = "Direccion";
                lbl4.Hide();
                tbx4.Hide();

            }
            else if(mode == 1)
            {
                lbl1.Text = "Nombre";
                lbl2.Text = "Autor";
                lbl3.Text = "Paginas";
                lbl4.Text = "Stock";
            }
            else
            {
                this.Close();
            }

            test();
        }

        private void bntAccept_Click(object sender, EventArgs e)
        {
            DateTime Hoy = DateTime.Now;
            if(mode == 0)
                add(tbx1.Text, tbx2.Text, tbx3.Text, Hoy.ToString());
            else
                add(tbx1.Text, tbx2.Text, tbx3.Text, tbx4.Text);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int add(string value1, string value2, string value3, string value4)
        {
            
            string sqlcmd = null;
            if (mode == 0)
            {
                sqlcmd = "INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('" + value1 + "', '" + value2 + "', '" + value3 + "','" + value4 + "')";
            }
            else if (mode == 1)
            {
                sqlcmd = "INSERT INTO books VALUES('" + tbx1.Text + "','" + tbx2.Text + "','" + tbx3.Text + "','" + tbx4.Text + "')";
            }


            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;

            try
            {
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show("Error while inserting book", "Error");
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

        private void test()
        {
            DateTime hoy = DateTime.Now;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../testAddClient.txt", true))
            {
                for (int i = 0; i <= 100; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        if (add("Nombre" + j, "Apellido" + j, "Direccion" + j, hoy.ToString()) == 0)
                        {
                            file.WriteLine("Connection Success: " + i + "; cliente: " + j);
                        }
                    }
                }
            }
        }
    }
}
