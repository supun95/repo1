private void button1_Click(object sender, EventArgs e)
{
    //If the logPath exists, delete the file
    string logPath = "Output.Log";
    if (File.Exists(logPath))
    {
        File.Delete(logPath);
    }
    string[] Servers = richTextBox1.Text.Split('\n');

    //Pass each server name from the listview to the 'Server' variable
    foreach (string Server in Servers)
    {
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

        //Create PowerShell Instance
        PowerShell psInstance = PowerShell.Create();

        //Add PowerShell Script
        psInstance.AddScript(PSScript);

        //Pass the Server variable in to the $server parameter within the PS script
        psInstance.AddParameter("server", Server);

        //Execute Script
        Collection<PSObject> results = new Collection<PSObject>();
        try
        {
            results = psInstance.Invoke();
        }
        catch (Exception ex)
        {
            results.Add(new PSObject((Object)ex.Message));
        }

        //Loop through each of the results in the PowerShell window
        foreach (PSObject result in results)
        {
           File.AppendAllText(logPath, result + Environment.NewLine);
           // listBox1.Items.Add(result);
        }
        psInstance.Dispose();
    }
}

