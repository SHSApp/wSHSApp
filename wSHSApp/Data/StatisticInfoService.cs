using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;
using wSHSApp.Models;

namespace wSHSApp.Data;

public class StatisticInfoService : InfoService
{
    public async Task<List<double>> GetDisciplineStatistic(int CurrentYear)
    {
        string Query1 = "SELECT Count(*) FROM Data\\distipl WHERE vdata LIKE ?";
        string Query2 = "SELECT Count(*) FROM Data\\pooshren WHERE vdataob LIKE ?";
        return await Task.FromResult(new List<double>
        {
            await GetCountAsync("%" + CurrentYear.ToString() + "%", OleDbType.VarChar, Query1),
            await GetCountAsync("%" + (CurrentYear - 1).ToString() + "%", OleDbType.VarChar, Query1),
            await GetCountAsync("%" + CurrentYear.ToString() + "%", OleDbType.VarChar, Query2),
            await GetCountAsync("%" + (CurrentYear - 1).ToString() + "%", OleDbType.VarChar, Query2)
        });
    }
    public async Task<List<double>> GetDisciplineStatisticMini()
    {
        string Query1 = "SELECT Count(*) FROM Data\\distipl WHERE vdata LIKE ?";
        string Query2 = "SELECT Count(*) FROM Data\\pooshren WHERE vdataob LIKE ?";
        return await Task.FromResult(new List<double>
        {
            await GetCountAsync("%" + DateTime.Now.ToString("MM.yyyy") + "%", OleDbType.VarChar, Query1),
            await GetCountAsync("%" + DateTime.Now.ToString("MM.yyyy") + "%", OleDbType.VarChar, Query2),
        });
    }
}
