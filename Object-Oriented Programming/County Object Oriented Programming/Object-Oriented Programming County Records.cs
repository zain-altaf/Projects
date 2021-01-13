#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Bme121
{
    // Represent one record from the dataset by Office of the National Coordinator (ONC)
    // Regional Extension Centers (REC) on Key Performance Indicators (KPIs) by county.
    // The ONC REC program provides assistance to health care providers to adopt
    // and meaningfully use certified Electronic Health Records (EHR) technology.
    // Data covers the date range from April 2012 to June 2015.
    // The Office of the National Coordinator for Health Information Technology
    // is a division of the U.S. Department of Health and Human Services.
    // See https://dashboard.healthit.gov/datadashboard/documentation/
    // ONC-REC-kpi-county-data-documentation.php.

    class EhrKpiRecord
    {
        public string State                                { get; private set; }
        public string StateCode                            { get; private set; }
        public string CountyName                           { get; private set; }
        public string StateFips                            { get; private set; }
        public string CountyFips                           { get; private set; }
        public string Fips                                 { get; private set; }
        public string Period                               { get; private set; }
        public int?   NumProvidersSignedUp                 { get; private set; }
        public int?   NumPrimaryCareProvidersSignedUp      { get; private set; }
        public int?   NumProvidersGoLive                   { get; private set; }
        public int?   NumPrimaryCareProvidersGoLive        { get; private set; }
        public int?   NumProvidersMeaningfulUse            { get; private set; }
        public int?   NumPrimaryCareProvidersMeaningfulUse { get; private set; }

        public EhrKpiRecord
        (
            string state,
            string stateCode,
            string countyName,
            string stateFips,
            string countyFips,
            string fips,
            string period,
            int?   numProvidersSignedUp,
            int?   numPrimaryCareProvidersSignedUp,
            int?   numProvidersGoLive,
            int?   numPrimaryCareProvidersGoLive,
            int?   numProvidersMeaningfulUse,
            int?   numPrimaryCareProvidersMeaningfulUse
        )
        {
            State                                = state;
            StateCode                            = stateCode;
            CountyName                           = countyName;
            StateFips                            = stateFips;
            CountyFips                           = countyFips;
            Fips                                 = fips;
            Period                               = period;
            NumProvidersSignedUp                 = numProvidersSignedUp;
            NumPrimaryCareProvidersSignedUp      = numPrimaryCareProvidersSignedUp;
            NumProvidersGoLive                   = numProvidersGoLive;
            NumPrimaryCareProvidersGoLive        = numPrimaryCareProvidersGoLive;
            NumProvidersMeaningfulUse            = numProvidersMeaningfulUse;
            NumPrimaryCareProvidersMeaningfulUse = numPrimaryCareProvidersMeaningfulUse;
        }
    }

    static class Program
    {
        static void Main( )
        {

            List< EhrKpiRecord > ehrKpiRecords = new List< EhrKpiRecord >( );

            // TO DO: Complete this code.

            // Load the ONC REC KPI dataset on EHR adoption by county.
            const string path = "REC_KPI_County.csv";
            const FileMode mode = FileMode.Open;
            const FileAccess access = FileAccess.Read;

            using FileStream file = new FileStream ( path, mode, access );
            using StreamReader reader = new StreamReader (file);
            reader.ReadLine();


            while(! reader.EndOfStream)
            {
              string line = reader.ReadLine();
              string[] columns = line.Split(',');

              for (int i = 0; i < columns.Length; i++)
              {
                  columns[i] = columns[i].Trim('"');
              }

              string state       = columns [0];
              string stateCode   = columns [1];
              string countyName  = columns [2];
              string stateFips   = columns [3];
              string countyFips  = columns [4];
              string fips        = columns [5];
              string period      = columns [6];


              int? numProvidersSignedUp;
              int? numPrimaryCareProvidersSignedUp;
              int? numProvidersGoLive;
              int? numPrimaryCareProvidersGoLive;
              int? numProvidersMeaningfulUse;
              int? numPrimaryCareProvidersMeaningfulUse;

              // Int32 is yuck use int.Parse()
              if(columns[7] == "NA") numProvidersSignedUp = null;
              else numProvidersSignedUp = Convert.ToInt32(columns[7]);

              if(columns[8] == "NA") numPrimaryCareProvidersSignedUp = null;
              else numPrimaryCareProvidersSignedUp = Convert.ToInt32(columns[8]);

              if(columns[9] == "NA") numProvidersGoLive = null;
              else numProvidersGoLive = Convert.ToInt32(columns[9]);

              if(columns[10] == "NA") numPrimaryCareProvidersGoLive = null;
              else numPrimaryCareProvidersGoLive = Convert.ToInt32(columns[10]);

              if(columns[11] == "NA") numProvidersMeaningfulUse = null;
              else numProvidersMeaningfulUse = Convert.ToInt32(columns[11]);

              if(columns[12] == "NA") numPrimaryCareProvidersMeaningfulUse = null;
              else numPrimaryCareProvidersMeaningfulUse = Convert.ToInt32(columns[12]);

              //
              //EhrKpiRecord [] e = [10]
              EhrKpiRecord recordList = new EhrKpiRecord (state,
                stateCode, countyName, stateFips, countyFips, fips, period, numProvidersSignedUp, numPrimaryCareProvidersSignedUp,
                numProvidersGoLive, numPrimaryCareProvidersGoLive, numProvidersMeaningfulUse, numPrimaryCareProvidersMeaningfulUse);

              ehrKpiRecords.Add(recordList);

            }

            WriteLine( "ehrKpiRecords.Count = {0:n0}", ehrKpiRecords.Count );

            // Display all unique ( State, StateCode, StateFips ) three-tuples.

            HashSet< ( string, string, string ) > states
                = new HashSet< ( string, string, string ) >( );

            foreach( EhrKpiRecord r in ehrKpiRecords )
            {
                states.Add( ( r.State, r.StateCode, r.StateFips ) );
            }

            foreach( ( string, string, string ) s in states )
            {
                WriteLine( s );
            }
        }
    }
}
