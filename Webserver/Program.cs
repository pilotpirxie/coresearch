using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Webserv
{
    static int port = 0;

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

                HttpListenerContext ctx = listener.GetContext();
                ctx.Response.StatusCode = 200;
                string name = ctx.Request.QueryString["name"];

                StreamWriter writer = new StreamWriter(ctx.Response.OutputStream);
                writer.WriteLine("<P>Hello, {0}</P>", name);

                writer.WriteLine("<ul>");
                foreach (string header in ctx.Request.Headers.Keys)
                {
                    writer.WriteLine("<li><b>{0}:</b> {1}</li>", header, ctx.Request.Headers[header]);
                }
                writer.WriteLine("</ul>");

                writer.Close();
                ctx.Response.Close();
                listener.Stop();
            }
        }
    }
}