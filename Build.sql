USE HumanResourcesManager;

-- Eliminar tablas si existen
IF OBJECT_ID('employee_project', 'U') IS NOT NULL DROP TABLE [employee_project];
IF OBJECT_ID('time_off_request', 'U') IS NOT NULL DROP TABLE [time_off_request];
IF OBJECT_ID('payroll', 'U') IS NOT NULL DROP TABLE [payroll];
IF OBJECT_ID('employee', 'U') IS NOT NULL DROP TABLE [employee];
IF OBJECT_ID('project', 'U') IS NOT NULL DROP TABLE [project];
IF OBJECT_ID('department', 'U') IS NOT NULL DROP TABLE [department];

-- Crear tablas
CREATE TABLE [department] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [name] varchar(255),
  [description] varchar(255)
);

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
);

CREATE TABLE [project] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [name] varchar(255),
  [description] text,
  [start_date] date,
  [end_date] date,
  [budget] decimal(15,2),
  [status] nvarchar(255) NOT NULL CHECK ([status] IN ('upcoming', 'active', 'canceled'))
);

CREATE TABLE [employee_project] (
  [employee_id] integer,
  [project_id] integer,
  [role] varchar(100),
  [start_date] date,
  [end_date] date
);

CREATE TABLE [time_off_request] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [employee_id] integer,
  [start_date] date,
  [end_date] date,
  [type] nvarchar(255) NOT NULL CHECK ([type] IN ('vacation', 'sick_leave')),
  [status] nvarchar(255) NOT NULL CHECK ([status] IN ('approved', 'pending', 'rejected')),
  [request_date] date
);

CREATE TABLE [payroll] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [employee_id] integer,
  [pay_date] date,
  [gross_salary] decimal(10,2),
  [deductions] decimal(10,2),
  [net_salary] decimal(10,2)
);

-- Crear índices
CREATE INDEX [employee_index_0] ON [employee] (department_id);
CREATE INDEX [employee_project_index_1] ON [employee_project] (employee_id, project_id);
CREATE INDEX [time_off_request_index_2] ON [time_off_request] (employee_id);
CREATE INDEX [payroll_index_3] ON [payroll] (employee_id);

-- Crear claves foráneas con CASCADE en DELETE
ALTER TABLE [employee] ADD FOREIGN KEY ([department_id]) REFERENCES [department] ([id]);
ALTER TABLE [employee_project] ADD FOREIGN KEY ([employee_id]) REFERENCES [employee] ([id]) ON DELETE CASCADE;
ALTER TABLE [employee_project] ADD FOREIGN KEY ([project_id]) REFERENCES [project] ([id]);
ALTER TABLE [time_off_request] ADD FOREIGN KEY ([employee_id]) REFERENCES [employee] ([id]) ON DELETE CASCADE;

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
('Juan'), ('Miguel'), ('Carlos'), ('David'), ('Daniel'),
('Jaime'), ('Roberto'), ('Guillermo'), ('José'), ('Marc'),
('Fernando'), ('Pablo'), ('Álvaro'), ('Jesús'), ('Raúl');

DECLARE @FemaleFirstNames TABLE (name VARCHAR(255));
INSERT INTO @FemaleFirstNames VALUES
('María'), ('Emilia'), ('Jessica'), ('Sara'), ('Laura'),
('Ana'), ('Sofía'), ('Olivia'), ('Emma'), ('Isabela'),
('Gracia'), ('Mía'), ('Clara'), ('Lola'), ('Amelia');

DECLARE @LastNames TABLE (name VARCHAR(255));
INSERT INTO @LastNames VALUES
('García'), ('Martínez'), ('López'), ('Sánchez'), ('Pérez'),
('Ferrer'), ('Costa'), ('Navarro'), ('Serra'), ('Moll'),
('Moreno'), ('Castillo'), ('Ribas'), ('Segura'), ('Ortega');

DECLARE @Positions TABLE (position VARCHAR(100));
INSERT INTO @Positions VALUES
('Manager'), ('Developer'), ('Analyst'), ('Coordinator'),
('Engineer'), ('Consultant'), ('Specialist'), ('Assistant'),
('Technician'), ('Director');

-- Generar 50 empleados ficticios
DECLARE @i INT = 1;
WHILE @i <= 50
BEGIN
    DECLARE @FirstName VARCHAR(255);
    IF RAND() < 0.5
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
    DECLARE @Picture VARBINARY(MAX) = NULL;

    INSERT INTO employee (first_name, last_name, email, phone_number, hire_date, salary, department_id, position, date_of_birth, picture)
    VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @HireDate, @Salary, @DepartmentId, @Position, @DateOfBirth, @Picture);

    SET @i = @i + 1;
END;

-- Insertar 3 proyectos
INSERT INTO project (name, description, start_date, end_date, budget, status)
VALUES 
('Project A', 'Development of a new software platform', '2024-01-01', '2024-12-31', 500000, 'active'),
('Project B', 'Implementation of a company-wide ERP system', '2024-03-01', '2024-11-30', 300000, 'active'),
('Project C', 'Research and development of AI tools', '2024-02-15', '2024-09-30', 200000, 'upcoming');

-- Asignar empleados a proyectos
DECLARE @EmployeeCount INT = (SELECT COUNT(*) FROM employee);
DECLARE @ProjectCount INT = (SELECT COUNT(*) FROM project);
SET @i = 1;

WHILE @i <= @EmployeeCount
BEGIN
    DECLARE @EmployeeId INT = (SELECT id FROM employee ORDER BY id OFFSET @i - 1 ROWS FETCH NEXT 1 ROWS ONLY);
    DECLARE @PositionRol VARCHAR(100) = (SELECT position FROM employee WHERE id = @EmployeeId);
    DECLARE @ProjectId INT = (SELECT id FROM project ORDER BY NEWID() OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY);

    INSERT INTO employee_project (employee_id, project_id, role, start_date, end_date)
    VALUES (@EmployeeId, @ProjectId, @PositionRol, GETDATE(), NULL);

    IF RAND() < 0.5
    BEGIN
        DECLARE @SecondProjectId INT = (SELECT TOP 1 id FROM project WHERE id != @ProjectId ORDER BY NEWID());
        IF @SecondProjectId IS NOT NULL
        BEGIN
            INSERT INTO employee_project (employee_id, project_id, role, start_date, end_date)
            VALUES (@EmployeeId, @SecondProjectId, @PositionRol, GETDATE(), NULL);
        END
    END

    SET @i = @i + 1;
END;

-- Generar nóminas
DECLARE @CurrentMonth DATE = GETDATE();
DECLARE @IncreaseRate DECIMAL(5,4) = 1.025;

DELETE FROM payroll;

WITH PayrollCTE AS (
    SELECT 
        id as employee_id,
        CAST(salary AS DECIMAL(10,2)) as initial_salary,
        0 as month_offset
    FROM employee
    
    UNION ALL
    
    SELECT 
        employee_id,
        CAST(initial_salary * POWER(@IncreaseRate, month_offset) AS DECIMAL(10,2)) as initial_salary,
        month_offset + 1
    FROM PayrollCTE
    WHERE month_offset < 11
)
INSERT INTO payroll (employee_id, pay_date, gross_salary, deductions, net_salary)
SELECT 
    employee_id,
    DATEADD(MONTH, -month_offset, @CurrentMonth) as pay_date,
    ROUND(initial_salary, 2) as gross_salary,
    ROUND(initial_salary * 0.25, 2) as deductions,
    ROUND(initial_salary * 0.75, 2) as net_salary
FROM PayrollCTE
ORDER BY employee_id, pay_date;
