using System;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;

namespace MessagesSender.Client
{
    class Client
    {
        private NamedPipeClientStream ClientStream;
        public bool Start()
        {
            ClientStream = new NamedPipeClientStream(".", "messages", PipeDirection.InOut, PipeOptions.Asynchronous, 
                TokenImpersonationLevel.Impersonation);
            try
            {
                ClientStream.Connect(1000);
            }
            catch (TimeoutException)
            {
                return false;
            }
            return true;
        }
        public string SendMessage(string message)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                if(!ClientStream.IsConnected)
                    return "Клиент не подключился к серверу";
                ClientStream.Write(buffer, 0, buffer.Length);
                ClientStream.Flush();
                const int maxResponseSize = 1024;
                buffer = new byte[maxResponseSize];
                int responseSize = ClientStream.Read(buffer, 0, maxResponseSize);
                string response = Encoding.UTF8.GetString(buffer, 0, responseSize);
                if (response != "OK")
                    return "Неизвестная ошибка на сервере";
                return null;
            }
            catch (System.IO.IOException)
            {
                return "Сервер не отвечает";
            }
        }
    }
}