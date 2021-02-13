#!/bin/bash
#start SQL Server
sh -c "
echo 'Sleeping 20 seconds before running setup script'
sleep 20
echo 'Starting setup script'
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P IamTan86 -i /app/Migration_CreateDB.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P IamTan86 -i /app/Migration_ApplicationDBContext.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P IamTan86 -i /app/Migration_MyAlbumDbContext.sql
echo 'Finished setup script'
exit" &
exec /opt/mssql/bin/sqlservr