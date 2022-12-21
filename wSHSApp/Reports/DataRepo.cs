using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using wSHSApp.Reports.DisciplineReport.Data;

namespace wSHSApp.Reports;

public class DataRepo
{
    public DataRepo(ReportDbContext? Context)
    {
        context = Context;
        Personal = context?.Personal.Include(i => i.Rank).Include(i => i.Department).Include(i => i.Position).ToList();
        Violations = context?.Material.ToList();
        Places = context?.Places.ToList();
        Options = context?.Options?.ToList();
    }

    private readonly ReportDbContext? context;

    public List<Employee>? Personal { get; set; }
    public List<Violation>? Violations { get; set; }
    public List<Place>? Places { get; set; }
    public List<Option>? Options { get; set; }

    public List<Employee>? GetDPNUs()
    {
        return Personal!.Where(i => i.Position?.ShortName == "ДПНУ").ToList();
    }

}
