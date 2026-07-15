using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Entities.Exam;
using School.Domain.Entities.Fees;
using School.Domain.Entities.Inventory;
using School.Domain.Entities.Library;
using School.Domain.Entities.Masters;
using School.Domain.Entities.Staff;
using School.Domain.Entities.Student;

namespace School.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // ── Auth ────────────────────────────────────────────────
    public DbSet<User>               Users              => Set<User>();

    // ── Masters ─────────────────────────────────────────────
    public DbSet<FinancialYear>      FinancialYears     => Set<FinancialYear>();
    public DbSet<AcademicYear>       AcademicYears      => Set<AcademicYear>();
    public DbSet<Class>              Classes            => Set<Class>();
    public DbSet<Division>           Divisions          => Set<Division>();
    public DbSet<Batch>              Batches            => Set<Batch>();
    public DbSet<Subject>            Subjects           => Set<Subject>();

    // ── Student ─────────────────────────────────────────────
    public DbSet<Student>            Students           => Set<Student>();
    public DbSet<StudentParent>      StudentParents     => Set<StudentParent>();
    public DbSet<StudentAddress>     StudentAddresses   => Set<StudentAddress>();
    public DbSet<StudentContact>     StudentContacts    => Set<StudentContact>();
    public DbSet<StudentAdmission>   StudentAdmissions  => Set<StudentAdmission>();
    public DbSet<StudentAttendance>  StudentAttendances => Set<StudentAttendance>();
    public DbSet<StudentRemark>      StudentRemarks     => Set<StudentRemark>();
    public DbSet<StudentDocument>    StudentDocuments   => Set<StudentDocument>();
    public DbSet<StudentLeaveRequest>StudentLeaveRequests => Set<StudentLeaveRequest>();
    public DbSet<StudentRfid>        StudentRfids       => Set<StudentRfid>();
    public DbSet<ParentAppStatus>    ParentAppStatuses  => Set<ParentAppStatus>();
    public DbSet<ParentStudentLink>  ParentStudentLinks => Set<ParentStudentLink>();

    // ── Staff ───────────────────────────────────────────────
    public DbSet<Staff>              StaffMembers       => Set<Staff>();
    public DbSet<StaffAttendance>    StaffAttendances   => Set<StaffAttendance>();
    public DbSet<StaffRfid>          StaffRfids         => Set<StaffRfid>();
    public DbSet<StaffRfidLog>       StaffRfidLogs      => Set<StaffRfidLog>();
    public DbSet<StaffPhoto>         StaffPhotos        => Set<StaffPhoto>();
    public DbSet<StaffSignature>     StaffSignatures    => Set<StaffSignature>();
    public DbSet<SalaryMaster>       SalaryMasters      => Set<SalaryMaster>();
    public DbSet<StaffSalary>        StaffSalaries      => Set<StaffSalary>();
    public DbSet<ClassTeacherMap>    ClassTeacherMaps   => Set<ClassTeacherMap>();
    public DbSet<TeacherSubjectMap>  TeacherSubjectMaps => Set<TeacherSubjectMap>();
    public DbSet<StaffGroup>         StaffGroups        => Set<StaffGroup>();
    public DbSet<StaffGroupMap>      StaffGroupMaps     => Set<StaffGroupMap>();
    public DbSet<LeaveType>          LeaveTypes         => Set<LeaveType>();
    public DbSet<StaffLeave>         StaffLeaves        => Set<StaffLeave>();
    public DbSet<LeaveBalance>       LeaveBalances      => Set<LeaveBalance>();
    public DbSet<StaffHoliday>       StaffHolidays      => Set<StaffHoliday>();
    public DbSet<StaffDocument>      StaffDocuments     => Set<StaffDocument>();
    public DbSet<StaffSupervisor>    StaffSupervisors   => Set<StaffSupervisor>();

    // ── Fees ────────────────────────────────────────────────
    public DbSet<FeeMaster>          FeeMasters         => Set<FeeMaster>();
    public DbSet<FeeApplication>     FeeApplications    => Set<FeeApplication>();
    public DbSet<FeeLedger>          FeeLedgers         => Set<FeeLedger>();
    public DbSet<FeePayment>         FeePayments        => Set<FeePayment>();
    public DbSet<FeeDiscount>        FeeDiscounts       => Set<FeeDiscount>();
    public DbSet<DepositMaster>      DepositMasters     => Set<DepositMaster>();
    public DbSet<DepositTransaction> DepositTransactions=> Set<DepositTransaction>();
    public DbSet<Cheque>             Cheques            => Set<Cheque>();
    public DbSet<Voucher>            Vouchers           => Set<Voucher>();
    public DbSet<FeeRefund>          FeeRefunds         => Set<FeeRefund>();
    public DbSet<ClassBankMapping>   ClassBankMappings  => Set<ClassBankMapping>();

    // ── Exam ────────────────────────────────────────────────
    public DbSet<ExamMaster>         ExamMasters        => Set<ExamMaster>();
    public DbSet<ExamDetail>         ExamDetails        => Set<ExamDetail>();
    public DbSet<MarksEntry>         MarksEntries       => Set<MarksEntry>();
    public DbSet<ExamResult>         ExamResults        => Set<ExamResult>();
    public DbSet<ExamRemark>         ExamRemarks        => Set<ExamRemark>();
    public DbSet<GraceMark>          GraceMarks         => Set<GraceMark>();
    public DbSet<ExamCategoryMap>    ExamCategoryMaps   => Set<ExamCategoryMap>();
    public DbSet<ExamGroupMap>       ExamGroupMaps      => Set<ExamGroupMap>();
    public DbSet<Mcq>                Mcqs               => Set<Mcq>();
    public DbSet<McqAnswer>          McqAnswers         => Set<McqAnswer>();
    public DbSet<ExamSeatNo>         ExamSeatNos        => Set<ExamSeatNo>();

    // ── Inventory ───────────────────────────────────────────
    public DbSet<Category>           Categories         => Set<Category>();
    public DbSet<Product>            Products           => Set<Product>();
    public DbSet<ProductVariant>     ProductVariants    => Set<ProductVariant>();
    public DbSet<Packaging>          Packagings         => Set<Packaging>();
    public DbSet<StockLedger>        StockLedgers       => Set<StockLedger>();
    public DbSet<Invoice>            Invoices           => Set<Invoice>();
    public DbSet<InvoiceItem>        InvoiceItems       => Set<InvoiceItem>();
    public DbSet<CreditPayment>      CreditPayments     => Set<CreditPayment>();
    public DbSet<InventoryOrder>     InventoryOrders    => Set<InventoryOrder>();
    public DbSet<Expense>            Expenses           => Set<Expense>();

    // ── Library ─────────────────────────────────────────────
    public DbSet<BookCategory>       BookCategories     => Set<BookCategory>();
    public DbSet<LibraryBook>        LibraryBooks       => Set<LibraryBook>();
    public DbSet<BookIssue>          BookIssues         => Set<BookIssue>();
    public DbSet<BookReturn>         BookReturns        => Set<BookReturn>();
    public DbSet<BookLedger>         BookLedgers        => Set<BookLedger>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply decimal(18,2) globally to all decimal properties
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.FullName).HasMaxLength(100);
            e.Property(u => u.Email).HasMaxLength(150);
            e.Property(u => u.PasswordHash).HasMaxLength(256);
        });

        modelBuilder.Entity<Student>(e =>
            e.HasIndex(s => s.GRNumber).IsUnique());

        modelBuilder.Entity<Staff>(e =>
            e.HasIndex(s => s.EmployeeCode).IsUnique());

        modelBuilder.Entity<LibraryBook>(e =>
            e.HasIndex(b => b.ISBN).IsUnique());
    }
}
