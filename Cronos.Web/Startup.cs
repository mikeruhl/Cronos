using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cronos.Web.Services;
using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Extensions;
using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Handler;
using FluentSpotifyApi.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Cronos.Web.Models;

namespace Cronos.Web
{
    public class Startup
    {
        private const string SpotifyAuthenticationScheme = SpotifyDefaults.AuthenticationScheme;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(typeof(MockSpotifyService));
            services.AddTransient(typeof(SpotifyService));

            services
                .AddFluentSpotifyClient(clientBuilder => clientBuilder
                    .ConfigurePipeline(pipeline => pipeline
                        .AddAspNetCoreAuthorizationCodeFlow(
                            spotifyAuthenticationScheme: SpotifyAuthenticationScheme)));

            var id = Configuration["SPOTIFY_CLIENTID"];
            var secret = Configuration["SPOTIFY_CLIENT_SECRET"];

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    o =>
                    {
                        o.LoginPath = new PathString("/login");
                        o.LogoutPath = new PathString("/logout");
                    })
                .AddSpotify(
                    SpotifyAuthenticationScheme,
                    o =>
                    {
                        o.ClientId = id;
                        o.ClientSecret = secret;
                        o.Scope.Add("playlist-read-private");
                        o.Scope.Add("playlist-read-collaborative");
                        o.Scope.Add("playlist-modify-private");
                        o.Scope.Add("playlist-modify-public");
                        o.Scope.Add("user-read-private");
                        o.Scope.Add("user-top-read");
                        o.SaveTokens = true;
                    });

            //Automapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(MappingProfile));
            });

            services.AddMvc();

            //Session Handling
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            //Spotify Auth Mapping
            app.Map(
                "/login",
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            // Return a challenge to invoke the Spotify authentication scheme
                            await context.ChallengeAsync(SpotifyAuthenticationScheme, properties: new AuthenticationProperties() { RedirectUri = "/", IsPersistent = true });
                        });
                });

            // Listen for requests on the /logout path, and sign the user out
            app.Map(
                "/logout",
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
                            await context.SignOutAsync();

                            // Redirect the user to the home page after signing out
                            context.Response.Redirect("/");
                        });
                });

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
