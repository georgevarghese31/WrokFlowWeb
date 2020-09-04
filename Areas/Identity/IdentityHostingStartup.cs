using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WrokFlowWeb.Areas.Identity.Data;
using WrokFlowWeb.Data;

[assembly: HostingStartup(typeof(WrokFlowWeb.Areas.Identity.IdentityHostingStartup))]
namespace WrokFlowWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WrokFlowWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WrokFlowWebContextConnection")));

            });
        }
    }
}