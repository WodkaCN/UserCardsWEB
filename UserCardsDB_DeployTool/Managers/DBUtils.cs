using Microsoft.Data.SqlClient;

namespace UserCardsDB_DeployTool.Managers
{
    public static class DBUtils
    {
        public static bool DropDatabase(string localDB)
        {
            string dropCmd = "DROP DATABASE \"UserCards\"";

            try
            {
                using (var connection = new SqlConnection(localDB))
                {
                    using (var command = new SqlCommand(dropCmd, connection))
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
