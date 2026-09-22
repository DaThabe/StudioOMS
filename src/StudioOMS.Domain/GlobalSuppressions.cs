// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;


// 领域异常无需构造其他方法
[assembly: SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "<挂起>", Scope = "type", Target = "~T:StudioOMS.Employees.EmployeeMustHaveRoleException")]
