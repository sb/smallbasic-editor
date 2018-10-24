// <copyright file="Program.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor
{
    using Microsoft.AspNetCore.Blazor.Builder;
    using Microsoft.AspNetCore.Blazor.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using SuperBasic.Editor.Bridge;
    using SuperBasic.Editor.Pages;
    using SuperBasic.Utilities;

    public static class Program
    {
        public static void Main()
        {
            BlazorWebAssemblyHost
                .CreateDefaultBuilder()
                .UseBlazorStartup<Startup>()
                .Build()
                .Run();
        }
    }

#pragma warning disable CA1801, CA1822
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
#pragma warning restore CA1801, CA1822
}
