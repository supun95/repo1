   public void member_bug_assign()
        {
            mySqlConnection.Open();
            String select_user = "SELECT * FROM users";
            SqlCommand SqlCommand = new SqlCommand(select_user, mySqlConnection);
            try
            {
                SqlDataReader Data_reader = SqlCommand.ExecuteReader();

                while (Data_reader.Read())
                {
                    assign_input.Items.Add(Data_reader["Hello"]);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            mySqlConnection.Close();
        }
06/01/2017 13:40:25