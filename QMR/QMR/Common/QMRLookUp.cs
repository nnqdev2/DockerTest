using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QMR.Data;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using QMR.Models;

namespace QMR.Common
{
    public static class QMRTLookUp
    {
        public static List<Principal> memberList = new List<Principal>();
        internal static List<string> GetMeasureGroup(QMRContext db)
        {
            return (from e in db.MeasureGroups
                    select e.MeasureGroupName.ToString()).ToList();
        }
        internal static List<string> GetMeasureType(QMRContext db)
        {
            return (from e in db.MeasureType
                    select e.MeasureTypeDescription.ToString()).ToList();
        }
        internal static int GetMeasureTypeId(QMRContext db, String selectedType)
        {
            return (from e in db.MeasureType
                    where e.MeasureTypeDescription == selectedType
                    select e.MeasureTypeId).FirstOrDefault();
        }
        internal static int GetSeasonId(QMRContext db, String selectedseason)
        {
            return (from e in db.Season
                    where e.SeasonName == selectedseason
                    select e.SeasonId).FirstOrDefault();
        }

        internal static int GetMeasureGroupId(QMRContext db, String selectedGroup)
        {
            return (from e in db.MeasureGroups
                    where e.MeasureGroupName == selectedGroup
                    select e.MeasureGroupId).FirstOrDefault();
        }
        internal static List<string> GetMeasurebyselectedGroup(QMRContext db, String selectedGroup)
        {
            int selectedGroupId = GetMeasureGroupId(db, selectedGroup);
            return (from e in db.Measure
                    where e.MeasureGroupId == selectedGroupId
                    select e.Title.ToString()).ToList();
        }
        internal static List<string> GetAllMeasure(QMRContext db)
        {
            return (from e in db.Measure
                    select e.Title.ToString()).ToList();
        }
        internal static int GetMeasureId(QMRContext db, string measureTitle)
        {
            return (from e in db.Measure
                    where e.Title == measureTitle
                    select e.MeasureId).FirstOrDefault();
        }

        internal static List<string> GetSeason(QMRContext db)
        {
            return (from e in db.Season
                    select e.SeasonName.ToString()).ToList();
        }
        internal static List<string> GetMeasureOwner()
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "deq.state.or.us");

            GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx,
                                                   IdentityType.Name,
                                                   "QMRStewards");
            memberList = grp.Members.ToList();
            var myDomainUsers = new List<string>();
            foreach (var member in memberList)
            {
                myDomainUsers.Add(member.Name);
            }

            return myDomainUsers;
        }

        internal static List<string> GetAllActions(QMRContext db)
        {
            return (from e in db.measureAction
                    select e.ActionName.ToString()).ToList();
        }
        internal static List<string> GetAllActiveActions(QMRContext db)
        {
            return (from e in db.measureAction.OrderBy(e => e.ActionName)
                    where e.ActionStatus == true
                    select e.ActionName.ToString()).ToList();
        }
        internal static string GetActionName(QMRContext db, int id)
        {
            return (from e in db.measureAction
                    where e.ActionId == id
                    select e.ActionName.ToString()).FirstOrDefault();
        }
        internal static List<string> GetAllTrends(QMRContext db)
        {
            return (from e in db.Trend
                    select e.TrendName.ToString()).ToList();
        }
        internal static List<string> GetAllMeasureUnits(QMRContext db)
        {
            return (from e in db.Measure
                    select e.MeasureUnit.ToString()).Distinct().ToList();
        }
        internal static string GetTrend(QMRContext db, int id)
        {
            return (from e in db.Trend
                    where e.TrendId == id
                    select e.TrendName.ToString()).FirstOrDefault();
        }
        internal static string GetQuarter(QMRContext db, int QuarterId)
        {
            var query = (from e in db.QuarterlyMeasure
                         where e.QuarterId == QuarterId
                         select string.Concat(e.Quarter.ToString(), "-", e.Year.ToString())).FirstOrDefault();
            return query == null ? "" : query;
        }
        internal static string GetQuarterStatus(QMRContext db, int QuarterId)
        {
            var query = (from e in db.QuarterlyMeasure
                         where e.QuarterId == QuarterId
                         select e.Locked.ToString()).FirstOrDefault();
            return query == null ? "" : query;
        }
        internal static List<string> GetCurrentValueOptions(QMRContext db)
        {
            return (from e in db.CurrentValueOption
                    select e.CurrentValueOptionDescription.ToString()).ToList();
        }
        internal static int GetCurrentValueOptionId(QMRContext db, String selectedCurrentValueOption)
        {
            return (from e in db.CurrentValueOption
                    where e.CurrentValueOptionDescription == selectedCurrentValueOption
                    select e.CurrentValueOptionId).FirstOrDefault();
        }
        internal static string GetCurrentValueOptionDescription(QMRContext db, int currentValueOptionId)
        {
            return (from e in db.CurrentValueOption
                    where e.CurrentValueOptionId == currentValueOptionId
                    select e.CurrentValueOptionDescription).FirstOrDefault();
        }
        internal static int GetQuarterId(QMRContext db, int year, string quarter)
        {
            return (from e in db.QuarterlyMeasure
                    where e.Quarter == quarter && e.Year == year
                    select e.QuarterId).FirstOrDefault();
        }
        internal static bool IsReporting(QMRContext db, int measureId)
        {
            return (from e in db.Measure
                    where e.MeasureId == measureId
                    select e.Reporting).FirstOrDefault();
        }
    }
}
