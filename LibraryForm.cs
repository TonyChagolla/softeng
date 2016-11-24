using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        }
    }
}
