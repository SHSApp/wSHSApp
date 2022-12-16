using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FSLib.Declension;
using System;

namespace wSHSApp.Reports.DisciplineReport.Data;

public class DisciplineReportDataFiller
{
    private readonly ReportDbContext context;

    List<Department>? departments;
    List<Rank>? ranks;
    List<Position>? positions;
    public DisciplineReportDataFiller(ReportDbContext Context)
    {
        context = Context;
    }

    public Task FillTables()
    {
        FillDepartments();
        FillRanks();
        FillPositions();
        return Task.CompletedTask;
    }

    private Task FillDepartments()
    {
        context.Departments.ExecuteDelete();
        departments = new() 
        {
            new Department
            {
                ShortName = "ОО",
                FullName = "Оперативный отдел"
            },
            new Department
            {
                ShortName = "ОБ",
                FullName = "Отдел безопасности"
            },
            new Department
            {
                ShortName = "ОВРО",
                FullName = "Отдел по воспитательной работе с осужденными"
            },
            new Department
            {
                ShortName = "ОКБИиХО",
                FullName = "Отдел комунально-бытового интендантского и хозяйственного обеспечения"
            },
            new Department
            {
                ShortName = "ДЧ",
                FullName = "Дежурная часть"
            },
            new Department
            {
                ShortName = "ПЧ",
                FullName = "Пожарная часть"
            }
        };
        context.Departments.AddRange(departments);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    private Task FillRanks()
    {
        context.Ranks.ExecuteDelete();
        ranks = new()
        {
            new Rank
            {
                ShortName = "рядовой",
                VeryShortName = "р-й",
                FullName = "рядовой"
            },
            new Rank
            {
                ShortName = "мл. сержант",
                VeryShortName = "мл. с-т",
                FullName = "младший сержант"
            },
            new Rank
            {
                ShortName = "сержант",
                VeryShortName = "с-т",
                FullName = "сержант"
            },
            new Rank
            {
                ShortName = "ст. сержант",
                VeryShortName = "ст. с-т",
                FullName = "старший сержант"
            },
            new Rank
            {
                ShortName = "прапорщик",
                VeryShortName = "пр-к",
                FullName = "прапорщик"
            },
            new Rank
            {
                ShortName = "ст. прапорщик",
                VeryShortName = "ст. пр-к",
                FullName = "старший прапорщик"
            },
            new Rank
            {
                ShortName = "мл. лейтенант",
                VeryShortName = "мл. л-т",
                FullName = "младший лейтенант"
            },
            new Rank
            {
                ShortName = "лейтенант",
                VeryShortName = "л-т",
                FullName = "лейтенант"
            },
            new Rank
            {
                ShortName = "ст. лейтенант",
                VeryShortName = "ст. л-т",
                FullName = "старший лейтенант"
            },
            new Rank
            {
                ShortName = "капитан",
                VeryShortName = "к-н",
                FullName = "капитан"
            },
            new Rank
            {
                ShortName = "майор",
                VeryShortName = "м-р",
                FullName = "майор"
            },
            new Rank
            {
                ShortName = "подполковник",
                VeryShortName = "п/п-к",
                FullName = "подполковник"
            },
            new Rank
            {
                ShortName = "полковник",
                VeryShortName = "п-к",
                FullName = "полковник"
            }
        };
        context.Ranks.AddRange(ranks);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    private Task FillPositions()
    {
        context.Positions.ExecuteDelete();
        positions = new()
        {
            new Position
            {
                ShortName = "начальник отряда ОВРО",
                FullName = "начальник отряда отдела по воспитательной работе с осужденными"
            },
            new Position
            {
                ShortName = "начальник ОВРО",
                FullName = "начальник отдела по воспитательной работе с осужденными"
            },
            new Position
            {
                ShortName = "мл. оперуполномоченный ОО",
                FullName = "младший оперуполномоченный оперативного отдела"
            },
            new Position
            {
                ShortName = "оперуполномоченный ОО",
                FullName = "оперуполномоченный оперативного отдела"
            },
            new Position
            {
                ShortName = "ст. оперуполномоченный ОО",
                FullName = "старший оперуполномоченный оперативного отдела"
            },
            new Position
            {
                ShortName = "начальник ОО",
                FullName = "начальник оперативного отдела"
            },
            new Position
            {
                ShortName = "мл. инспектор ОБ",
                FullName = "младший инспектор отдела безопасности"
            },
            new Position
            {
                ShortName = "инспектор ОБ",
                FullName = "инспектор отдела безопасности"
            },
            new Position
            {
                ShortName = "ст. инспектор ОБ",
                FullName = "старший инспектор отдела безопасности"
            },
            new Position
            {
                ShortName = "начальник ОБ",
                FullName = "начальник отдела безопасности"
            },
            new Position
            {
                ShortName = "мл. инспектор ОКБИиХО",
                FullName = "младший инспектор отдела комунально-бытового интендантского и хозяйственного обеспечения"
            },
            new Position
            {
                ShortName = "инспектор ОКБИиХО",
                FullName = "инспектор отдела комунально-бытового интендантского и хозяйственного обеспечения"
            },
            new Position
            {
                ShortName = "ст. инспектор ОКБИиХО",
                FullName = "старший инспектор отдела комунально-бытового интендантского и хозяйственного обеспечения"
            },
            new Position
            {
                ShortName = "начальник ОКБИиХО",
                FullName = "начальник отдела комунально-бытового интендантского и хозяйственного обеспечения"
            },
            new Position
            {
                ShortName = "ОГН ОБ",
                FullName = "оператор группы надзора отдела безопасности"
            },
            new Position
            {
                ShortName = "ЗДПНУ",
                FullName = "заместитель дежурного помощника начальника учреждения"
            },
            new Position
            {
                ShortName = "ДПНУ",
                FullName = "дежурный помощник начальника учреждения"
            },                        
            new Position
            {
                ShortName = "водитель ПЧ",
                FullName = "водитель пожарной части 1 разряда"
            },
            new Position
            {
                ShortName = "начальник караула ПЧ",
                FullName = "начальник караула пожарной части 1 разряда"
            },
            new Position
            {
                ShortName = "начальник ПЧ",
                FullName = "начальник пожарной части 1 разряда"
            },
            new Position
            {
                ShortName = "ЗНУ",
                FullName = "заместитель начальника учреждения"
            },
            new Position
            {
                ShortName = "Врио начальника",
                FullName = "временно исполняющий обязанности начальника учреждения"
            },
            new Position
            {
                ShortName = "НУ",
                FullName = "начальник учреждения"
            }
        };
        context.Positions.AddRange(positions);
        context.SaveChanges();
        return Task.CompletedTask;
    }
}
