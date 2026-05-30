using Microsoft.EntityFrameworkCore;
using LiveGamingApp.Data;
using LiveGamingApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR(); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- अपडेटेड CORS पॉलिसी (SignalR च्या लाईव्ह कनेक्शनसाठी) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // कुठूनही रिक्वेस्ट आली तरी चालू दे
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // SignalR ला हे लागतंच!
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.MapControllers();
app.MapHub<GameHub>("/gamehub"); 

// --- सर्व्हर चालू झाल्यावर आपोआप डेटाबेस बनवणे (Cloud साठी) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); // जर डेटाबेस नसेल, तर नवीन बनव!
}

app.Run();