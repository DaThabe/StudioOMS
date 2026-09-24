using StudioOMS.Employees;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Commissions;


public static class CommissionCalculator
{
    public static Money Calc(TimingOrder order, Employee employee, int month)
    {
        var days = order.Consumes
            .Where(x => x.Timestamp.Month == month && x.EmployeeId == employee.Id)
            .Sum(x => x.Days);

        // 避免除数为0
        if (order.TotalDays == 0)
            return Money.From(0, order.Price.Currency);


        var price = days / order.TotalDays * order.Price.Amount * employee.CommissionRate.Decimal;
        return Money.From(price, order.Price.Currency);
    }
}
