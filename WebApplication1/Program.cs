using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common;
using WebApplication1.Setup;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MyDbContext>(opt => {
    string connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; //格式化时间
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(); //修改

//修改
var ApiName = "NetCore项目框架";//定义项目名称
builder.Services.AddSwaggerGen(options =>
{
    #region 设置API文档信息
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        // {ApiName} 定义成全局变量，方便修改
        Version = "v1",
        Title = $"{ApiName} 接口文档——Net 6",
        Description = $"{ApiName} HTTP API v1",
    });
    options.OrderActionsBy(o => o.RelativePath);
    #endregion

    #region 引用接口注释
    // using System.Reflection;
    //获取xml注释文件目录
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    //默认的第二个参数是false，这个是controller的注释，true时会显示注释，否则只显示方法注释
    //options.IncludeXmlComments(xmlPath, true);

    var xmlModelname = "Medel.xml";//Model层的xml文件名
    var xmlModelPath = Path.Combine(AppContext.BaseDirectory, xmlModelname);
    #endregion

    #region 在接口中添加token
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    //Token绑定到ConfigureServices
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
        Name = "Authorization",//jwt默认的参数名称
        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
        Type = SecuritySchemeType.ApiKey
    });
    #endregion
});
//jwt授权验证
builder.Services.AddAuthorizationSetup();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//注意中间件的顺序，UseRouting放在最前边，UseAuthentication在UseAuthorization前边
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
