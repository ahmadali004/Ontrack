@echo off
echo Generating controllers...

dotnet aspnet-codegenerator controller -name StudentController -m Student -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout
dotnet aspnet-codegenerator controller -name ClassController -m Class -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout
dotnet aspnet-codegenerator controller -name TeacherController -m Teacher -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout
dotnet aspnet-codegenerator controller -name SubjectController -m Subject -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout
dotnet aspnet-codegenerator controller -name ExaminationController -m Examination -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout
dotnet aspnet-codegenerator controller -name PaymentController -m Payment -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout

echo Controllers generated successfully.
pause
