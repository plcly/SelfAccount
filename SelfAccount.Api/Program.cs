using SelfAccount.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.

var app = builder.Build();

//app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/test", AccountApi.Test);
app.MapGet("/GetAll", AccountApi.GetAll);
app.MapPost("/PushAll", AccountApi.PushAll);



app.Run();
