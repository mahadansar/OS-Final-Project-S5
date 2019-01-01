using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_Project
{
    public class RoundRobin
    {
        DataGridView dataGridView;

        //----------------RoundRobin Class Constructor-------------------
        public RoundRobin(ref DataGridView temp_dataGridView)
        {
            dataGridView = temp_dataGridView;
        }

        //----------------Main Round Robin Algorithm Method-------------------
        public void runRoundRobin(ref NewProcess[] multiNewProcesses, int quantum)
        {
            //Assign Each Process Its Execution Time
            foreach (var NewProcess in multiNewProcesses)
            {
                //RemaingTime = Total Time @ when process starts execution
                NewProcess.remainingTime = NewProcess.time;
            }
            while (true)
            {
                //Close loop on default, if the value is not changed due to available processes executions
                bool executionFinished = true;
                
                //Loop through all processes until the loop ends
                foreach (var NewProcess in multiNewProcesses)
                {
                    if (NewProcess.remainingTime == 0)
                    {
                        NewProcess.status = "Completed";
                        updateDataGridView(dataGridView, multiNewProcesses);
                    }
                    //Check if the process has any burst time left
                    else if (NewProcess.remainingTime > 0)
                    {
                        //Continue the loop, as a process is executing now and we need to recheck for others
                        executionFinished = false;

                        //Check if the process remaining time is greater than quantum
                        if (NewProcess.remainingTime > quantum)
                        {
                            //Process Status to Running as its Under Execution
                            NewProcess.status = "Running";
                            updateDataGridView(dataGridView, multiNewProcesses);
                            executionTimer(quantum);

                            //Remove the quantum time from the remaining time
                            NewProcess.remainingTime = NewProcess.remainingTime - quantum;

                            //Swap Process to Ready State after execution and continue for next
                            NewProcess.status = "Ready";
                            updateDataGridView(dataGridView, multiNewProcesses);
                        }
                        //Only runs when the process is on its last cpu burst cycle
                        else
                        {
                            //Check if the process has an IO left before it finishes its last cpu burst cycle
                            while (NewProcess.IO > 0)
                            {
                                ioExecution(multiNewProcesses, NewProcess.ID, NewProcess.IO);
                                NewProcess.IO = NewProcess.IO - 1;
                            }

                            //Process Status to Running as its Under Execution
                            NewProcess.status = "Running";
                            updateDataGridView(dataGridView, multiNewProcesses);
                            executionTimer(NewProcess.remainingTime);

                            //Set remaining time to 0, as the last cpu burst ended
                            NewProcess.remainingTime = 0;

                            //Change Process Status to Completed
                            NewProcess.status = "Completed";
                            updateDataGridView(dataGridView, multiNewProcesses);
                        }  
                    }
                    //Execute Single IO after every CPU burst cycle
                    if (NewProcess.IO > 0)
                    {
                        ioExecution(multiNewProcesses, NewProcess.ID, NewProcess.IO);
                        NewProcess.IO = NewProcess.IO - 1;
                    }
                }

                //When All Processes have completed their execution
                if (executionFinished == true)
                {
                    break;
                }
            }
        }

        //----------------Update Data Grid View Method-------------------------------
        public void updateDataGridView(DataGridView dataGridView, NewProcess[] multiNewProcesses)
        {
            //Remove Current Data Grid Rows and Refresh it
            dataGridView.Rows.Clear();
            dataGridView.Refresh();

            //Add Processes rows again to data grid view with updated values
            foreach (var NewProcess in multiNewProcesses)
            {
                string[] row = { NewProcess.ID.ToString(), NewProcess.name, NewProcess.remainingTime.ToString(), NewProcess.IO.ToString(), NewProcess.status };
                dataGridView.Rows.Add(row);
            }
        }

        //----------------Process IO Execution Method
        public void ioExecution(NewProcess[] multiNewProcesses, int id, int interupt)
        {
            //Change Process State to Waiting when it goes to IO
            foreach (var NewProcess in multiNewProcesses)
            {
                if (NewProcess.ID == id && NewProcess.status != "Completed")
                {
                    NewProcess.status = "Waiting";
                }
            }
            updateDataGridView(dataGridView, multiNewProcesses);

            //Execute the IO for 1 second
            executionTimer(1);

            //Change Process back to Ready State after IO has completed
            foreach (var NewProcess in multiNewProcesses)
            {
                if (NewProcess.ID == id && NewProcess.status!="Completed")
                {
                    NewProcess.status = "Ready";
                }
            }
            updateDataGridView(dataGridView, multiNewProcesses);
        }

        //----------------Process Execution Timer Method
        public void executionTimer(int tempTime)
        {
            int executionTime = tempTime * 1000;
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            if (executionTime == 0 || executionTime < 0)
            {
                return;
            }
            timer1.Interval = executionTime;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };
            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
    }
}