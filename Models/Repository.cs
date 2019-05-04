using System;
using System.Collections.Generic;
using System.Linq;

namespace ISAM4332.Assignment05.Models
{
    public class Repository
    {
        private static List<MortgageApplication> MortgageApplications = new List<MortgageApplication>();

        public static IEnumerable<MortgageApplication> Get()
        {
            return MortgageApplications;
        }

        public static MortgageApplication Get(Guid id)
        {
            return MortgageApplications.FirstOrDefault(k => k.ApplicationId == id);
        }

        public static bool Add(MortgageApplication application)
        {
            bool added = false;
            try
            {
                if (application.ApplicationId == Guid.Empty)
                {
                    application.ApplicationId = Guid.NewGuid();
                }
                var app = MortgageApplications.FirstOrDefault(k => k.ApplicationId == application.ApplicationId);
                if (app == null)
                {
                    MortgageApplications.Add(application);
                    added = true;
                }
            }
            catch (Exception)
            {
                added = false;
            }
            return added;
        }

        public static bool Update(MortgageApplication application)
        {
            bool updated = false;
            try
            {
                var app = MortgageApplications.FirstOrDefault(k => k.ApplicationId == application.ApplicationId);
                if (app != null)
                {
                    bool removed = MortgageApplications.Remove(app);
                    if (removed)
                    {
                        MortgageApplications.Add(application);
                        updated = true;
                    }
                }
            }
            catch (Exception)
            {
                updated = false;
            }
            return updated;
        }

        public static bool Remove(MortgageApplication application)
        {
            bool removed = false;
            try
            {
                if (application.ApplicationId != Guid.Empty)
                {
                    var app = MortgageApplications.FirstOrDefault(k => k.ApplicationId == application.ApplicationId);
                    if (app != null)
                    {
                        removed = MortgageApplications.Remove(app);
                    }
                }
            }
            catch (Exception)
            {
                removed = false;
            }
            return removed;
        }

    }
}