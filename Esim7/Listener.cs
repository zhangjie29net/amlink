using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace Biaopan2
{
    public class Listener
    {


        private Thread th;
        private TcpListener tcpl;
        public bool listenerRun = true;
        //listenerRun为true，表示可以接受连接请求，false则为结束程序

        public void Stop()
        {
            tcpl.Stop();
            th.Abort();//终止线程
        }
        private string Listen()
        {
            string msg = "";
            try
            {
                tcpl = new TcpListener(5656);//在5656端口新建一个TcpListener对象
                tcpl.Start();
                Console.WriteLine("started listening..");

                Socket s = tcpl.AcceptSocket();
                string remote = s.RemoteEndPoint.ToString();
                Byte[] stream = new Byte[80];
                int i = s.Receive(stream);//接受连接请求的字节流
                msg = "<" + remote + ">" + System.Text.Encoding.UTF8.GetString(stream);


                Console.WriteLine(msg);//在控制台显示字符串
            }

            catch (System.Security.SecurityException)
            {
                Console.WriteLine("firewall says no no to application – application cries..");
            }
            catch (Exception)
            {
                Console.WriteLine("stoped listening..");
            }

            return msg;
        }










    }
}