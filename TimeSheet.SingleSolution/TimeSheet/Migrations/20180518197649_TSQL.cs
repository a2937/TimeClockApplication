using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TimeSheet.Migrations
{
    public partial class TSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = "Create function TimeSheet.GetEstimatedPay( @EmployeeId NvarChar(450) ) "
            + "RETURNS MONEY WITH SCHEMABINDING " +
              "BEGIN " +
              "DECLARE @Result MONEY; " +
              "SELECT @Result = ([HoursWorked] * [HourlyWage]) FROM TimeSheet.WorkDays, TimeSheet.AspNetUsers " +
              " WHERE TimeSheet.WorkDays.EmployeeId = @EmployeeId AND TimeSheet.WorkDays.EmployeeId = TimeSheet.AspNetUsers.Id; RETURN @Result END";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION [TimeSheet].[GetEstimatedPay]");
        }
    }
}