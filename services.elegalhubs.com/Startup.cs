using library.elegalhubs.com.lms.Admin.Reports;
using library.elegalhubs.com.Notification;
using library.elegalhubs.com.Ticketing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using services.elegalhubs.com.Repository;
using services.elegalhubs.com.Repository.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com
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
            services.AddScoped<ICommanRepository<CategoryTypes>, CategoryTypesRepository>();
            services.AddScoped<ICommanRepository<ContactMethods>, ContactMethodsRepository>();
            services.AddScoped<ICommanRepository<ContactTypes>, ContactTypesRepository>();
            services.AddScoped<ICommanRepository<PriorityTypes>, PriorityTypesRepository>();
            services.AddScoped<ICommanRepository<StatusTypes>, StatusTypesRepository>();
            services.AddScoped<ICommanRepository<TicketDetails>, TicketDetailsRepository>();
            services.AddScoped<ICommanRepository<Tickets>, TicketsRepository>();
            services.AddScoped<ICommanRepository<TicketTypes>, TicketTypeRepository>();
            services.AddScoped<ICommanRepository<User>, UsersRepository>();
            services.AddScoped<ICommanRepository<Departments>, DepartmentsRepository>();
            services.AddScoped<ICommanRepository<Projects>, ProjectsRepository>();
            services.AddScoped<ICommanRepository<TicketAttachements>, TicketAttachementsRepository>();
            services.AddScoped<ICommanRepository<TicketComments>, TicketCommentsRepository>();
            services.AddScoped<ICommanRepository<Notifications>, NotificationRepository>();
            services.AddScoped<ICommanRepository<NotificationTypes>, NotificationTypeRepository>();
            services.AddScoped<TicketDetailsRepository, TicketDetailsRepository>();
            services.AddScoped<TicketAttachementsRepository, TicketAttachementsRepository>();
            services.AddScoped<TicketCommentsRepository, TicketCommentsRepository>();
            services.AddScoped<UsersRepository, UsersRepository>();
            services.AddScoped<IndexReport, IndexReport>();
            services.AddScoped<ReportsRepository, ReportsRepository>();
            services.AddScoped<TicketsRepository, TicketsRepository>();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
