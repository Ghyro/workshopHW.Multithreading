﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static string username;
        private const string localhost = "127.0.0.1";
        private const int port = 8888;
        private static TcpClient client;
        private static NetworkStream stream;
        private static int count;
 
        static void Main(string[] args)
        {
            Console.Write("Please, enter your name: ");
            username = Console.ReadLine();
            client = new TcpClient();
            try
            {
                client.Connect(localhost, port);

                stream = client.GetStream();
 
                string message = username;

                byte[] data = Encoding.Unicode.GetBytes(message);

                stream.Write(data, 0, data.Length);

                Task receiveTask = new Task(ReceiveMessage);
                receiveTask.Start();

                Console.WriteLine($"Welcome to the chat, {username}");

                count = new Random().Next(5, 15);

                Thread.Sleep(1000);

                SendMessage(count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        static void SendMessage(int count)
        {
            List<string> messages = new List<string>()
            {
                "Hello, world!",
                "Hello everyone!",
                "Hi, Dear! I'm here!",
                "Hello, EPAM!",
                "I have connected to the server!",
                "Ops, I do not need to be here!",
                "I work as a Software Engineer.",
                "That was great!",
                "Excuse me"
            };

            int k = 0;

            while (k < count)
            {
                int index = new Random().Next(messages.Count);
                string message = messages[index];
                Console.WriteLine($"{username}: {message}");
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(new Random().Next(5000,6000));
                k++;
            }
        }
        
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder stringBuilder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
 
                    string message = stringBuilder.ToString();
                    Console.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("The connection has been interrupted!");
                    Thread.Sleep(1000);
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }
 
        static void Disconnect()
        {
            //client.Close();

            //if (stream != null)
            //{
            //    stream.Close();
            //}           

            Environment.Exit(0);
        }
    }
}