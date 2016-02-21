﻿using AutomaticCodeGenerator.Class.DAL;
using System;
using System.Windows.Forms;

namespace AutomaticCodeGenerator.Class.BLL
{
    /// <summary>
    /// Automatic Code Generator
    /// Developed by: Abdullah Al-Muzahid
    /// </summary>
    /// 
    public class DBOperationManager:IDisposable
    {
        private SQLOperation provider;
        private DataGridView grdColumns;      
        private string table;

        public DBOperationManager(string connectionString, DataGridView grdColumns, string table)
        {
            this.provider = new SQLOperation(connectionString);
            this.grdColumns = grdColumns;          
            this.table = table;
        }

        public bool createTable(out string message)
        {
            message = string.Empty;

            try
            {

                string ts = "CREATE TABLE [dbo].[" + table + "]("
                + "[" + table + "ID] [int] IDENTITY(1,1) NOT NULL, "
                + "[" + table + "Name] [varchar](256) NOT NULL, ";

                foreach (DataGridViewRow dr in grdColumns.Rows)
                {
                    if (dr.Cells[0].Value != null && dr.Cells[1].Value != null)
                    {
                        //Ignoring the attibutes which are defined as ID
                        string idCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (idCol.Equals("id") || idCol.Equals(table.Trim().ToLower() + "id"))
                            continue;
                        string nameCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (nameCol.Equals("name") || idCol.Equals(table.Trim().ToLower() + "name"))
                            continue;

                        if (dr.Cells[1].Value.ToString() == "int" || dr.Cells[1].Value.ToString() == "float" || dr.Cells[1].Value.ToString() == "DateTime" || dr.Cells[1].Value.ToString() == "ntext")
                        {
                            ts += "[" + dr.Cells[0].Value.ToString() + "] [" + dr.Cells[1].Value.ToString() + "], ";
                        }
                        else
                        {
                            ts += "[" + dr.Cells[0].Value.ToString() + "] [" + dr.Cells[1].Value.ToString() + "](" + dr.Cells[2].Value.ToString() + "), ";
                        }
                    }
                }

                ts += "[ActiveStatus] [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, "
                    + "[InsertedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, "
                    + "[InsertedOn] [datetime] NULL, "
                    + "[UpdatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, "
                    + "[UpdatedOn] [datetime] NULL, "
                    + "CONSTRAINT [PK_" + table + "] PRIMARY KEY CLUSTERED "
                    + "([" + table + "ID] ASC"
                    + ") WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]"
                    + ") ON [PRIMARY]";

                if (this.provider.executeQuery(ts))
                    return true;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                ErrorManager errL = new ErrorManager();
                errL.WriteError("", Ex.Message.ToString(), Ex.Source.ToString(), Ex.StackTrace.ToString());
                message  = "\n\n" + Ex.Message + "\n\n";
                return false;
            }
        }


        public bool createStoredProcedures(out string message)
        {
            message = string.Empty;

            try
            {
                int err = 0;

                string sp = ""
                    + "-- =============================================\n"
                    + "-- Author: Abdullah Al-Muzahid, Agradut IT       \n"
                    + "-- Generated On: " + DateTime.Now.ToString() + " \n"
                    + "-- Generated By: Automatic Code Generator (V 1.0.0.0)\n"
                    + "-- Description:	This procedure is automatically generated by Code Gnerator\n"
                    + "-- It is used to insert and update record in the " + table + "table\n"
                    + "-- =============================================\n"
                    + "CREATE PROCEDURE [dbo].[Proc_InsertUpdate_" + table + "]\n"
                    + "-- The parameters for the stored procedure\n"
                    + "(\n"
                    + "@ID int OUTPUT,\n"
                    + "@Name VARCHAR(256),\n";

                foreach (DataGridViewRow dr in grdColumns.Rows)
                {
                    if (dr.Cells[0].Value != null && dr.Cells[1].Value != null)
                    {
                        //Ignoring the attibutes which are defined as ID
                        string idCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (idCol.Equals("id") || idCol.Equals(table.Trim().ToLower() + "id"))
                            continue;
                        string nameCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (nameCol.Equals("name") || idCol.Equals(table.Trim().ToLower() + "name"))
                            continue;

                        if (dr.Cells[1].Value.ToString() == "int" || dr.Cells[1].Value.ToString() == "float" || dr.Cells[1].Value.ToString() == "DateTime" || dr.Cells[1].Value.ToString() == "ntext")
                        {
                            sp += "@" + dr.Cells[0].Value.ToString() + " " + dr.Cells[1].Value.ToString() + "=null,\n ";
                        }
                        else
                        {
                            sp += "@" + dr.Cells[0].Value.ToString() + " " + dr.Cells[1].Value.ToString() + " (" + dr.Cells[2].Value.ToString() + ")=null, \n";
                        }
                    }
                }

                sp += "@ActiveStatus char(1)='A',\n"
                   + "@User varchar(50)=null\n"
                   + ")\n"
                   + "AS\n"
                   + "BEGIN\n"
                   + "SET NOCOUNT ON;\n"
                   + "IF NOT EXISTS (SELECT 1 FROM [dbo].[" + table + "] WHERE [" + table + "ID]=@ID)\n"
                   + "BEGIN\n"
                   + "INSERT INTO [dbo].[" + table + "]\n"
                   + "(\n"
                   + "[" + table + "Name],\n";
                foreach (DataGridViewRow dr in grdColumns.Rows)
                {
                    if (dr.Cells[0].Value != null && dr.Cells[1].Value != null)
                    {
                        //Ignoring the attibutes which are defined as ID
                        string idCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (idCol.Equals("id") || idCol.Equals(table.Trim().ToLower() + "id"))
                            continue;
                        string nameCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (nameCol.Equals("name") || idCol.Equals(table.Trim().ToLower() + "name"))
                            continue;

                        sp += "[" + dr.Cells[0].Value.ToString() + "],\n";
                    }
                }
                sp += "[ActiveStatus], \n [InsertedBy], \n [InsertedOn] \n ) \n VALUES \n ( \n @Name, \n";
                foreach (DataGridViewRow dr in grdColumns.Rows)
                {
                    if (dr.Cells[0].Value != null && dr.Cells[1].Value != null)
                    {
                        //Ignoring the attibutes which are defined as ID
                        string idCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (idCol.Equals("id") || idCol.Equals(table.Trim().ToLower() + "id"))
                            continue;
                        string nameCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (nameCol.Equals("name") || idCol.Equals(table.Trim().ToLower() + "name"))
                            continue;

                        sp += "@" + dr.Cells[0].Value.ToString() + ",\n";
                    }
                }
                sp += "@ActiveStatus, \n @User, \n GETDATE() \n )"
                    + "SET @ID = @@IDENTITY \n"
                    + "END \n ELSE \n BEGIN \n"
                    + "UPDATE  [dbo].[" + table + "] \n"
                    + "SET \n"
                    + "[" + table + "Name]=@Name,\n";
                foreach (DataGridViewRow dr in grdColumns.Rows)
                {
                    if (dr.Cells[0].Value != null && dr.Cells[1].Value != null)
                    {
                        //Ignoring the attibutes which are defined as ID
                        string idCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (idCol.Equals("id") || idCol.Equals(table.Trim().ToLower() + "id"))
                            continue;
                        string nameCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (nameCol.Equals("name") || idCol.Equals(table.Trim().ToLower() + "name"))
                            continue;

                        sp += "[" + dr.Cells[0].Value.ToString() + "] = " + "@" + dr.Cells[0].Value.ToString() + ", \n";
                    }
                }
                sp += "[ActiveStatus]=@ActiveStatus, \n"
                    + "[UpdatedBy]=@User, \n"
                    + "[UpdatedOn]=GETDATE() \n"
                    + "WHERE [" + table + "ID] = @ID \n"
                    + "END \n"
                    + "END \n";


                if (!this.provider.executeQuery(sp))
                    err += 1;




                sp = ""
                     + "-- =============================================\n"
                    + "-- Author: Abdullah Al-Muzahid, Agradut IT        \n"
                    + "-- Generated On: " + DateTime.Now.ToString() + " \n"
                    + "-- Generated By: Automatic Code Generator (V 1.0.0.0)\n"
                    + "-- Description:	This procedure is automatically generated by Code Gnerator\n"
                    + "-- It is used to retrieve records from the " + table + "table\n"
                    + "-- =============================================\n"
                    + "CREATE PROCEDURE [dbo].[Proc_Retrieve_" + table + "]\n"
                    + "-- The parameters for the stored procedure\n"
                    + "(\n @ID INT=NULL \n ,@ActiveStatus char(1)=NULL \n ) \n"
                    + "AS \n BEGIN \n SET NOCOUNT ON; \n"
                    + "SELECT   [" + table + "ID] AS 'ID', [" + table + "Name] AS 'Name', \n";

                foreach (DataGridViewRow dr in grdColumns.Rows)
                {
                    if (dr.Cells[0].Value != null && dr.Cells[1].Value != null)
                    {
                        //Ignoring the attibutes which are defined as ID
                        string idCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (idCol.Equals("id") || idCol.Equals(table.Trim().ToLower() + "id"))
                            continue;
                        string nameCol = dr.Cells[0].Value.ToString().Trim().ToLower();
                        if (nameCol.Equals("name") || idCol.Equals(table.Trim().ToLower() + "name"))
                            continue;

                        sp += "[" + dr.Cells[0].Value.ToString() + "],\n";
                    }
                }

                sp += "[ActiveStatus], \n [InsertedBy] \n, [InsertedOn] \n"
                    + "FROM [dbo].[" + table + "] \n"
                    + "WHERE [" + table + "ID]=ISNULL(@ID,[" + table + "ID]) \n"
                    + "AND [ActiveStatus]=ISNULL(@ActiveStatus,[ActiveStatus]) \n END ";

                if (!this.provider.executeQuery(sp))
                    err += 1;


                sp = ""
                     + "-- =============================================\n"
                    + "-- Author: Abdullah Al-Muzahid, Agradut IT       \n"
                    + "-- Generated On: " + DateTime.Now.ToString() + " \n"
                    + "-- Generated By: Automatic Code Generator (V 1.0.0.0)\n"
                    + "-- Description:	This procedure is automatically generated by Code Gnerator\n"
                    + "-- It is used to delete record from the " + table + "table\n"
                    + "-- =============================================\n"
                    + "CREATE PROCEDURE [dbo].[Proc_Delete_" + table + "]\n"
                     + "-- The parameters for the stored procedure\n"
                    + "(\n @ID INT \n) \n"
                    + "AS \n BEGIN \n "
                    + "DELETE [" + table + "] WHERE [" + table + "ID] = @ID; \n END";

                if (!this.provider.executeQuery(sp))
                    err += 1;

                if (err > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception Ex)
            {
                ErrorManager errL = new ErrorManager();
                errL.WriteError("", Ex.Message.ToString(), Ex.Source.ToString(), Ex.StackTrace.ToString());
                message = "\n\n" + Ex.Message + "\n\n";
                return false;
            }
        }

        public void Dispose()
        {
            provider = null;
            grdColumns = null;
            table = null;
        }
    }
}