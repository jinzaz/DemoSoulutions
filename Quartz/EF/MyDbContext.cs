using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using QuartzDemo.EF.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzDemo.EF
{
    public class MyDbContext : DbContext
    {
        public DbSet<TB_User> TB_User { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env , MyDbContext DbContext)
        {
            DbContext.Database.EnsureCreated();//如果数据库不存在的话，自动创建
        }
    }
}
