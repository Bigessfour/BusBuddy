#!/bin/bash
# This script runs the initialization SQL scripts in the container

# Wait for SQL Server to start
sleep 30

# Run the initialization scripts
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /docker-entrypoint-initdb.d/init-db.sql

echo "SQL initialization completed"
