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

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(UserMapper));

builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddScoped<FolderGenerator>();
builder.Services.AddScoped<UserUtility>();
builder.Services.AddScoped<PostUtility>();
builder.Services.AddScoped<CommentUtility>();
builder.Services.AddScoped<TokenGenerator>();

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
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });

   options.AddPolicy("AllowAll",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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


}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/GetStarted";  // Update path as needed
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseCors("AllowAll");
/*app.UseHttpsRedirection();*/
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
