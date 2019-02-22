using System.IO.Pipes;
using System.Threading;
using System.Text;

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
    }
}