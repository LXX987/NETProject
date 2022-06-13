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
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; //��ʽ��ʱ��
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(); //�޸�

//�޸�
var ApiName = "NetCore��Ŀ���";//������Ŀ����
builder.Services.AddSwaggerGen(options =>
{
    #region ����API�ĵ���Ϣ
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        // {ApiName} �����ȫ�ֱ����������޸�
        Version = "v1",
        Title = $"{ApiName} �ӿ��ĵ�����Net 6",
        Description = $"{ApiName} HTTP API v1",
    });
    options.OrderActionsBy(o => o.RelativePath);
    #endregion

    #region ���ýӿ�ע��
    // using System.Reflection;
    //��ȡxmlע���ļ�Ŀ¼
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    //Ĭ�ϵĵڶ���������false�������controller��ע�ͣ�trueʱ����ʾע�ͣ�����ֻ��ʾ����ע��
    //options.IncludeXmlComments(xmlPath, true);

    var xmlModelname = "Medel.xml";//Model���xml�ļ���
    var xmlModelPath = Path.Combine(AppContext.BaseDirectory, xmlModelname);
    #endregion

    #region �ڽӿ������token
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    //Token�󶨵�ConfigureServices
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
        Name = "Authorization",//jwtĬ�ϵĲ�������
        In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
        Type = SecuritySchemeType.ApiKey
    });
    #endregion
});
//jwt��Ȩ��֤
builder.Services.AddAuthorizationSetup();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//ע���м����˳��UseRouting������ǰ�ߣ�UseAuthentication��UseAuthorizationǰ��
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
