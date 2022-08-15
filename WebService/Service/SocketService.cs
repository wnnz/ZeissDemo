using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using WebService.Models;

namespace WebService.Service
{
    public class SocketService
    {
        /// <summary>
        /// 启动接收socket数据的服务
        /// </summary>
        public static void StartReceiveSocket(WebApplication app)
        {
            Task.Factory.StartNew(async () =>
            {
                //获取ef上下文
                var dbcontext = GetDbcontext(app);

                //创建ws客户端并连接
                string url = "ws://machinestream.herokuapp.com/ws";
                ClientWebSocket clientWebSocket = new();
                try
                {
                    await clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接服务器失败\n" + ex.Message);
                }

                byte[] buffer = new byte[1024];

                while (true)
                {
                    //接收数据
                    var result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    string content = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    JObject jobj = JObject.Parse(content);

                    //将数据转为实体,这样存进数据库的话可以根据字段筛选数据
                    SocketResult? socketResult = null;
                    try
                    {
                        socketResult = jobj.ToObject<SocketResult>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("转换为实体失败\n" + ex);
                    }

                    if (socketResult == null)
                    {
                        socketResult = new SocketResult();
                    }
                    socketResult.Id = Guid.NewGuid();
                    socketResult.Result = content;

                    //将实体存入数据库，本程序使用内存数据库做演示
                    try
                    {
                        await dbcontext.AddAsync(socketResult);
                        var i = await dbcontext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("保存到数据库失败\n" + ex);

                    }
                }
            });
        }
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        static InMemDb GetDbcontext(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetService(typeof(InMemDb)) as InMemDb;
            if (dbcontext == null)
            {
                Console.WriteLine("获取数据库上下文失败");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return dbcontext!;
        }
    }
}
