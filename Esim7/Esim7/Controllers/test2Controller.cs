using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace Esim7.Controllers
{     /// <summary>
/// 设备测试
/// </summary>
    public class test2Controller : ApiController
    {

        private readonly bool m_isSerevr;
        public Socket sock;          //定义一个Socket类的对象 (默认为protected)                                    //Socket Clientsock;
        private Socket soc;
        private IPEndPoint iep;
        private Thread th;
        private bool bol = false;
        List<Socket> arrya = new List<Socket>();
        String str1;
        IPAddress clientIp;
        public void strat() {



            th = new Thread(new ThreadStart(BeginListen));          //创建一个新的线程专门用于处理监听,这句话可以分开写的,比如: ThreadStart ts=new ThreadStart(BeginListen); th=new Thread (ts); 不过要注意, ThreadStart的构造函数的参数一定要是无参数的函数. 在此函数名其实就是其指针, 这里是委托吗?
            th.Start();





        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public delegate void UpdateControlEventHandler( EventArgs e, Socket sock);
        public static event UpdateControlEventHandler UpdateControl;
        public class StateObjcet
        {
            public Socket workSocket;
            public const int BufferSize = 256;

            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }
        /// <summary>
        /// 异步接收信息
        /// </summary>
        /// <param name="er"></param>
        public static void AcceptCallback(IAsyncResult er)
        {
            try
            {
                allDone.Set();
                // 异步获取用户对象
                Socket listener = (Socket)er.AsyncState;
                //异步接收连接通讯

                Socket handler = listener.EndAccept(er);

                StateObjcet state = new StateObjcet();
                state.workSocket = handler;

                handler.BeginReceive(state.buffer, 0, StateObjcet.BufferSize, 0, new AsyncCallback(ReadCallback), state);

                UpdateControl( new EventArgs(), handler);

                string msg = "From [" + handler.RemoteEndPoint.ToString() + "]:" + System.Text.Encoding.UTF8.GetString(state.buffer) + "\n";   //GetString()函数将byte数组转换为string类型.;
            }
            catch (ObjectDisposedException ex)
            {
                return;
            }
            // setRich(msg);

        }
      
        private void BeginListen()               //Socket监听函数, 等下作为创建新线程的参数
        {
            // 捕获所有错误的线程调用
            //  Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                soc.Bind(iep);                                  //Socket类的一个重要函数, 绑定一个IP,
                soc.Listen(1000);//监听状态
                while (true)
                {
                    allDone.Reset();
                    soc.BeginAccept(new AsyncCallback(AcceptCallback), soc);// 异步接收
                    allDone.WaitOne();
                    System.GC.Collect();
                }
            }

            catch (SocketException se)              //捕捉异常,
            {
              
                // listBox1.Text = se.ToString();       //将其显示出来, 在此亦可以自定义错误.
            }
            catch (ObjectDisposedException ex)
            {

            }
        }



        public static void ReadCallback(IAsyncResult er)
        {
            // 获取异步用户信息
            StateObjcet so = (StateObjcet)er.AsyncState;
            Socket s = so.workSocket;
            try
            {
                //异读取步
                int read = s.EndReceive(er);
                if (read > 0)
                {
                  //  UpdateList( new EventArgs(), Encoding.ASCII.GetString(so.buffer, 0, so.buffer.Length));
                    //so.sb.Append(Encoding.ASCII.GetString(so.buffer, 0, read));
                    s.BeginReceive(so.buffer, 0, StateObjcet.BufferSize, 0, new AsyncCallback(ReadCallback), so);
                }
                else
                {
                   // UpdateComBox(new Form1(), new EventArgs(), s);
                    s.Shutdown(SocketShutdown.Both);
                    // s.Close();
                }
            }

            catch (SocketException)
            {
                //   UpdateComBox(new Form1(), new EventArgs(), s);
                s.Shutdown(SocketShutdown.Both);
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }

       



    }
}
