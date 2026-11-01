namespace GymManagementPL.Extensions
{
    public static class GymApplicationPipelineExtensions
    {
        public static WebApplication UseGymManagementPipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}").WithStaticAssets();

            return app;
        }
    }
}
