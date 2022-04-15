#!/usr/bin/env bash
./wait-for-it.sh -s db:1433 -t 0 -- /opt/mssql-tools/bin/sqlcmd -S db,1433 -U SA -P 'w6&BEZ-6a7]93%rC' -i ./init.sql
exit 0