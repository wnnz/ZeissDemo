using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebService.Models;
using WebService.Service;

namespace WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });

            //添加内存数据库依赖注入
            builder.Services.AddDbContext<InMemDb>(opt => opt.UseInMemoryDatabase("InMemDb"));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseHttpsRedirection();
            app.MapControllers();
            app.UseAuthorization();

            //启动Socket服务
            SocketService.StartReceiveSocket(app);

            app.Run();
        }

    }
}