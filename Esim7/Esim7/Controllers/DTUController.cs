using Esim7.Models;
using Esim7.UNity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;


namespace Esim7.Controllers
{
    public class DTUController : ApiController
    {
        private TcpListener listener;
        static TcpClient client = null;          //客户端对象，用来接收或发送消息
        static NetworkStream stream = null;     //流对象，完成接收或发送消息操作
        TcpListener server = null;
        [Route("test")]
        [HttpGet]
        public IHttpActionResult GetMessage(string welcome)
        {


            int recv;//用于表示客户端发送的信息长度
            byte[] data = new byte[1024];//用于缓存客户端所发送的信息,通过socket传递的信息必须为字节数组
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9090);//本机预使用的IP和端口
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newsock.Bind(ipep);//绑定
            newsock.Listen(10);//监听
            Console.WriteLine("waiting for a client");
            Socket client = newsock.Accept();//当有可用的客户端连接尝试时执行，并返回一个新的socket,用于与客户端之间的通信
            IPEndPoint clientip = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("connect with client:" + clientip.Address + " at port:" + clientip.Port);

            data = Encoding.ASCII.GetBytes(welcome);
            client.Send(data, data.Length, SocketFlags.None);//发送信息
           
            //用死循环来不断的从客户端获取信息
                data = new byte[1024];
                recv = client.Receive(data);
                Console.WriteLine("recv=" + recv);
              //  if (recv == 0)//当信息长度为0，说明客户端连接断开
                   
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                client.Send(data, recv, SocketFlags.None);
          
            Console.WriteLine("Disconnected from" + clientip.Address);


            return Json<object>(clientip.Address);








        }

        [Route("open")]
        [HttpGet]
        /// <summary>
        /// 接受数据
        /// </summary>      
      /// <param name="port">端口号</param>
        /// <param name="runing">是否监听</param>
        /// <returns></returns>
        public IHttpActionResult Server(int port, string host)
        {

            // 将将IP地址字符串转换为IPAddress对象
            IPAddress ip = IPAddress.Parse(host);
            // 终结点EndPoint
            IPEndPoint ipe = new IPEndPoint(ip, port);

            //============================================================================//
            // 2、创建socket连接服务端并监听端口
            //============================================================================//
            //创建TCP Socket对象
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //绑定EndPoint对象（地址）
            server.Bind(ipe);
            //开始监听
            server.Listen(1000);
            Console.WriteLine("已经处于监听状态，等待客户端连接 . . . ");
            //============================================================================//
            // 3、与客户端交互
            //============================================================================//
            List<Receives> li = new List<Receives>();

            Receives r = new Receives();

            Socket remote = server.Accept();
            Console.WriteLine("客户端连接 . . . ");
            // 接收客户端消息
            while (true)
            {
                Console.WriteLine("客户端连接 . . . ");
                r.Receive = Unit.Receive(remote);
                Console.WriteLine(r.Receive);
                r.Send = "null";
                r.flg = "1";
                li.Add(r);

            }

            return Json<List<Receives>>(li);

          
        }

        



        [Route("Send")]
        [HttpGet]
        public void Send(int port, string host, string send)
        {


            //============================================================================//
            // 1、待连接的socket服务地址
            //============================================================================//


            // 将将IP地址字符串转换为IPAddress对象
            IPAddress ip = IPAddress.Parse(host);
            // 创建终结点EndPoint
            IPEndPoint ipe = new IPEndPoint(ip, port);

            //============================================================================//
            // 2、创建socket连接客户端
            //============================================================================//        
            // 创建Socket并连接到服务器
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 连接到服务器
            client.Connect(ipe);

            //============================================================================//
            // 3、与服务器端交互
            //============================================================================//
            Unit.Send(client, send);
            Unit.Receive(client);

            //============================================================================//
            // 4、关闭Socket连接
            //============================================================================//
            if (client != null)
            {
                client.Close();
                client = null;
            }

            Console.WriteLine("what21.com prompt, Press any key to continue . . . ");
            Console.ReadKey(true);


        }

        [Route("Start")]
        [HttpGet]
        public string statr(string ip, int port)
        {
            string flg = "";
            try
            {
                listener = new TcpListener(new IPEndPoint(IPAddress.Parse(ip), port));

                listener.Start();//开启侦听，对连接的客户端的数目没有限制 }
                flg = "ok";
            }
            catch (Exception ex)
            {



                flg = "fale" + ex;
            }         //监听类
            return flg;



        }


        [Route("OPen2")]
        [HttpGet]
        public void start3(string host, int port) {

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint p = new IPEndPoint(ip, port);              //创建TCP连接的端点

            server = new TcpListener(p);                         //初始化TcpListener的新实例
            server.Start();                                      //开始监听客户端的请求


            client = server.AcceptTcpClient();                   //执行挂起和接收连接请求，获得客户端对象




        }

        [Route("start2")]
        [HttpGet]
        public void Send2(){










        }
    }
}
