using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechDancer.Framework.Net.Presets;
using MechDancer.Framework.Net.Modules.Multicast;
using MechDancer.Framework.Net.Resources;
using System.Net;
using System.Threading;

namespace PlaybackTool
{
    /// <summary>
    /// UDP通信
    /// </summary>
    class UdpNet
    {
        /// <summary>
        /// 接收回调
        /// </summary>
        public Action<byte[], byte> OnDataReceived = null;

        // 通信组件
        private RemoteHub hub = null;
        // 组播地址端口
        IPEndPoint group = null;
        // 运行标志
        bool running = true;

        /// <summary>
        /// UDP通信
        /// </summary>
        /// <param name="ip">组播地址</param>
        /// <param name="port">组播端口</param>
        public UdpNet(string ip, int port)
        {
            group = new IPEndPoint(IPAddress.Parse(ip), port);
            hub = CreateHub();
            //hub.Monitor.OpenOne(true);
            hub.Monitor.OpenAll();
            hub.Yell();
            Thread thread = new Thread(WorkThreadCast);
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            running = false;
            hub.Yell();
            int timeoutCnt = 100;   // 1s超时
            while(timeoutCnt-- > 0 && !running)
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 创建通信组件
        /// </summary>
        private RemoteHub CreateHub()
        {
            var listen = new MulticastListener(
                pack => OnDataReceived?.Invoke(pack.Payload, pack.Command), (byte)UdpCmd.Common);
            return new RemoteHub("PlaybackTool" + new Random().Next(1000,9999), group: group, additions: listen);
        }

        /// <summary>
        /// 获取网卡列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetNetWorkList()
        {
            List<string> networks = new List<string>();
            foreach (var net in hub.Modules.OfType<Networks>().First().View)
            {
                networks.Add(net.Key.Name + "(" + net.Key.Description + ")");
            }
            return networks;
        }

        /// <summary>
        /// 设置网卡
        /// </summary>
        public int SetNetWork(int index)
        {
            try
            {
                hub = CreateHub();
                //hub.Monitor.OpenWhere();// TODO
                return index;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 组播接收
        /// </summary>
        private void WorkThreadCast()
        {
            while(running)
            {
                try
                {
                    hub.Invoke();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Thread.Sleep(1000);
                }
            }
            running = true;
        }

        /// <summary>
        /// 组播发送
        /// </summary>
        public void Cast(byte[] data, byte type)
        {
            try
            {
                hub.Broadcast(type, data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Thread.Sleep(1000);
            }
        }

    }
}
