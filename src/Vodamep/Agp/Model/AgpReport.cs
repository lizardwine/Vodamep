﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.Data.Dummy;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class AgpReport : ITravelTimeReport
    {
        public ReportType ReportType => ReportType.Agp;
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReport.Institution => this.Institution;

        IList<IPerson> IReport.Persons => this.Persons.Select(x => x as IPerson).ToList();
       
        IList<IStaff> ITravelTimeReport.Staffs => this.Staffs.Select(x => x as IStaff).ToList();
        
        IList<ITravelTime> ITravelTimeReport.TravelTimes => this.TravelTimes.Select(x => x as ITravelTime).ToList();

        public static AgpReport CreateDummyData()
        {
            var r = new AgpReport()
            {
                Institution = new Institution()
                {
                    Id = "test",
                    Name = "Test"
                }
            };

            r.FromD = DateTime.Today.FirstDateInMonth().AddMonths(-1);
            r.ToD = r.FromD.LastDateInMonth();

            r.AddDummyPerson();
            r.AddDummyStaff();
            r.AddDummyActivities();

            return r.AsSorted();

        }

        public static AgpReport ReadFile(string file)
        {
            var report = new AgpReportSerializer().DeserializeFile(file);
            return report;
        }

        public static AgpReport Read(byte[] data)
        {
            var report = new AgpReportSerializer().Deserialize(data);
            return report;
        }

        public static AgpReport Read(Stream data)
        {
            var report = new AgpReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new AgpReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new AgpReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new AgpReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash() => SHAHasher.GetReportHash(this.ToByteArray());


        //public DiffResult Diff(AgpReport report) => new HkpAgvReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(AgpReport report) => new AgpReportDiffer().DiffList(this, report);


       
    }
}