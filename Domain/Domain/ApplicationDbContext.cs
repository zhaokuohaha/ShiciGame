﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		
		}
		
		public DbSet<ShiciAns> Shici_Ans { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<ShiciAns>().ToTable("shici_ans");
			base.OnModelCreating(builder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}
	}
}