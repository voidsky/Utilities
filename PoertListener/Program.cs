using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PortListener
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse(args[0]);

                server = new TcpListener(localAddr, int.Parse(args[1]));
                server.Start();

                Byte[] bytes = new Byte[256];
                
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    NetworkStream stream = client.GetStream();

                    while (true)
                    {
                        if (!Console.KeyAvailable)
                        {
                            int i;
                            while (stream.DataAvailable && (i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                String data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                Console.WriteLine(data);
                            }
                        }
                        else
                        {
                            Console.Write(">");
                            string lineFromConsole = Console.ReadLine();
                            if (lineFromConsole.Equals("exit")) break;
                            stream.Write(Encoding.ASCII.GetBytes(lineFromConsole), 0, Encoding.ASCII.GetBytes(lineFromConsole).Length);
                            Console.WriteLine("> sent: {0}", lineFromConsole);
                        }

                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


        }
    }
}
