using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStreamWriterBugReproduction
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // app.UseStaticFiles();

            app.UseRouting();


            app.UseWhen(
               delegate (Microsoft.AspNetCore.Http.HttpContext context)
               {
                   bool b = context.Request.Path.Equals("/");
                   return b;
               }
               , delegate (Microsoft.AspNetCore.Builder.IApplicationBuilder appBuilder)
               {
                   appBuilder.Use(
                       async delegate (Microsoft.AspNetCore.Http.HttpContext context, System.Func<System.Threading.Tasks.Task> next)
                       {
                           context.Response.Redirect("/ajax/WithStreamWriter.ashx");
                           await System.Threading.Tasks.Task.CompletedTask;
                       });
               }
            );

            // http://localhost:63755/ajax/WithStreamWriter.ashx
            app.UseWhen(
                delegate (Microsoft.AspNetCore.Http.HttpContext context)
                {
                    bool b = context.Request.Path.StartsWithSegments("/ajax/WithStreamWriter.ashx");
                    return b;
                }
                , delegate (Microsoft.AspNetCore.Builder.IApplicationBuilder appBuilder)
                {
                    appBuilder.Use(
                        async delegate (Microsoft.AspNetCore.Http.HttpContext context, System.Func<System.Threading.Tasks.Task> next)
                        {
                            try
                            {
                                // throw new System.Exception("SQL-Execution Error");

                                System.Exception caughtByCodeAndReturnedFromFunction = new System.Exception("SQL-Execution Error");

                                using (System.IO.TextWriter output = new System.IO.StreamWriter(context.Response.Body, new System.Text.UTF8Encoding(false), 4096, true))
                                // using (System.IO.StreamWriter output = new System.IO.StreamWriter(context.Response.Body, new System.Text.UTF8Encoding(false), 4096, true) { AutoFlush = false })
                                // using (AAAA output = new AAAA(context.Response.Body, new System.Text.UTF8Encoding(false), 4096, true) { ExceptionFetcher = delegate { return caughtByCodeAndReturnedFromFunction; } })
                                {
                                    throw new System.Exception("SQL-Execution Error");
                                    // SomeClass sc = new SomeClass();
                                    // await Helpers.ToJSON(context.Response.Body, sc);
                                } // End Using output 

                            } // End Try 
                            catch (System.Exception ex)
                            {
                                try
                                {
                                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                                    // context.Response.Headers["X-Error-Message"] = "Incorrect username or password";
                                    context.Response.Headers["X-Error-Message"] = ex.Message;

                                    // context.Response.ContentType = "text/plain";
                                    // byte[] msg = System.Text.Encoding.UTF8.GetBytes("this is a test");
                                    // await context.Response.Body.WriteAsync(msg, 0, msg.Length);

                                    context.Response.ContentType = "application/json";
                                    await Helpers.ToJSON(context.Response.Body, ex);
                                }
                                catch (System.Exception ex2)
                                {
                                    System.Console.WriteLine(ex2.Message);
                                    System.Console.WriteLine(ex2.StackTrace);
                                    NativeMethods.ShowMessageBox(ex2.Message, ex2.StackTrace);
                                }
                            } // End Catch 

                        }
                    );

                }
            );


            // app.UseAuthorization();

            // app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });


            // http://localhost:63755/ajax/AAAA.ashx
            app.UseWhen(
                delegate (Microsoft.AspNetCore.Http.HttpContext context)
                {
                    bool b = context.Request.Path.StartsWithSegments("/ajax/AAAA.ashx");
                    return b;
                }
                , delegate (Microsoft.AspNetCore.Builder.IApplicationBuilder appBuilder)
                {
                    appBuilder.Use(
                        async delegate (Microsoft.AspNetCore.Http.HttpContext context, System.Func<System.Threading.Tasks.Task> next)
                        {
                            try
                            {
                                // throw new System.Exception("SQL-Execution Error");

                                System.Exception caughtByCodeAndReturnedFromFunction = new System.Exception("SQL-Execution Error");

                                // using (System.IO.TextWriter output = new System.IO.StreamWriter(context.Response.Body, new System.Text.UTF8Encoding(false), 4096, true))
                                // using (System.IO.StreamWriter output = new System.IO.StreamWriter(context.Response.Body, new System.Text.UTF8Encoding(false), 4096, true) { AutoFlush = false })
                                using (AAAA output = new AAAA(context.Response.Body, new System.Text.UTF8Encoding(false), 4096, true) { ExceptionFetcher = delegate { return caughtByCodeAndReturnedFromFunction; } })
                                {
                                    throw new System.Exception("SQL-Execution Error");
                                    // SomeClass sc = new SomeClass();
                                    // await Helpers.ToJSON(context.Response.Body, sc);
                                } // End Using output 

                            } // End Try 
                            catch (System.Exception ex)
                            {
                                try
                                {
                                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                                    // context.Response.Headers["X-Error-Message"] = "Incorrect username or password";
                                    context.Response.Headers["X-Error-Message"] = ex.Message;

                                    // context.Response.ContentType = "text/plain";
                                    // byte[] msg = System.Text.Encoding.UTF8.GetBytes("this is a test");
                                    // await context.Response.Body.WriteAsync(msg, 0, msg.Length);

                                    context.Response.ContentType = "application/json";
                                    await Helpers.ToJSON(context.Response.Body, ex);
                                }
                                catch (System.Exception ex2)
                                {
                                    System.Console.WriteLine(ex2.Message);
                                    System.Console.WriteLine(ex2.StackTrace);
                                    NativeMethods.ShowMessageBox(ex2.Message, ex2.StackTrace);
                                }
                            } // End Catch 

                        }
                    );

                }
            );

        }
    }
}
