using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TimeSheet.Migrations
{
    public partial class WhatNow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedPay",
                schema: "TimeSheet",
                table: "WorkDays",
                nullable: false,
                computedColumnSql: "TimeSheet.GetEstimatedPay([EmployeeId])",
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedPay",
                schema: "TimeSheet",
                table: "WorkDays",
                nullable: false,
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "TimeSheet.GetEstimatedPay([EmployeeId])");
        }
    }
}
