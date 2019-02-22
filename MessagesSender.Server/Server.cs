using System;
using System.IO.Pipes;
using System.Threading;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace MessagesSender.Server
{
    class Server
    {
        private NamedPipeServerStream ServerStream;
        private MainWindow Form;
        public void Start(object form)
        {
            ServerStream = new NamedPipeServerStream("messages", PipeDirection.InOut, 10);
            ServerStream.WaitForConnection();
            Form = (MainWindow)form;
            Form.Status = "Клиент подключился";
            var thread = new Thread(ReadMessages);
            thread.Start();
        }
        private void ReadMessages()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int messageSize = ServerStream.Read(buffer, 0, 1024);
                    string message = Encoding.UTF8.GetString(buffer, 0, messageSize);
                    Form.Message = message;
                    SaveMessageInDatabase(message); 
                    byte[] response = Encoding.UTF8.GetBytes("OK");
                    ServerStream.Write(response, 0, response.Length);
                }
                catch (System.IO.IOException)
                {
                    ServerStream.Close();
                    break;
                }
            }
        }
        private void SaveMessageInDatabase(string message)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into Messages (ID, Text, Created) values(@ID, @Text, @Created)";
                    command.Parameters.AddWithValue("@ID", Guid.NewGuid());
                    command.Parameters.AddWithValue("@Text", message);
                    command.Parameters.AddWithValue("@Created", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}