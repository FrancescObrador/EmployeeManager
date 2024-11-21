---------- Database for WPF final project ----------
---------- Drop tables if they exist ----------

USE HumanResourcesManager
GO

IF OBJECT_ID('employee_project', 'U') IS NOT NULL DROP TABLE [employee_project];
IF OBJECT_ID('time_off_request', 'U') IS NOT NULL DROP TABLE [time_off_request];
IF OBJECT_ID('payroll', 'U') IS NOT NULL DROP TABLE [payroll];
IF OBJECT_ID('employee', 'U') IS NOT NULL DROP TABLE [employee];
IF OBJECT_ID('project', 'U') IS NOT NULL DROP TABLE [project];
IF OBJECT_ID('department', 'U') IS NOT NULL DROP TABLE [department];
GO

---------- Create tables ----------

CREATE TABLE [department] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [name] varchar(255),
  [description] varchar(255)
)
GO

CREATE TABLE [employee] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [first_name] varchar(255),
  [last_name] varchar(255),
  [email] varchar(255) UNIQUE,
  [phone_number] varchar(50),
  [hire_date] date,
  [salary] decimal(10,2),
  [department_id] integer,
  [position] varchar(100),
  [date_of_birth] date,
  [picture] varbinary(max)
)
GO

CREATE TABLE [project] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [name] varchar(255),
  [description] text,
  [start_date] date,
  [end_date] date,
  [budget] decimal(15,2),
  [status] nvarchar(255) NOT NULL CHECK ([status] IN ('upcoming', 'active', 'canceled'))
)
GO

CREATE TABLE [employee_project] (
  [employee_id] integer,
  [project_id] integer,
  [role] varchar(100),
  [start_date] date,
  [end_date] date
)
GO

CREATE TABLE [time_off_request] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [employee_id] integer,
  [start_date] date,
  [end_date] date,
  [type] nvarchar(255) NOT NULL CHECK ([type] IN ('vacation', 'sick_leave')),
  [status] nvarchar(255) NOT NULL CHECK ([status] IN ('approved', 'pending', 'rejected')),
  [request_date] date
)
GO

CREATE TABLE [payroll] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [employee_id] integer,
  [pay_date] date,
  [gross_salary] decimal(10,2),
  [deductions] decimal(10,2),
  [net_salary] decimal(10,2)
)
GO

CREATE INDEX [employee_index_0] ON [employee] ("department_id")
GO

CREATE INDEX [employee_project_index_1] ON [employee_project] ("employee_id", "project_id")
GO

CREATE INDEX [time_off_request_index_2] ON [time_off_request] ("employee_id")
GO

CREATE INDEX [payroll_index_3] ON [payroll] ("employee_id")
GO

ALTER TABLE [employee] ADD FOREIGN KEY ([department_id]) REFERENCES [department] ([id])
GO

ALTER TABLE [employee_project] ADD FOREIGN KEY ([employee_id]) REFERENCES [employee] ([id])
GO

ALTER TABLE [employee_project] ADD FOREIGN KEY ([project_id]) REFERENCES [project] ([id])
GO

ALTER TABLE [time_off_request] ADD FOREIGN KEY ([employee_id]) REFERENCES [employee] ([id])
GO

ALTER TABLE [payroll] ADD FOREIGN KEY ([employee_id]) REFERENCES [employee] ([id])
GO

---------- Populate ----------

USE TestWPF;
GO

-- Generar 10 departamentos ficticios
INSERT INTO department (name, description)
VALUES 
('HR', 'Human Resources Department'),
('IT', 'Information Technology Department'),
('Finance', 'Finance and Accounting Department'),
('Marketing', 'Marketing and PR Department'),
('Operations', 'Operations Management Department'),
('Sales', 'Sales and Business Development'),
('Customer Service', 'Customer Support Department'),
('R&D', 'Research and Development'),
('Logistics', 'Logistics and Supply Chain'),
('Admin', 'Administrative Department');

-- Variables para datos ficticios
DECLARE @MaleFirstNames TABLE (name VARCHAR(255));
INSERT INTO @MaleFirstNames VALUES
('John'), ('Michael'), ('Chris'), ('David'), ('Daniel'),
('James'), ('Robert'), ('William'), ('Joseph'), ('Mark'),
('Kevin'), ('Paul'), ('Brian'), ('Jason'), ('Ryan');

DECLARE @FemaleFirstNames TABLE (name VARCHAR(255));
INSERT INTO @FemaleFirstNames VALUES
('Jane'), ('Emily'), ('Jessica'), ('Sarah'), ('Laura'),
('Anna'), ('Sophia'), ('Olivia'), ('Emma'), ('Isabella'),
('Grace'), ('Mia'), ('Chloe'), ('Lily'), ('Amelia');

DECLARE @LastNames TABLE (name VARCHAR(255));
INSERT INTO @LastNames VALUES
('Smith'), ('Johnson'), ('Williams'), ('Brown'), ('Jones'),
('Garcia'), ('Miller'), ('Davis'), ('Rodriguez'), ('Martinez'),
('Taylor'), ('Anderson'), ('Thomas'), ('Hernandez'), ('Moore');

DECLARE @Positions TABLE (position VARCHAR(100));
INSERT INTO @Positions VALUES
('Manager'), ('Developer'), ('Analyst'), ('Coordinator'),
('Engineer'), ('Consultant'), ('Specialist'), ('Assistant'),
('Technician'), ('Director');

-- Generar 100 empleados ficticios
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @FirstName VARCHAR(255);
IF RAND() < 0.5 -- Probabilidad del 50%
    SET @FirstName = (SELECT TOP 1 name FROM @MaleFirstNames ORDER BY NEWID());
ELSE
    SET @FirstName = (SELECT TOP 1 name FROM @FemaleFirstNames ORDER BY NEWID());

    DECLARE @LastName VARCHAR(255) = (SELECT TOP 1 name FROM @LastNames ORDER BY NEWID());
    DECLARE @Email VARCHAR(255) = LOWER(@FirstName + '.' + @LastName + CAST(@i AS VARCHAR(10)) + '@example.com');
    DECLARE @PhoneNumber VARCHAR(50) = '555-' + CAST(FLOOR(RAND() * 10000) AS VARCHAR(4)) + '-' + CAST(FLOOR(RAND() * 10000) AS VARCHAR(4));
    DECLARE @HireDate DATE = DATEADD(DAY, -FLOOR(RAND() * 1000), GETDATE());
    DECLARE @Salary DECIMAL(10, 2) = ROUND(30000 + RAND() * 70000, 2);
    DECLARE @DepartmentId INT = FLOOR(RAND() * 10) + 1;
    DECLARE @Position VARCHAR(100) = (SELECT TOP 1 position FROM @Positions ORDER BY NEWID());
    DECLARE @DateOfBirth DATE = DATEADD(YEAR, -FLOOR(RAND() * 25) - 20, GETDATE());
    DECLARE @Picture VARBINARY(MAX) = NULL; -- Imagen vacía por simplicidad

    INSERT INTO employee (first_name, last_name, email, phone_number, hire_date, salary, department_id, position, date_of_birth, picture)
    VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @HireDate, @Salary, @DepartmentId, @Position, @DateOfBirth, @Picture);

    SET @i = @i + 1;
END;
GO
