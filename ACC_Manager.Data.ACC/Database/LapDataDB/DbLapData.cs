﻿using ACCManager.Broadcast;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCManager.Data.ACC.Database.LapDataDB
{
    /// <summary>
    /// All data except for the Index must be divided by 1000 to get the actual value (floating point precision is annoying)
    /// </summary>
    public class DbLapData
    {
#pragma warning disable IDE1006 // Naming Styles
        public Guid _id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        public Guid RaceSessionGuid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The time when this lap was completed
        /// </summary>
        public DateTime UtcCompleted { get; set; }

        /// <summary>
        /// Lap Index
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Lap Time in milliseconds
        /// </summary>
        public int Time { get; set; } = -1;
        public int Sector1 { get; set; } = -1;
        public int Sector2 { get; set; } = -1;
        public int Sector3 { get; set; } = -1;

        public bool IsValid { get; set; } = true;

        public LapType LapType { get; set; } = LapType.ERROR;

        /// <summary>
        /// Milliliters of Fuel left at the end of the lap, divide by 1000...
        /// </summary>
        public int FuelUsage { get; set; } = -1;

        public override string ToString()
        {
            return $"Lap: {Index}, Time: {this.GetLapTime():F3}, IsValid: {IsValid}, S1: {this.GetSector1():F3}, S2: {this.GetSector2():F3}, S3: {this.GetSector3():F3}, LapType: {this.LapType}";
        }
    }

    public class LapDataCollection
    {
        private static ILiteCollection<DbLapData> _collection;
        private static ILiteCollection<DbLapData> Collection
        {
            get
            {
                if (_collection == null)
                    _collection = LocalDatabase.Database.GetCollection<DbLapData>();

                return _collection;
            }
        }

        public static void Insert(DbLapData lap)
        {
            Collection.EnsureIndex(x => x._id, true);
            Collection.Insert(lap);
        }

        public static List<DbLapData> GetForSession(Guid sessionId)
        {
            return Collection.Find(x => x.RaceSessionGuid == sessionId).ToList();
        }
    }
}
