using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Webserver
{
    static int port = 0;
    static int requestsCount = 0;

    public static void Main()
    {
        int portToCheck = 3485;
        while (port == 0)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect("127.0.0.1", portToCheck);
                    Console.WriteLine($"Port {portToCheck} open, checking ${portToCheck+1}");
                    portToCheck++;
                }
                catch (Exception)
                {
                    port = portToCheck;
                }
            }
        }

        while (true)
        {
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add($"http://localhost:{port}/");
                listener.Start();
                Console.WriteLine($"Listening on http://localhost:{port}");

                while (true)
                {
                    try
                    {
                        requestsCount++;

                        HttpListenerContext context = listener.GetContext();
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;

                        Console.WriteLine($"Request #{requestsCount}");
                        Console.WriteLine($"{request.HttpMethod} {request.Url}");
                        Console.WriteLine($"{request.UserHostAddress} {request.UserHostName}");
                        foreach (string header in request.Headers)
                        {
                            Console.WriteLine($"{header}: {request.Headers[header]}");
                        }
                        foreach (string queryKey in request.QueryString)
                        {
                            Console.WriteLine($"{queryKey}={request.QueryString[queryKey]} \n");
                        }

                        string name = request.QueryString["name"];

                        response.StatusCode = 200;
                        StreamWriter writer = new StreamWriter(context.Response.OutputStream);
                        writer.Write($"{name}");

                        writer.Close();
                        response.Close();
                    } catch (Exception)
                    {
                        Console.WriteLine("Connection error");
                    }
                }
            }
        }
    }
}