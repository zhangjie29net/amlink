
using System;

using System.Collections.Generic;

using System.Text;

using System.Threading;

using System.Net.Sockets;

using System.Net;



namespace Esim7

{

    class TcpIpServerEx

    {

        public EndPoint RemoteEndPoint { get; private set; } //当前客户端的网络结点



        Thread threadwatch = null;//负责监听客户端的线程

        Socket socket = null;//负责监听客户端的套接字

        // Dictionary<ip和端口, Socket> 定义一个集合，存储客户端信息

        public Dictionary<EndPoint, Socket> dic = new Dictionary<EndPoint, Socket> { };



        private StringBuilder msg = new StringBuilder();

        public string Msg

        {

            get { return msg.ToString(); }

            private set

            {

                msg.AppendLine(value);

                Console.WriteLine(value + "\r\n");

            }



        }



        private TcpIpServerEx() { }

        public TcpIpServerEx(int port = 11000)

        {

            //定义一个套接字用于监听客户端发来的消息，包含三个参数（IP4寻址协议，流式连接，Tcp协议）

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);



            //将IP地址和端口号绑定到网络节点point上

            IPEndPoint point = new IPEndPoint(IPAddress.Any, port);//设置服务器端口，IP是本程序所在PC的内网IP



            //监听绑定的网络节点

            socket.Bind(point);



            //将套接字的监听队列长度限制为20

            socket.Listen(20);



            //创建一个监听线程

            threadwatch = new Thread(watchconnecting);



            //将窗体线程设置为与后台同步，随着主线程结束而结束

            threadwatch.IsBackground = true;



            //启动线程   

            threadwatch.Start();



            //启动线程后显示相应提示

            Msg = ("开始监听客户端传来的信息!" + "\r\n");

        }



        //监听客户端发来的请求

        private void watchconnecting()

        {

            Socket connection = null;

            while (true)  //持续不断监听客户端发来的请求   

            {

                try

                {

                    connection = socket.Accept();

                }

                catch (Exception ex)

                {

                    Msg = (ex.Message); //提示套接字监听异常   

                    break;

                }



                //让客户显示"连接成功的"的信息

                string sendmsg = "连接服务端成功！你的IP是" + connection.RemoteEndPoint;

                byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendmsg);

                connection.Send(arrSendMsg);





                RemoteEndPoint = connection.RemoteEndPoint; //客户端网络结点号

                Msg = ("成功与" + RemoteEndPoint + "客户端建立连接！\t\n");     //显示与客户端连接情况

                dic.Add(RemoteEndPoint, connection);    //添加客户端信息





                //创建一个通信线程    

                ParameterizedThreadStart pts = new ParameterizedThreadStart(recv);

                Thread thread = new Thread(pts);

                thread.IsBackground = true;//设置为后台线程，随着主线程退出而退出           

                thread.Start(connection);//启动线程  

            }

        }



        ///   

        /// 接收客户端发来的信息    

        ///   

        ///客户端套接字对象  

        private void recv(object socketclientpara)

        {



            Socket socketServer = socketclientpara as Socket;

            while (true)

            {

                //创建一个内存缓冲区 其大小为1024*1024字节  即1M   

                byte[] arrServerRecMsg = new byte[1024 * 1024];

                //将接收到的信息存入到内存缓冲区,并返回其字节数组的长度  

                try

                {

                    int length = socketServer.Receive(arrServerRecMsg);



                    //将机器接受到的字节数组转换为人可以读懂的字符串   

                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);



                    //将发送的字符串信息附加到文本框txtMsg上   

                    Msg = ("客户端:" + GetCurrentTime() + socketServer.RemoteEndPoint + "的消息:" + strSRecMsg + "\r\n");

                }

                catch (Exception ex)

                {

                    Msg = ("客户端:" + GetCurrentTime() + socketServer.RemoteEndPoint + "已经中断连接" + "\r\n"); //提示套接字监听异常 

                    //listBoxOnlineList.Items.Remove(socketServer.RemoteEndPoint.ToString());//从listbox中移除断开连接的客户端

                    socketServer.Close();//关闭之前accept出来的和客户端进行通信的套接字

                    break;

                }

            }





        }





        //获取当前系统时间

        private string GetCurrentTime()

        {

            string timeStr = System.DateTime.Now.ToString("yyyy年MM月dd日hh时mm分ss秒fff毫秒。");

            return timeStr;

        }



        /// <summary>

        /// 发送信息到客户端

        /// </summary>

        /// <param name="smallname"></param>

        /// <param name="sendMsg">要发送的信息</param>

        public void SentMsg(EndPoint endPoint, string sendMsg)

        {



            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sendMsg);   //将要发送的信息转化为字节数组，因为Socket发送数据时是以字节的形式发送的

            dic[endPoint].Send(bytes);   //发送数据

            Msg = (GetCurrentTime() + endPoint + "的消息:" + sendMsg + "\r\n");

        }

    }

}


