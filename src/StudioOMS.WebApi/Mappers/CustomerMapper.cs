using StudioOMS.Customers;

namespace StudioOMS.WebApi.Mappers;

public static class CustomerMapper
{
    public static CustomerCreateRequest ToRequest(this CustomerCreateDto dto)
    {
        return new()
        {
            Name = CustomerName.From(dto.Name)
        };
    }
    public static CustomerRenameRequest ToRequest(this CustomerRenameDto dto, Guid customerId)
    {
        return new()
        {
            Id = CustomertId.From(customerId),
            Name = CustomerName.From(dto.Name)
        };
    }
}
