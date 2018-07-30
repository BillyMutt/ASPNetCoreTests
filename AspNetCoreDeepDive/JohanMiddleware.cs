using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCoreDeepDive
{
    //  Middleware options (in this case, the name of the log file).
    public class JohanMiddlewareOptions
    {
        public string FileName { get; set; }

        public JohanMiddlewareOptions()
        {
            FileName = "log.txt";
        }

        public JohanMiddlewareOptions(string fname)
        {
            FileName = fname;
        }
    }

    //  Custom middleware interface definition.
    public interface IJohanMiddleware
    {
        void LogRequest(HttpRequest request);
        void LogResponse(HttpResponse response);
    }

    //  Custom middleware implementation.
    public class JohanMiddleware : IJohanMiddleware
    {
        private readonly RequestDelegate _next;
        private JohanMiddlewareOptions _options;

        public JohanMiddleware(RequestDelegate next, IOptions<JohanMiddlewareOptions> options)
        {
            //  Cache the objects we need later on.
            _next = next;
            _options = options.Value;
        }

        public void LogRequest(HttpRequest request)
        {
            var requestLogMessage = $"REQUEST:\r\n{request.Method} - {request.Path.Value} {request.QueryString}";
            requestLogMessage += $"\r\nContentType: {request.ContentType ?? "Not specified"}";
            requestLogMessage += $"\r\nHost: {request.Host}";
            File.AppendAllText(_options.FileName, $"{DateTime.Now.ToString("s")}\r\n{requestLogMessage}\r\n");
        }

        public void LogResponse(HttpResponse response)
        {
            var responseLogMessage = $"RESPONSE:\r\nStatus Code: {response.StatusCode}";
            File.AppendAllText(_options.FileName, $"{DateTime.Now.ToString("s")}\r\n{$"{responseLogMessage}\r\n"}\r\n");
        }

        public async Task Invoke(HttpContext context)
        {
            //  Do something related to the incoming request: log info to file.
            LogRequest(context.Request);

            //  Call the next middleware in the pipeline.
            await _next.Invoke(context);

            //  Do something related to the response: log info to file.
            LogResponse(context.Response);
        }
    }

    //  Extension methods. 
    public static class JohanMiddlewareExtensions
    {
        //  Extension method for IApplicationBuilder to make using the middleware simpler.
        public static IApplicationBuilder UseJohan(this IApplicationBuilder app, JohanMiddlewareOptions options)
        {
            return app.UseMiddleware<JohanMiddleware>(Options.Create(options));
        }

        //  Extension method for IServiceCollection to make registering the middleware 
        //  with the ASP.Net Core dependency injection container easier.
        public static IServiceCollection AddJohan(this IServiceCollection svc)
        {
            return svc.AddSingleton<IJohanMiddleware, JohanMiddleware>();
        }
    }
}
