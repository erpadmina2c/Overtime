using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Overtime.Repository;
using Overtime.Services;

namespace Overtime
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);//You can set Time   
            });
            services.AddDbContext<DBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionStr")));
            services.AddControllersWithViews();
            services.AddTransient<IUser, UserRepository>();
            services.AddTransient<IRole, RoleRepository>();
            services.AddTransient<IDepartment, DepartmentRepository>();
            services.AddTransient<IWorkflow, WorkflowRepository>();
            services.AddTransient<IWorkflowDetail, WorkflowDetailRepository>();
            services.AddTransient<IWorkflowTracker,WorkflowTrackerRepository>();
            services.AddTransient<IOverTimeRequest, OverTimeRequestRepository>();
            services.AddTransient<IDocuments, DocumentsRepository>();
            services.AddTransient<IHold, HoldRepository>();
            services.AddTransient<IRoleMenu, RoleMenuRepository>();
            services.AddTransient<IMenu, MenuRepository>();
            services.AddTransient<IUserDepartment, UserDeapatmentRepository>();
            services.AddTransient<IInsight, InsightRepository>();
            services.AddTransient<IHourRate, HourRateRepository>();
            services.AddTransient<IWorkingHour, WorkingHourRepository>();
            services.AddTransient<ILoginLog, LoginLogRepository>();
            services.AddTransient<IBioMatrix, BioMatrixRepository>();
            services.AddTransient<ITraining, TrainingRepository>();
            services.AddTransient<ILeavetype, LeavetypeRepository>();
            services.AddTransient<ILeaveDetail, LeaveDetailRepository>();
            services.AddTransient<IUserShift, UserShiftRepository>();
            services.AddTransient<IUserReportingHierarchy, UserReportingHierarchyRepository>();
            services.AddTransient<IFoodSchedule, FoodScheduleRepository>();
            services.AddTransient<IUserBioDepartment, UserBioDepartmentRepository>();
            services.AddTransient<IInAndOut, InAndOutRepository>();
            services.AddTransient<IAccomadation, AccomadationRepository>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
