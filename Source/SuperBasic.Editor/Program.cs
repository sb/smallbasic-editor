// <copyright file="Program.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor
{
    using System.Globalization;
    using Microsoft.AspNetCore.Blazor.Builder;
    using Microsoft.AspNetCore.Blazor.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using SuperBasic.Editor.Components.Layout;

    public static class Program
    {
        // TODO: in all components overrides, call base: https://github.com/aspnet/Blazor/pull/1619
        public static void Main()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            BlazorWebAssemblyHost
                .CreateDefaultBuilder()
                .UseBlazorStartup<Startup>()
                .Build()
                .Run();
        }
    }

    public class Startup
    {
#pragma warning disable CA1801, CA1822
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
#pragma warning restore CA1801, CA1822
    }
}
