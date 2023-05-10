using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Adirev.Interface;
using Adirev.Model;

namespace Adirev.Service
{
    class MSDatabase : IDatabase
    {
        #region Public Methods
        public List<string> GetDatabases(ServerManager server)
        {
            List<string> listDatabases = new List<string>();
            SqlConnection conn;
            if (server.Login == null)
            { conn = new SqlConnection($@"Data Source={server.ServerDatabase};Integrated Security=True"); }
            else
            { conn = new SqlConnection($@"Data Source={server.ServerDatabase};uid = {server.Login};pwd={server.Password}"); }

            try
            {
                if (server.ServerDatabase.Length == 0)
                { new NullReferenceException("Server name is empty"); }
                else
                {
                    conn.Open();

                    string query = "SELECT name FROM sysdatabases ORDER BY name ASC";
                    SqlCommand command = new SqlCommand(query, conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listDatabases.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new NullReferenceException("Login failed", ex);
            }
            finally
            {
                conn.Close();
            }

            return listDatabases;
        }

        public List<string> GetItems(DatabaseManager.TypeScript type, DatabaseManager database)
        {
            List<string> databaseItems = new List<string>();
            SqlConnection conn;

            if (database.Login == null)
            { conn = new SqlConnection($@"Data Source={database.Server};Initial Catalog={database.DatabaseEntity};Integrated Security=True"); }
            else
            { conn = new SqlConnection($@"Data Source={database.Server};Initial Catalog={database.DatabaseEntity};uid = {database.Login};pwd={database.Password}"); }

            try
            {
                if (database.Server.Length == 0)
                { new NullReferenceException("Server name is empty"); }
                else
                {
                    conn.Open();

                    #region Query
                    string query = " SELECT  s.name AS 'schema_name'						  "
                                    + "	,o.name												  "
                                    + "FROM sys.objects o									  "
                                    + "JOIN sys.schemas s ON s.schema_id = o.schema_id		  "
                                    + "WHERE type IN ('"
                                    + Enum.GetName(typeof(DatabaseManager.TypeScript), type)
                                    + "')                                                     "
                                    + "ORDER BY s.name, o.name ASC 							  ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name_item = reader["schema_name"].ToString() + "." + reader["name"].ToString();

                            databaseItems.Add(name_item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new NullReferenceException("Login failed", ex);
            }
            finally
            {
                conn.Close();
            }

            return databaseItems;
        }

        public List<DatabaseItem> GetItemsContents(DatabaseManager.TypeScript type, DatabaseManager database, DatabaseManager.OpcionExport opcionExport, List<string> downloadList = null)
        {
            List<DatabaseItem> listItemsContents = new List<DatabaseItem>();
            SqlConnection conn;

            if (database.Login == null)
            { conn = new SqlConnection($@"Data Source={database.Server};Initial Catalog={database.DatabaseEntity};Integrated Security=True"); }
            else
            { conn = new SqlConnection($@"Data Source={database.Server};Initial Catalog={database.DatabaseEntity};uid = {database.Login};pwd={database.Password}"); }

            if (database.Server.Length == 0)
            { new NullReferenceException("Server name is empty"); }

            conn.Open();

            try
            {
                if (type != DatabaseManager.TypeScript.U)
                {
                    string query = " SELECT object_definition(object_id) AS proc_definition   "
                                + "	,s.name AS 'schema_name'							  "
                                + "	,o.name												  "
                                + "FROM sys.objects o									  "
                                + "JOIN sys.schemas s ON s.schema_id = o.schema_id		  "
                                + "WHERE type IN ('"
                                + Enum.GetName(typeof(DatabaseManager.TypeScript), type)
                                + "')                                                     "
                                + "ORDER BY s.name, o.name ASC 							  ";


                    SqlCommand command = new SqlCommand(query, conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name_item = reader["schema_name"].ToString() + "." + reader["name"].ToString();
                            string content = reader["proc_definition"].ToString();

                            var is_export = downloadList?.Contains(name_item);
                            is_export = opcionExport == DatabaseManager.OpcionExport.ALL ? true : is_export;

                            if (is_export == true)
                            { listItemsContents.Add(new DatabaseItem() { Name = name_item, Contents = content }); }
                        }
                    }
                }
                else
                {
                    List<string> tables = GetItems(DatabaseManager.TypeScript.U, database);

                    foreach (var tableName in tables)
                    {
                        #region Query
                        string query = "DECLARE @table_name SYSNAME                                                                                                                                                      "
                        + " 																																			    	    	    	    						  "
                        + $"SELECT @table_name = '{tableName}'																											    	    	    	    				  "
                        + "																																				    	    	    	    					  "
                        + "DECLARE @object_name SYSNAME																													    	    	    	    					  "
                        + "	,@object_id INT																																    	    	    	    					  "
                        + "																																				    	    	    	    					  "
                        + "SELECT @object_name = '[' + s.name + '].[' + o.name + ']'																					    	    	    	    						  "
                        + "	,@object_id = o.[object_id]																													    	    	    	    					  "
                        + "FROM sys.objects o WITH (NOWAIT)																												    	    	    	    					  "
                        + "JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]																			    	    	    	    						  "
                        + "WHERE s.name + '.' + o.name = @table_name																									    	    	    	    						  "
                        + "	AND o.[type] = 'U'																															    	    	    	    					  "
                        + "	AND o.is_ms_shipped = 0																														    	    	    	    					  "
                        + "																																				    	    	    	    					  "
                        + "DECLARE @SQL NVARCHAR(MAX) = '';																												    	    	    	    					  "
                        + "																																				    	    	    	    					  "
                        + "WITH index_column																															    	    	    	    						  "
                        + "AS (																																			    	    	    	    					  "
                        + "	SELECT ic.[object_id]																														    	    	    	    					  "
                        + "		,ic.index_id																															    	    	    	    					  "
                        + "		,ic.is_descending_key																													    	    	    	    					  "
                        + "		,ic.is_included_column																													    	    	    	    					  "
                        + "		,c.name																																	    	    	    	    					  "
                        + "	FROM sys.index_columns ic WITH (NOWAIT)																										    	    	    	    					  "
                        + "	JOIN sys.columns c WITH (NOWAIT) ON ic.[object_id] = c.[object_id]																			    	    	    	    					  "
                        + "		AND ic.column_id = c.column_id																											    	    	    	    					  "
                        + "	WHERE ic.[object_id] = @object_id																											    	    	    	    					  "
                        + "	)																																			    	    	    	    					  "
                        + "	,fk_columns																																	    	    	    	    					  "
                        + "AS (																																			    	    	    	    					  "
                        + "	SELECT k.constraint_object_id																												    	    	    	    					  "
                        + "		,cname = c.name																															    	    	    	    					  "
                        + "		,rcname = rc.name																														    	    	    	    					  "
                        + "	FROM sys.foreign_key_columns k WITH (NOWAIT)																								    	    	    	    					  "
                        + "	JOIN sys.columns rc WITH (NOWAIT) ON rc.[object_id] = k.referenced_object_id																    	    	    	    					  "
                        + "		AND rc.column_id = k.referenced_column_id																								    	    	    	    					  "
                        + "	JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id																		    	    	    	    					  "
                        + "		AND c.column_id = k.parent_column_id																									    	    	    	    					  "
                        + "	WHERE k.parent_object_id = @object_id																										    	    	    	    					  "
                        + "	)																																			    	    	    	    					  "
                        + "SELECT @SQL = 'CREATE TABLE ' + @object_name + CHAR(13) + '(' + CHAR(13) + STUFF((															    	    	    	    					  "
                        + "			SELECT CHAR(9) + ', [' + c.name + '] ' + CASE 																						    	    	    	    					  "
                        + "					WHEN c.is_computed = 1																										    	    	    	    					  "
                        + "						THEN 'AS ' + cc.[definition]																							    	    	    	    					  "
                        + "					ELSE UPPER(tp.name) + CASE 																									    	    	    	    					  "
                        + "							WHEN tp.name IN (																									    	    	    	    					  "
                        + "									'varchar'																									    	    	    	    					  "
                        + "									,'char'																										    	    	    	    					  "
                        + "									,'varbinary'																								    	    	    	    					  "
                        + "									,'binary'																									    	    	    	    					  "
                        + "									,'text'																										    	    	    	    					  "
                        + "									)																											    	    	    	    					  "
                        + "								THEN '(' + CASE 																								    	    	    	    					  "
                        + "										WHEN c.max_length = - 1																					    	    	    	    					  "
                        + "											THEN 'MAX'																							    	    	    	    					  "
                        + "										ELSE CAST(c.max_length AS VARCHAR(5))																	    	    	    	    					  "
                        + "										END + ')'																								    	    	    	    					  "
                        + "							WHEN tp.name IN (																									    	    	    	    					  "
                        + "									'nvarchar'																									    	    	    	    					  "
                        + "									,'nchar'																									    	    	    	    					  "
                        + "									,'ntext'																									    	    	    	    					  "
                        + "									)																											    	    	    	    					  "
                        + "								THEN '(' + CASE 																								    	    	    	    					  "
                        + "										WHEN c.max_length = - 1																					    	    	    	    					  "
                        + "											THEN 'MAX'																							    	    	    	    					  "
                        + "										ELSE CAST(c.max_length / 2 AS VARCHAR(5))																    	    	    	    					  "
                        + "										END + ')'																								    	    	    	    					  "
                        + "							WHEN tp.name IN (																									    	    	    	    					  "
                        + "									'datetime2'																									    	    	    	    					  "
                        + "									,'time2'																									    	    	    	    					  "
                        + "									,'datetimeoffset'																							    	    	    	    					  "
                        + "									)																											    	    	    	    					  "
                        + "								THEN '(' + CAST(c.scale AS VARCHAR(5)) + ')'																	    	    	    	    					  "
                        + "							WHEN tp.name = 'decimal'																							    	    	    	    					  "
                        + "								THEN '(' + CAST(c.[precision] AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'							    	    	    	    					  "
                        + "							ELSE ''																												    	    	    	    					  "
                        + "							END + CASE 																											    	    	    	    					  "
                        + "							WHEN c.collation_name IS NOT NULL																					    	    	    	    					  "
                        + "								THEN ' COLLATE ' + c.collation_name																				    	    	    	    					  "
                        + "							ELSE ''																												    	    	    	    					  "
                        + "							END + CASE 																											    	    	    	    					  "
                        + "							WHEN c.is_nullable = 1																								    	    	    	    					  "
                        + "								THEN ' NULL'																									    	    	    	    					  "
                        + "							ELSE ' NOT NULL'																									    	    	    	    					  "
                        + "							END + CASE 																											    	    	    	    					  "
                        + "							WHEN dc.[definition] IS NOT NULL																					    	    	    	    					  "
                        + "								THEN ' DEFAULT' + dc.[definition]																				    	    	    	    					  "
                        + "							ELSE ''																												    	    	    	    					  "
                        + "							END + CASE 																											    	    	    	    					  "
                        + "							WHEN ic.is_identity = 1																								    	    	    	    					  "
                        + "								THEN ' IDENTITY(' + CAST(ISNULL(ic.seed_value, '0') AS CHAR(1)) + ',' + CAST(ISNULL(ic.increment_value, '1') AS     CHAR    (1))     +  ')'  				  "
                        + "							ELSE ''																												    	    	    	    					  "
                        + "							END																													    	    	    	    					  "
                        + "					END + CHAR(13)																												    	    	    	    					  "
                        + "			FROM sys.columns c WITH (NOWAIT)																									    	    	    	    					  "
                        + "			JOIN sys.types tp WITH (NOWAIT) ON c.user_type_id = tp.user_type_id																	    	    	    	    					  "
                        + "			LEFT JOIN sys.computed_columns cc WITH (NOWAIT) ON c.[object_id] = cc.[object_id]													    	    	    	    					  "
                        + "				AND c.column_id = cc.column_id																									    	    	    	    					  "
                        + "			LEFT JOIN sys.default_constraints dc WITH (NOWAIT) ON c.default_object_id != 0														    	    	    	    					  "
                        + "				AND c.[object_id] = dc.parent_object_id																							    	    	    	    					  "
                        + "				AND c.column_id = dc.parent_column_id																							    	    	    	    					  "
                        + "			LEFT JOIN sys.identity_columns ic WITH (NOWAIT) ON c.is_identity = 1																    	    	    	    					  "
                        + "				AND c.[object_id] = ic.[object_id]																								    	    	    	    					  "
                        + "				AND c.column_id = ic.column_id																									    	    	    	    					  "
                        + "			WHERE c.[object_id] = @object_id																									    	    	    	    					  "
                        + "			ORDER BY c.column_id																												    	    	    	    					  "
                        + "			FOR XML PATH('')																													    	    	    	    					  "
                        + "				,TYPE																															    	    	    	    					  "
                        + "			).value('.', 'NVARCHAR(MAX)'), 1, 2, CHAR(9) + ' ') + ISNULL((																		    	    	    	    					  "
                        + "			SELECT CHAR(9) + ', CONSTRAINT [' + k.name + '] PRIMARY KEY (' + (																	    	    	    	    					  "
                        + "					SELECT STUFF((																												    	    	    	    					  "
                        + "								SELECT ', [' + c.name + '] ' + CASE 																			    	    	    	    					  "
                        + "										WHEN ic.is_descending_key = 1																			    	    	    	    					  "
                        + "											THEN 'DESC'																							    	    	    	    					  "
                        + "										ELSE 'ASC'																								    	    	    	    					  "
                        + "										END																										    	    	    	    					  "
                        + "								FROM sys.index_columns ic WITH (NOWAIT)																			    	    	    	    					  "
                        + "								JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = ic.[object_id]												    	    	    	    					  "
                        + "									AND c.column_id = ic.column_id																				    	    	    	    					  "
                        + "								WHERE ic.is_included_column = 0																					    	    	    	    					  "
                        + "									AND ic.[object_id] = k.parent_object_id																		    	    	    	    					  "
                        + "									AND ic.index_id = k.unique_index_id																			    	    	    	    					  "
                        + "								FOR XML PATH(N'')																								    	    	    	    					  "
                        + "									,TYPE																										    	    	    	    					  "
                        + "								).value('.', 'NVARCHAR(MAX)'), 1, 2, '')																		    	    	    	    					  "
                        + "					) + ')' + CHAR(13)																											    	    	    	    					  "
                        + "			FROM sys.key_constraints k WITH (NOWAIT)																							    	    	    	    					  "
                        + "			WHERE k.parent_object_id = @object_id																								    	    	    	    					  "
                        + "				AND k.[type] = 'PK'																												    	    	    	    					  "
                        + "			), '') + ')' + CHAR(13) + ISNULL((																									    	    	    	    					  "
                        + "			SELECT (																															    	    	    	    					  "
                        + "					SELECT CHAR(13) + 'ALTER TABLE ' + @object_name + ' WITH' + CASE 															    	    	    	    					  "
                        + "							WHEN fk.is_not_trusted = 1																							    	    	    	    					  "
                        + "								THEN ' NOCHECK'																									    	    	    	    					  "
                        + "							ELSE ' CHECK'																										    	    	    	    					  "
                        + "							END + ' ADD CONSTRAINT [' + fk.name + '] FOREIGN KEY(' + STUFF((													    	    	    	    					  "
                        + "								SELECT ', [' + k.cname + ']'																					    	    	    	    					  "
                        + "								FROM fk_columns k																								    	    	    	    					  "
                        + "								WHERE k.constraint_object_id = fk.[object_id]																	    	    	    	    					  "
                        + "								FOR XML PATH('')																								    	    	    	    					  "
                        + "									,TYPE																										    	    	    	    					  "
                        + "								).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')' + ' REFERENCES [' + SCHEMA_NAME(ro.[schema_id]) + '].[' + ro.name     +  '] ('     +  STUFF((				  "
                        + "								SELECT ', [' + k.rcname + ']'																					    	    	    	    					  "
                        + "								FROM fk_columns k																								    	    	    	    					  "
                        + "								WHERE k.constraint_object_id = fk.[object_id]																	    	    	    	    					  "
                        + "								FOR XML PATH('')																								    	    	    	    					  "
                        + "									,TYPE																										    	    	    	    					  "
                        + "								).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')' + CASE 															    	    	    	    					  "
                        + "							WHEN fk.delete_referential_action = 1																				    	    	    	    					  "
                        + "								THEN ' ON DELETE CASCADE'																						    	    	    	    					  "
                        + "							WHEN fk.delete_referential_action = 2																				    	    	    	    					  "
                        + "								THEN ' ON DELETE SET NULL'																						    	    	    	    					  "
                        + "							WHEN fk.delete_referential_action = 3																				    	    	    	    					  "
                        + "								THEN ' ON DELETE SET DEFAULT'																					    	    	    	    					  "
                        + "							ELSE ''																												    	    	    	    					  "
                        + "							END + CASE 																											    	    	    	    					  "
                        + "							WHEN fk.update_referential_action = 1																				    	    	    	    					  "
                        + "								THEN ' ON UPDATE CASCADE'																						    	    	    	    					  "
                        + "							WHEN fk.update_referential_action = 2																				    	    	    	    					  "
                        + "								THEN ' ON UPDATE SET NULL'																						    	    	    	    					  "
                        + "							WHEN fk.update_referential_action = 3																				    	    	    	    					  "
                        + "								THEN ' ON UPDATE SET DEFAULT'																					    	    	    	    					  "
                        + "							ELSE ''																												    	    	    	    					  "
                        + "							END + CHAR(13) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + fk.name + ']' + CHAR(13)					    	    	    	    					  "
                        + "					FROM sys.foreign_keys fk WITH (NOWAIT)																						    	    	    	    					  "
                        + "					JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = fk.referenced_object_id												    	    	    	    					  "
                        + "					WHERE fk.parent_object_id = @object_id																						    	    	    	    					  "
                        + "					FOR XML PATH(N'')																											    	    	    	    					  "
                        + "						,TYPE																													    	    	    	    					  "
                        + "					).value('.', 'NVARCHAR(MAX)')																								    	    	    	    					  "
                        + "			), '') + ISNULL((																													    	    	    	    					  "
                        + "			(																																	    	    	    	    					  "
                        + "				SELECT CHAR(13) + 'CREATE' + CASE 																								    	    	    	    					  "
                        + "						WHEN i.is_unique = 1																									    	    	    	    					  "
                        + "							THEN ' UNIQUE'																										    	    	    	    					  "
                        + "						ELSE ''																													    	    	    	    					  "
                        + "						END + ' NONCLUSTERED INDEX [' + i.name + '] ON ' + @object_name + ' (' + STUFF((										    	    	    	    					  "
                        + "							SELECT ', [' + c.name + ']' + CASE 																					    	    	    	    					  "
                        + "									WHEN c.is_descending_key = 1																				    	    	    	    					  "
                        + "										THEN ' DESC'																							    	    	    	    					  "
                        + "									ELSE ' ASC'																									    	    	    	    					  "
                        + "									END																											    	    	    	    					  "
                        + "							FROM index_column c																									    	    	    	    					  "
                        + "							WHERE c.is_included_column = 0																						    	    	    	    					  "
                        + "								AND c.index_id = i.index_id																						    	    	    	    					  "
                        + "							FOR XML PATH('')																									    	    	    	    					  "
                        + "								,TYPE																											    	    	    	    					  "
                        + "							).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')' + ISNULL(CHAR(13) + 'INCLUDE (' + STUFF((							    	    	    	    					  "
                        + "								SELECT ', [' + c.name + ']'																						    	    	    	    					  "
                        + "								FROM index_column c																								    	    	    	    					  "
                        + "								WHERE c.is_included_column = 1																					    	    	    	    					  "
                        + "									AND c.index_id = i.index_id																					    	    	    	    					  "
                        + "								FOR XML PATH('')																								    	    	    	    					  "
                        + "									,TYPE																										    	    	    	    					  "
                        + "								).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')', '') + CASE 														    	    	    	    					  "
                        + "						WHEN ISNULL(i.filter_definition, '') = ''																				    	    	    	    					  "
                        + "							THEN ''																												    	    	    	    					  "
                        + "						ELSE ' WHERE ' + i.filter_definition																					    	    	    	    					  "
                        + "						END + CHAR(13)																											    	    	    	    					  "
                        + "				FROM sys.indexes i WITH (NOWAIT)																								    	    	    	    					  "
                        + "				WHERE i.[object_id] = @object_id																								    	    	    	    					  "
                        + "					AND i.is_primary_key = 0																									    	    	    	    					  "
                        + "					AND i.[type] = 2																											    	    	    	    					  "
                        + "				FOR XML PATH('')																												    	    	    	    					  "
                        + "					,TYPE																														    	    	    	    					  "
                        + "				).value('.', 'NVARCHAR(MAX)')																									    	    	    	    					  "
                        + "			), '')																																    	    	    	    					  "
                        + "																																				    	    	    	    					  "
                        + "SELECT @table_name as [name], @SQL AS table_definition																														  ";
                        #endregion

                        SqlCommand command = new SqlCommand(query, conn);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name_item = reader["name"].ToString();
                                string content = reader["table_definition"].ToString();

                                var is_export = downloadList?.Contains(name_item);

                                if (is_export == true || opcionExport == DatabaseManager.OpcionExport.ALL)
                                {
                                    listItemsContents.Add(new DatabaseItem() { Name = name_item, Contents = content });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new NullReferenceException("Login failed", ex);
            }
            finally
            {
                conn.Close();
            }

            return listItemsContents;
        }

        #endregion
    }
}
