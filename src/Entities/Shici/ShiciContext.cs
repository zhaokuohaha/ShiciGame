using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Shici
{
    public class ShiciContext : DbContext
    {
		public DbSet<ShiciAns> shici_ans { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//将模型映射到自己指定的表
			modelBuilder.Configurations.Add(new EntityTypeConfiguration<User>().ToTable("t_user"));
			base.OnModelCreating(modelBuilder);
		}
	}
}
