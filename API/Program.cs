using Core.Identity;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Identity;
using Persistence.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
         builder.WithOrigins("http://localhost:4200", "http://easytobuy.runasp.net", "http://easytobuy.runasp.net/#/") // Replace with your Angular app's domain
     .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB, adjust as needed
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commers.API", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nEnter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
                    new string[] {}
                }
            });
});

builder.Services.AddDbContext<MembershipDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MSSQL"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 1000;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<MembershipDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
        // 🔥 THIS PART IS MISSING IN YOUR CODE
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {

                var accessToken = context.Request.Query["access_token"];
                // If the request is for our hubs
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/api/videoCallHub") || path.StartsWithSegments("/api/notificationHub")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSignalR();
//.AddHubOptions<NotificationHub>(options =>
//{
//    options.UserIdProvider = new NameIdentifierUserIdProvider();
//});

builder.Services.AddSingleton(new DataAccessHelper(builder.Configuration));
builder.Services.AddSingleton(new SecurityHelper(builder.Configuration));

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAspNetUsersRepository, AspNetUsersRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUploadedFileRepository, UploadedFileRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IUserPackageRepository, UserPackageRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISlotRepository, SlotRepository>();
builder.Services.AddScoped<ISlotAssignmentRepository, SlotAssignmentRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserFileRepository, UserFileRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingRescheduleRepository, BookingRescheduleRepository>();
builder.Services.AddScoped<IFinalTestResultRepository, FinalTestResultRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ILessonProgresRepository, LessonProgresRepository>();
builder.Services.AddScoped<IVehicleAvailabilityRepository, VehicleAvailabilityRepository>();
builder.Services.AddScoped<IExpenditureHeadRepository, ExpenditureHeadRepository>();
builder.Services.AddScoped<IExpenditureRepository, ExpenditureRepository>();
builder.Services.AddScoped<ICheckListRepository, CheckListRepository>();
builder.Services.AddScoped<IBookingCheckListRepository, BookingCheckListRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IPackTypeRepository, PackTypeRepository>();
builder.Services.AddScoped<IMeasurementUnitRepository , MeasurementUnitRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderPaymentRepository, OrderPaymentRepository>();
builder.Services.AddScoped<IStatusMasterRepository, StatusMasterRepository>();
builder.Services.AddScoped<IPaymentGatewayTypeRepository, PaymentGatewayTypeRepository>();
builder.Services.AddScoped<IPaymentGatewayConfigRepository, PaymentGatewayConfigRepository>();
builder.Services.AddScoped<IEPSPGRepository, EPSPGRepository>();
builder.Services.AddScoped<IWebsiteSectionRepository, WebsiteSectionRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/api/notificationHub");
app.Run();
