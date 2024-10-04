using ApexCore.DAL.Entities;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCore.DAL.Interfaces
{
    internal interface ICosmosWriter
    {
        Task<ItemResponse<DrivingEvent>> UpsertDrivingEvent();
    }
}
