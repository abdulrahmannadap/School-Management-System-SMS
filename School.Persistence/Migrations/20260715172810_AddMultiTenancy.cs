using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiTenancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_GRNumber",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StaffMembers_EmployeeCode",
                table: "StaffMembers");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_ISBN",
                table: "LibraryBooks");

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Vouchers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "TeacherSubjectMaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentRfids",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentRemarks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentParents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentLeaveRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentContacts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentAttendances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentAdmissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StudentAddresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StockLedgers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffSupervisors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffSignatures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffSalaries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffRfids",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffRfidLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffPhotos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffMembers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffLeaves",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffHolidays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffGroupMaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "StaffAttendances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "SalaryMasters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ProductVariants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ParentStudentLinks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ParentAppStatuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Packagings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Mcqs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "McqAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "MarksEntries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "LibraryBooks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "LeaveTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "LeaveBalances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "InvoiceItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "InventoryOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "GraceMarks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "FinancialYears",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "FeeRefunds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "FeePayments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "FeeMasters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "FeeLedgers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "FeeDiscounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "FeeApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ExamSeatNos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ExamResults",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ExamRemarks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ExamMasters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ExamGroupMaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ExamDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ExamCategoryMaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Divisions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "DepositTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "DepositMasters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "CreditPayments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ClassTeacherMaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ClassBankMappings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Cheques",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "BookReturns",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "BookLedgers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "BookIssues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "BookCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Batches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "AcademicYears",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId_GRNumber",
                table: "Students",
                columns: new[] { "SchoolId", "GRNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_SchoolId_EmployeeCode",
                table: "StaffMembers",
                columns: new[] { "SchoolId", "EmployeeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_SchoolId_ISBN",
                table: "LibraryBooks",
                columns: new[] { "SchoolId", "ISBN" },
                unique: true);

            // ── Backfill: assign all pre-existing data to a default school ──
            migrationBuilder.Sql(@"
                INSERT INTO Schools (Id, Name, Address, ContactEmail, ContactPhone, IsActive, CreatedAt)
                VALUES ('11111111-1111-1111-1111-111111111111', 'Wisdom Academy', '', '', '', 1, GETUTCDATE());
            ");

            migrationBuilder.Sql("UPDATE [AcademicYears] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Batches] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [BookCategories] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [BookIssues] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [BookLedgers] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [BookReturns] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Categories] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Cheques] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ClassBankMappings] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ClassTeacherMaps] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Classes] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [CreditPayments] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [DepositMasters] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [DepositTransactions] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Divisions] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ExamCategoryMaps] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ExamDetails] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ExamGroupMaps] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ExamMasters] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ExamRemarks] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ExamResults] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ExamSeatNos] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Expenses] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [FeeApplications] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [FeeDiscounts] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [FeeLedgers] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [FeeMasters] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [FeePayments] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [FeeRefunds] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [FinancialYears] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [GraceMarks] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [InventoryOrders] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [InvoiceItems] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Invoices] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [LeaveBalances] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [LeaveTypes] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [LibraryBooks] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [MarksEntries] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [McqAnswers] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Mcqs] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Packagings] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ParentAppStatuses] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ParentStudentLinks] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [ProductVariants] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Products] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [SalaryMasters] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffAttendances] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffDocuments] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffGroupMaps] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffGroups] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffHolidays] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffLeaves] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffMembers] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffPhotos] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffRfidLogs] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffRfids] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffSalaries] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffSignatures] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StaffSupervisors] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StockLedgers] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentAddresses] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentAdmissions] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentAttendances] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentContacts] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentDocuments] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentLeaveRequests] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentParents] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentRemarks] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [StudentRfids] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Students] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Subjects] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [TeacherSubjectMaps] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");
            migrationBuilder.Sql("UPDATE [Vouchers] SET [SchoolId] = '11111111-1111-1111-1111-111111111111';");

            migrationBuilder.Sql("UPDATE [Users] SET [SchoolId] = '11111111-1111-1111-1111-111111111111' WHERE [Role] <> 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Students_SchoolId_GRNumber",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StaffMembers_SchoolId_EmployeeCode",
                table: "StaffMembers");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_SchoolId_ISBN",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "TeacherSubjectMaps");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentRfids");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentRemarks");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentParents");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentLeaveRequests");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentContacts");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentAdmissions");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StockLedgers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffSupervisors");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffSignatures");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffSalaries");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffRfids");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffRfidLogs");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffPhotos");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffMembers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffLeaves");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffHolidays");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffGroups");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffGroupMaps");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffDocuments");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StaffAttendances");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "SalaryMasters");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ParentStudentLinks");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ParentAppStatuses");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Packagings");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "McqAnswers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "MarksEntries");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "LeaveBalances");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "InventoryOrders");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "GraceMarks");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FinancialYears");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeeRefunds");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeePayments");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeeMasters");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeeLedgers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeeDiscounts");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeeApplications");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ExamSeatNos");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ExamRemarks");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ExamMasters");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ExamGroupMaps");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ExamDetails");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ExamCategoryMaps");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "DepositTransactions");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "DepositMasters");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "CreditPayments");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ClassTeacherMaps");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ClassBankMappings");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Cheques");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "BookReturns");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "BookLedgers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "BookIssues");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "AcademicYears");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GRNumber",
                table: "Students",
                column: "GRNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_EmployeeCode",
                table: "StaffMembers",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_ISBN",
                table: "LibraryBooks",
                column: "ISBN",
                unique: true);
        }
    }
}
