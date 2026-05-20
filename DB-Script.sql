CREATE DATABASE company_management_db;
USE company_management_db;

DROP TABLE IF EXISTS Reports;
DROP TABLE IF EXISTS Task;
DROP TABLE IF EXISTS Project;
DROP TABLE IF EXISTS Head_of_Dept;
DROP TABLE IF EXISTS Employee;
DROP TABLE IF EXISTS Department;
DROP TABLE IF EXISTS Person;

CREATE TABLE Person (
    ID INT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Salary DECIMAL(10, 2) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Joining_Date DATE NOT NULL,
    PRIMARY KEY (ID)
);

CREATE TABLE Department (
    Dept_ID VARCHAR(10) NOT NULL,
    Dept_Name VARCHAR(100) NOT NULL,
    PRIMARY KEY (Dept_ID)
);

CREATE TABLE Employee (
    ID INT NOT NULL,
    Role VARCHAR(100) NOT NULL,
    Dept_ID VARCHAR(10),
    PRIMARY KEY (ID),
    FOREIGN KEY (ID) REFERENCES Person(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (Dept_ID) REFERENCES Department(Dept_ID) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Head_of_Dept (
    ID INT NOT NULL,
    Office_No VARCHAR(50) NOT NULL,
    Dept_ID VARCHAR(10),
    PRIMARY KEY (ID),
    FOREIGN KEY (ID) REFERENCES Person(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (Dept_ID) REFERENCES Department(Dept_ID) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Project (
    Project_ID VARCHAR(10) NOT NULL,
    Project_Name VARCHAR(150) NOT NULL,
    Dept_ID VARCHAR(10),
    Start_Date DATE NOT NULL,
    Status VARCHAR(50) NOT NULL,
    PRIMARY KEY (Project_ID),
    FOREIGN KEY (Dept_ID) REFERENCES Department(Dept_ID) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Task (
    Task_ID VARCHAR(10) NOT NULL,
    Title VARCHAR(150) NOT NULL,
    Task_description TEXT,
    Priority VARCHAR(20) NOT NULL,
    Deadline DATE NOT NULL,
    Project_ID VARCHAR(10),
    Employee_ID INT,
    PRIMARY KEY (Task_ID),
    FOREIGN KEY (Project_ID) REFERENCES Project(Project_ID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (Employee_ID) REFERENCES Employee(ID) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Reports (
    Report_ID VARCHAR(10) NOT NULL,
    Report_Title VARCHAR(150) NOT NULL,
    Report_Description TEXT,
    Submission_Date DATE NOT NULL,
    Status_Update VARCHAR(50) NOT NULL,
    PRIMARY KEY (Report_ID)
);

INSERT INTO Person (ID, Name, Email, Salary, Password, Joining_Date) VALUES
(101, 'Moeez Ahmad', 'moeez@tech.com', 95000.00, 'hash_p@ss1', '2025-06-15'),
(102, 'Aisha Khan', 'aisha@tech.com', 110000.00, 'secure_432', '2024-01-10'),
(103, 'Hamza Ali', 'hamza@tech.com', 65000.00, 'dev_pwd99', '2026-02-01');

INSERT INTO Department (Dept_ID, Dept_Name) VALUES
('D_01', 'Engineering & Development'),
('D_02', 'Marketing & UI Design');

INSERT INTO Employee (ID, Role, Dept_ID) VALUES
(101, 'Lead Web Engineer', 'D_01'),
(103, 'Junior Developer', 'D_01');

INSERT INTO Head_of_Dept (ID, Office_No, Dept_ID) VALUES
(102, 'Room-404', 'D_01');

INSERT INTO Project (Project_ID, Project_Name, Dept_ID, Start_Date, Status) VALUES
('P_99', 'Color Magic', 'D_01', '2026-01-05', 'Active'),
('P_100', 'Portfolio Optimization', 'D_01', '2026-05-10', 'Planning');

INSERT INTO Task (Task_ID, Title, Task_description, Priority, Deadline, Project_ID, Employee_ID) VALUES
('T_501', 'Refactor Core Auth', 'Optimize login routing', 'High', '2026-05-25', 'P_99', 101),
('T_502', 'Setup GitHub Actions', 'Automate FTP deployment workflow', 'Medium', '2026-05-30', 'P_99', 101);

INSERT INTO Reports (Report_ID, Report_Title, Report_Description, Submission_Date, Status_Update) VALUES
('R_01', 'Sprint 2 Deployment', 'Automated workflow verified successfully', '2026-05-19', 'Approved');
