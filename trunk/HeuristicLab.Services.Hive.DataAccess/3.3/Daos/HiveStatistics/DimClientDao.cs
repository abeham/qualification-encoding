﻿#region License Information
/* HeuristicLab
 * Copyright (C) Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace HeuristicLab.Services.Hive.DataAccess.Daos.HiveStatistics {
  public class DimClientDao : GenericDao<Guid, DimClient> {
    public DimClientDao(DataContext dataContext) : base(dataContext) { }

    public override DimClient GetById(Guid id) {
      return GetByIdQuery(DataContext, id);
    }

    public IQueryable<DimClient> GetAllOnlineClients() {
      return Table.Where(x => x.DateExpired == null);
    }

    public IQueryable<DimClient> GetAllExpiredClients() {
      return Table.Where(x => x.DateExpired != null);
    }

    public IQueryable<DimClient> GetAllOnlineSlaves() {
      return Table.Where(x => x.DateExpired == null && x.ResourceType == "Slave");
    }

    public IQueryable<DimClient> GetAllOnlineSlaveGroups() {
      return Table.Where(x => x.DateExpired == null && x.ResourceType == "GROUP");
    }

    public IQueryable<DimClient> GetAllExpiredSlaves() {
      return Table.Where(x => x.DateExpired != null && x.ResourceType == "Slave");
    }

    public IQueryable<DimClient> GetAllExpiredSlaveGroups() {
      return Table.Where(x => x.DateExpired != null && x.ResourceType == "GROUP");
    }

    public int UpdateExpirationTime(IEnumerable<Guid> ids, DateTime time) {
      string paramIds = string.Join(",", ids.Select(x => string.Format("'{0}'", x)));
      if (!string.IsNullOrWhiteSpace(paramIds)) {
        string query = string.Format(UpdateDateExpiredQuery, "{0}", paramIds);
        return DataContext.ExecuteCommand(query, time);
      }
      return 0;
    }

    #region Compiled queries
    private static readonly Func<DataContext, Guid, DimClient> GetByIdQuery =
      CompiledQuery.Compile((DataContext db, Guid id) =>
        (from dimClient in db.GetTable<DimClient>()
         where dimClient.Id == id
         select dimClient).SingleOrDefault());
    #endregion

    #region String queries
    private const string UpdateDateExpiredQuery =
      @"UPDATE [statistics].[DimClient] 
           SET DateExpired = {0} 
         WHERE Id IN ({1});";
    #endregion
  }
}
