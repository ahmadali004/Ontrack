using Microsoft.EntityFrameworkCore;
using Ontrack.Models;
namespace Ontrack.Data
{
	public class SchoolContext : DbContext
	{
		public SchoolContext(DbContextOptions<SchoolContext> options)
		: base(options)
		{ }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
			modelBuilder.Entity<Teacher>()
				.ToTable("Teachers")
				.HasKey(t => t.TeacherID);
			modelBuilder.Entity<Teacher>()
				.Property(t => t.TeacherID)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Parent>()
				.ToTable("Parents")
				.HasKey(p => p.ParentID);
			modelBuilder.Entity<Parent>()
				.Property(p => p.ParentID)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Student>()
				.HasOne(s => s.Parent)
				.WithMany(p => p.Students)
				.HasForeignKey(s => s.ParentID)
				.OnDelete(DeleteBehavior.Restrict); 

			modelBuilder.Entity<Class>()
				.ToTable("Classes")
				.HasKey(c => c.ClassID);
			modelBuilder.Entity<Class>()
				.Property(c => c.ClassID)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Examination>()
				.ToTable("Examinations")
				.HasKey(e => e.ExaminationID);
			modelBuilder.Entity<Examination>()
				.Property(e => e.ExaminationID)
				.ValueGeneratedOnAdd();
			modelBuilder.Entity<Examination>()
				.HasOne(e => e.Class)
				.WithMany(c => c.Examinations)
				.HasForeignKey(e => e.ClassID)
				.OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Payment>()
         .HasOne(p => p.Parent)
         .WithMany(p => p.Payments)
         .HasForeignKey(p => p.ParentID)
         .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
       .HasOne(p => p.Student)
       .WithMany(s => s.Payments)
       .HasForeignKey(p => p.StudentID)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassTeacher>()
				.HasOne(ct => ct.Teacher)
				.WithMany(t => t.ClassTeachers)
				.HasForeignKey(ct => ct.TeacherID)
				.OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<ClassTeacher>()
				.Property(ct => ct.ClassTeacherID)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<StudentExamsResult>()
				.ToTable("StudentExamsResult")
				.HasKey(er => er.StudentExamResultID);
			modelBuilder.Entity<StudentExamsResult>()
				.HasOne(er => er.Student)
				.WithMany(s => s.StudentExamsResult)
				.HasForeignKey(er => er.StudentID)
				.OnDelete(DeleteBehavior.Restrict); 
			modelBuilder.Entity<StudentExamsResult>()
				.HasOne(er => er.Examination)
				.WithMany(e => e.StudentExamsResult)
				.HasForeignKey(er => er.ExaminationID)
				.OnDelete(DeleteBehavior.Restrict); 
			
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Class> Classes { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Examination> Examinations { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Parent> Parents { get; set; }
        public DbSet<StudentExamsResult> StudentExamsResult { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    

        public DbSet<ClassTeacher> ClassTeachers { get; set; }
	public DbSet<Ontrack.Models.Subject> Subject { get; set; } = default!;
	    public DbSet<Ontrack.Models.Attendance> Attendance { get; set; } = default!;



	}
}