using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Linq;
using coresearch;

namespace Webserver { 
    public class Webserver
    {
        private static int _port = 0;
        private static int _requestsCount = 0;
        private static int _filesCount = 0;
        private static Coresearch _coresearch = new Coresearch(true);

        public static void Main()
        {
            int portToCheck = 3485;
            while (_port == 0)
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
                        _port = portToCheck;
                    }
                }
            }

            while (true)
            {
                using (HttpListener listener = new HttpListener())
                {
                    listener.Prefixes.Add($"http://localhost:{_port}/");
                    listener.Start();
                    Console.WriteLine($"Listening on http://localhost:{_port}");

                    while (true)
                    {
                        try
                        {
                            _requestsCount++;

                            HttpListenerContext context = listener.GetContext();
                            ThreadPool.QueueUserWorkItem((_) =>
                            {
                                HttpListenerRequest request = context.Request;
                                HttpListenerResponse response = context.Response;

                                try
                                {
                                    Debug(request);
                                    Route(request, response);
                                } catch (Exception ex)
                                {
                                    Console.WriteLine($"Request error {ex}");
                                    XElement body = new XElement("response",
                                        new XElement("status", "bad request")
                                    );

                                    WriteResponse(response, body.ToString(), 400);
                                }
                            });
                        
                        } catch (Exception)
                        {
                            Console.WriteLine("Request error");
                        }
                    }
                }
            }
        }

        public static void Debug(HttpListenerRequest request)
        {
            Console.WriteLine($"{DateTime.Now} #{_requestsCount}");
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            Console.WriteLine($"{request.UserHostAddress} {request.UserHostName}");
            foreach (string header in request.Headers)
            {
                Console.WriteLine($"{header}: {request.Headers[header]}");
            }
            foreach (string queryKey in request.QueryString)
            {
                Console.WriteLine($"{queryKey}={request.QueryString[queryKey]} {Environment.NewLine}");
            }
        }

        public static void Route(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch(request.Url.LocalPath)
            {
                case "/search":
                case "/get":
                    Search(request, response);
                    break;
                case "/source":
                case "/load":
                    LoadFromSource(request, response);
                    break;
                case "/delete":
                    Delete(request, response);
                    break;
                case "/flush":
                    Flush(request, response);
                    break;
                case "/add":
                case "/insert":
                    Insert(request, response);
                    break;
                case "/query":
                    if (request.QueryString["type"] == "search") Search(request, response);
                    if (request.QueryString["type"] == "shallow") QueryShallow(request, response);
                    if (request.QueryString["type"] == "deep") QueryDeep(request, response);
                    break;
                default:
                    NotFound(request, response);
                    break;
            }
        }

        private static void WriteResponse(HttpListenerResponse response, string body, int statusCode = 200)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/xml";
            StreamWriter writer = new StreamWriter(response.OutputStream);
            writer.Write(body);
            writer.Close();
            response.Close();
        }

        private static void NotFound(HttpListenerRequest request, HttpListenerResponse response)
        {
            XElement body = new XElement("response",
                new XElement("status", "not found")
            );

            WriteResponse(response, body.ToString(), 404);
        }

        private static void QueryDeep(HttpListenerRequest request, HttpListenerResponse response)
        {
            List<string> results = _coresearch.QueryDeep(request.QueryString["key"]);

            if (_coresearch.Debug)
            {
                Console.WriteLine($"{results.Count} results for {request.QueryString["key"]}");
            }

            foreach (string el in results)
            {
                Console.WriteLine(el);
            }

            XElement xres = new XElement("results");
            foreach (string result in results)
            {
                xres.Add(new XElement("resource", result));
            }

            XElement body = new XElement("response",
                new XElement("status", "success"),
                new XElement("query", request.QueryString["key"]),
                new XElement("queryType", "deep"),
                xres
            );

            WriteResponse(response, body.ToString());
        }

        private static void QueryShallow(HttpListenerRequest request, HttpListenerResponse response)
        {
            List<string> results = _coresearch.QueryShallow(request.QueryString["key"]);

            if (_coresearch.Debug)
            {
                Console.WriteLine($"{results.Count} results for {request.QueryString["key"]}");
            }

            foreach (string el in results)
            {
                Console.WriteLine(el);
            }


            XElement xres = new XElement("results");
            foreach (string result in results)
            {
                xres.Add(new XElement("resource", result));
            }

            XElement body = new XElement("response",
                new XElement("status", "success"),
                new XElement("query", request.QueryString["key"]),
                new XElement("queryType", "shallow"),
                xres
            );

            WriteResponse(response, body.ToString());
        }

        private static void Flush(HttpListenerRequest request, HttpListenerResponse response)
        {
            _coresearch.Flush();
            _filesCount = 0;
            XElement body = new XElement("response",
                new XElement("status", "success")
            );

            WriteResponse(response, body.ToString());
        }

        private static void Insert(HttpListenerRequest request, HttpListenerResponse response)
        {
            _coresearch.InsertResource(request.QueryString["resourceName"], request.QueryString["content"]);
            XElement body = new XElement("response",
                new XElement("status", "success")
            );

            WriteResponse(response, body.ToString());
        }

        private static void Delete(HttpListenerRequest request, HttpListenerResponse response)
        {
            _coresearch.Remove(request.QueryString["key"]);

            XElement body = new XElement("response",
                new XElement("status", "success")
            );

            WriteResponse(response, body.ToString());
        }

        private static void LoadFromSource(HttpListenerRequest request, HttpListenerResponse response)
        {

            foreach (string file in Directory.EnumerateFiles(request.QueryString["path"], request.QueryString["extension"], SearchOption.AllDirectories))
            {
                _filesCount++;
                foreach (string line in File.ReadLines(file))
                {
                    _coresearch.InsertResource(file, line);
                }
            }

            if (_coresearch.Debug)
            {
                Console.WriteLine($"Words inserted {_coresearch.Count} from {_filesCount} files with memory usage of {GC.GetTotalMemory(false)} bytes");
            }

            XElement body = new XElement("response",
                new XElement("status", "success"),
                new XElement("words", _coresearch.Count),
                new XElement("files", _filesCount),
                new XElement("memory", GC.GetTotalMemory(false))
            );

            WriteResponse(response, body.ToString());
        }

        private static void Search(HttpListenerRequest request, HttpListenerResponse response)
        {
            List<string> results = _coresearch.Get(request.QueryString["key"]);

            if (_coresearch.Debug)
            {
                Console.WriteLine($"{results.Count} results for {request.QueryString["key"]}");
            }

            XElement xres = new XElement("results");
            foreach (string result in results)
            {
                xres.Add(new XElement("resource", result));
            }

            XElement body = new XElement("response",
                new XElement("status", "success"),
                new XElement("query", request.QueryString["key"]),
                new XElement("queryType", "exact"),
                xres
            );

            WriteResponse(response, body.ToString());
        }
    }
}