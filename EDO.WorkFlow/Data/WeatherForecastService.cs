namespace EDO.WorkFlow.Data;


public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "�3 ��� ISIN", "�3 ��� ���", "�3 ��� ����", "��������"
    };
    private static readonly string[] Summaries11 = new[]
    {
        "����� �����������������"
    };
    private static readonly string[] Summaries21 = new[]
    {
        "���������"
    };
    private static readonly string[] Summaries31 = new[]
    {
        "��������"
    };
    private static readonly string[] Summaries41 = new[]
    {
        "��������"
    };
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        return Task.FromResult(Enumerable.Range(1, 15).Select(index => new WeatherForecast
        {
            Username = Summaries[Random.Shared.Next(Summaries.Length)],
            Towhom = Summaries11[Random.Shared.Next(Summaries11.Length)],
            Shippingstatus = Summaries21[Random.Shared.Next(Summaries21.Length)],
            Description = Summaries31[Random.Shared.Next(Summaries31.Length)],
            Data = Summaries41[Random.Shared.Next(Summaries41.Length)]
        }).ToArray());
    }
}