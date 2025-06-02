using CarWebShop.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Mappers;
using PixelNestBackend.Repository;
using PixelNestBackend.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using PixelNestBackend.Security;
using PixelNestBackend.Utility;
using PixelNestBackend.Gateaway;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using PixelNestBackend.Middleware;
using System.Net.WebSockets;
using PixelNestBackend.Services.Menagers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using PixelNestBackend.Services.Google;
using PixelNestBackend.Utility.Google;
using PixelNestBackend.Interfaces.GeoLocation;
using PixelNestBackend.Services.GeoLocation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(UserMapper));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<PasswordEncoder>();
builder.Services.AddScoped<PasswordHasher<string>>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IFileUpload, FileUpload>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IGoogleService, GoogleService>();
builder.Services.AddScoped<IGoogleRepository, GoogleRepository>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<GoogleUtility>();
builder.Services.AddScoped<FolderGenerator>();
builder.Services.AddScoped<UserUtility>();
builder.Services.AddScoped<PostUtility>();
builder.Services.AddScoped<CommentUtility>();
builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<WebSocketConnectionMenager>();
builder.Services.AddScoped(x =>
{
    var configuration = x.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetSection("AzureBlobStorage:ConnectionString").Value;
    var containerName = configuration.GetSection("AzureBlobStorage:ContainerName").Value;

    return new SASTokenGenerator(containerName, connectionString);
});

builder.Services.AddScoped(x =>
{
    var configuration = x.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetSection("AzureBlobStorage:ConnectionString").Value;
    var containerName = configuration.GetSection("AzureBlobStorage:ContainerName").Value;
    var dataContext = x.GetRequiredService<DataContext>();
    var blobServiceClient = new BlobServiceClient(connectionString);
    return new BlobStorageUpload(dataContext,blobServiceClient, containerName);
});
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
 });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://pixelnest.netlify.app/", "http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });

   options.AddPolicy("AllowPixelnestAndLocalhost",
            builder =>
            {
                builder.WithOrigins("https://pixelnest.netlify.app/", "http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
    option.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["jwtToken"];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {

            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };


})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Get Started";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});
builder.Services.AddAuthorization();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString:queue"]!, preferMsi: true);
});
builder.Services.AddAuthorization();
//builder.Services.AddAzureClients(clientBuilder =>
//{
//    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString:blob"]!, preferMsi: true);
//    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString:queue"]!, preferMsi: true);
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseCors("AllowPixelnestAndLocalhost");
app.UseMiddleware<APICallLimiter>();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);


app.UseMiddleware<WebSocketMiddleware>();
/*app.UseHttpsRedirection();*/
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseSession();

app.Run();
