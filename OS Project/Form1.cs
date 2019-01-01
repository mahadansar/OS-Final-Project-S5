using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_Project
{
    public partial class Form1 : Form
    {
        NewProcess p1;
        Form2 form;
        public Form1()
        {
            InitializeComponent();
            form = new Form2();
            form.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            boxID.Text = "1";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int error = 0;
            try
            {
                p1 = new NewProcess();
                p1.name = boxName.Text;
                p1.ID = Convert.ToInt32(boxID.Text);
                p1.time = Convert.ToInt32(boxTime.Text);
                p1.IO = Convert.ToInt32(boxIO.Text);

                form.AddProcess(p1);
            }
            catch (Exception)
            {
                MessageBox.Show("Fill All Fields!");
                error = 1;
            }

            if (error != 1)
            {
                boxName.Text = "";
                boxTime.Text = "";
                boxIO.Text = "";
 
                int tempID = Convert.ToInt32(boxID.Text);
                tempID = tempID + 1;
                boxID.Text = tempID.ToString();
            }
        }

        private void boxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            boxName.Text = "";
            boxTime.Text = "";
            boxIO.Text = "";
        }

        private void boxIO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void boxTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void boxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }
    }
}