using System;
using System.IO.Pipes;
using System.Security.Principal;

namespace MessagesSender.Client
{
    class Client
    {
        public static bool Start()
        {
            var pipeClient = new NamedPipeClientStream(".", "messages", PipeDirection.InOut, PipeOptions.Asynchronous, 
                TokenImpersonationLevel.Impersonation);
            try
            {
                pipeClient.Connect(1000);
            }
            catch (TimeoutException)
            {
                return false;
            }
            return true;
        }
    }
}