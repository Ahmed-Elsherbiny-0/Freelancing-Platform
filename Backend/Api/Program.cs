using Api;
using Api.Hubs;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services.SignalR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDependancies(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();


try
{
	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
	await context.Database.MigrateAsync();
    var presenceTracker = scope.ServiceProvider.GetRequiredService<IPresenceTracker>();
    await presenceTracker.ClearAllAsync();
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
	throw;
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("cors");

app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();
