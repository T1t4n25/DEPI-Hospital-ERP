#!/bin/bash
set -e

# Run the default SQL Server entrypoint first
/opt/mssql/bin/sqlservr &
SQL_PID=$!

# Wait for SQL Server to be ready (max 60 seconds)
echo "Waiting for SQL Server to start..."
for i in {1..60}; do
  if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" &>/dev/null 2>&1; then
    echo "SQL Server is ready!"
    break
  fi
  if [ $i -eq 60 ]; then
    echo "Error: SQL Server failed to start after 60 seconds"
    kill $SQL_PID 2>/dev/null || true
    exit 1
  fi
  sleep 1
done

# Run initialization scripts from /docker-entrypoint-initdb.d/
if [ -d /docker-entrypoint-initdb.d ]; then
  echo "Running initialization scripts..."
  for f in /docker-entrypoint-initdb.d/*.sql; do
    if [ -f "$f" ]; then
      echo "Executing initialization script: $f"
      if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -i "$f"; then
        echo "Successfully executed: $f"
      else
        echo "Warning: Failed to execute $f (this might be normal if objects already exist)"
      fi
    fi
  done
  echo "Initialization scripts completed"
fi

# Wait for SQL Server process
wait $SQL_PID

