using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Erika
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "admin")
            {
                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
            else if (textBox1.Text != "admin" && textBox2.Text == "admin")
            {
                MessageBox.Show("Username Incorrect!");
            }
            else if (textBox1.Text == "admin" && textBox2.Text != "admin")
            {
                MessageBox.Show("Password Incorrect!");
            }
            else {
                MessageBox.Show("Both Username and Password Incorrect!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}
