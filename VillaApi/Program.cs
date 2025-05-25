using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using VillaApi;
using VillaApi.Data;
using VillaApi.Repository;
using VillaApi.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);
//use to add logger 
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                                                    .WriteTo.File("Log./Mylogger.txt", rollingInterval: RollingInterval.Day)
                                                    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(c =>
{
    c.Filters.Add(new ConsumesAttribute("application/json"));  //request types rahegi
    c.Filters.Add(new ProducesAttribute("application/json")); //response types rahegi
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters(); //used to allow xml format for content negotiation

builder.Services.AddAutoMapper(typeof(MappingConfig));//mapping config added here global configuration
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( opt =>
{
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
});

builder.Services.AddScoped<IVillaRepository, VillaRepository>(); //Dependency injection register here for Villa 
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>(); //Dependency injection register here for VillaNumber


builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    opt.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:IssuerAudience"),
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key")!))
    };
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

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
