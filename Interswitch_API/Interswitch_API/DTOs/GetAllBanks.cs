namespace Interswitch_API.DTOs;


public class GetAllBanks
{
    public List<Bank> Banks { get; set; }
}

public class Bank
{
    public int Id { get; set; }
    public string CbnCode { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}