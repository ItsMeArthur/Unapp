
using Microsoft.EntityFrameworkCore;
using Una.Presentation.API.Context;
using Una.Presentation.API.Repositories.Contracts;
using Una.Presentation.API.Repositories.Implementations;

namespace Una.Presentation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Register Services
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<UnaDbContext>(options =>
            {
                options.UseSqlite("Data Source=Una.db");
            });

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                //options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            builder.Services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));

            // Configure Middlewares
            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors();
            app.Run();
        }
    }
}