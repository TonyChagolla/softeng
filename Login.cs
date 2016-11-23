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
    public partial class Login : Form
    {
        public int tryCount;
        public int employee_id;
        public Login()
        {
            InitializeComponent();
            tryCount = 0;
            employee_id = -1;
        }


        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (tryCount >= 3)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }

            if (tbxUser.Text.Length <= 1) return;
            if (tbxPass.Text.Length <= 1) return;

            string sqlcmd;
            int count;
            sqlcmd = "SELECT count(*) AS contador, employee_id FROM employee WHERE e_user = '" + tbxUser.Text + "' AND e_password = '" + tbxPass.Text + "' GROUP BY employee_id";
            SqlConnection connection = new SqlConnection(SqlConnect.SqlString());
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                cmd = new SqlCommand(sqlcmd, connection);
                reader = cmd.ExecuteReader();
                reader.Read();
                count = (int)reader["contador"];
                employee_id = (int)reader["employee_id"];
                reader.Close();
                if (count == 1)
                {
                    MessageBox.Show("Connection Success");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nombre de usuario o contraseña invalido", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                tryCount++;


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
    }
}
