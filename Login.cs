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
                this.Close();
            }

            if (tbxUser.Text.Length <= 1) return;
            if (tbxPass.Text.Length <= 1) return;

            log(tbxUser.Text, tbxPass.Text);
            
        }
        private int log(string user, string password)
        {
            string sqlcmd;
            int count;
            sqlcmd = "SELECT count(*) AS contador FROM employee WHERE e_user = '" + user + "' AND e_password = '" + password + "'";
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
               
                reader.Close();
                if (count == 1)
                {
                    sqlcmd = "SELECT employee_id FROM employee WHERE e_user = '" + user + "' AND e_password = '" + password + "'";
                    cmd = new SqlCommand(sqlcmd, connection);
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    employee_id = (int)reader["employee_id"];
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("Nombre de usuario o contraseña invalido", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
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
            return 0;
        }
        private void Login_Load(object sender, EventArgs e)
        {
            //Testing Connection
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../testusers2.txt", true))
            {
                for (int i = 0; i <= 100; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        if (log("master" + j, "newpassword" + j) == 0)
                        {
                            file.WriteLine("Connection Success: " + i + "; user: "  + j);
                        }
                        else
                        {
                            file.WriteLine("Success; Password or User not found: " + i + "; user: " + j);
                        }
                    }
                }
            }

        }
    }
}
