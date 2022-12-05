using System.Threading.Tasks;

namespace wSHSApp.Reports;

public interface IReport
{
    string Name { set; get; }
    string OutputFileName { set; get; }
    string Description { set; get; }
    string Author { set; get; }
    public Task<string> EntryPoint(string[] args);
    public Task RemoveReport();
}

