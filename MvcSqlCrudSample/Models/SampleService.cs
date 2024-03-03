namespace MvcSqlCrudSample.Models
{
public class SampleService
{
    private readonly ISampleRepository _sampleRepository;

    public SampleService(ISampleRepository sampleRepository)
    {
        _sampleRepository = sampleRepository;
    }

    public void Get()
    {
        var data = _sampleRepository.Get();
        Console.WriteLine(data);
    }
    }
}
