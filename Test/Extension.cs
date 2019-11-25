using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public static class Extension
    {
        public static IServiceCollection AddStudent(this IServiceCollection services,IConfiguration configuration)
        {
            var section = configuration.GetSection("Student");
            // 首先 创建实例
            var student = new Student();
            // 然后 属性赋值
            new ConfigureFromConfigurationOptions<Student>(section).Configure(student);
            services.AddSingleton(student);
            return services;
        }
    }
}
