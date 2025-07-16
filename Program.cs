namespace SignalR_With_Angular;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSignalR();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyPolicy", p =>
            {
                p.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("MyPolicy");
        app.UseHttpsRedirection();
        app.MapHub<ShoppingListHub>("/shoppingListHub");
        app.Run();
    }
}