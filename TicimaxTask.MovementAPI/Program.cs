using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TicimaxTask.BLL.RepositoryPattern.IServices;
using TicimaxTask.BLL.RepositoryPattern.Services;
using TicimaxTask.DAL.PostgreSqlDb;
using TicimaxTask.DAL.Repositories.Interfaces;
using TicimaxTask.DAL.Repositories.Repositories;
using TicimaxTask.MovementAPI.RabbitMq.ConsumerServices;
using TicimaxTask.MovementAPI.RabbitMq.GeneralServices;
using TicimaxTask.MovementAPI.RabbitMq.PublisherServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMq")),DispatchConsumersAsync=true });
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddSingleton<RabbitMqClientService>();


builder.Services.AddScoped<ICheckInOutService, CheckInOutService>();
builder.Services.AddScoped<ICheckInOutRepository, CheckInOutRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));


builder.Services.AddHostedService<EnterExitConsumerService>();
builder.Services.AddDbContext<PostgreSqlContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), mgrt => mgrt.MigrationsAssembly("TicimaxTask.DAL")));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
