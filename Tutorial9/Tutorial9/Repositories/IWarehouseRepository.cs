﻿using Microsoft.Data.SqlClient;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public interface IWarehouseRepository
{
    Task<bool> Exists(int id, SqlTransaction transaction);
}