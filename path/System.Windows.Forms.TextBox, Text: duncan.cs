private void button1_Click(object sender, EventArgs e)
        {
            //If the logPath exists, delete the file
            string logPath = "Output.Log";
            if (File.Exists(logPath)) {
                File.Delete(logPath);
            }

            string[] Servers = richTextBox1.Text.Split('\n');

            //Pass each server name from the listview to the 'Server' variable
            foreach (string Server in Servers) {
                //PowerShell Script
                string PSScript = @"
            param([Parameter(Mandatory = $true, ValueFromPipeline = $true)][string] $server)

            Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned -Force;
            Import-Module SQLServer;
            Try 
            {
                Set-Location SQLServer:\\SQL\\$server -ErrorAction Stop; 
                Get-ChildItem | Select-Object -ExpandProperty Name;
            } 
            Catch 
            {
                echo 'No SQL Server Instances'; 
            }
            ";
                using (PowerShell psInstance = PowerShell.Create()) {                               
                    psInstance.AddScript(PSScript);
                    psInstance.AddParameter("server", Server);
                    Collection<PSObject> results = psInstance.Invoke();
                    if (psInstance.Streams.Error.Count > 0) {
                        foreach (var errorRecord in psInstance.Streams.Error) {
                            MessageBox.Show(errorRecord.ToString());
                        }
                    }               
                    foreach (PSObject result in results) {
                        File.AppendAllText(logPath, result + Environment.NewLine);
                        // listBox1.Items.Add(result);
                    }               
                }

            }
        }


13/01/2017 11:31:51