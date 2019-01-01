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
    public partial class Form2 : Form
    {
        LinkedList<NewProcess> processesList;
        RoundRobin rrAlgo;
        public Form2()
        {
            InitializeComponent();
            processesList = new LinkedList<NewProcess>();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        public void AddProcess(NewProcess tempProcess)
        {
            //Create Strings of each process value and send to Data Grid View
            string ID = tempProcess.ID.ToString();
            string name = tempProcess.name;
            string time = tempProcess.time.ToString();
            string IO = tempProcess.IO.ToString();
            string[] tempProcessArray = { ID, name, time, IO, "Ready" };
            dataGridView.Rows.Add(tempProcessArray);

            //Add the process to processess list
            processesList.AddLast(tempProcess);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Hide();
            quantum_Box.Hide();
            label2.Hide();

            label1.Text = "Quantum Value: " + quantum_Box.Value;

            //Convert Processes List to Array
            NewProcess[] processesArray = processesList.ToArray();
            
            //Fetch quantum from Form Text Box
            int quantum = (int)quantum_Box.Value;

            //Bind and Pass Data Grid View to RoundRobin Class
            rrAlgo = new RoundRobin(ref dataGridView);

            //Pass Array and Quantum to Round Robin Main Method
            rrAlgo.runRoundRobin(ref processesArray, quantum);
        }
    }
}