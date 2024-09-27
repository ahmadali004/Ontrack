﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ontrack.Data;

#nullable disable

namespace Ontrack.Migrations
{
    [DbContext(typeof(SchoolContext))]
    partial class SchoolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ontrack.Models.Class", b =>
                {
                    b.Property<int>("ClassID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassID"));

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TeacherID")
                        .HasColumnType("int");

                    b.HasKey("ClassID");

                    b.HasIndex("TeacherID");

                    b.ToTable("Classes", (string)null);
                });

            modelBuilder.Entity("Ontrack.Models.ClassTeacher", b =>
                {
                    b.Property<int>("ClassTeacherID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassTeacherID"));

                    b.Property<int>("ClassID")
                        .HasColumnType("int");

                    b.Property<int?>("TeacherID")
                        .HasColumnType("int");

                    b.HasKey("ClassTeacherID");

                    b.HasIndex("ClassID");

                    b.HasIndex("TeacherID");

                    b.ToTable("ClassTeachers");
                });

            modelBuilder.Entity("Ontrack.Models.Examination", b =>
                {
                    b.Property<int>("ExaminationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExaminationID"));

                    b.Property<int>("ClassID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Score")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("StudentID")
                        .HasColumnType("int");

                    b.Property<int>("SubjectID")
                        .HasColumnType("int");

                    b.HasKey("ExaminationID");

                    b.HasIndex("ClassID");

                    b.HasIndex("StudentID");

                    b.HasIndex("SubjectID");

                    b.ToTable("Examinations", (string)null);
                });

            modelBuilder.Entity("Ontrack.Models.Parent", b =>
                {
                    b.Property<int>("ParentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParentID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParentID");

                    b.ToTable("Parents", (string)null);
                });

            modelBuilder.Entity("Ontrack.Models.Payment", b =>
                {
                    b.Property<int>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ParentID")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("PaymentID");

                    b.HasIndex("ParentID");

                    b.HasIndex("StudentID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Ontrack.Models.Student", b =>
                {
                    b.Property<int>("StudentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClassID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentID")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentID");

                    b.HasIndex("ClassID");

                    b.HasIndex("ParentID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Ontrack.Models.StudentExamResult", b =>
                {
                    b.Property<int>("StudentExamResultID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentExamResultID"));

                    b.Property<int>("ExaminationID")
                        .HasColumnType("int");

                    b.Property<decimal>("Score")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("StudentExamResultID");

                    b.HasIndex("ExaminationID");

                    b.HasIndex("StudentID");

                    b.ToTable("ExamResults", (string)null);
                });

            modelBuilder.Entity("Ontrack.Models.Subject", b =>
                {
                    b.Property<int>("SubjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudentID")
                        .HasColumnType("int");

                    b.Property<int?>("TeacherID")
                        .HasColumnType("int");

                    b.HasKey("SubjectID");

                    b.HasIndex("StudentID");

                    b.HasIndex("TeacherID");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Ontrack.Models.Teacher", b =>
                {
                    b.Property<int>("TeacherID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeacherID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeacherID");

                    b.ToTable("Teachers", (string)null);
                });

            modelBuilder.Entity("Ontrack.Models.Class", b =>
                {
                    b.HasOne("Ontrack.Models.Teacher", "teacher")
                        .WithMany("Classes")
                        .HasForeignKey("TeacherID");

                    b.Navigation("teacher");
                });

            modelBuilder.Entity("Ontrack.Models.ClassTeacher", b =>
                {
                    b.HasOne("Ontrack.Models.Class", "Class")
                        .WithMany("ClassTeachers")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ontrack.Models.Teacher", "Teacher")
                        .WithMany("ClassTeachers")
                        .HasForeignKey("TeacherID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Class");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Ontrack.Models.Examination", b =>
                {
                    b.HasOne("Ontrack.Models.Class", "Class")
                        .WithMany("Examinations")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ontrack.Models.Student", null)
                        .WithMany("Examinations")
                        .HasForeignKey("StudentID");

                    b.HasOne("Ontrack.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Ontrack.Models.Payment", b =>
                {
                    b.HasOne("Ontrack.Models.Parent", "Parent")
                        .WithMany("Payments")
                        .HasForeignKey("ParentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ontrack.Models.Student", "Student")
                        .WithMany("Payments")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Ontrack.Models.Student", b =>
                {
                    b.HasOne("Ontrack.Models.Class", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ontrack.Models.Parent", "Parent")
                        .WithMany("Students")
                        .HasForeignKey("ParentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Ontrack.Models.StudentExamResult", b =>
                {
                    b.HasOne("Ontrack.Models.Examination", "Examination")
                        .WithMany("StudentExamResults")
                        .HasForeignKey("ExaminationID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ontrack.Models.Student", "Student")
                        .WithMany("StudentExamResults")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Examination");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Ontrack.Models.Subject", b =>
                {
                    b.HasOne("Ontrack.Models.Student", null)
                        .WithMany("Subjects")
                        .HasForeignKey("StudentID");

                    b.HasOne("Ontrack.Models.Teacher", null)
                        .WithMany("Subjects")
                        .HasForeignKey("TeacherID");
                });

            modelBuilder.Entity("Ontrack.Models.Class", b =>
                {
                    b.Navigation("ClassTeachers");

                    b.Navigation("Examinations");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Ontrack.Models.Examination", b =>
                {
                    b.Navigation("StudentExamResults");
                });

            modelBuilder.Entity("Ontrack.Models.Parent", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Ontrack.Models.Student", b =>
                {
                    b.Navigation("Examinations");

                    b.Navigation("Payments");

                    b.Navigation("StudentExamResults");

                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("Ontrack.Models.Teacher", b =>
                {
                    b.Navigation("ClassTeachers");

                    b.Navigation("Classes");

                    b.Navigation("Subjects");
                });
#pragma warning restore 612, 618
        }
    }
}
