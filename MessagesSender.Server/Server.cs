using System.IO.Pipes;
using System.Threading;
using System.Text;

namespace MessagesSender.Server
{
    class Server
    {
        private NamedPipeServerStream ServerStream;
        public void Start(object form)
        {
            ServerStream = new NamedPipeServerStream("messages", PipeDirection.InOut, 10);
            ServerStream.WaitForConnection();
            ((MainWindow)form).Status = "Клиент подключился";
            var thread = new Thread(ReadMessages);
            thread.Start();
        }
        private void ReadMessages()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int messageSize = ServerStream.Read(buffer, 0, 1024);
                string message = Encoding.UTF8.GetString(buffer, 0, messageSize);
            }
        }
    }
}