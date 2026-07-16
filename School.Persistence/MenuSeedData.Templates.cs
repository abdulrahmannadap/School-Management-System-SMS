using School.Domain.Enums;

namespace School.Persistence;

public static partial class MenuSeedData
{
    public record TemplateItem(
        string? Section, string Label, string Icon,
        string? Area, string? Controller, string? Action,
        List<TemplateItem> Children);

    public static readonly Dictionary<UserRole, List<TemplateItem>> Templates = new()
    {
        [UserRole.SuperAdmin] =
        [
            new("Main", "Dashboard", "bi-speedometer2", "SuperAdmin", "Home", "Index", []),
            new("Management", "Schools", "bi-building", null, null, null, [
                new(null, "All Schools", "bi-list-ul", "SuperAdmin", "Schools", "Index", []),
            ]),
            new("Management", "Users", "bi-people", null, null, null, [
                new(null, "All Users", "bi-list-ul", null, null, null, []),
                new(null, "Add User", "bi-person-plus", null, null, null, []),
                new(null, "Roles", "bi-shield-check", null, null, null, []),
            ]),
            new("Management", "Reports", "bi-bar-chart-line", null, null, null, [
                new(null, "System Report", "bi-file-bar-chart", "SuperAdmin", "Reports", "SystemReport", []),
                new(null, "Activity Log", "bi-activity", null, null, null, []),
            ]),
            new("Management", "Settings", "bi-gear", null, null, null, [
                new(null, "General", "bi-sliders", null, null, null, []),
                new(null, "Email Config", "bi-envelope-gear", null, null, null, []),
                new(null, "Backup", "bi-database-gear", null, null, null, []),
            ]),
        ],
        [UserRole.SchoolAdmin] =
        [
            new("Main", "Dashboard", "bi-speedometer2", "SchoolAdmin", "Home", "Index", []),
            new("Academic", "Students", "bi-people-fill", null, null, null, [
                new(null, "All Students", "bi-list-ul", "SchoolAdmin", "Students", "Index", []),
                new(null, "Admission", "bi-person-plus", "SchoolAdmin", "Students", "Index", []),
                new(null, "Attendance", "bi-calendar-check", null, null, null, []),
                new(null, "Documents", "bi-file-earmark-person", null, null, null, []),
                new(null, "Leave Requests", "bi-door-open", null, null, null, []),
                new(null, "Portal Accounts", "bi-shield-lock", "SchoolAdmin", "PortalAccounts", "Index", []),
            ]),
            new("Academic", "Staff", "bi-person-badge", null, null, null, [
                new(null, "All Staff", "bi-list-ul", "SchoolAdmin", "Staff", "Index", []),
                new(null, "Add Staff", "bi-person-plus", "SchoolAdmin", "Staff", "Index", []),
                new(null, "Attendance", "bi-calendar-check", null, null, null, []),
                new(null, "Leave", "bi-calendar-x", null, null, null, []),
                new(null, "Salary", "bi-cash", null, null, null, []),
            ]),
            new("Academic", "Exams", "bi-file-earmark-text", null, null, null, [
                new(null, "Exam Master", "bi-journal-text", "SchoolAdmin", "Exam", "Index", []),
                new(null, "Marks Entry", "bi-pencil-square", null, null, null, []),
                new(null, "Results", "bi-award", null, null, null, []),
                new(null, "MCQ", "bi-patch-question", null, null, null, []),
            ]),
            new("Academic", "Library", "bi-book", null, null, null, [
                new(null, "Books", "bi-bookshelf", "SchoolAdmin", "Library", "Books", []),
                new(null, "Issue / Return", "bi-arrow-left-right", "SchoolAdmin", "Library", "IssueReturn", []),
                new(null, "Ledger", "bi-journal-bookmark", "SchoolAdmin", "Library", "BookLedger", []),
            ]),
            new("Finance", "Fees", "bi-cash-stack", null, null, null, [
                new(null, "Fee Master", "bi-collection", "SchoolAdmin", "Fees", "Index", []),
                new(null, "Deposit Master", "bi-safe", "SchoolAdmin", "Fees", "DepositMasters", []),
                new(null, "Collection", "bi-receipt", null, null, null, []),
                new(null, "Pending Fees", "bi-clock-history", null, null, null, []),
                new(null, "Discounts", "bi-percent", null, null, null, []),
            ]),
            new("Finance", "Inventory", "bi-box-seam", null, null, null, [
                new(null, "Products", "bi-boxes", "SchoolAdmin", "Inventory", "Products", []),
                new(null, "Stock Ledger", "bi-graph-up", "SchoolAdmin", "Inventory", "StockLedger", []),
                new(null, "Invoices", "bi-receipt", "SchoolAdmin", "Inventory", "Invoices", []),
                new(null, "Expenses", "bi-wallet2", "SchoolAdmin", "Inventory", "Expenses", []),
            ]),
            new("Finance", "Reports", "bi-bar-chart", null, null, null, [
                new(null, "Academic", "bi-file-bar-chart", null, null, null, []),
                new(null, "Financial", "bi-file-earmark-bar-graph", "SchoolAdmin", "Fees", "FeeAlerts", []),
                new(null, "Attendance", "bi-people", null, null, null, []),
            ]),
            new("Setup", "Masters", "bi-sliders", null, null, null, [
                new(null, "Academic Year", "bi-calendar3", "SchoolAdmin", "Masters", "AcademicYears", []),
                new(null, "Financial Year", "bi-calendar-week", "SchoolAdmin", "Masters", "FinancialYears", []),
                new(null, "Classes", "bi-grid", "SchoolAdmin", "Masters", "Classes", []),
                new(null, "Divisions", "bi-diagram-3", "SchoolAdmin", "Masters", "Divisions", []),
                new(null, "Batches", "bi-collection", "SchoolAdmin", "Masters", "Batches", []),
                new(null, "Subjects", "bi-journal", "SchoolAdmin", "Masters", "Subjects", []),
                new(null, "Holidays", "bi-flag", null, null, null, []),
            ]),
        ],
        [UserRole.Teacher] =
        [
            new("Main", "Dashboard", "bi-speedometer2", "Teacher", "Home", "Index", []),
            new("Classroom", "My Classes", "bi-grid-3x3-gap", null, null, null, [
                new(null, "Class List", "bi-list-ul", null, null, null, []),
                new(null, "My Students", "bi-people", null, null, null, []),
                new(null, "Timetable", "bi-calendar3", null, null, null, []),
            ]),
            new("Classroom", "Attendance", "bi-calendar-check", null, null, null, [
                new(null, "Mark Attendance", "bi-plus-circle", "Teacher", "Attendance", "Mark", []),
                new(null, "View Records", "bi-eye", "Teacher", "Attendance", "View", []),
            ]),
            new("Classroom", "Exams", "bi-pencil-square", null, null, null, [
                new(null, "Marks Entry", "bi-input-cursor-text", "Teacher", "Marks", "Entry", []),
                new(null, "Results", "bi-award", null, null, null, []),
            ]),
            new("Personal", "Leave", "bi-calendar-x", null, null, null, [
                new(null, "Apply Leave", "bi-plus-circle", null, null, null, []),
                new(null, "Leave History", "bi-clock-history", null, null, null, []),
                new(null, "Leave Balance", "bi-bar-chart", null, null, null, []),
            ]),
            new("Personal", "My Profile", "bi-person-circle", null, null, null, []),
        ],
        [UserRole.Accountant] =
        [
            new("Main", "Dashboard", "bi-speedometer2", "Accountant", "Home", "Index", []),
            new("Fees", "Fee Collection", "bi-cash-stack", null, null, null, [
                new(null, "Collect Fee", "bi-plus-circle", "Accountant", "Fees", "Index", []),
                new(null, "Pending Fees", "bi-clock-history", null, null, null, []),
                new(null, "Fee Ledger", "bi-receipt", null, null, null, []),
                new(null, "Refunds", "bi-arrow-counterclockwise", "Accountant", "Fees", "Index", []),
                new(null, "Discounts", "bi-percent", "Accountant", "Fees", "Index", []),
            ]),
            new("Banking", "Banking", "bi-bank", null, null, null, [
                new(null, "Cheques", "bi-check2-circle", "Accountant", "Fees", "Index", []),
                new(null, "Deposits", "bi-safe", "Accountant", "Fees", "Index", []),
                new(null, "Vouchers", "bi-file-earmark-text", "Accountant", "Fees", "Vouchers", []),
            ]),
            new("Billing", "Billing", "bi-receipt", null, null, null, [
                new(null, "New Invoice", "bi-file-earmark-plus", "SchoolAdmin", "Inventory", "NewInvoice", []),
                new(null, "All Invoices", "bi-list-ul", "SchoolAdmin", "Inventory", "Invoices", []),
                new(null, "Expenses", "bi-wallet2", "SchoolAdmin", "Inventory", "Expenses", []),
            ]),
            new("Billing", "Reports", "bi-bar-chart", null, null, null, [
                new(null, "Collection Report", "bi-file-bar-chart", "Accountant", "Fees", "Reports", []),
                new(null, "Expense Report", "bi-file-earmark-bar-graph", "Accountant", "Fees", "Reports", []),
                new(null, "Day Book", "bi-graph-up", "Accountant", "Fees", "Reports", []),
            ]),
        ],
        [UserRole.Staff] =
        [
            new("Main", "Dashboard", "bi-speedometer2", "Staff", "Home", "Index", []),
            new("My Account", "Attendance", "bi-calendar-check", null, null, null, [
                new(null, "My Records", "bi-eye", null, null, null, []),
                new(null, "Summary", "bi-bar-chart", null, null, null, []),
            ]),
            new("My Account", "Leave", "bi-calendar-x", null, null, null, [
                new(null, "Apply Leave", "bi-plus-circle", null, null, null, []),
                new(null, "Leave History", "bi-clock-history", null, null, null, []),
                new(null, "Leave Balance", "bi-bar-chart", null, null, null, []),
            ]),
            new("My Account", "Salary", "bi-cash", null, null, null, [
                new(null, "Salary Slips", "bi-receipt", null, null, null, []),
                new(null, "History", "bi-graph-up", null, null, null, []),
            ]),
            new("My Account", "My Profile", "bi-person-circle", null, null, null, []),
        ],
        [UserRole.Parent] =
        [
            new("Main", "Dashboard", "bi-speedometer2", "Parent", "Home", "Index", []),
            new("My Child", "Children", "bi-person-hearts", null, null, null, [
                new(null, "My Children", "bi-list-ul", "Parent", "Home", "Index", []),
            ]),
            new("My Child", "Attendance", "bi-calendar-check", null, null, null, [
                new(null, "View Attendance", "bi-eye", "Parent", "Home", "Index", []),
            ]),
            new("My Child", "Fees", "bi-cash-stack", null, null, null, [
                new(null, "Pending Fees", "bi-clock-history", "Parent", "Home", "Index", []),
                new(null, "Payment History", "bi-receipt", "Parent", "Home", "Index", []),
            ]),
            new("My Child", "Results", "bi-award", null, null, null, [
                new(null, "Exam Results", "bi-list-check", "Parent", "Home", "Index", []),
            ]),
            new("My Child", "Library", "bi-book", null, null, null, [
                new(null, "Issued Books", "bi-arrow-left-right", "Parent", "Home", "Index", []),
            ]),
        ],
        [UserRole.Student] =
        [
            new("Main", "Dashboard", "bi-speedometer2", "Student", "Home", "Index", []),
            new("My Portal", "Attendance", "bi-calendar-check", null, null, null, [
                new(null, "View Attendance", "bi-eye", "Student", "Attendance", "Index", []),
            ]),
            new("My Portal", "Exams", "bi-award", null, null, null, [
                new(null, "My Results", "bi-list-check", "Student", "Results", "Index", []),
                new(null, "MCQ Tests", "bi-patch-question", null, null, null, []),
            ]),
            new("My Portal", "Library", "bi-book", null, null, null, [
                new(null, "Issued Books", "bi-arrow-left-right", "Student", "Library", "Index", []),
            ]),
            new("My Portal", "Fees", "bi-cash-stack", null, null, null, [
                new(null, "Pending Fees", "bi-clock-history", "Student", "Fees", "Index", []),
                new(null, "Payment History", "bi-receipt", "Student", "Fees", "Index", []),
            ]),
            new("My Portal", "My Profile", "bi-person-circle", null, null, null, []),
        ],
    };
}
