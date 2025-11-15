-- SQL Server initialization script (if needed)
-- This file can be used for custom SQL initialization
-- Note: EF Core migrations handle schema creation, this is for additional setup if needed

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HospitalERP')
BEGIN
    CREATE DATABASE HospitalERP;
END
GO

USE HospitalERP;
GO

