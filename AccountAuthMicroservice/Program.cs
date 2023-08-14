using AccountAuthMicroservice.Config;
using AccountAuthMicroservice.Middlewares;
using AccountAuthMicroservice.Repositories;
using AccountAuthMicroservice.Repositories.Interface;
using AccountAuthMicroservice.Security;
using AccountAuthMicroservice.Services;
using AccountAuthMicroservice.Services.Impl;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region  ============== Setting Swagger dengan security ====================
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name:JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Input token : `Bearer {token}`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[] {}
        }
    });
});
#endregion

#region ============== Inject AppDbContext =============
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region  ============== Dependency Injection =============
builder.Services.AddTransient<IPersistence, DbPersistence>();
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IStoreService, StoreService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IJwtUtil, JwtUtil>();
builder.Services.AddTransient<ILoginHistoryService, LoginHistoryService>();
#endregion

//  ============== Setting Middleware  ==============
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
// ========================================================

//  ============== Setting Jwt ambil dari class yang sudah dibuatkan  ==============
builder.AddAppAuthentication();
// ==================================================================================

//  ============== Konfigurasi Auto Mapper ==============
IMapper mapper = MappingConfig.RegisterMapper().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// ===================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

//Tambahkan auto migration disini
ApplyMigration();
app.Run();

// ===================== Auto update setelah migration =================
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
        {
            
        }
    }
}