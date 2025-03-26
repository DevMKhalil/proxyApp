using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// إضافة الخدمات الأساسية وإعداد HttpClient
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// تمكين Swagger في بيئة التطوير للمساعدة في التوثيق والاختبار
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// استخدام ميدلوير التحقق من API Key
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
